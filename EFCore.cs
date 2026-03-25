using EF_Core_Lesson.Data;
using EF_Core_Lesson.Models;
using System;
using System.Linq;

namespace EFConsole
{
    internal class Program
    {
        static SchoolContext context = new SchoolContext();

        static void Main(string[] args)
        {
            // Створення БД та міграцій при першому запуску
            context.Database.EnsureCreated();
            while (true)
            {
                Console.WriteLine("\n УПРАВЛІННЯ ШКОЛОЮ (EF Core)");
                Console.WriteLine("1. Створити студента");
                Console.WriteLine("2. Створити викладача");
                Console.WriteLine("3. Показати всіх студентів");
                Console.WriteLine("4. Показати всіх викладачів");
                Console.WriteLine("5. Видалити студента");
                Console.WriteLine("6. Видалити викладача");
                Console.WriteLine("7. Оновити студента");
                Console.WriteLine("8. Оновити викладача");
                Console.WriteLine("9. Вихід");
                Console.Write("\nВиберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateStudent();
                        break;
                    case "2":
                        CreateTeacher();
                        break;
                    case "3":
                        DisplayAllStudents();
                        break;
                    case "4":
                        DisplayAllTeachers();
                        break;
                    case "5":
                        DeleteStudent();
                        break;
                    case "6":
                        DeleteTeacher();
                        break;
                    case "7":
                        UpdateStudent();
                        break;
                    case "8":
                        UpdateTeacher();
                        break;
                    case "9":
                        context.Dispose();
                        return;
                    default:
                        Console.WriteLine("Невірна опція!");
                        break;
                }
            }
        }

        // СТУДЕНТИ

        static void CreateStudent()
        {
            Console.WriteLine("\n Додавання студента");
            
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine();

            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine();

            Console.Write("Вік: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Невірний вік!");
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine();

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Email = email
            };

            context.Students.Add(student);
            context.SaveChanges();
            Console.WriteLine("Студент успішно додана!");
        }

        static void DisplayAllStudents()
        {

            Console.WriteLine("\nСПИСОК СТУДЕНТІВ");

            var students = context.Students.ToList();

            if (students.Count == 0)
            {
                Console.WriteLine("Немає студентів в базі даних!");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($" ID: {student.Id,-23}");
                Console.WriteLine($" Ім'я: {student.FirstName,-20}");
                Console.WriteLine($" Прізвище: {student.LastName,-15}");
                Console.WriteLine($" Вік: {student.Age,-22}");
                Console.WriteLine($" Email: {student.Email,-19} ");
            }

            Console.WriteLine($"\nВсього студентів: {students.Count}");
        }

        static void DeleteStudent()
        {
            Console.Write("Введіть ID студента для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine(" Невірний ID!");
                return;
            }

            var student = context.Students.Find(id);
            if (student == null)
            {
                Console.WriteLine("Студента не знайдено!");
                return;
            }

            context.Students.Remove(student);
            context.SaveChanges();
            Console.WriteLine("Студент видалена!");
        }

        static void UpdateStudent()
        {
            Console.Write("Введіть ID студента для оновлення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Невірний ID!");
                return;
            }

            var student = context.Students.Find(id);
            if (student == null)
            {
                Console.WriteLine("Студента не знайдено!");
                return;
            }

            Console.Write("Нове ім'я (залишити пустим для пропуску): ");
            string firstName = Console.ReadLine();
            if (!string.IsNullOrEmpty(firstName))
                student.FirstName = firstName;

            Console.Write("Нове прізвище (залишити пустим для пропуску): ");
            string lastName = Console.ReadLine();
            if (!string.IsNullOrEmpty(lastName))
                student.LastName = lastName;

            Console.Write("Новий вік (залишити пустим для пропуску): ");
            string ageStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(ageStr) && int.TryParse(ageStr, out int age))
                student.Age = age;

            Console.Write("Новий email (залишити пустим для пропуску): ");
            string email = Console.ReadLine();
            if (!string.IsNullOrEmpty(email))
                student.Email = email;

            context.SaveChanges();
            Console.WriteLine("Студент оновлена!");
        }

        // ВИКЛАДАЧІ

        static void CreateTeacher()
        {
            Console.WriteLine("\nДодавання викладача");

            Console.Write("Повне ім'я: ");
            string fullName = Console.ReadLine();

            Console.Write("Вік: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Невірний вік!");
                return;
            }

            Console.Write("Зарплата: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
            {
                Console.WriteLine("Невірна зарплата!");
                return;
            }

            var teacher = new Teacher
            {
                FullName = fullName,
                Age = age,
                Salary = salary
            };

            context.Teachers.Add(teacher);
            context.SaveChanges();
            Console.WriteLine("Викладач успішно додана!");
        }

        static void DisplayAllTeachers()
        {
            Console.WriteLine(" СПИСОК ВИКЛАДАЧІВ");

            var teachers = context.Teachers.ToList();

            if (teachers.Count == 0)
            {
                Console.WriteLine("Немає викладачів в базі даних!");
                return;
            }

            foreach (var teacher in teachers)
            {

                Console.WriteLine($" ID: {teacher.Id,-23}");
                Console.WriteLine($" Ім'я: {teacher.FullName,-20}");
                Console.WriteLine($" Вік: {teacher.Age,-22}");
                Console.WriteLine($"Зарплата: {teacher.Salary:C,-13} ");
            }

            Console.WriteLine($"\nВсього викладачів: {teachers.Count}");
        }

        static void DeleteTeacher()
        {
            Console.Write("Введіть ID викладача для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Невірний ID!");
                return;
            }

            var teacher = context.Teachers.Find(id);
            if (teacher == null)
            {
                Console.WriteLine("Викладача не знайдено!");
                return;
            }

            context.Teachers.Remove(teacher);
            context.SaveChanges();
            Console.WriteLine("Викладач видалена!");
        }

        static void UpdateTeacher()
        {
            Console.Write("Введіть ID викладача для оновлення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Невірний ID!");
                return;
            }

            var teacher = context.Teachers.Find(id);
            if (teacher == null)
            {
                Console.WriteLine("Викладача не знайдено!");
                return;
            }

            Console.Write("Нове ім'я (залишити пустим для пропуску): ");
            string fullName = Console.ReadLine();
            if (!string.IsNullOrEmpty(fullName))
                teacher.FullName = fullName;

            Console.Write("Новий вік (залишити пустим для пропуску): ");
            string ageStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(ageStr) && int.TryParse(ageStr, out int age))
                teacher.Age = age;

            Console.Write("Нова зарплата (залишити пустим для пропуску): ");
            string salaryStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(salaryStr) && decimal.TryParse(salaryStr, out decimal salary))
                teacher.Salary = salary;

            context.SaveChanges();
            Console.WriteLine("Викладач оновлена!");
        }
    }
}