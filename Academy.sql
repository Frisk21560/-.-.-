CREATE TABLE Groups (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(10) NOT NULL UNIQUE CHECK (Name <> ''),
    Rating INT NOT NULL CHECK (Rating >= 0 AND Rating <= 5),
    Year INT NOT NULL CHECK (Year >= 1 AND Year <= 5)
);

-- "Кафедри"
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (Name <> ''),
    Financing MONEY NOT NULL DEFAULT 0 CHECK (Financing >= 0)
);

-- "Факультети"
CREATE TABLE Faculties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (Name <> '')
);

-- "Викладачі"
CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL CHECK (Name <> ''),
    Surname NVARCHAR(MAX) NOT NULL CHECK (Surname <> ''),
    EmploymentDate DATE NOT NULL CHECK (EmploymentDate >= '1990-01-01'),
    Salary MONEY NOT NULL CHECK (Salary > 0),
    Premium MONEY NOT NULL DEFAULT 0 CHECK (Premium >= 0)
);

-- Вставка даних у таблицю "Групи"
INSERT INTO Groups (Name, Rating, Year)
VALUES
('ІС-21', 5, 2),
('ІС-22', 4, 2),
('ІС-31', 3, 3);

SELECT * FROM Groups;

-- Вставка даних у таблицю "Кафедри"
INSERT INTO Departments (Name, Financing)
VALUES
('Інформатика', 500000.00),
('Математика', 300000.00),
('Програмування', 450000.00);

SELECT * FROM Departments;

-- Вставка даних у таблицю "Факультети"
INSERT INTO Faculties (Name)
VALUES
('Факультет комп''ютерних наук'),
('Факультет природничих наук'),
('Факультет точних наук');

SELECT * FROM Faculties;

-- Вставка даних у таблицю "Викладачі"
INSERT INTO Teachers (Name, Surname, EmploymentDate, Salary, Premium)
VALUES
('Іван', 'Петренко', '2015-09-01', 15000.00, 2000.00),
('Марія', 'Коваленко', '2010-01-15', 18000.00, 3000.00),
('Олег', 'Шевченко', '1995-06-20', 20000.00, 2500.00);

SELECT * FROM Teachers;