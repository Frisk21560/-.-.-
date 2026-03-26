using EFConsole5.Data;
using EFConsole5.Entities;
using EFConsole5.Services;

namespace EFConsole5
{
    class Program
    {
        static AppDbContext context = new AppDbContext();
        static StudentService studentService = new StudentService(context);
        static GroupService groupService = new GroupService(context);
        static TeacherService teacherService = new TeacherService(context);
        static SubjectService subjectService = new SubjectService(context);

        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            InitializeDatabase();
            MainMenu();
        }

        static void InitializeDatabase()
        {
            if (context.Departments.Any())
            {
                return;
            }

            var itDept = new Department { Name = "IT", Description = "Information Technology" };
            var mathDept = new Department { Name = "Mathematics", Description = "Mathematics Department" };
            var linguistDept = new Department { Name = "Linguist", Description = "Linguistics" };

            var groupIT1 = new Group { Name = "IT-101" };
            var groupMath1 = new Group { Name = "MATH-101" };
            var groupDB1 = new Group { Name = "DB-303" };

            var csharp = new Subject { Name = "C#", Description = ".NET", Department = itDept };
            var databases = new Subject { Name = "Databases", Description = "SQL Server", Department = itDept };
            var algebra = new Subject { Name = "Algebra", Description = "Linear Algebra", Department = mathDept };

            var teacher1 = new Teacher
            {
                FirstName = "John",
                LastName = "Smith",
                BirthDate = new DateTime(1985, 4, 10),
                Salary = 3000,
                Department = itDept,
                Subjects = new List<Subject> { csharp, databases },
                Groups = new List<Group> { groupIT1 }
            };

            var teacher2 = new Teacher
            {
                FirstName = "Anna",
                LastName = "Brown",
                BirthDate = new DateTime(1990, 6, 22),
                Salary = 2800,
                Department = mathDept,
                Subjects = new List<Subject> { algebra },
                Groups = new List<Group> { groupMath1 }
            };

            var student1 = new Student
            {
                FirstName = "Alex",
                LastName = "Johnson",
                Email = "alex@gmail.com",
                BirthDate = new DateTime(2004, 2, 15),
                Group = groupIT1
            };

            var student2 = new Student
            {
                FirstName = "Maria",
                LastName = "Petrov",
                Email = "maria@gmail.com",
                BirthDate = new DateTime(2005, 8, 5),
                Group = groupMath1
            };

            context.Departments.AddRange(itDept, mathDept, linguistDept);
            context.Groups.AddRange(groupIT1, groupMath1, groupDB1);
            context.Subjects.AddRange(csharp, databases, algebra);
            context.Teachers.AddRange(teacher1, teacher2);
            context.Students.AddRange(student1, student2);
            context.SaveChanges();

            Console.WriteLine("База даних ініціалізована!");
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛІННЯ АКАДЕМІЄЮ\n");
                Console.WriteLine("1. Управління студентами");
                Console.WriteLine("2. Управління групами");
                Console.WriteLine("3. Управління викладачами");
                Console.WriteLine("4. Управління предметами");
                Console.WriteLine("5. Вихід");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentMenu();
                        break;
                    case "2":
                        GroupMenu();
                        break;
                    case "3":
                        TeacherMenu();
                        break;
                    case "4":
                        SubjectMenu();
                        break;
                    case "5":
                        context.Dispose();
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void StudentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("СТУДЕНТИ\n");
                Console.WriteLine("1. Додати студента");
                Console.WriteLine("2. Показати всіх студентів");
                Console.WriteLine("3. Пошук студента за ID");
                Console.WriteLine("4. Оновити студента");
                Console.WriteLine("5. Видалити студента");
                Console.WriteLine("6. Повернутись");
                Console.Write("\nВиберіть: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        DisplayAllStudents();
                        break;
                    case "3":
                        SearchStudent();
                        break;
                    case "4":
                        UpdateStudent();
                        break;
                    case "5":
                        DeleteStudent();
                        break;
                    case "6":
                        return;
                }
            }
        }

        static void AddStudent()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ СТУДЕНТА\n");
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine();
            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Дата народження (YYYY-MM-DD): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());
            Console.Write("ID групи: ");
            int groupId = int.Parse(Console.ReadLine());

            studentService.AddStudent(firstName, lastName, email, birthDate, groupId);
            Console.ReadKey();
        }

        static void DisplayAllStudents()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК СТУДЕНТІВ\n");
            var students = studentService.GetAllStudents();
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.Id} | {student.FirstName} {student.LastName} | {student.Email} | Група: {student.Group?.Name}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void SearchStudent()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК СТУДЕНТА\n");
            Console.Write("Введіть ID: ");
            int id = int.Parse(Console.ReadLine());
            var student = studentService.GetStudentById(id);
            if (student != null)
            {
                Console.WriteLine($"\nID: {student.Id}");
                Console.WriteLine($"Ім'я: {student.FirstName} {student.LastName}");
                Console.WriteLine($"Email: {student.Email}");
                Console.WriteLine($"Дата народження: {student.BirthDate:dd.MM.yyyy}");
                Console.WriteLine($"Група: {student.Group?.Name}");
            }
            else
            {
                Console.WriteLine("Студент не знайдено!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void UpdateStudent()
        {
            Console.Clear();
            Console.WriteLine("ОНОВЛЕННЯ СТУДЕНТА\n");
            Console.Write("ID студента: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Нове ім'я: ");
            string firstName = Console.ReadLine();
            Console.Write("Нове прізвище: ");
            string lastName = Console.ReadLine();
            Console.Write("Новий email: ");
            string email = Console.ReadLine();
            Console.Write("Нова дата народження: ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());

            studentService.UpdateStudent(id, firstName, lastName, email, birthDate);
            Console.ReadKey();
        }

        static void DeleteStudent()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ СТУДЕНТА\n");
            Console.Write("ID студента: ");
            int id = int.Parse(Console.ReadLine());
            studentService.DeleteStudent(id);
            Console.ReadKey();
        }

        static void GroupMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ГРУПИ\n");
                Console.WriteLine("1. Додати групу");
                Console.WriteLine("2. Показати всі групи");
                Console.WriteLine("3. Пошук групи за ID");
                Console.WriteLine("4. Оновити групу");
                Console.WriteLine("5. Видалити групу");
                Console.WriteLine("6. Повернутись");
                Console.Write("\nВиберіть: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddGroup();
                        break;
                    case "2":
                        DisplayAllGroups();
                        break;
                    case "3":
                        SearchGroup();
                        break;
                    case "4":
                        UpdateGroup();
                        break;
                    case "5":
                        DeleteGroup();
                        break;
                    case "6":
                        return;
                }
            }
        }

        static void AddGroup()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ ГРУПИ\n");
            Console.Write("Назва групи: ");
            string name = Console.ReadLine();
            groupService.AddGroup(name);
            Console.ReadKey();
        }

        static void DisplayAllGroups()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ГРУП\n");
            var groups = groupService.GetAllGroups();
            foreach (var group in groups)
            {
                Console.WriteLine($"ID: {group.Id} | {group.Name} | Студентів: {group.Students.Count} | Викладачів: {group.Teachers.Count}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void SearchGroup()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК ГРУПИ\n");
            Console.Write("Введіть ID: ");
            int id = int.Parse(Console.ReadLine());
            var group = groupService.GetGroupById(id);
            if (group != null)
            {
                Console.WriteLine($"\nID: {group.Id}");
                Console.WriteLine($"Назва: {group.Name}");
                Console.WriteLine($"Студентів: {group.Students.Count}");
                Console.WriteLine($"Викладачів: {group.Teachers.Count}");
            }
            else
            {
                Console.WriteLine("Група не знайдена!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void UpdateGroup()
        {
            Console.Clear();
            Console.WriteLine("ОНОВЛЕННЯ ГРУПИ\n");
            Console.Write("ID групи: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Нова назва: ");
            string name = Console.ReadLine();
            groupService.UpdateGroup(id, name);
            Console.ReadKey();
        }

        static void DeleteGroup()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ ГРУПИ\n");
            Console.Write("ID групи: ");
            int id = int.Parse(Console.ReadLine());
            groupService.DeleteGroup(id);
            Console.ReadKey();
        }

        static void TeacherMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ВИКЛАДАЧИ\n");
                Console.WriteLine("1. Додати викладача");
                Console.WriteLine("2. Показати всіх викладачів");
                Console.WriteLine("3. Пошук викладача за ID");
                Console.WriteLine("4. Оновити викладача");
                Console.WriteLine("5. Видалити викладача");
                Console.WriteLine("6. Показати викладачів департаменту");
                Console.WriteLine("7. Призначити предмет викладачу");
                Console.WriteLine("8. Повернутись");
                Console.Write("\nВиберіть: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddTeacher();
                        break;
                    case "2":
                        DisplayAllTeachers();
                        break;
                    case "3":
                        SearchTeacher();
                        break;
                    case "4":
                        UpdateTeacher();
                        break;
                    case "5":
                        DeleteTeacher();
                        break;
                    case "6":
                        GetTeachersByDepartment();
                        break;
                    case "7":
                        AssignSubject();
                        break;
                    case "8":
                        return;
                }
            }
        }

        static void AddTeacher()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ ВИКЛАДАЧА\n");
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine();
            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine();
            Console.Write("Дата народження (YYYY-MM-DD): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Зарплата: ");
            decimal salary = decimal.Parse(Console.ReadLine());
            Console.Write("ID департаменту: ");
            int departmentId = int.Parse(Console.ReadLine());

            teacherService.AddTeacher(firstName, lastName, birthDate, salary, departmentId);
            Console.ReadKey();
        }

        static void DisplayAllTeachers()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ВИКЛАДАЧІВ\n");
            var teachers = teacherService.GetAllTeachers();
            foreach (var teacher in teachers)
            {
                string subjects = teacher.Subjects.Count > 0 ? string.Join(", ", teacher.Subjects.Select(s => s.Name)) : "Немає";
                Console.WriteLine($"ID: {teacher.Id} | {teacher.FirstName} {teacher.LastName} | Зарплата: {teacher.Salary} | Предмети: {subjects}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void SearchTeacher()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК ВИКЛАДАЧА\n");
            Console.Write("Введіть ID: ");
            int id = int.Parse(Console.ReadLine());
            var teacher = teacherService.GetTeacherById(id);
            if (teacher != null)
            {
                Console.WriteLine($"\nID: {teacher.Id}");
                Console.WriteLine($"Ім'я: {teacher.FirstName} {teacher.LastName}");
                Console.WriteLine($"Дата народження: {teacher.BirthDate:dd.MM.yyyy}");
                Console.WriteLine($"Зарплата: {teacher.Salary}");
                Console.WriteLine($"Департамент: {teacher.Department?.Name}");
                Console.WriteLine($"Предмети: {string.Join(", ", teacher.Subjects.Select(s => s.Name))}");
            }
            else
            {
                Console.WriteLine("Викладач не знайдено!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void UpdateTeacher()
        {
            Console.Clear();
            Console.WriteLine("ОНОВЛЕННЯ ВИКЛАДАЧА\n");
            Console.Write("ID викладача: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Нове ім'я: ");
            string firstName = Console.ReadLine();
            Console.Write("Нове прізвище: ");
            string lastName = Console.ReadLine();
            Console.Write("Нова зарплата: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            teacherService.UpdateTeacher(id, firstName, lastName, salary);
            Console.ReadKey();
        }

        static void DeleteTeacher()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ ВИКЛАДАЧА\n");
            Console.Write("ID викладача: ");
            int id = int.Parse(Console.ReadLine());
            teacherService.DeleteTeacher(id);
            Console.ReadKey();
        }

        static void GetTeachersByDepartment()
        {
            Console.Clear();
            Console.WriteLine("ВИКЛАДАЧИ ДЕПАРТАМЕНТУ\n");
            Console.Write("ID департаменту: ");
            int departmentId = int.Parse(Console.ReadLine());
            var teachers = teacherService.GetTeachersByDepartment(departmentId);
            foreach (var teacher in teachers)
            {
                string subjects = teacher.Subjects.Count > 0 ? string.Join(", ", teacher.Subjects.Select(s => s.Name)) : "Немає";
                Console.WriteLine($"ID: {teacher.Id} | {teacher.FirstName} {teacher.LastName} | Предмети: {subjects}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void AssignSubject()
        {
            Console.Clear();
            Console.WriteLine("ПРИЗНАЧЕННЯ ПРЕДМЕТУ\n");
            Console.Write("ID викладача: ");
            int teacherId = int.Parse(Console.ReadLine());
            Console.Write("ID предмету: ");
            int subjectId = int.Parse(Console.ReadLine());
            teacherService.AssignSubjectToTeacher(teacherId, subjectId);
            Console.ReadKey();
        }

        static void SubjectMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ПРЕДМЕТИ\n");
                Console.WriteLine("1. Додати предмет");
                Console.WriteLine("2. Показати всі предмети");
                Console.WriteLine("3. Пошук предмету за ID");
                Console.WriteLine("4. Оновити предмет");
                Console.WriteLine("5. Видалити предмет");
                Console.WriteLine("6. Показати предмети департаменту");
                Console.WriteLine("7. Повернутись");
                Console.Write("\nВиберіть: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddSubject();
                        break;
                    case "2":
                        DisplayAllSubjects();
                        break;
                    case "3":
                        SearchSubject();
                        break;
                    case "4":
                        UpdateSubject();
                        break;
                    case "5":
                        DeleteSubject();
                        break;
                    case "6":
                        GetSubjectsByDepartment();
                        break;
                    case "7":
                        return;
                }
            }
        }

        static void AddSubject()
        {
            Console.Clear();
            Console.WriteLine("ДОДАВАННЯ ПРЕДМЕТУ\n");
            Console.Write("Назва: ");
            string name = Console.ReadLine();
            Console.Write("Опис: ");
            string description = Console.ReadLine();
            Console.Write("ID департаменту: ");
            int departmentId = int.Parse(Console.ReadLine());

            subjectService.AddSubject(name, description, departmentId);
            Console.ReadKey();
        }

        static void DisplayAllSubjects()
        {
            Console.Clear();
            Console.WriteLine("СПИСОК ПРЕДМЕТІВ\n");
            var subjects = subjectService.GetAllSubjects();
            foreach (var subject in subjects)
            {
                Console.WriteLine($"ID: {subject.Id} | {subject.Name} | Департамент: {subject.Department?.Name} | Викладачів: {subject.Teachers.Count}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void SearchSubject()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК ПРЕДМЕТУ\n");
            Console.Write("Введіть ID: ");
            int id = int.Parse(Console.ReadLine());
            var subject = subjectService.GetSubjectById(id);
            if (subject != null)
            {
                Console.WriteLine($"\nID: {subject.Id}");
                Console.WriteLine($"Назва: {subject.Name}");
                Console.WriteLine($"Опис: {subject.Description}");
                Console.WriteLine($"Департамент: {subject.Department?.Name}");
                Console.WriteLine($"Викладачі: {string.Join(", ", subject.Teachers.Select(t => $"{t.FirstName} {t.LastName}"))}");
            }
            else
            {
                Console.WriteLine("Предмет не знайдено!");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }

        static void UpdateSubject()
        {
            Console.Clear();
            Console.WriteLine("ОНОВЛЕННЯ ПРЕДМЕТУ\n");
            Console.Write("ID предмету: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Нова назва: ");
            string name = Console.ReadLine();
            Console.Write("Новий опис: ");
            string description = Console.ReadLine();

            subjectService.UpdateSubject(id, name, description);
            Console.ReadKey();
        }

        static void DeleteSubject()
        {
            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ ПРЕДМЕТУ\n");
            Console.Write("ID предмету: ");
            int id = int.Parse(Console.ReadLine());
            subjectService.DeleteSubject(id);
            Console.ReadKey();
        }

        static void GetSubjectsByDepartment()
        {
            Console.Clear();
            Console.WriteLine(" ПРЕДМЕТИ ДЕПАРТАМЕНТУ\n");
            Console.Write("ID департаменту: ");
            int departmentId = int.Parse(Console.ReadLine());
            var subjects = subjectService.GetSubjectsByDepartment(departmentId);
            foreach (var subject in subjects)
            {
                Console.WriteLine($"ID: {subject.Id} | {subject.Name} | {subject.Description}");
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}