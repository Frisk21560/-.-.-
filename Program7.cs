using System;

namespace Inheritence
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Money class demo (UAH and kopecks). Menu:");

            Money? m1 = null;
            Money? m2 = null;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 - create money m1");
                Console.WriteLine("2 - create money m2");
                Console.WriteLine("3 - add m1 + m2");
                Console.WriteLine("4 - sub m1 - m2");
                Console.WriteLine("5 - m1 * n");
                Console.WriteLine("6 - m1 / n");
                Console.WriteLine("7 - ++m1 (increase by 1 kop)");
                Console.WriteLine("8 - --m1 (decrease by 1 kop)");
                Console.WriteLine("9 - compare m1 and m2");
                Console.WriteLine("0 - quick demo (auto)");
                Console.WriteLine("q - quit");
                Console.Write("Choice: ");
                string? ch = Console.ReadLine();

                try
                {
                    if (string.IsNullOrWhiteSpace(ch)) continue;
                    if (ch == "q") break;

                    switch (ch)
                    {
                        case "1":
                            m1 = ReadMoney("m1");
                            Console.WriteLine("m1 = " + m1);
                            break;
                        case "2":
                            m2 = ReadMoney("m2");
                            Console.WriteLine("m2 = " + m2);
                            break;
                        case "3":
                            EnsureNotNull(m1, "m1"); EnsureNotNull(m2, "m2");
                            var sum = m1 + m2;
                            Console.WriteLine($"{m1} + {m2} = {sum}");
                            break;
                        case "4":
                            EnsureNotNull(m1, "m1"); EnsureNotNull(m2, "m2");
                            var sub = m1 - m2;
                            Console.WriteLine($"{m1} - {m2} = {sub}");
                            break;
                        case "5":
                            EnsureNotNull(m1, "m1");
                            Console.Write("Enter integer multiplier n: ");
                            int n = int.Parse(Console.ReadLine() ?? "1");
                            var mul = m1 * n;
                            Console.WriteLine($"{m1} * {n} = {mul}");
                            break;
                        case "6":
                            EnsureNotNull(m1, "m1");
                            Console.Write("Enter integer divisor d: ");
                            int d = int.Parse(Console.ReadLine() ?? "1");
                            var div = m1 / d;
                            Console.WriteLine($"{m1} / {d} = {div}");
                            break;
                        case "7":
                            EnsureNotNull(m1, "m1");
                            m1 = ++m1;
                            Console.WriteLine("after ++m1: " + m1);
                            break;
                        case "8":
                            EnsureNotNull(m1, "m1");
                            m1 = --m1;
                            Console.WriteLine("after --m1: " + m1);
                            break;
                        case "9":
                            EnsureNotNull(m1, "m1"); EnsureNotNull(m2, "m2");
                            Console.WriteLine($"m1 = {m1}, m2 = {m2}");
                            Console.WriteLine("m1 == m2 ? " + (m1 == m2));
                            Console.WriteLine("m1 != m2 ? " + (m1 != m2));
                            Console.WriteLine("m1 < m2 ? " + (m1 < m2));
                            Console.WriteLine("m1 > m2 ? " + (m1 > m2));
                            break;
                        case "0":
                            QuickDemo();
                            break;
                        default:
                            Console.WriteLine("Unknown option");
                            break;
                    }
                }
                catch (BankruptException ex)
                {
                    Console.WriteLine("BankruptException caught!");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Time: " + ex.Time);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Format problem: " + ex.Message);
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine("Divide by zero: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Other exception: " + ex.Message);
                }
            }

            Console.WriteLine("Bye!");
        }

        static Money ReadMoney(string name)
        {
            Console.Write($"Enter {name} hryvnias (int): ");
            int h = int.Parse(Console.ReadLine() ?? "0");
            Console.Write($"Enter {name} kopecks (int 0..99 maybe >99 ok): ");
            int k = int.Parse(Console.ReadLine() ?? "0");
            var mm = new Money(h, k);
            return mm;
        }

        static void EnsureNotNull(object? o, string name)
        {
            if (o == null) throw new Exception($"{name} is null, create it first");
        }

        static void QuickDemo()
        {
            try
            {
                Console.WriteLine("Quick demo...");

                var a = new Money(10, 50); // 10.50
                var b = new Money(3, 75);  // 3.75
                Console.WriteLine("a = " + a);
                Console.WriteLine("b = " + b);

                var s = a + b;
                Console.WriteLine("a + b = " + s);

                var sub = a - b;
                Console.WriteLine("a - b = " + sub);

                Console.WriteLine("a * 3 = " + (a * 3));
                Console.WriteLine("a / 2 = " + (a / 2));

                Console.WriteLine("++a -> " + (++a));
                Console.WriteLine("--a -> " + (--a));

                // this will cause bankrupt (example)
                Console.WriteLine("Try subtracting bigger from smaller to cause Bankrupt:");
                Console.WriteLine("b - a ->");
                Console.WriteLine(b - a); // b (3.75) - a (10.50) -> exception
            }
            catch (BankruptException bex)
            {
                Console.WriteLine("Caught Bankrupt in quick demo: " + bex.Message);
                Console.WriteLine("Time: " + bex.Time);
            }
        }
    }
}