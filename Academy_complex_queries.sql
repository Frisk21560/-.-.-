DROP TABLE IF EXISTS GroupsLectures;
DROP TABLE IF EXISTS GroupsCurators;
DROP TABLE IF EXISTS Schedules;
DROP TABLE IF EXISTS Lectures;
DROP TABLE IF EXISTS Assistants;
DROP TABLE IF EXISTS Curators;
DROP TABLE IF EXISTS Heads;
DROP TABLE IF EXISTS Deans;
DROP TABLE IF EXISTS LectureRooms;
DROP TABLE IF EXISTS Groups;
DROP TABLE IF EXISTS Subjects;
DROP TABLE IF EXISTS Departments;
DROP TABLE IF EXISTS Faculties;
DROP TABLE IF EXISTS Teachers;

CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Surname NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Faculties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Building INT NOT NULL,
    DeanId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE Deans (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Building INT NOT NULL,
    FacultyId INT NOT NULL FOREIGN KEY REFERENCES Faculties(Id),
    HeadId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE Heads (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE Groups (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(10) NOT NULL UNIQUE,
    Year INT NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id)
);

CREATE TABLE Subjects (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Lectures (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(Id),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE LectureRooms (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(10) NOT NULL UNIQUE,
    Building INT NOT NULL
);

CREATE TABLE Schedules (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Class INT NOT NULL,
    DayOfWeek INT NOT NULL,
    Week INT NOT NULL,
    LectureId INT NOT NULL FOREIGN KEY REFERENCES Lectures(Id),
    LectureRoomId INT NOT NULL FOREIGN KEY REFERENCES LectureRooms(Id)
);

CREATE TABLE Assistants (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE Curators (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(Id)
);

CREATE TABLE GroupsCurators (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CuratorId INT NOT NULL FOREIGN KEY REFERENCES Curators(Id),
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Groups(Id)
);

CREATE TABLE GroupsLectures (
    Id INT PRIMARY KEY IDENTITY(1,1),
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Groups(Id),
    LectureId INT NOT NULL FOREIGN KEY REFERENCES Lectures(Id)
);

INSERT INTO Teachers (Name, Surname)
VALUES 
('Edward', 'Hopper'),
('Alex', 'Carmack'),
('John', 'Smith'),
('Sarah', 'Johnson'),
('Michael', 'Brown'),
('Emily', 'Davis'),
('David', 'Wilson'),
('Lisa', 'Martinez');

INSERT INTO Deans (TeacherId)
VALUES 
(1),
(2),
(3);

INSERT INTO Faculties (Name, Building, DeanId)
VALUES 
('Computer Science', 1, 1),
('Engineering', 2, 2),
('Business', 3, 3);

INSERT INTO Heads (TeacherId)
VALUES 
(4),
(5),
(6);

INSERT INTO Departments (Name, Building, FacultyId, HeadId)
VALUES 
('Software Development', 1, 1, 4),
('Hardware Engineering', 1, 1, 5),
('Business Administration', 3, 3, 6);

INSERT INTO Groups (Name, Year, DepartmentId)
VALUES 
('F505', 5, 1),
('F404', 4, 1),
('F301', 3, 1),
('E505', 5, 2);

INSERT INTO Subjects (Name)
VALUES 
('Database Design'),
('Web Development'),
('System Architecture'),
('Mobile Development'),
('Cloud Computing'),
('AI Fundamentals');

INSERT INTO Lectures (SubjectId, TeacherId)
VALUES 
(1, 1),
(2, 1),
(3, 2),
(4, 3),
(5, 4),
(6, 5);

INSERT INTO LectureRooms (Name, Building)
VALUES 
('A101', 1),
('A102', 1),
('A311', 6),
('A104', 6),
('B201', 2),
('C301', 3);

INSERT INTO Schedules (Class, DayOfWeek, Week, LectureId, LectureRoomId)
VALUES 
(1, 1, 1, 1, 1),
(2, 2, 1, 1, 2),
(3, 3, 2, 1, 3),
(1, 1, 1, 2, 1),
(2, 3, 1, 3, 2),
(1, 2, 1, 4, 4),
(3, 1, 2, 5, 5),
(2, 4, 1, 6, 6),
(3, 3, 1, 2, 3),
(1, 5, 1, 3, 4);

INSERT INTO Assistants (TeacherId)
VALUES 
(7),
(8),
(5);

INSERT INTO Curators (TeacherId)
VALUES 
(4),
(5),
(6);

INSERT INTO GroupsCurators (CuratorId, GroupId)
VALUES 
(1, 1),
(1, 2),
(2, 3),
(3, 4);

INSERT INTO GroupsLectures (GroupId, LectureId)
VALUES 
(1, 1),
(1, 2),
(1, 3),
(2, 1),
(2, 4),
(3, 5),
(4, 6);

-- 1. Вивести назви аудиторій, де читає лекції викладач «Edward Hopper»
SELECT DISTINCT LectureRooms.Name
FROM LectureRooms, Schedules, Lectures, Teachers
WHERE Schedules.LectureRoomId = LectureRooms.Id
  AND Schedules.LectureId = Lectures.Id
  AND Lectures.TeacherId = Teachers.Id
  AND Teachers.Name = 'Edward' AND Teachers.Surname = 'Hopper';

-- 2. Вивести прізвища асистентів, які читають лекції у групі «F505»
SELECT DISTINCT Teachers.Surname
FROM Teachers, Assistants, Lectures, GroupsLectures, Groups
WHERE Assistants.TeacherId = Teachers.Id
  AND Lectures.TeacherId = Teachers.Id
  AND GroupsLectures.LectureId = Lectures.Id
  AND GroupsLectures.GroupId = Groups.Id
  AND Groups.Name = 'F505';

-- 3. Вивести дисципліни, які читає викладач «Alex Carmack» для груп 5 курсу
SELECT DISTINCT Subjects.Name
FROM Subjects, Lectures, Teachers, GroupsLectures, Groups
WHERE Lectures.SubjectId = Subjects.Id
  AND Lectures.TeacherId = Teachers.Id
  AND GroupsLectures.LectureId = Lectures.Id
  AND GroupsLectures.GroupId = Groups.Id
  AND Teachers.Name = 'Alex' AND Teachers.Surname = 'Carmack'
  AND Groups.Year = 5;

-- 4. Вивести прізвища викладачів, які не читають лекції у понеділок
SELECT DISTINCT Teachers.Surname
FROM Teachers
WHERE Teachers.Id NOT IN (
    SELECT DISTINCT Teachers.Id FROM Teachers, Lectures, Schedules
    WHERE Lectures.TeacherId = Teachers.Id
      AND Schedules.LectureId = Lectures.Id
      AND Schedules.DayOfWeek = 1
);

-- 5. Вивести назви аудиторій та корпуси без лекцій у середу другого тижня на третій парі
SELECT DISTINCT LectureRooms.Name, LectureRooms.Building
FROM LectureRooms
WHERE LectureRooms.Id NOT IN (
    SELECT DISTINCT LectureRoomId FROM Schedules
    WHERE DayOfWeek = 3 AND Week = 2 AND Class = 3
);

-- 6. Вивести повні імена викладачів факультету «Computer Science», які не курирують групи кафедри «Software Development»
SELECT DISTINCT Teachers.Name + ' ' + Teachers.Surname AS [Full Name]
FROM Teachers, Lectures, Subjects
WHERE Lectures.TeacherId = Teachers.Id
  AND Lectures.SubjectId = Subjects.Id
  AND Teachers.Id NOT IN (
      SELECT DISTINCT Curators.TeacherId FROM Curators, GroupsCurators, Groups, Departments
      WHERE GroupsCurators.CuratorId = Curators.Id
        AND GroupsCurators.GroupId = Groups.Id
        AND Groups.DepartmentId = Departments.Id
        AND Departments.Name = 'Software Development'
  );

-- 7. Вивести список номерів усіх корпусів, які є у таблицях факультетів, кафедр та аудиторій
SELECT DISTINCT Building FROM Faculties
UNION
SELECT DISTINCT Building FROM Departments
UNION
SELECT DISTINCT Building FROM LectureRooms;

-- 8. Вивести повні імена викладачів у порядку: декани, завідувачі, куратори, асистенти
SELECT Teachers.Name + ' ' + Teachers.Surname AS [Full Name], 'Dean' AS [Position]
FROM Teachers, Deans
WHERE Teachers.Id = Deans.TeacherId
UNION
SELECT Teachers.Name + ' ' + Teachers.Surname, 'Head'
FROM Teachers, Heads
WHERE Teachers.Id = Heads.TeacherId
UNION
SELECT Teachers.Name + ' ' + Teachers.Surname, 'Curator'
FROM Teachers, Curators
WHERE Teachers.Id = Curators.TeacherId
UNION
SELECT Teachers.Name + ' ' + Teachers.Surname, 'Assistant'
FROM Teachers, Assistants
WHERE Teachers.Id = Assistants.TeacherId;

-- 9. Вивести дні тижня без повторень, в які є заняття в аудиторіях «A311» та «A104» корпусу 6
SELECT DISTINCT Schedules.DayOfWeek
FROM Schedules, LectureRooms
WHERE Schedules.LectureRoomId = LectureRooms.Id
  AND LectureRooms.Building = 6
  AND (LectureRooms.Name = 'A311' OR LectureRooms.Name = 'A104');