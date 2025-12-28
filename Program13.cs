using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp13
{
    // Employee info
    public class Employee
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }

        public override string ToString()
        {
            return $"{FullName} | {Position} | {Phone} | {Email} | ${Salary}";
        }
    }

    // Firm (company) type
    public class Firm
    {
        public string Name { get; set; }
        public DateTime Founded { get; set; }
        public string Profile { get; set; } // marketing, IT, etc
        public string DirectorFullName { get; set; } // e.g. John White
        public int EmployeesCount { get; set; }
        public string Address { get; set; } // simple address, may contain city
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public override string ToString()
        {
            return $"{Name} | founded {Founded:d} | {Profile} | director {DirectorFullName} | emp={EmployeesCount} | addr={Address}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Student style demo for Firms and Employees (LINQ queries)\n");

            // create some sample firms
            var firms = new List<Firm>()
            {
                new Firm
                {
                    Name = "BestFood Ltd",
                    Founded = DateTime.Now.AddYears(-5).AddDays(-10),
                    Profile = "Food",
                    DirectorFullName = "Alice White",
                    EmployeesCount = 150,
                    Address = "10 Downing St, London",
                    Employees = new List<Employee>
                    {
                        new Employee { FullName="Lionel Messi", Position="Chef", Phone="2312345", Email="lionel@mail.com", Salary=3000 },
                        new Employee { FullName="John Cook", Position="Manager", Phone="2390000", Email="john@bestfood.com", Salary=2500 },
                        new Employee { FullName="Diana Good", Position="Waiter", Phone="2301111", Email="dianna@bestfood.com", Salary=800 }
                    }
                },
                new Firm
                {
                    Name = "MarketPro",
                    Founded = DateTime.Now.AddYears(-1), // less than 2 years
                    Profile = "Marketing",
                    DirectorFullName = "Bob Black",
                    EmployeesCount = 80,
                    Address = "12 Market St, Manchester",
                    Employees = new List<Employee>
                    {
                        new Employee { FullName="Diana Prince", Position="Manager", Phone="4512345", Email="di@marketpro.com", Salary=2200 },
                        new Employee { FullName="Paul Small", Position="Analyst", Phone="2387654", Email="paul@marketpro.com", Salary=1500 }
                    }
                },
                new Firm
                {
                    Name = "ITWhite Solutions",
                    Founded = DateTime.Now.AddDays(-123), // exactly 123 days ago
                    Profile = "IT",
                    DirectorFullName = "Charles Black",
                    EmployeesCount = 250,
                    Address = "50 Tech Rd, London",
                    Employees = new List<Employee>
                    {
                        new Employee { FullName="Lionel Brown", Position="Developer", Phone="2309999", Email="l.brown@itwhite.com", Salary=4000 },
                        new Employee { FullName="Ann Di", Position="Manager", Phone="2310000", Email="di@itwhite.com", Salary=4200 }
                    }
                },
                new Firm
                {
                    Name = "Foodies & Co",
                    Founded = DateTime.Now.AddYears(-3),
                    Profile = "Food",
                    DirectorFullName = "Sandra Green",
                    EmployeesCount = 40,
                    Address = "1 River St, Bristol",
                    Employees = new List<Employee>
                    {
                        new Employee { FullName="Mark One", Position="Chef", Phone="1234567", Email="mark@foodies.com", Salary=1200 }
                    }
                },
                new Firm
                {
                    Name = "Classic Marketing LLC",
                    Founded = DateTime.Now.AddYears(-10),
                    Profile = "Marketing",
                    DirectorFullName = "Peter White",
                    EmployeesCount = 110,
                    Address = "7 King St, London",
                    Employees = new List<Employee>
                    {
                        new Employee { FullName="Diogo Silva", Position="Manager", Phone="2311111", Email="diogo@classic.com", Salary=2800 },
                        new Employee { FullName="Lionel Small", Position="Intern", Phone="2012345", Email="lionel.small@mail.com", Salary=600 }
                    }
                }
            };

            // TASK 1
            Console.WriteLine("TASK 1 (query syntax)");

            // 1. all firms
            var q_all = from f in firms select f;
            PrintHeader("All firms:");
            foreach (var f in q_all) Console.WriteLine(f);

            // 2. firms with word 'Food' in name
            var q_food = from f in firms where f.Name.IndexOf("Food", StringComparison.OrdinalIgnoreCase) >= 0 select f;
            PrintHeader("\nFirms with 'Food' in name:");
            foreach (var f in q_food) Console.WriteLine(f);

            // 3. firms in profile marketing
            var q_mark = from f in firms where f.Profile.Equals("Marketing", StringComparison.OrdinalIgnoreCase) select f;
            PrintHeader("\nFirms in Marketing:");
            foreach (var f in q_mark) Console.WriteLine(f);

            // 4. firms in Marketing OR IT
            var q_markit = from f in firms where f.Profile.Equals("Marketing", StringComparison.OrdinalIgnoreCase) || f.Profile.Equals("IT", StringComparison.OrdinalIgnoreCase) select f;
            PrintHeader("\nFirms in Marketing or IT:");
            foreach (var f in q_markit) Console.WriteLine(f);

            // 5. firms with employees count > 100
            var q_gt100 = from f in firms where f.EmployeesCount > 100 select f;
            PrintHeader("\nFirms with employees > 100:");
            foreach (var f in q_gt100) Console.WriteLine(f);

            // 6. firms with employees count between 100 and 300
            var q_100_300 = from f in firms where f.EmployeesCount >= 100 && f.EmployeesCount <= 300 select f;
            PrintHeader("\nFirms with employees between 100 and 300:");
            foreach (var f in q_100_300) Console.WriteLine(f);

            // 7. firms in London (simple address contains 'London')
            var q_london = from f in firms where f.Address.IndexOf("London", StringComparison.OrdinalIgnoreCase) >= 0 select f;
            PrintHeader("\nFirms in London:");
            foreach (var f in q_london) Console.WriteLine(f);

            // 8. firms where director last name White (check last token = White)
            var q_dir_white = from f in firms let parts = f.DirectorFullName.Split(' ') where parts.Last().Equals("White", StringComparison.OrdinalIgnoreCase) select f;
            PrintHeader("\nFirms with director last name White:");
            foreach (var f in q_dir_white) Console.WriteLine(f);

            // 9. firms founded more than 2 years ago
            var q_old = from f in firms where (DateTime.Now - f.Founded).TotalDays > 365 * 2 select f;
            PrintHeader("\nFirms founded more than 2 years ago:");
            foreach (var f in q_old) Console.WriteLine(f);

            // 10. firms whose founded day passed exactly 123 days ago
            var q_123 = from f in firms where (DateTime.Now - f.Founded).Days == 123 select f;
            PrintHeader("\nFirms with founded day exactly 123 days ago:");
            foreach (var f in q_123) Console.WriteLine(f);

            // 11. firms with director last name Black and firm name contains White
            var q_black_white = from f in firms
                                let last = f.DirectorFullName.Split(' ').Last()
                                where last.Equals("Black", StringComparison.OrdinalIgnoreCase) && f.Name.IndexOf("White", StringComparison.OrdinalIgnoreCase) >= 0
                                select f;
            PrintHeader("\nFirms where director last name Black and name contains 'White':");
            foreach (var f in q_black_white) Console.WriteLine(f);

            // TASK 2
            Console.WriteLine("\nTASK 2 (method syntax)");

            // 1. all firms
            var m_all = firms.Select(f => f);
            PrintHeader("All firms (method):");
            foreach (var f in m_all) Console.WriteLine(f);

            // 2. firms with 'Food' in name
            var m_food = firms.Where(f => f.Name.IndexOf("Food", StringComparison.OrdinalIgnoreCase) >= 0);
            PrintHeader("\nFirms with 'Food' in name (method):");
            foreach (var f in m_food) Console.WriteLine(f);

            // 3. marketing firms
            var m_mark = firms.Where(f => f.Profile.Equals("Marketing", StringComparison.OrdinalIgnoreCase));
            PrintHeader("\nMarketing firms (method):");
            foreach (var f in m_mark) Console.WriteLine(f);

            // 4. marketing or IT
            var m_markit = firms.Where(f => f.Profile.Equals("Marketing", StringComparison.OrdinalIgnoreCase) || f.Profile.Equals("IT", StringComparison.OrdinalIgnoreCase));
            PrintHeader("\nMarketing or IT firms (method):");
            foreach (var f in m_markit) Console.WriteLine(f);

            // 5. employees count > 100
            var m_gt100 = firms.Where(f => f.EmployeesCount > 100);
            PrintHeader("\nFirms with employees > 100 (method):");
            foreach (var f in m_gt100) Console.WriteLine(f);

            // 6. employees between 100 and 300
            var m_100_300 = firms.Where(f => f.EmployeesCount >= 100 && f.EmployeesCount <= 300);
            PrintHeader("\nFirms with employees between 100 and 300 (method):");
            foreach (var f in m_100_300) Console.WriteLine(f);

            // 7. located in London
            var m_london = firms.Where(f => f.Address.IndexOf("London", StringComparison.OrdinalIgnoreCase) >= 0);
            PrintHeader("\nFirms in London (method):");
            foreach (var f in m_london) Console.WriteLine(f);

            // 8. director last name White
            var m_dir_white = firms.Where(f => f.DirectorFullName.Split(' ').Last().Equals("White", StringComparison.OrdinalIgnoreCase));
            PrintHeader("\nFirms with director last name White (method):");
            foreach (var f in m_dir_white) Console.WriteLine(f);

            // 9. founded more than 2 years
            var m_old = firms.Where(f => (DateTime.Now - f.Founded).TotalDays > 365 * 2);
            PrintHeader("\nFirms founded > 2 years (method):");
            foreach (var f in m_old) Console.WriteLine(f);

            // 10. founded exactly 123 days ago
            var m_123 = firms.Where(f => (DateTime.Now - f.Founded).Days == 123);
            PrintHeader("\nFirms with founded day == 123 days (method):");
            foreach (var f in m_123) Console.WriteLine(f);

            // 11. director Black and name contains White
            var m_black_white = firms.Where(f => f.DirectorFullName.Split(' ').Last().Equals("Black", StringComparison.OrdinalIgnoreCase) && f.Name.IndexOf("White", StringComparison.OrdinalIgnoreCase) >= 0);
            PrintHeader("\nFirms where director Black and name contains 'White' (method):");
            foreach (var f in m_black_white) Console.WriteLine(f);

            // TASK 3
            Console.WriteLine("\nTASK 3 (employees queries)");

            // A. get all employees of a specific firm (example: BestFood Ltd)
            var firmName = "BestFood Ltd";
            var emp_of_firm = firms.Where(f => f.Name.Equals(firmName, StringComparison.OrdinalIgnoreCase)).SelectMany(f => f.Employees).ToList();
            PrintHeader($"\nEmployees of firm '{firmName}':");
            emp_of_firm.ForEach(e => Console.WriteLine(e));

            // B. employees of a firm with salary > given (e.g. >2000) for BestFood Ltd
            double salaryLimit = 2000;
            var emp_high = firms.Where(f => f.Name.Equals(firmName, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(f => f.Employees)
                                .Where(e => e.Salary > salaryLimit)
                                .ToList();
            PrintHeader($"\nEmployees of '{firmName}' with salary > {salaryLimit}:");
            emp_high.ForEach(e => Console.WriteLine(e));

            // C. employees of all firms with position 'Manager'
            var managers = firms.SelectMany(f => f.Employees).Where(e => e.Position.Equals("Manager", StringComparison.OrdinalIgnoreCase)).ToList();
            PrintHeader("\nAll Managers across firms:");
            managers.ForEach(e => Console.WriteLine(e));

            // D. employees with phone starts with '23'
            var phone23 = firms.SelectMany(f => f.Employees).Where(e => e.Phone != null && e.Phone.StartsWith("23")).ToList();
            PrintHeader("\nEmployees with phone starting '23':");
            phone23.ForEach(e => Console.WriteLine(e));

            // E. employees with Email starts with 'di' (case-insensitive)
            var emailDi = firms.SelectMany(f => f.Employees).Where(e => e.Email != null && e.Email.StartsWith("di", StringComparison.OrdinalIgnoreCase)).ToList();
            PrintHeader("\nEmployees with email starting 'di':");
            emailDi.ForEach(e => Console.WriteLine(e));

            // F. employees with first name Lionel (check first token)
            var lionels = firms.SelectMany(f => f.Employees).Where(e => e.FullName.Split(' ')[0].Equals("Lionel", StringComparison.OrdinalIgnoreCase)).ToList();
            PrintHeader("\nEmployees with name Lionel:");
            lionels.ForEach(e => Console.WriteLine(e));

            Console.WriteLine("\nDemo finished. Press any key to exit...");
            Console.ReadKey();
        }

        static void PrintHeader(string s)
        {
            Console.WriteLine(s);
        }
    }
}