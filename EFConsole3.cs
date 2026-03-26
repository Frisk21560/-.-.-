using EFConsole3.Data;
using EFConsole3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFConsole3
{
    internal class Program
    {
        static AcademyContext context = new AcademyContext();

        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("СИСТЕМА УПРАВЛІННЯ АКАДЕМІЄЮ");
                Console.WriteLine();
                Console.WriteLine("1. Управління групами");
                Console.WriteLine("2. Управління студентами");
                Console.WriteLine("3. Управління паспортами");
                Console.WriteLine("4. Управління викладачами");
                Console.WriteLine("5. Управління предметами");
                Console.WriteLine("6. Управління кафедрами");
                Console.WriteLine("7. Вихід");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GroupMenu();
                        break;
                    case "2":
                        StudentMenu();
                        break;
                    case "3":
                        PassportMenu();
                        break;
                    case "4":
                        TeacherMenu();
                        break;
                    case "5":
                        SubjectMenu();
                        break;
                    case "6":
                        DepartmentMenu();
                        break;
                    case "7":
                        context.Dispose();
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void GroupMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ ГРУПАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати групу");
                Console.WriteLine("2. Показати всі групи");
                Console.WriteLine("3. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddGroup();
                        break;
                    case "2":
                        DisplayGroups();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddGroup()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОЇ ГРУПИ");
            Console.WriteLine();

            Console.Write("Введіть назву групи (max 10 символів): ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Назва не може бути пустою!");
                Console.ReadKey();
                return;
            }

            if (name.Length > 10)
            {
                Console.WriteLine("Назва перевищує 10 символів!");
                Console.ReadKey();
                return;
            }

            var group = new Group { Name = name };
            context.Groups.Add(group);
            context.SaveChanges();

            Console.WriteLine("Група успішно додана!");
            Console.ReadKey();
        }

        static void DisplayGroups()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ГРУП");
            Console.WriteLine();

            var groups = context.Groups.Include(g => g.Students).ToList();

            if (groups.Count == 0)
            {
                Console.WriteLine("Немає груп в базі даних!");
            }
            else
            {
                foreach (var group in groups)
                {
                    Console.WriteLine($"ID: {group.Id}");
                    Console.WriteLine($"Назва: {group.Name}");
                    Console.WriteLine($"Кількість студентів: {group.Students.Count}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void PassportMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ ПАСПОРТАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати паспорт");
                Console.WriteLine("2. Показати всі паспорти");
                Console.WriteLine("3. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPassport();
                        break;
                    case "2":
                        DisplayPassports();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddPassport()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОГО ПАСПОРТУ");
            Console.WriteLine();

            Console.Write("Введіть номер паспорту (9 цифр): ");
            string passportNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(passportNumber) || passportNumber.Length != 9 || !passportNumber.All(char.IsDigit))
            {
                Console.WriteLine("Номер паспорту повинен містити рівно 9 цифр!");
                Console.ReadKey();
                return;
            }

            var passport = new Passport { PassportNumber = passportNumber };
            context.Passports.Add(passport);
            context.SaveChanges();

            Console.WriteLine("Паспорт успішно додан!");
            Console.ReadKey();
        }

        static void DisplayPassports()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ПАСПОРТІВ");
            Console.WriteLine();

            var passports = context.Passports.Include(p => p.Students).ToList();

            if (passports.Count == 0)
            {
                Console.WriteLine("Немає паспортів в базі даних!");
            }
            else
            {
                foreach (var passport in passports)
                {
                    Console.WriteLine($"ID: {passport.Id}");
                    Console.WriteLine($"Номер: {passport.PassportNumber}");
                    Console.WriteLine($"Кількість студентів: {passport.Students.Count}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void StudentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ СТУДЕНТАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати студента");
                Console.WriteLine("2. Показати всіх студентів");
                Console.WriteLine("3. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        DisplayStudents();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОГО СТУДЕНТА");
            Console.WriteLine();

            Console.Write("Введіть ім'я студента (max 50 символів): ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
            {
                Console.WriteLine("Невірне ім'я!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть email: ");
            string email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                Console.WriteLine("Невірний email!");
                Console.ReadKey();
                return;
            }

            if (context.Students.Any(s => s.Email == email))
            {
                Console.WriteLine("Цей email вже використовується!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть ID групи: ");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                Console.WriteLine("Невірний ID групи!");
                Console.ReadKey();
                return;
            }

            var group = context.Groups.Find(groupId);
            if (group == null)
            {
                Console.WriteLine("Група не знайдена!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть ID паспорту: ");
            if (!int.TryParse(Console.ReadLine(), out int passportId))
            {
                Console.WriteLine("Невірний ID паспорту!");
                Console.ReadKey();
                return;
            }

            var passport = context.Passports.Find(passportId);
            if (passport == null)
            {
                Console.WriteLine("Паспорт не знайдено!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть стипендію: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal scholarship))
            {
                Console.WriteLine("Невірна стипендія!");
                Console.ReadKey();
                return;
            }

            var student = new Student
            {
                Name = name,
                Email = email,
                GroupId = groupId,
                PassportId = passportId,
                Scholarship = scholarship
            };

            context.Students.Add(student);
            context.SaveChanges();

            Console.WriteLine("Студент успішно додан!");
            Console.ReadKey();
        }

        static void DisplayStudents()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК СТУДЕНТІВ");
            Console.WriteLine();

            var students = context.Students
                .Include(s => s.Group)
                .Include(s => s.Passport)
                .ToList();

            if (students.Count == 0)
            {
                Console.WriteLine("Немає студентів в базі даних!");
            }
            else
            {
                foreach (var student in students)
                {
                    Console.WriteLine($"ID: {student.Id}");
                    Console.WriteLine($"Ім'я: {student.Name}");
                    Console.WriteLine($"Email: {student.Email}");
                    Console.WriteLine($"Група: {student.Group?.Name}");
                    Console.WriteLine($"Паспорт: {student.Passport?.PassportNumber}");
                    Console.WriteLine($"Стипендія: {student.Scholarship}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void DepartmentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ КАФЕДРАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати кафедру");
                Console.WriteLine("2. Показати всі кафедри");
                Console.WriteLine("3. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddDepartment();
                        break;
                    case "2":
                        DisplayDepartments();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddDepartment()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОЇ КАФЕДРИ");
            Console.WriteLine();

            Console.Write("Введіть назву кафедри (max 50 символів): ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
            {
                Console.WriteLine("Невірна назва кафедри!");
                Console.ReadKey();
                return;
            }

            var department = new Department { Name = name };
            context.Departments.Add(department);
            context.SaveChanges();

            Console.WriteLine("Кафедра успішно додана!");
            Console.ReadKey();
        }

        static void DisplayDepartments()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК КАФЕДР");
            Console.WriteLine();

            var departments = context.Departments.Include(d => d.Subjects).ToList();

            if (departments.Count == 0)
            {
                Console.WriteLine("Немає кафедр в базі даних!");
            }
            else
            {
                foreach (var department in departments)
                {
                    Console.WriteLine($"ID: {department.Id}");
                    Console.WriteLine($"Назва: {department.Name}");
                    Console.WriteLine($"Кількість предметів: {department.Subjects.Count}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void SubjectMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ ПРЕДМЕТАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати предмет");
                Console.WriteLine("2. Показати всі предмети");
                Console.WriteLine("3. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddSubject();
                        break;
                    case "2":
                        DisplaySubjects();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddSubject()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОГО ПРЕДМЕТУ");
            Console.WriteLine();

            Console.Write("Введіть назву предмету (max 50 символів): ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
            {
                Console.WriteLine("Невірна назва предмету!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть опис предмету (можна залишити пустим): ");
            string description = Console.ReadLine();

            Console.Write("Введіть ID кафедри: ");
            if (!int.TryParse(Console.ReadLine(), out int departmentId))
            {
                Console.WriteLine("Невірний ID кафедри!");
                Console.ReadKey();
                return;
            }

            var department = context.Departments.Find(departmentId);
            if (department == null)
            {
                Console.WriteLine("Кафедра не знайдена!");
                Console.ReadKey();
                return;
            }

            var subject = new Subject
            {
                Name = name,
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                DepartmentId = departmentId
            };

            context.Subjects.Add(subject);
            context.SaveChanges();

            Console.WriteLine("Предмет успішно додан!");
            Console.ReadKey();
        }

        static void DisplaySubjects()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ПРЕДМЕТІВ");
            Console.WriteLine();

            var subjects = context.Subjects
                .Include(s => s.Department)
                .Include(s => s.Teachers)
                .ToList();

            if (subjects.Count == 0)
            {
                Console.WriteLine("Немає предметів в базі даних!");
            }
            else
            {
                foreach (var subject in subjects)
                {
                    Console.WriteLine($"ID: {subject.Id}");
                    Console.WriteLine($"Назва: {subject.Name}");
                    Console.WriteLine($"Опис: {subject.Description ?? "Не вказаний"}");
                    Console.WriteLine($"Кафедра: {subject.Department?.Name}");
                    Console.WriteLine($"Кількість викладачів: {subject.Teachers.Count}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }

        static void TeacherMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ ВИКЛАДАЧАМИ");
                Console.WriteLine();
                Console.WriteLine("1. Додати викладача");
                Console.WriteLine("2. Показати всіх викладачів");
                Console.WriteLine("3. Повернутись в головне меню");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTeacher();
                        break;
                    case "2":
                        DisplayTeachers();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddTeacher()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ НОВОГО ВИКЛАДАЧА");
            Console.WriteLine();

            Console.Write("Введіть ім'я викладача (max 50 символів): ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
            {
                Console.WriteLine("Невірне ім'я викладача!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть зарплату (більше 0): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salary) || salary <= 0)
            {
                Console.WriteLine("Невірна зарплата!");
                Console.ReadKey();
                return;
            }

            var teacher = new Teacher
            {
                Name = name,
                Salary = salary
            };

            context.Teachers.Add(teacher);
            context.SaveChanges();

            Console.WriteLine("Викладач успішно додан!");
            Console.ReadKey();
        }

        static void DisplayTeachers()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ВИКЛАДАЧІВ");
            Console.WriteLine();

            var teachers = context.Teachers.Include(t => t.Subjects).ToList();

            if (teachers.Count == 0)
            {
                Console.WriteLine("Немає викладачів в базі даних!");
            }
            else
            {
                foreach (var teacher in teachers)
                {
                    Console.WriteLine($"ID: {teacher.Id}");
                    Console.WriteLine($"Ім'я: {teacher.Name}");
                    Console.WriteLine($"Зарплата: {teacher.Salary}");
                    Console.WriteLine($"Кількість предметів: {teacher.Subjects.Count}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey();
        }
    }
}