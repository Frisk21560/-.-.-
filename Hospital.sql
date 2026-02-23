DROP TABLE IF EXISTS ExaminationsDiseases;
DROP TABLE IF EXISTS DoctorsExaminations;
DROP TABLE IF EXISTS Examinations;
DROP TABLE IF EXISTS Donations;
DROP TABLE IF EXISTS Vacations;
DROP TABLE IF EXISTS DoctorsSpecializations;
DROP TABLE IF EXISTS Wards;
DROP TABLE IF EXISTS Doctors;
DROP TABLE IF EXISTS Diseases;
DROP TABLE IF EXISTS Sponsors;
DROP TABLE IF EXISTS Specializations;
DROP TABLE IF EXISTS Departments;

CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Building NVARCHAR(10) NOT NULL
);

CREATE TABLE Specializations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Sponsors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Diseases (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Severity INT NOT NULL
);

CREATE TABLE Doctors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Surname NVARCHAR(MAX) NOT NULL,
    Salary MONEY NOT NULL,
    Premium MONEY NOT NULL DEFAULT 0,
    Phone NVARCHAR(20) NOT NULL
);

CREATE TABLE Wards (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(20) NOT NULL UNIQUE,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id),
    Floor INT NOT NULL
);

CREATE TABLE DoctorsSpecializations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id),
    SpecializationId INT NOT NULL FOREIGN KEY REFERENCES Specializations(Id)
);

CREATE TABLE Vacations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);

CREATE TABLE Donations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Amount MONEY NOT NULL,
    [Date] DATE NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id),
    SponsorId INT NOT NULL FOREIGN KEY REFERENCES Sponsors(Id)
);

CREATE TABLE Examinations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    DayOfWeek INT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL
);

CREATE TABLE DoctorsExaminations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id),
    ExaminationId INT NOT NULL FOREIGN KEY REFERENCES Examinations(Id),
    WardId INT NOT NULL FOREIGN KEY REFERENCES Wards(Id)
);

CREATE TABLE ExaminationsDiseases (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ExaminationId INT NOT NULL FOREIGN KEY REFERENCES Examinations(Id),
    DiseaseId INT NOT NULL FOREIGN KEY REFERENCES Diseases(Id),
    [Date] DATE NOT NULL
);

INSERT INTO Departments (Name, Building)
VALUES 
('Хірургічне', 'A'),
('Інфекційне', 'B'),
('Терапевтичне', 'A'),
('Кардіологічне', 'C'),
('Неврологічне', 'C'),
('Ортопедичне', 'B'),
('Педіатричне', 'D'),
('Онкологічне', 'D'),
('Хіміотерапевтичне', 'E'),
('Реабілітаційне', 'E'),
('Intensive Treatment', 'F');

INSERT INTO Specializations (Name)
VALUES 
('Хірургія'),
('Кардіологія'),
('Неврологія'),
('Ортопедія'),
('Педіатрія'),
('Онкологія'),
('Терапія'),
('Інфекціоністика');

INSERT INTO Sponsors (Name)
VALUES 
('Umbrella Corporation'),
('Red Cross'),
('WHO'),
('Gates Foundation'),
('Local Charity');

INSERT INTO Diseases (Name, Severity)
VALUES 
('Грип', 2),
('COVID-19', 4),
('Пневмонія', 3),
('Інфаркт', 5),
('Інсульт', 4),
('Переломи', 2),
('Рак легенів', 5),
('Лейкемія', 5),
('Астма', 2),
('Діабет', 3);

INSERT INTO Doctors (Name, Surname, Salary, Premium, Phone)
VALUES 
('Наталія', 'Перебийніс', 1704, 867, '3367827881'),
('Данило', 'Стельмах', 1541, 618, '6230026371'),
('Юхим', 'Заєць', 1088, 826, '2386251432'),
('Валентина', 'Ґжицький', 1940, 747, '3398403648'),
('Вікторія', 'Фоменко', 1255, 549, '1723675039'),
('Варвара', 'Хоменко', 2888, 996, '6371821267'),
('Омелян', 'Тимченко', 1161, 387, '0929880040'),
('Маруся', 'Москаль', 1600, 565, '2666515146'),
('Борис', 'Жук', 2287, 849, '6516904350'),
('Симон', 'Шеремета', 2813, 633, '5391026638'),
('Камілла', 'Андріїшин', 1740, 882, '4674705210'),
('Єфрем', 'Ільєнко', 1393, 591, '2828151719'),
('Леопольд', 'Алексійчук', 1142, 721, '7810330259'),
('Аарон', 'Гайдабура', 2354, 46, '3580005898'),
('Helen', 'Williams', 2000, 700, '5551234567');

INSERT INTO Wards (Name, DepartmentId, Floor)
VALUES 
('101', 1, 1),
('102', 1, 1),
('201', 2, 2),
('202', 2, 2),
('301', 3, 3),
('302', 3, 3),
('401', 4, 4),
('402', 4, 4),
('501', 5, 5),
('502', 5, 5),
('603', 6, 6),
('604', 6, 6),
('701', 7, 7),
('702', 7, 7),
('801', 8, 8),
('802', 8, 8),
('IT-01', 11, 1),
('IT-02', 11, 2),
('IT-03', 11, 3);

INSERT INTO DoctorsSpecializations (DoctorId, SpecializationId)
VALUES 
(1, 1),
(1, 7),
(2, 2),
(3, 3),
(4, 2),
(5, 4),
(6, 5),
(7, 6),
(8, 7),
(9, 2),
(10, 1),
(11, 3),
(12, 4),
(13, 5),
(14, 8),
(15, 1);

INSERT INTO Vacations (DoctorId, StartDate, EndDate)
VALUES 
(1, '2026-01-15', '2026-01-30'),
(3, '2026-02-01', '2026-02-15'),
(5, '2026-03-10', '2026-03-25');

INSERT INTO Donations (Amount, [Date], DepartmentId, SponsorId)
VALUES 
(150000, '2026-02-01', 1, 1),
(200000, '2026-01-15', 2, 1),
(50000, '2026-02-10', 3, 2),
(80000, '2026-02-05', 4, 3),
(120000, '2026-02-20', 5, 4),
(30000, '2026-02-18', 6, 5),
(90000, '2026-01-20', 7, 1),
(110000, '2026-02-12', 8, 2),
(75000, '2026-02-08', 9, 3),
(95000, '2026-02-15', 10, 4),
(105000, '2026-02-22', 11, 1);

INSERT INTO Examinations (Name, DayOfWeek, StartTime, EndTime)
VALUES 
('Voluptatibus Test', 2, '12:00:00', '13:00:00'),
('Recusandae Test', 7, '11:00:00', '12:00:00'),
('Aliquid Test', 4, '12:00:00', '14:00:00'),
('Delectus Test', 6, '13:00:00', '14:00:00'),
('Consectetur Test', 3, '13:00:00', '14:00:00'),
('Non Test', 6, '12:00:00', '13:00:00'),
('Exercitationem Test', 5, '10:00:00', '11:00:00'),
('Temporibus Test', 2, '15:00:00', '17:00:00'),
('Aliquam Test', 3, '11:00:00', '13:00:00'),
('Vitae Test', 7, '08:00:00', '09:00:00');

INSERT INTO DoctorsExaminations (DoctorId, ExaminationId, WardId)
VALUES 
(1, 1, 1),
(2, 2, 4),
(3, 3, 5),
(4, 4, 8),
(5, 5, 10),
(6, 6, 12),
(7, 7, 14),
(8, 8, 16),
(9, 9, 2),
(10, 10, 3),
(15, 1, 17),
(15, 2, 18),
(15, 3, 19);

INSERT INTO ExaminationsDiseases (ExaminationId, DiseaseId, [Date])
VALUES 
(1, 1, '2026-02-20'),
(2, 2, '2026-02-18'),
(3, 3, '2026-02-15'),
(4, 4, '2026-02-10'),
(5, 5, '2026-02-12'),
(6, 6, '2026-02-08'),
(7, 7, '2026-02-05'),
(8, 8, '2026-02-03'),
(9, 2, '2026-02-22'),
(10, 3, '2026-02-25');

-- 1. Вивести повні імена лікарів та їх спеціалізації
SELECT Doctors.Name + ' ' + Doctors.Surname AS [Full Name], Specializations.Name
FROM Doctors, DoctorsSpecializations, Specializations
WHERE Doctors.Id = DoctorsSpecializations.DoctorId 
  AND DoctorsSpecializations.SpecializationId = Specializations.Id;

-- 2. Вивести прізвища та зарплати лікарів, які не перебувають у відпустці
SELECT Doctors.Surname, Doctors.Salary + Doctors.Premium AS [Total Salary]
FROM Doctors
WHERE Doctors.Id NOT IN (
    SELECT DoctorId FROM Vacations
    WHERE GETDATE() BETWEEN StartDate AND EndDate
);

-- 3. Вивести назви палат у відділенні "Intensive Treatment"
SELECT Wards.Name 
FROM Wards, Departments
WHERE Wards.DepartmentId = Departments.Id
  AND Departments.Name = 'Intensive Treatment';

-- 4. Вивести назви відділень без повторень, спонсоровані "Umbrella Corporation"
SELECT DISTINCT Departments.Name
FROM Departments, Donations, Sponsors
WHERE Departments.Id = Donations.DepartmentId
  AND Donations.SponsorId = Sponsors.Id
  AND Sponsors.Name = 'Umbrella Corporation';

-- 5. Вивести всі пожертвування за останній місяць
SELECT Departments.Name, Sponsors.Name, Donations.Amount, Donations.[Date]
FROM Donations, Departments, Sponsors
WHERE Donations.DepartmentId = Departments.Id
  AND Donations.SponsorId = Sponsors.Id
  AND DATEDIFF(MONTH, Donations.[Date], GETDATE()) = 0;

-- 6. Вивести прізвища лікарів та відділення (лише будні дні)
SELECT DISTINCT Doctors.Surname, Departments.Name
FROM Doctors, DoctorsExaminations, Examinations, Wards, Departments
WHERE Doctors.Id = DoctorsExaminations.DoctorId
  AND DoctorsExaminations.ExaminationId = Examinations.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND Wards.DepartmentId = Departments.Id
  AND Examinations.DayOfWeek BETWEEN 2 AND 6;

-- 7. Вивести назви палат та корпуси для Helen Williams
SELECT Wards.Name, Departments.Building
FROM Wards, Departments, Doctors, DoctorsExaminations
WHERE Wards.Id = DoctorsExaminations.WardId
  AND Wards.DepartmentId = Departments.Id
  AND Doctors.Id = DoctorsExaminations.DoctorId
  AND Doctors.Name = 'Helen' AND Doctors.Surname = 'Williams';

-- 8. Вивести назви відділень з пожертвуваннями > 100000 та їх лікарів
SELECT DISTINCT Departments.Name, Doctors.Surname
FROM Departments, Donations, Doctors, Wards, DoctorsExaminations
WHERE Departments.Id = Donations.DepartmentId
  AND Donations.Amount > 100000
  AND Wards.DepartmentId = Departments.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND Doctors.Id = DoctorsExaminations.DoctorId;

-- 9. Вивести назви відділень, у яких є лікарі без надбавки
SELECT DISTINCT Departments.Name
FROM Departments, Wards, Doctors, DoctorsExaminations
WHERE Wards.DepartmentId = Departments.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND Doctors.Id = DoctorsExaminations.DoctorId
  AND Doctors.Premium = 0;

-- 10. Вивести назви спеціалізацій для захворювань із ступенем > 3
SELECT DISTINCT Specializations.Name
FROM Specializations, Diseases
WHERE Diseases.Severity > 3;

-- 11. Вивести назви відділень та захворювань за останні півроку
SELECT DISTINCT Departments.Name, Diseases.Name
FROM Departments, Wards, DoctorsExaminations, Examinations, ExaminationsDiseases, Diseases
WHERE Wards.DepartmentId = Departments.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND DoctorsExaminations.ExaminationId = Examinations.Id
  AND Examinations.Id = ExaminationsDiseases.ExaminationId
  AND ExaminationsDiseases.DiseaseId = Diseases.Id
  AND DATEDIFF(MONTH, ExaminationsDiseases.[Date], GETDATE()) <= 6;

-- 12. Вивести назви відділень та палат із заразливих захворювань
SELECT DISTINCT Departments.Name, Wards.Name
FROM Departments, Wards, DoctorsExaminations, Examinations, ExaminationsDiseases, Diseases
WHERE Wards.DepartmentId = Departments.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND DoctorsExaminations.ExaminationId = Examinations.Id
  AND Examinations.Id = ExaminationsDiseases.ExaminationId
  AND ExaminationsDiseases.DiseaseId = Diseases.Id
  AND (Diseases.Name = 'COVID-19' OR Diseases.Name = 'Грип');