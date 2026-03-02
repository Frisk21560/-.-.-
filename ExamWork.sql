
-- ЧАСТИНА 1: СТВОРЕННЯ ТАБЛИЦЬ, ЗВ'ЯЗКІВ ТА ТРИГЕРІВ

DROP TABLE IF EXISTS ListItems;
DROP TABLE IF EXISTS Lists;
DROP TABLE IF EXISTS PeopleInvolved;
DROP TABLE IF EXISTS Titles;
DROP TABLE IF EXISTS Roles;
DROP TABLE IF EXISTS People;
DROP TABLE IF EXISTS Users;

-- Таблиця Users - зареєстровані користувачі платформи
CREATE TABLE Users (
  UserID INT IDENTITY(1,1) PRIMARY KEY,
  Username NVARCHAR(200) NOT NULL UNIQUE,
  Email NVARCHAR(200) NOT NULL UNIQUE CHECK (Email LIKE '%@%.%'),
  PasswordHash NVARCHAR(256) NOT NULL,
  RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
  Role NVARCHAR(20) NOT NULL CHECK (Role IN ('User', 'Admin', 'Moderator')),
  Rating INT NOT NULL DEFAULT 0
);

-- Таблиця Titles - фільми
CREATE TABLE Titles (
  TitleID INT IDENTITY(1,1) PRIMARY KEY,
  Title NVARCHAR(200) NOT NULL,
  Description NVARCHAR(MAX),
  ReleaseYear INT,
  DurationMinutes INT,
  UserID INT NOT NULL,
  AdditionDT DATETIME NOT NULL DEFAULT GETDATE(),
  ModificationDT DATETIME,
  FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Таблиця Lists - користувацькі списки фільмів
CREATE TABLE Lists (
  ListID INT IDENTITY(1,1) PRIMARY KEY,
  UserID INT NOT NULL,
  Title NVARCHAR(200) NOT NULL CHECK (Title <> ''),
  Description NVARCHAR(MAX),
  IsPublic BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Таблиця ListItems - елементи списків
CREATE TABLE ListItems (
  ListItemID INT IDENTITY(1,1) PRIMARY KEY,
  ListID INT NOT NULL,
  TitleID INT NOT NULL,
  AddedDate DATETIME NOT NULL DEFAULT GETDATE(),
  FOREIGN KEY (ListID) REFERENCES Lists(ListID),
  FOREIGN KEY (TitleID) REFERENCES Titles(TitleID)
);

-- Таблиця Roles - ролі кіноперсон
CREATE TABLE Roles (
  RoleID INT IDENTITY(1,1) PRIMARY KEY,
  RoleTitle NVARCHAR(50) NOT NULL UNIQUE
);

-- Таблиця People - кіноперсони
CREATE TABLE People (
  PersonID INT IDENTITY(1,1) PRIMARY KEY,
  FullName NVARCHAR(200) NOT NULL,
  BirthDate DATE,
  Biography NVARCHAR(MAX)
);

-- Таблиця PeopleInvolved - зв'язкова таблиця між фільмами та кіноперсонами
CREATE TABLE PeopleInvolved (
  TitleID INT NOT NULL,
  PersonID INT NOT NULL,
  RoleID INT NOT NULL,
  PRIMARY KEY (TitleID, PersonID, RoleID),
  FOREIGN KEY (TitleID) REFERENCES Titles(TitleID),
  FOREIGN KEY (PersonID) REFERENCES People(PersonID),
  FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

GO

-- ТРИГЕР: При додаванні фільму збільшити рейтинг користувача на 1


CREATE TRIGGER trg_IncreaseUserRatingOnTitleAdd
ON Titles
AFTER INSERT
AS
BEGIN
  UPDATE Users
  SET Rating = Rating + 1
  WHERE UserID IN (SELECT UserID FROM inserted);
END;

GO

-- ЧАСТИНА 2: ВСТАВЛЕННЯ ДАНИХ

-- Вставлення ролей (має бути перше)
INSERT INTO Roles (RoleTitle)
VALUES
  ('Actor'),
  ('Director'),
  ('Screenwriter'),
  ('Producer'),
  ('Cinematographer'),
  ('Composer');

-- Вставлення користувачів
INSERT INTO Users (Username, Email, PasswordHash, Role)
VALUES
  ('PixelMaks', 'pixel.maks@gmail.com', 'hashed_password_1', 'User'),
  ('CinemaLover', 'cinema.lover@outlook.com', 'hashed_password_2', 'Admin'),
  ('FilmCritic', 'film.critic@gmail.com', 'hashed_password_3', 'Moderator'),
  ('MovieAddict', 'movie.addict@gmail.com', 'hashed_password_4', 'User'),
  ('BlockbusterFan', 'blockbuster.fan@outlook.com', 'hashed_password_5', 'User'),
  ('ScienceFictionNerd', 'scifi.nerd@gmail.com', 'hashed_password_6', 'User'),
  ('DramaQueenUser', 'drama.queen@gmail.com', 'hashed_password_7', 'User'),
  ('ActionHero', 'action.hero@outlook.com', 'hashed_password_8', 'User');

-- Власний запис для Users (Частина 2)
INSERT INTO Users (Username, Email, PasswordHash, Role)
VALUES
  ('VictorFilmFreak', 'victor.film@gmail.com', 'hashed_password_9', 'User');

-- Вставлення кіноперсон
INSERT INTO People (FullName, BirthDate, Biography)
VALUES
  ('Timothée Chalamet', '1994-12-27', 'French-American actor known for contemporary films'),
  ('Denis Villeneuve', '1967-10-03', 'Canadian filmmaker known for sci-fi and thriller films'),
  ('Hans Zimmer', '1957-09-12', 'German film composer known for epic scores'),
  ('Zendaya', '1996-09-01', 'American actress and singer'),
  ('Oscar Isaac', '1979-03-09', 'Guatemalan-American actor'),
  ('David Fincher', '1962-08-28', 'American film director and producer'),
  ('Dario Argento', '1940-09-07', 'Italian film director and producer'),
  ('Joaquin Phoenix', '1974-10-28', 'American actor'),
  ('Jodie Foster', '1962-11-19', 'American actress'),
  ('Christopher Nolan', '1970-07-30', 'British-American film director'),
  ('Steven Spielberg', '1946-12-18', 'American filmmaker'),
  ('Cillian Murphy', '1976-05-25', 'Irish actor'),
  ('Emma Stone', '1988-11-06', 'American actress'),
  ('Damien Chazelle', '1985-01-19', 'American film director and screenwriter'),
  ('Ryan Gosling', '1980-11-12', 'Canadian actor and musician'),
  ('Serhiy Stelmakh', '1985-03-15', 'Ukrainian actor and producer'),
  ('Volodymyr Zelenskyy', '1978-01-25', 'Ukrainian actor and politician'),
  ('Marlon Brando', '1924-04-03', 'American legendary actor');

-- Вставлення фільмів (базові)
INSERT INTO Titles (Title, Description, ReleaseYear, DurationMinutes, UserID)
VALUES
  ('Dune', 'Epic science fiction film set on the desert planet Arrakis', 2021, 155, 1),
  ('The Social Network', 'Drama about the founding of Facebook', 2010, 120, 2),
  ('Interstellar', 'Science fiction epic about space exploration and time', 2014, 169, 3),
  ('Insomnia', 'Psychological thriller about a detective in Alaska', 2002, 118, 1),
  ('Люксембург, Люксембург', 'Ukrainian comedy about modern life in modern society', 2015, 102, 2),
  ('La La Land', 'Musical romance about two artists in Los Angeles', 2016, 128, 4),
  ('Drive', 'Neo-noir thriller about a getaway driver in Los Angeles', 2011, 100, 5),
  ('Oppenheimer', 'Biographical drama about J. Robert Oppenheimer and atomic bomb', 2023, 180, 6),
  ('Blade Runner 2049', 'Science fiction sequel set in a dystopian future', 2017, 163, 7),
  ('The Killer', 'Thriller about a professional assassin on global missions', 2023, 118, 1);

-- Власний запис для Titles (Частина 2) - Український фільм від нового користувача
INSERT INTO Titles (Title, Description, ReleaseYear, DurationMinutes, UserID)
VALUES
  ('Мати', 'Драма про материнську любов та жертви в умовах сучасної України', 2020, 135, 9);

-- Вставлення фільмів до списків користувача PixelMaks
INSERT INTO Lists (UserID, Title, Description, IsPublic)
VALUES
  (1, 'Найкращі наукові фантастики', 'Список найвідомішх та найвпливовіших фільмів sci-fi', 1),
  (1, 'Пулярні драми', 'Психологічні драми та биографічні фільми для розуму', 0),
  (1, 'Екшени та трилери', 'Динамічні фільми для бурхливого вечора', 1);

-- Власний запис для Lists (Частина 2) - Від другого користувача
INSERT INTO Lists (UserID, Title, Description, IsPublic)
VALUES
  (2, 'Мої супер рецензії', 'Фільми з найкращими режисерськими рішеннями та гра акторів', 1);

-- Вставлення елементів до списків користувача PixelMaks
INSERT INTO ListItems (ListID, TitleID)
VALUES
  -- Список "Найкращі наукові фантастики"
  (1, 1),   -- Dune
  (1, 3),   -- Interstellar
  (1, 9),   -- Blade Runner 2049
  -- Список "Популярні драми"
  (2, 2),   -- The Social Network
  (2, 4),   -- Insomnia
  (2, 8),   -- Oppenheimer
  -- Список "Екшени та трилери"
  (3, 7),   -- Drive
  (3, 10);  -- The Killer

-- Власний запис для ListItems (Частина 2)
INSERT INTO ListItems (ListID, TitleID)
VALUES
  (4, 2),   -- The Social Network у список "Мої супер рецензії"
  (4, 6),   -- La La Land
  (4, 8);   -- Oppenheimer

-- Кіноперсони для фільму "Люксембург, Люксембург" та інші
INSERT INTO PeopleInvolved (TitleID, PersonID, RoleID)
VALUES
  -- Dune (1)
  (1, 1, 1),    -- Timothée Chalamet як Actor
  (1, 2, 2),    -- Denis Villeneuve як Director
  (1, 3, 6),    -- Hans Zimmer як Composer
  (1, 4, 1),    -- Zendaya як Actor
  
  -- The Social Network (2)
  (2, 5, 1),    -- Oscar Isaac як Actor
  (2, 6, 2),    -- David Fincher як Director
  
  -- Interstellar (3)
  (3, 12, 1),   -- Cillian Murphy як Actor
  (3, 10, 2),   -- Christopher Nolan як Director
  
  -- Insomnia (4)
  (4, 8, 1),    -- Joaquin Phoenix як Actor
  (4, 6, 2),    -- David Fincher як Director
  
  -- Люксембург, Люксембург (5)
  (5, 16, 1),   -- Serhiy Stelmakh як Actor
  (5, 17, 1),   -- Volodymyr Zelenskyy як Actor
  (5, 2, 2),    -- Denis Villeneuve як Director (для запиту про режисерів на D)
  
  -- La La Land (6)
  (6, 13, 1),   -- Emma Stone як Actor
  (6, 14, 2),   -- Damien Chazelle як Director
  (6, 15, 1),   -- Ryan Gosling як Actor
  
  -- Drive (7)
  (7, 15, 1),   -- Ryan Gosling як Actor
  (7, 14, 2),   -- Damien Chazelle як Director
  
  -- Oppenheimer (8)
  (8, 12, 1),   -- Cillian Murphy як Actor
  (8, 10, 2),   -- Christopher Nolan як Director
  
  -- Blade Runner 2049 (9)
  (9, 1, 1),    -- Timothée Chalamet як Actor
  (9, 2, 2),    -- Denis Villeneuve як Director
  
  -- The Killer (10)
  (10, 8, 1),   -- Joaquin Phoenix як Actor
  (10, 7, 2),   -- Dario Argento як Director
  
  -- Мати (11)
  (11, 16, 1),  -- Serhiy Stelmakh як Actor
  (11, 17, 2);  -- Volodymyr Zelenskyy як Director

-- Власний запис для PeopleInvolved (Частина 2)
-- Додаємо нового актора Marlon Brando до фільму Oppenheimer
INSERT INTO PeopleInvolved (TitleID, PersonID, RoleID)
VALUES
  (8, 18, 1);   -- Marlon Brando як Actor у Oppenheimer

GO

-- ЧАСТИНА 3: SQL-ЗАПИТИ ДЛЯ ОТРИМАННЯ ІНФОРМАЦІЇ

-- ЗАПИТ 1: Отримати ТОП-5 користувачів з найвищим рейтингом
PRINT 'ЗАПИТ 1: ТОП-5 користувачів з найвищим рейтингом';
SELECT TOP 5 UserID, Username, Email, Rating
FROM Users
ORDER BY Rating DESC;

PRINT '';

-- ЗАПИТ 2: Знайти середню тривалість фільмів
PRINT 'АПИТ 2: Середня тривалість фільмів';
SELECT AVG(DurationMinutes) AS AvgDurationMinutes
FROM Titles
WHERE DurationMinutes IS NOT NULL;

PRINT '';

-- ЗАПИТ 3: Показати назви фільмів, в яких знімався актор 'Timothée Chalamet'
PRINT 'ЗАПИТ 3: Фільми з акторомThimothée Chalamet';
SELECT DISTINCT T.Title
FROM Titles T
JOIN PeopleInvolved PI ON T.TitleID = PI.TitleID
JOIN People P ON PI.PersonID = P.PersonID
WHERE P.FullName = 'Timothée Chalamet' AND PI.RoleID = 1;

PRINT '';

-- ЗАПИТ 4: Вивести всі списки користувача 'PixelMaks' і фільми в них
PRINT 'ЗАПИТ 4: Списки користувача PixelMaks і фільми в них';
SELECT L.ListID, L.Title AS ListTitle, L.Description, T.TitleID, T.Title AS MovieTitle
FROM Lists L
LEFT JOIN ListItems LI ON L.ListID = LI.ListID
LEFT JOIN Titles T ON LI.TitleID = T.TitleID
WHERE L.UserID = (SELECT UserID FROM Users WHERE Username = 'PixelMaks')
ORDER BY L.ListID, T.Title;

PRINT '';

-- ЗАПИТ 5: Показати всю команду зйомок фільму "Люксембург, Люксембург" та їх ролі
PRINT 'ЗАПИТ 5: Команда зйомок фільму "Люксембург, Люксембург"';
SELECT P.PersonID, P.FullName, R.RoleTitle, P.BirthDate
FROM Titles T
JOIN PeopleInvolved PI ON T.TitleID = PI.TitleID
JOIN People P ON PI.PersonID = P.PersonID
JOIN Roles R ON PI.RoleID = R.RoleID
WHERE T.Title = 'Люксембург, Люксембург'
ORDER BY R.RoleTitle, P.FullName;

PRINT '';

-- ЗАПИТ 6: Вивести Id, назву та кількість елементів в кожному списку
PRINT 'ЗАПИТ 6: Id, назва та кількість елементів кожного списку';
SELECT L.ListID, L.Title, COUNT(LI.ListItemID) AS ItemCount
FROM Lists L
LEFT JOIN ListItems LI ON L.ListID = LI.ListID
GROUP BY L.ListID, L.Title
ORDER BY L.ListID;

PRINT '';

-- ЗАПИТ 7: Вивести всі публічні списки з 3 або більше фільмів
PRINT 'ЗАПИТ 7: Публічні списки з 3+ фільмів';
SELECT L.ListID, L.Title, L.Description, COUNT(LI.ListItemID) AS MovieCount
FROM Lists L
LEFT JOIN ListItems LI ON L.ListID = LI.ListID
WHERE L.IsPublic = 1
GROUP BY L.ListID, L.Title, L.Description
HAVING COUNT(LI.ListItemID) >= 3
ORDER BY MovieCount DESC;

PRINT '';

-- ЗАПИТ 8: Знайти фільми, що зняті режисерами, ім'я яких починається на літеру 'D'
PRINT 'ЗАПИТ 8: Фільми режисерів з імені на літеру D';
SELECT DISTINCT T.TitleID, T.Title, P.FullName AS DirectorName
FROM Titles T
JOIN PeopleInvolved PI ON T.TitleID = PI.TitleID
JOIN People P ON PI.PersonID = P.PersonID
JOIN Roles R ON PI.RoleID = R.RoleID
WHERE R.RoleTitle = 'Director' AND P.FullName LIKE 'D%'
ORDER BY T.Title;

PRINT '';

-- ЗАПИТ 9: Знайти повні імена людей, які мають дві різні ролі в одному фільмі
PRINT 'ЗАПИТ 9: Люди з двома різними ролями в одному фільмі';
SELECT P.FullName, T.Title, COUNT(DISTINCT PI.RoleID) AS RoleCount,
       STRING_AGG(R.RoleTitle, ', ') AS Roles
FROM People P
JOIN PeopleInvolved PI ON P.PersonID = PI.PersonID
JOIN Titles T ON PI.TitleID = T.TitleID
JOIN Roles R ON PI.RoleID = R.RoleID
GROUP BY P.PersonID, P.FullName, T.TitleID, T.Title
HAVING COUNT(DISTINCT PI.RoleID) >= 2
ORDER BY P.FullName, T.Title;

PRINT '';

GO


-- ПЕРЕВІРКА ДАНИХ


PRINT 'ПЕРЕВІРКА ТАБЛИЦІ USERS';
SELECT * FROM Users;

PRINT '';
PRINT 'ПЕРЕВІРКА ТАБЛИЦІ TITLES';
SELECT * FROM Titles;

PRINT '';
PRINT 'ПЕРЕВІРКА ТАБЛИЦІ LISTS';
SELECT * FROM Lists;

PRINT '';
PRINT 'ПЕРЕВІРКА ТАБЛИЦІ LISTITEMS';
SELECT * FROM ListItems;

PRINT '';
PRINT 'ПЕРЕВІРКА ТАБЛИЦІ PEOPLE';
SELECT * FROM People;

PRINT '';
PRINT 'ПЕРЕВІРКА ТАБЛИЦІ ROLES';
SELECT * FROM Roles;

PRINT '';
PRINT 'ПЕРЕВІРКА ТАБЛИЦІ PEOPLEINVOLVED';
SELECT * FROM PeopleInvolved;