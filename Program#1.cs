using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".NET tasks\n");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Choose task:");
                Console.WriteLine("1 - Temperature convert (F <-> C)");
                Console.WriteLine("2 - Show even numbers in range");
                Console.WriteLine("3 - Check Armstrong number");
                Console.WriteLine("0 - Exit");
                Console.Write("Your choice: ");
                string cho = Console.ReadLine();

                switch (cho)
                {
                    case "1":
                        DoTemperature();
                        break;
                    case "2":
                        DoEvenRange();
                        break;
                    case "3":
                        DoArmstrong();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Wrong choice, try again.");
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Bye! Press any key...");
            Console.ReadKey();
        }

        // Task 1
        static void DoTemperature()
        {
            Console.WriteLine("\n--- Temperature converter ---");
            Console.WriteLine("Enter 1 to convert Fahrenheit -> Celsius");
            Console.WriteLine("Enter 2 to convert Celsius -> Fahrenheit");
            Console.Write("Choice: ");
            string ch = Console.ReadLine();

            if (ch != "1" && ch != "2")
            {
                Console.WriteLine("Bad choice, return to menu.");
                return;
            }

            Console.Write("Enter temperature value (number): ");
            string inp = Console.ReadLine();
            double t;
            if (!double.TryParse(inp, out t))
            {
                Console.WriteLine("Bad number input!");
                return;
            }

            if (ch == "1")
            {
                // F -> C
                double c = (t - 32.0) * 5.0 / 9.0;
                Console.WriteLine($"{t} °F -> {Math.Round(c, 2)} °C");
            }
            else
            {
                // C -> F
                double f = t * 9.0 / 5.0 + 32.0;
                Console.WriteLine($"{t} °C -> {Math.Round(f, 2)} °F");
            }
        }

        // Task 2
        static void DoEvenRange()
        {
            Console.WriteLine("\n--- Even numbers in range ---");
            Console.Write("Enter first integer: ");
            string s1 = Console.ReadLine();
            Console.Write("Enter second integer: ");
            string s2 = Console.ReadLine();

            int a, b;
            if (!Int32.TryParse(s1, out a) || !Int32.TryParse(s2, out b))
            {
                Console.WriteLine("Bad integer input!");
                return;
            }

            // normalize
            int start = a;
            int end = b;
            if (start > end)
            {
                // swap like student code
                int tmp = start;
                start = end;
                end = tmp;
            }

            Console.WriteLine($"Range normalized: {start} .. {end}");
            Console.WriteLine("Even numbers:");

            bool any = false;
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    Console.Write(i + " ");
                    any = true;
                }
            }
            if (!any)
            {
                Console.WriteLine("(no even numbers in range)");
            }
            else
            {
                Console.WriteLine(); // new line after list
            }
        }

        // Task 3
        static void DoArmstrong()
        {
            Console.WriteLine("\n--- Armstrong number check ---");
            Console.Write("Enter integer (positive): ");
            string s = Console.ReadLine();
            int n;
            if (!Int32.TryParse(s, out n) || n < 0)
            {
                Console.WriteLine("Bad input, need non-negative integer.");
                return;
            }

            if (IsArmstrong(n))
            {
                Console.WriteLine($"{n} is Armstrong number!");
            }
            else
            {
                Console.WriteLine($"{n} is NOT Armstrong number.");
            }
        }

        // helper: check Armstrong using digit math
        static bool IsArmstrong(int num)
        {
            // simple math
            int tmp = num;
            // count digits
            int digits = 0;
            if (tmp == 0) digits = 1;
            while (tmp > 0)
            {
                digits++;
                tmp /= 10;
            }

            int sum = 0;
            tmp = num;
            while (tmp > 0)
            {
                int d = tmp % 10;
                // power d^digits (use Math.Pow but cast)
                double pw = Math.Pow(d, digits);
                sum += (int)pw; // small cast
                tmp /= 10;
            }

            return sum == num;
        }
    }
}