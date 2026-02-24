DROP TABLE IF EXISTS Sales;
DROP TABLE IF EXISTS History;
DROP TABLE IF EXISTS Archive;
DROP TABLE IF EXISTS LastUnit;
DROP TABLE IF EXISTS EmployeeArchive;
DROP TABLE IF EXISTS Clients;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Employees;

CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(MAX) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    HireDate DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Salary MONEY NOT NULL
);

CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Type NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    CostPrice MONEY NOT NULL,
    Manufacturer NVARCHAR(100) NOT NULL,
    SalePrice MONEY NOT NULL
);

CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Discount DECIMAL(5,2) DEFAULT 0,
    Subscription BIT DEFAULT 0,
    TotalPurchase MONEY DEFAULT 0
);

CREATE TABLE Sales (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
    ClientId INT FOREIGN KEY REFERENCES Clients(Id),
    SalePrice MONEY NOT NULL,
    Quantity INT NOT NULL,
    SaleDate DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE History (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    SalePrice MONEY NOT NULL,
    Quantity INT NOT NULL,
    SaleDate DATE NOT NULL,
    EmployeeFullName NVARCHAR(MAX) NOT NULL,
    ClientFullName NVARCHAR(MAX)
);

CREATE TABLE Archive (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Type NVARCHAR(100) NOT NULL,
    Manufacturer NVARCHAR(100) NOT NULL,
    CostPrice MONEY NOT NULL,
    SalePrice MONEY NOT NULL,
    ArchiveDate DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE LastUnit (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    AlertDate DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE EmployeeArchive (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT NOT NULL,
    FullName NVARCHAR(MAX) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    HireDate DATE NOT NULL,
    DismissalDate DATE NOT NULL DEFAULT GETDATE(),
    Gender NVARCHAR(10) NOT NULL,
    Salary MONEY NOT NULL
);

GO

DROP TRIGGER IF EXISTS trg_InsertOrUpdateProduct;
DROP TRIGGER IF EXISTS trg_ManageEmployees;
DROP TRIGGER IF EXISTS trg_InsertSaleToHistory;
DROP TRIGGER IF EXISTS trg_MoveToArchive;
DROP TRIGGER IF EXISTS trg_PreventDuplicateClient;
DROP TRIGGER IF EXISTS trg_PreventClientDeletion;
DROP TRIGGER IF EXISTS trg_UpdateClientDiscount;
DROP TRIGGER IF EXISTS trg_CheckLastUnit;
DROP TRIGGER IF EXISTS trg_PreventSpecificManufacturer;
DROP TRIGGER IF EXISTS trg_PreventOldEmployeeDeletion;
DROP TRIGGER IF EXISTS trg_PreventExcessSellers;

GO

-- ТРИГЕР 1: При додаванні нового товару перевіряє його наявність на складі
-- Якщо товар існує і нові дані збігаються, оновлюється кількість товару

CREATE TRIGGER trg_InsertOrUpdateProduct
ON Products
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @ProductName NVARCHAR(100);
    DECLARE @ProductType NVARCHAR(100);
    DECLARE @Quantity INT;
    DECLARE @CostPrice MONEY;
    DECLARE @Manufacturer NVARCHAR(100);
    DECLARE @SalePrice MONEY;
    DECLARE @ExistingProductId INT;
    
    SELECT 
        @ProductName = Name,
        @ProductType = Type,
        @Quantity = Quantity,
        @CostPrice = CostPrice,
        @Manufacturer = Manufacturer,
        @SalePrice = SalePrice
    FROM inserted;
    
    -- Перевіряємо, чи це фірма "Спорт, сонце та штанга"
    IF @Manufacturer = 'Спорт, сонце та штанга'
    BEGIN
        RAISERROR('Додавання товару цієї фірми заборонено!', 16, 1);
        RETURN;
    END
    
    -- Перевіряємо, чи існує товар з такою ж назвою
    SELECT @ExistingProductId = Id
    FROM Products
    WHERE Name = @ProductName;
    
    -- Якщо товар існує
    IF @ExistingProductId IS NOT NULL
    BEGIN
        -- Перевіряємо, чи збігаються всі дані про товар
        IF EXISTS (
            SELECT 1 FROM Products
            WHERE Id = @ExistingProductId
            AND Type = @ProductType
            AND CostPrice = @CostPrice
            AND Manufacturer = @Manufacturer
            AND SalePrice = @SalePrice
        )
        BEGIN
            -- Оновлюємо кількість товару
            UPDATE Products
            SET Quantity = Quantity + @Quantity
            WHERE Id = @ExistingProductId;
            
            PRINT 'Товар вже існує. Оновлена кількість на складі.';
        END
        ELSE
        BEGIN
            -- Дані не збігаються, не дозволяємо додавання
            RAISERROR('Товар з такою назвою уже існує, але дані не збігаються!', 16, 1);
        END
    END
    ELSE
    BEGIN
        -- Товар не існує, додаємо його
        INSERT INTO Products (Name, Type, Quantity, CostPrice, Manufacturer, SalePrice)
        SELECT Name, Type, Quantity, CostPrice, Manufacturer, SalePrice FROM inserted;
        
        PRINT 'Новий товар успішно додано.';
    END
END;

GO

-- При звільненні та додаванні співробітників
-- - При звільненні переносить інформацію до архіву
-- - Забороняє видалення працівників, прийнятих до 2015 року
-- - Забороняє додавати нового продавця, якщо їх більше 6

CREATE TRIGGER trg_ManageEmployees
ON Employees
INSTEAD OF INSERT, DELETE
AS
BEGIN
    -- Якщо це INSERT
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        DECLARE @Position NVARCHAR(100);
        DECLARE @SellerCount INT;
        
        SELECT @Position = Position FROM inserted;
        
        -- Перевіряємо, чи це позиція "Продавець"
        IF @Position = 'Продавець'
        BEGIN
            -- Підраховуємо кількість існуючих продавців
            SELECT @SellerCount = COUNT(*)
            FROM Employees
            WHERE Position = 'Продавець';
            
            -- Якщо продавців вже більше або дорівнює 6
            IF @SellerCount >= 6
            BEGIN
                RAISERROR('Неможливо додати нового продавця! Кількість продавців уже досягла максимуму (6 осіб).', 16, 1);
                RETURN;
            END
        END
        
        -- Додаємо нового співробітника
        INSERT INTO Employees (FullName, Position, HireDate, Gender, Salary)
        SELECT FullName, Position, HireDate, Gender, Salary FROM inserted;
        
        PRINT 'Новий співробітник успішно додано.';
    END
    
    -- Якщо це DELETE
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Перевіряємо, чи це працівник, прийнятий до 2015 року
        IF EXISTS (SELECT 1 FROM deleted WHERE YEAR(HireDate) < 2015)
        BEGIN
            RAISERROR('Видалення працівників, прийнятих до 2015 року, заборонено!', 16, 1);
            RETURN;
        END
        
        -- Переносимо інформацію про звільненого співробітника до архіву
        INSERT INTO EmployeeArchive (EmployeeId, FullName, Position, HireDate, Gender, Salary)
        SELECT Id, FullName, Position, HireDate, Gender, Salary FROM deleted;
        
        -- Видаляємо працівника з основної таблиці
        DELETE FROM Employees WHERE Id IN (SELECT Id FROM deleted);
        
        PRINT 'Інформація про звільненого співробітника архівована.';
    END
END;

GO

-- ТРИГЕР 3: При продажу товару за��осити інформацію у таблицю History

CREATE TRIGGER trg_InsertSaleToHistory
ON Sales
AFTER INSERT
AS
BEGIN
    INSERT INTO History (ProductName, SalePrice, Quantity, SaleDate, EmployeeFullName, ClientFullName)
    SELECT 
        P.Name,
        S.SalePrice,
        S.Quantity,
        S.SaleDate,
        E.FullName,
        ISNULL(C.FullName, 'Невідомий покупець')
    FROM inserted S
    JOIN Products P ON S.ProductId = P.Id
    JOIN Employees E ON S.EmployeeId = E.Id
    LEFT JOIN Clients C ON S.ClientId = C.Id;
END;

GO

-- Перевести повністю проданий товар до Archive

CREATE TRIGGER trg_MoveToArchive
ON Sales
AFTER INSERT
AS
BEGIN
    DECLARE @ProductId INT;
    DECLARE @QuantityToSell INT;
    DECLARE @RemainingQuantity INT;
    
    SELECT @ProductId = ProductId, @QuantityToSell = Quantity FROM inserted;
    
    UPDATE Products
    SET Quantity = Quantity - @QuantityToSell
    WHERE Id = @ProductId;
    
    SELECT @RemainingQuantity = Quantity FROM Products WHERE Id = @ProductId;
    
    IF @RemainingQuantity = 0
    BEGIN
        INSERT INTO Archive (ProductName, Type, Manufacturer, CostPrice, SalePrice)
        SELECT Name, Type, Manufacturer, CostPrice, SalePrice
        FROM Products
        WHERE Id = @ProductId;
        
        DELETE FROM Products WHERE Id = @ProductId;
    END
END;

GO
-- Управління клієнтами (вставка та видалення)
-- - Не дозволяти реєструвати існуючого клієнта
-- - Заборонити видалення клієнтів

CREATE TRIGGER trg_ManageClients
ON Clients
INSTEAD OF INSERT, DELETE
AS
BEGIN
    -- Якщо це INSERT
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        DECLARE @FullName NVARCHAR(MAX);
        DECLARE @Email NVARCHAR(100);
        
        SELECT @FullName = FullName, @Email = Email FROM inserted;
        
        -- Перевіряємо, чи не існує клієнт з такою ж назвою або email
        IF EXISTS (
            SELECT 1 FROM Clients
            WHERE FullName = @FullName
               OR (Email = @Email AND @Email IS NOT NULL)
        )
        BEGIN
            RAISERROR('Цей клієнт уже зареєстрований!', 16, 1);
            RETURN;
        END
        
        -- Додаємо нового клієнта
        INSERT INTO Clients (FullName, Email, Phone, Gender, Discount, Subscription)
        SELECT FullName, Email, Phone, Gender, Discount, Subscription FROM inserted;
    END
    
    -- Якщо це DELETE
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        RAISERROR('Видалення клієнтів заборонено!', 16, 1);
    END
END;

GO

-- При новій покупці перевіряти суму і встановлювати 15% знижку

CREATE TRIGGER trg_UpdateClientDiscount
ON Sales
AFTER INSERT
AS
BEGIN
    DECLARE @ClientId INT;
    DECLARE @TotalAmount MONEY;
    
    SELECT @ClientId = ClientId, @TotalAmount = (SalePrice * Quantity) FROM inserted;
    
    IF @ClientId IS NOT NULL
    BEGIN
        UPDATE Clients
        SET TotalPurchase = TotalPurchase + @TotalAmount
        WHERE Id = @ClientId;
        
        UPDATE Clients
        SET Discount = 15
        WHERE TotalPurchase > 50000 AND Id = @ClientId;
    END
END;

GO

-- При продажу перевіряти кількість і заносити останню одиницю до LastUnit

CREATE TRIGGER trg_CheckLastUnit
ON Sales
AFTER INSERT
AS
BEGIN
    DECLARE @ProductId INT;
    DECLARE @QuantityToSell INT;
    DECLARE @RemainingQuantity INT;
    DECLARE @ProductName NVARCHAR(100);
    
    SELECT @ProductId = ProductId, @QuantityToSell = Quantity FROM inserted;
    SELECT @ProductName = Name, @RemainingQuantity = (Quantity - @QuantityToSell) 
    FROM Products WHERE Id = @ProductId;
    
    IF @RemainingQuantity = 1
    BEGIN
        INSERT INTO LastUnit (ProductName, Quantity)
        VALUES (@ProductName, @RemainingQuantity);
    END
END;

GO

INSERT INTO Employees (FullName, Position, HireDate, Gender, Salary)
VALUES 
('Іван Петренко', 'Продавець', '2014-05-10', 'Чоловік', 15000),
('Марія Коваленко', 'Продавець', '2016-03-20', 'Жінка', 15000),
('Сергій Шевченко', 'Менеджер', '2018-07-15', 'Чоловік', 20000),
('Олена Мельник', 'Касир', '2020-01-10', 'Жінка', 12000);

INSERT INTO Products (Name, Type, Quantity, CostPrice, Manufacturer, SalePrice)
VALUES 
('Спортивна футболка', 'Одяг', 48, 150, 'Nike', 350),
('Кросівки бігові', 'Взуття', 29, 500, 'Adidas', 1200),
('Шорти спортивні', 'Одяг', 20, 120, 'Puma', 280),
('Рукавиці боксерські', 'Аксесуари', 15, 800, 'Everlast', 1800),
('Йога килимок', 'Аксесуари', 5, 200, 'Reebok', 450);

INSERT INTO Clients (FullName, Email, Phone, Gender, Discount, Subscription, TotalPurchase)
VALUES 
('Олег Сидоренко', 'oleg@gmail.com', '0501234567', 'Чоловік', 0, 1, 2100),
('Юлія Нечипоренко', 'yuliya@outlook.com', '0509876543', 'Жінка', 0, 0, 1200),
('Андрій Гавриленко', 'andrii@gmail.com', '0505555555', 'Чоловік', 0, 1, 0);

GO

-- Вставка першого продажу
PRINT ' ПРИКЛАД 1: Вставка першого продажу';
INSERT INTO Sales (ProductId, EmployeeId, ClientId, SalePrice, Quantity, SaleDate)
VALUES (1, 1, 1, 350, 2, '2026-02-25');

GO

-- Вставка другого продажу
PRINT 'ПРИКЛАД 2: Вставка другого продажу';
INSERT INTO Sales (ProductId, EmployeeId, ClientId, SalePrice, Quantity, SaleDate)
VALUES (2, 2, 2, 1200, 1, '2026-02-25');

GO

-- Вставка третьго продажу
PRINT 'ПРИКЛАД 3: Вставка третього продажу';
INSERT INTO Sales (ProductId, EmployeeId, ClientId, SalePrice, Quantity, SaleDate)
VALUES (3, 1, 1, 280, 5, '2026-02-25');

GO

-- Історія продажів
PRINT 'ІСТОРІЯ ПРОДАЖІВ';
SELECT * FROM History;

GO

-- Останні одиниці
PRINT 'ОСТАННІ ОДИНИЦІ';
SELECT * FROM LastUnit;

GO

-- Клієнти зі оновленими даними
PRINT 'КЛІЄНТИ';
SELECT * FROM Clients;

GO

-- Залишки товарів
PRINT 'ЗАЛИШКИ ТОВАРІВ';
SELECT * FROM Products;

GO

-- Архівовані товари
PRINT 'АРХІВОВАНІ ТОВАРИ';
SELECT * FROM Archive;

GO

-- Архів співробітників
PRINT 'АРХІВ СПІВРОБІТНИКІВ';
SELECT * FROM EmployeeArchive;

GO