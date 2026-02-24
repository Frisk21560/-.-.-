
DROP TABLE IF EXISTS GroupsLectures;
DROP TABLE IF EXISTS Lectures;
DROP TABLE IF EXISTS Subjects;
DROP TABLE IF EXISTS Groups;
DROP TABLE IF EXISTS Teachers;
DROP TABLE IF EXISTS Departments;
DROP TABLE IF EXISTS Faculties;

CREATE TABLE Faculties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Financing MONEY NOT NULL DEFAULT 0,
    FacultyId INT NOT NULL FOREIGN KEY REFERENCES Faculties(Id)
);

CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Surname NVARCHAR(MAX) NOT NULL,
    Salary MONEY NOT NULL
);

CREATE TABLE Subjects (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Groups (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(10) NOT NULL UNIQUE,
    Year INT NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id)
);

CREATE TABLE Lectures (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DayOfWeek INT NOT NULL,
    LectureRoom NVARCHAR(MAX) NOT NULL,
    SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE GroupsLectures (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Groups(Id),
    LectureId INT NOT NULL FOREIGN KEY REFERENCES Lectures(Id)
);

INSERT INTO Faculties (Name)
VALUES 
('Computer Science'),
('Engineering'),
('Business');

INSERT INTO Departments (Name, Financing, FacultyId)
VALUES 
('Software Development', 50000, 1),
('Hardware Engineering', 45000, 1),
('Business Administration', 40000, 3),
('Data Science', 55000, 1);

INSERT INTO Teachers (Name, Surname, Salary)
VALUES 
('Dave', 'McQueen', 3000),
('Jack', 'Underhill', 3500),
('John', 'Smith', 2800),
('Sarah', 'Johnson', 3200),
('Mike', 'Brown', 2900),
('Emily', 'Davis', 3100);

INSERT INTO Subjects (Name)
VALUES 
('Database Design'),
('Web Development'),
('System Architecture'),
('Mobile Development'),
('Cloud Computing'),
('AI Fundamentals');

INSERT INTO Groups (Name, Year, DepartmentId)
VALUES 
('P-SD-24-01', 1, 1),
('P-SD-24-02', 1, 1),
('P-HE-24-01', 1, 2),
('P-BA-24-01', 1, 3),
('P-DS-24-01', 2, 4);

INSERT INTO Lectures (DayOfWeek, LectureRoom, SubjectId, TeacherId)
VALUES 
(1, 'D201', 1, 1),
(2, 'D202', 2, 1),
(3, 'D201', 3, 2),
(4, 'D203', 4, 3),
(1, 'D202', 5, 4),
(2, 'D201', 6, 5),
(3, 'D203', 1, 2),
(4, 'D202', 2, 1),
(5, 'D201', 3, 4),
(1, 'D203', 4, 6);

INSERT INTO GroupsLectures (GroupId, LectureId)
VALUES 
(1, 1),
(1, 2),
(1, 3),
(2, 1),
(2, 4),
(3, 5),
(4, 6),
(5, 7),
(5, 8),
(1, 9),
(2, 10);

-- 1. Вивести кількість викладачів кафедри «Software Development»
SELECT COUNT(DISTINCT Teachers.Id) AS [Кількість викладачів]
FROM Teachers, Lectures, Subjects, Departments
WHERE Lectures.TeacherId = Teachers.Id
  AND Lectures.SubjectId = Subjects.Id
  AND Departments.Name = 'Software Development';

-- 2. Вивести кількість лекцій, які читає викладач «Dave McQueen»
SELECT COUNT(*) AS [Кількість лекцій]
FROM Lectures, Teachers
WHERE Lectures.TeacherId = Teachers.Id
  AND Teachers.Name = 'Dave' AND Teachers.Surname = 'McQueen';

-- 3. Вивести кількість занять, які проводяться в аудиторії «D201»
SELECT COUNT(*) AS [Кількість занять]
FROM Lectures
WHERE Lectures.LectureRoom = 'D201';

-- 4. Вивести назви аудиторій та кількість лекцій, що проводяться в них
SELECT Lectures.LectureRoom, COUNT(*) AS [Кількість лекцій]
FROM Lectures
GROUP BY Lectures.LectureRoom;

-- 5. Вивести кількість студентів, які відвідують лекції викладача «Jack Underhill»
SELECT COUNT(DISTINCT GroupsLectures.GroupId) AS [Кількість груп]
FROM GroupsLectures, Lectures, Teachers
WHERE GroupsLectures.LectureId = Lectures.Id
  AND Lectures.TeacherId = Teachers.Id
  AND Teachers.Name = 'Jack' AND Teachers.Surname = 'Underhill';

-- 6. Вивести середню ставку викладачів факультету «Computer Science»
SELECT AVG(Teachers.Salary) AS [Середня ставка]
FROM Teachers, Lectures, Subjects, Departments, Faculties
WHERE Lectures.TeacherId = Teachers.Id
  AND Lectures.SubjectId = Subjects.Id
  AND Faculties.Name = 'Computer Science';

-- 7. Вивести середній фонд фінансування кафедр
SELECT AVG(Departments.Financing) AS [Середній фонд фінансування]
FROM Departments;

-- 8. Вивести повні імена викладачів та кількість читаних ними дисциплін
SELECT Teachers.Name + ' ' + Teachers.Surname AS [Повне імя], COUNT(DISTINCT Lectures.SubjectId) AS [Кількість дисциплін]
FROM Teachers, Lectures
WHERE Teachers.Id = Lectures.TeacherId
GROUP BY Teachers.Name, Teachers.Surname;

-- 9. Вивести кількість лекцій щодня протягом тижня
SELECT Lectures.DayOfWeek, COUNT(*) AS [Кількість лекцій]
FROM Lectures
GROUP BY Lectures.DayOfWeek
ORDER BY Lectures.DayOfWeek;

-- 10. Вивести номери аудиторій та кількість кафедр, чиї лекції в них читаються
SELECT Lectures.LectureRoom, COUNT(DISTINCT Departments.Id) AS [Кількість кафедр]
FROM Lectures, Subjects, Departments
WHERE Lectures.SubjectId = Subjects.Id
GROUP BY Lectures.LectureRoom;

-- 11. Вивести назви факультетів та кількість дисциплін, які на них читаються
SELECT Faculties.Name, COUNT(DISTINCT Lectures.SubjectId) AS [Кількість дисциплін]
FROM Faculties, Departments, Teachers, Lectures
WHERE Departments.FacultyId = Faculties.Id
  AND Lectures.TeacherId = Teachers.Id
GROUP BY Faculties.Name;

-- 12. Вивести кількість лекцій для кожної пари викладач-аудиторія
SELECT Teachers.Name + ' ' + Teachers.Surname AS [Викладач], Lectures.LectureRoom, COUNT(*) AS [Кількість лекцій]
FROM Teachers, Lectures
WHERE Teachers.Id = Lectures.TeacherId
GROUP BY Teachers.Name, Teachers.Surname, Lectures.LectureRoom;