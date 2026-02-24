DROP TABLE IF EXISTS DoctorsExaminations;
DROP TABLE IF EXISTS Interns;
DROP TABLE IF EXISTS Professors;
DROP TABLE IF EXISTS Examinations;
DROP TABLE IF EXISTS Wards;
DROP TABLE IF EXISTS Diseases;
DROP TABLE IF EXISTS Doctors;
DROP TABLE IF EXISTS Departments;

CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Building INT NOT NULL,
    Financing MONEY NOT NULL DEFAULT 0
);

CREATE TABLE Diseases (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Doctors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Surname NVARCHAR(MAX) NOT NULL,
    Salary MONEY NOT NULL
);

CREATE TABLE Wards (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(20) NOT NULL UNIQUE,
    Places INT NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id)
);

CREATE TABLE Examinations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE DoctorsExaminations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id),
    ExaminationId INT NOT NULL FOREIGN KEY REFERENCES Examinations(Id),
    DiseaseId INT NOT NULL FOREIGN KEY REFERENCES Diseases(Id),
    WardId INT NOT NULL FOREIGN KEY REFERENCES Wards(Id),
    [Date] DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Interns (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id)
);

CREATE TABLE Professors (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(Id)
);

INSERT INTO Departments (Name, Building, Financing)
VALUES 
('Cardiology', 1, 50000),
('Ophthalmology', 2, 30000),
('Physiotherapy', 3, 25000),
('Surgery', 5, 60000),
('Neurology', 4, 45000),
('Pediatrics', 5, 35000);

INSERT INTO Diseases (Name)
VALUES 
('Hypertension'),
('Myopia'),
('Arthritis'),
('Fracture'),
('Stroke'),
('Fever'),
('Allergy'),
('Diabetes');

INSERT INTO Doctors (Name, Surname, Salary)
VALUES 
('John', 'Smith', 3000),
('Sarah', 'Johnson', 3500),
('Michael', 'Brown', 2800),
('Emily', 'Davis', 3200),
('David', 'Wilson', 4000),
('Lisa', 'Martinez', 2900),
('Robert', 'Taylor', 3100),
('Jennifer', 'Anderson', 2700);

INSERT INTO Wards (Name, Places, DepartmentId)
VALUES 
('101', 4, 1),
('102', 6, 1),
('201', 5, 2),
('202', 3, 2),
('301', 8, 3),
('302', 4, 3),
('401', 10, 5),
('402', 6, 5),
('501', 16, 4),
('502', 5, 4),
('601', 7, 6);

INSERT INTO Examinations (Name)
VALUES 
('Blood Pressure Test'),
('Eye Examination'),
('X-Ray'),
('ECG'),
('Physical Therapy'),
('Neurological Test');

INSERT INTO DoctorsExaminations (DoctorId, ExaminationId, DiseaseId, WardId, [Date])
VALUES 
(1, 1, 1, 1, '2026-02-24'),
(1, 1, 1, 2, '2026-02-23'),
(2, 2, 2, 3, '2026-02-22'),
(3, 3, 4, 5, '2026-02-20'),
(4, 5, 3, 6, '2026-02-18'),
(5, 4, 5, 8, '2026-02-17'),
(6, 6, 5, 9, '2026-02-16'),
(7, 1, 1, 1, '2026-02-15'),
(2, 2, 2, 4, '2026-02-24'),
(3, 3, 7, 7, '2026-02-21');

INSERT INTO Interns (DoctorId)
VALUES 
(1),
(3),
(6);

INSERT INTO Professors (DoctorId)
VALUES 
(2),
(5),
(7);

-- 1. Вивести назви та місткості палат, розташованих у 5-му корпусі, місткістю 5 і більше місць
SELECT Wards.Name, Wards.Places
FROM Wards, Departments
WHERE Wards.DepartmentId = Departments.Id
  AND Departments.Building = 5
  AND Wards.Places >= 5
  AND Departments.Id IN (
      SELECT DepartmentId FROM Wards WHERE Places > 15
  );

-- 2. Вивести назви відділень, у яких проводилося хоча б одне обстеження за останній тиждень
SELECT DISTINCT Departments.Name
FROM Departments, Wards, DoctorsExaminations
WHERE Wards.DepartmentId = Departments.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND DATEDIFF(DAY, DoctorsExaminations.[Date], GETDATE()) <= 7;

-- 3. Вивести назви захворювань, для яких не проводяться обстеження
SELECT Diseases.Name
FROM Diseases
WHERE Diseases.Id NOT IN (
    SELECT DISTINCT DiseaseId FROM DoctorsExaminations
);

-- 4. Вивести повні імена лікарів, які не проводять обстеження
SELECT Doctors.Name + ' ' + Doctors.Surname AS [Full Name]
FROM Doctors
WHERE Doctors.Id NOT IN (
    SELECT DISTINCT DoctorId FROM DoctorsExaminations
);

-- 5. Вивести назви відділень, у яких не проводяться обстеження
SELECT Departments.Name
FROM Departments
WHERE Departments.Id NOT IN (
    SELECT DISTINCT Wards.DepartmentId FROM Wards, DoctorsExaminations
    WHERE DoctorsExaminations.WardId = Wards.Id
);

-- 6. Вивести прізвища лікарів, які є інтернами
SELECT Doctors.Surname
FROM Doctors, Interns
WHERE Doctors.Id = Interns.DoctorId;

-- 7. Вивести прізвища інтернів, ставки яких більші, ніж ставка хоча б одного з лікарів
SELECT DISTINCT Doctors.Surname
FROM Doctors, Interns
WHERE Doctors.Id = Interns.DoctorId
  AND Doctors.Salary > ANY (
      SELECT Salary FROM Doctors
  );

-- 8. Вивести назви палат, чия місткість більша, ніж місткість кожної палати в 3-му корпусі
SELECT Wards.Name
FROM Wards, Departments
WHERE Wards.DepartmentId = Departments.Id
  AND Wards.Places > ALL (
      SELECT Wards2.Places FROM Wards AS Wards2, Departments AS Depts2
      WHERE Wards2.DepartmentId = Depts2.Id AND Depts2.Building = 3
  );

-- 9. Вивести прізвища лікарів, які проводять обстеження у відділеннях «Ophthalmology» та «Physiotherapy»
SELECT DISTINCT Doctors.Surname
FROM Doctors, DoctorsExaminations, Wards, Departments
WHERE Doctors.Id = DoctorsExaminations.DoctorId
  AND DoctorsExaminations.WardId = Wards.Id
  AND Wards.DepartmentId = Departments.Id
  AND (Departments.Name = 'Ophthalmology' OR Departments.Name = 'Physiotherapy');

-- 10. Вивести назви відділень, у яких працюють інтерни та професори
SELECT DISTINCT Departments.Name
FROM Departments, Wards, Doctors
WHERE Wards.DepartmentId = Departments.Id
  AND EXISTS (
      SELECT * FROM Interns WHERE Interns.DoctorId IN (
          SELECT DoctorsExaminations.DoctorId FROM DoctorsExaminations
          WHERE DoctorsExaminations.WardId = Wards.Id
      )
  )
  AND EXISTS (
      SELECT * FROM Professors WHERE Professors.DoctorId IN (
          SELECT DoctorsExaminations.DoctorId FROM DoctorsExaminations
          WHERE DoctorsExaminations.WardId = Wards.Id
      )
  );

-- 11. Вивести повні імена лікарів та відділення у яких вони проводять обстеження (Financing > 20000)
SELECT DISTINCT Doctors.Name + ' ' + Doctors.Surname AS [Full Name], Departments.Name
FROM Doctors, DoctorsExaminations, Wards, Departments
WHERE Doctors.Id = DoctorsExaminations.DoctorId
  AND DoctorsExaminations.WardId = Wards.Id
  AND Wards.DepartmentId = Departments.Id
  AND Departments.Financing > 20000;

-- 12. Вивести назву відділення, в якому проводить обстеження лікар із найбільшою ставкою
SELECT Departments.Name
FROM Departments, Wards, DoctorsExaminations, Doctors
WHERE Wards.DepartmentId = Departments.Id
  AND DoctorsExaminations.WardId = Wards.Id
  AND Doctors.Id = DoctorsExaminations.DoctorId
  AND Doctors.Salary = (SELECT MAX(Salary) FROM Doctors);

-- 13. Вивести назви захворювань та кількість проведених за ними обстежень
SELECT Diseases.Name, COUNT(*) AS [Кількість обстежень]
FROM Diseases, DoctorsExaminations
WHERE Diseases.Id = DoctorsExaminations.DiseaseId
GROUP BY Diseases.Id, Diseases.Name;