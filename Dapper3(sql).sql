Copilot said: Розумію! Ось SQL скрипт для створення всього
Розумію! Ось SQL скрипт для створення всього з нуля для бази Dapper3:

SQL
-- Створити базу даних (якщо її немає)
CREATE DATABASE Dapper3;
GO

USE Dapper3;
GO

-- Створити таблицю Adopters (опікуни)
CREATE TABLE Adopters (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(MAX) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL
);

GO

-- Створити таблицю Dogs (собаки)
CREATE TABLE Dogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Breed NVARCHAR(100) NOT NULL,
    IsAdopted BIT DEFAULT 0,
    AdopterId INT NULL,
    FOREIGN KEY (AdopterId) REFERENCES Adopters(Id)
);

GO

-- Вставити тестові дані (опікуни)
INSERT INTO Adopters (FullName, PhoneNumber)
VALUES 
    ('Анна Кравець', '+380501234567'),
    ('Іван Петров', '+380502345678'),
    ('Марія Коваленко', '+380503456789');

GO

-- Вставити тестові дані (собаки)
INSERT INTO Dogs (Name, Age, Breed, IsAdopted, AdopterId)
VALUES 
    ('Макс', 3, 'Лабрадор', 0, NULL),
    ('Белла', 2, 'Овчарка', 0, NULL),
    ('Чарлі', 4, 'Бульдог', 0, NULL),
    ('Діна', 1, 'Той-тер''єр', 0, NULL),
    ('Бобик', 5, 'Мішаник', 1, 1),
    ('Луна', 2, 'Кокер-спаніель', 1, 2);

GO

-- Перевірити дані
SELECT * FROM Adopters;
SELECT * FROM Dogs;

GO

-- Перегляд собак з опікунами
SELECT D.Id, D.Name, D.Age, D.Breed, D.IsAdopted, D.AdopterId,
       A.Id AS AdopterId, A.FullName, A.PhoneNumber
FROM Dogs D
LEFT JOIN Adopters A ON D.AdopterId = A.Id;