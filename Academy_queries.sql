-- ИДАЛЕННЯ СТАРОЇ БАЗИ ДАНИХ
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'Academy')
BEGIN
    ALTER DATABASE Academy SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Academy;
END
GO

USE Academy_queries;
GO

-- Таблиця "Факультети"
CREATE TABLE Faculties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (Name <> ''),
    Dean NVARCHAR(MAX) NOT NULL CHECK (Dean <> '')
);

-- Таблиця "Кафедри"
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (Name <> ''),
    Financing MONEY NOT NULL DEFAULT 0 CHECK (Financing >= 0)
);

-- Таблиця "Групи"
CREATE TABLE Groups (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(10) NOT NULL UNIQUE CHECK (Name <> ''),
    Rating INT NOT NULL CHECK (Rating >= 0 AND Rating <= 5),
    Year INT NOT NULL CHECK (Year >= 1 AND Year <= 5)
);

-- Таблиця "Викладачі"
CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL CHECK (Name <> ''),
    Surname NVARCHAR(MAX) NOT NULL CHECK (Surname <> ''),
    Position NVARCHAR(MAX) NOT NULL CHECK (Position <> ''),
    EmploymentDate DATE NOT NULL CHECK (EmploymentDate >= '1990-01-01'),
    Salary MONEY NOT NULL CHECK (Salary > 0),
    Premium MONEY NOT NULL DEFAULT 0 CHECK (Premium >= 0),
    IsAssistant BIT NOT NULL DEFAULT 0,
    IsProfessor BIT NOT NULL DEFAULT 0
);

-- Вставка даних у таблицю "Факультети"
INSERT INTO Faculties (Name, Dean)
VALUES 
('Computer Science', 'Марія Коваленко'),
('Engineering', 'Ганна Мельник'),
('Mathematics', 'Марія Коваленко');

-- Вставка даних у таблицю "Кафедри"
INSERT INTO Departments (Name, Financing)
VALUES 
('Інформатика', 15000),
('Математика', 8000),
('Програмування', 30000),
('Системи та мережі', 5000);

-- Вставка даних у таблицю "Групи"
INSERT INTO Groups (Name, Rating, Year)
VALUES 
('ІС-21', 5, 2),
('ІС-22', 4, 2),
('ІС-31', 3, 3),
('ІС-51', 2, 5),
('ІС-52', 4, 5);

-- Вставка даних у таблицю "Викладачі"
INSERT INTO Teachers (Name, Surname, Position, EmploymentDate, Salary, Premium, IsAssistant, IsProfessor)
VALUES 
('Іван', 'Петренко', 'Асистент', '2015-09-01', 500, 200, 1, 0),
('Марія', 'Коваленко', 'Професор', '2010-01-15', 1500, 400, 0, 1),
('Олег', 'Шевченко', 'Асистент', '1995-06-20', 600, 250, 1, 0),
('Ганна', 'Мельник', 'Професор', '1990-05-10', 1800, 500, 0, 1),
('Борис', 'Гончар', 'Асистент', '2012-03-15', 450, 180, 1, 0);

SELECT * FROM Faculties;
SELECT * FROM Departments;
SELECT * FROM Groups;
SELECT * FROM Teachers;

-- 1. Таблиця кафедр у зворотному порядку полів
SELECT Financing, Name, Id
FROM Departments;

-- 2. Назви груп та їх рейтинги з нами "Group Name" та "Group Rating"
SELECT Name AS [Group Name], Rating AS [Group Rating]
FROM Groups;

-- 3. Прізвище, відсоток ставки до надбавки, відсоток ставки до зарплати
SELECT Surname,
       CAST((Salary / Premium * 100) AS INT) AS [% ставки до надбавки],
       CAST((Salary / (Salary + Premium) * 100) AS INT) AS [% ставки до зарплати]
FROM Teachers;

-- 4. Таблиця факультетів у вигляді одного поля
SELECT 'The dean of faculty ' + Name + ' is ' + Dean + '.' AS [Faculty Info]
FROM Faculties;

-- 5. Прізвища професорів зі ставкою > 1050
SELECT Surname
FROM Teachers
WHERE IsProfessor = 1 AND Salary > 1050;

-- 6. Кафедри з фінансуванням < 11000 або > 25000
SELECT Name
FROM Departments
WHERE Financing < 11000 OR Financing > 25000;

-- 7. Факультети, окрім "Computer Science"
SELECT Name
FROM Faculties
WHERE Name <> 'Computer Science';

-- 8. Прівища та посади викладачів, які НЕ професори
SELECT Surname, Position
FROM Teachers
WHERE IsProfessor = 0;

-- 9. Прізвища, посади, ставки та надбавки асистентів з надбавкою від 160 до 550
SELECT Surname, Position, Salary, Premium
FROM Teachers
WHERE IsAssistant = 1 AND Premium BETWEEN 160 AND 550;

-- 10. Прізвища та ставки асистентів
SELECT Surname, Salary
FROM Teachers
WHERE IsAssistant = 1;

-- 11. Прізвища та посади викладачів, прийнятих до 01.01.2000
SELECT Surname, Position
FROM Teachers
WHERE EmploymentDate < '2000-01-01';

-- 12. Кафедри в алфавітному порядку до "Software Development"
SELECT Name AS [Name of Department]
FROM Departments
WHERE Name < 'Software Development'
ORDER BY Name;

-- 13. Прізвища асистентів з зарплатою <= 1200
SELECT Surname
FROM Teachers
WHERE IsAssistant = 1 AND (Salary + Premium) <= 1200;

-- 14. Назви груп 5го курсу з рейтингом від 2 до 4
SELECT Name
FROM Groups
WHERE Year = 5 AND Rating BETWEEN 2 AND 4;

-- 15. Прізвища асистентів зі ставкою < 550 або надбавкою < 200
SELECT Surname
FROM Teachers
WHERE IsAssistant = 1 AND (Salary < 550 OR Premium < 200);