using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp11
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FindMedian\n");

            // Example 1: odd count numbers
            int[] nums1 = { 5, 2, 9, 1, 6 };
            var med1 = FindMedian(nums1);
            Console.WriteLine($"Nums1 (unsorted): [{string.Join(", ", nums1)}]");
            Console.WriteLine($"Median (expect 5) -> {med1}\n");

            // Example 2: odd count strings
            List<string> strs1 = new List<string> { "apple", "banana", "cherry", "date", "fig" };
            var med2 = FindMedian(strs1);
            Console.WriteLine($"Strs1 (unsorted): [{string.Join(", ", strs1)}]");
            Console.WriteLine($"Median (expect \"cherry\") -> {med2}\n");

            // Example 3: even count numbers
            var nums2 = new List<int> { 4, 1, 7, 9, 3, 8 };
            var med3 = FindMedian(nums2);
            Console.WriteLine($"Nums2 (unsorted): [{string.Join(", ", nums2)}]");
            Console.WriteLine($"Median (expect 5.5) -> {med3}\n");

            // Example 4: even count strings
            string[] strs2 = { "apple", "banana", "cherry", "date" };
            var med4 = FindMedian(strs2);
            Console.WriteLine($"Strs2 (unsorted): [{string.Join(", ", strs2)}]");
            Console.WriteLine($"Median (expect first of (banana,cherry)) -> {med4}\n");

            //empty collection throws
            try
            {
                var emp = new List<int>();
                var r = FindMedian(emp);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Empty collection test -> caught: " + ex.Message);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Generic FindMedian: works for T : IComparable<T>
        // Returns:
        // - if odd count: the middle element (type T)
        // - if even count and numeric T: average as double
        // - if even count and non-numeric (e.g. string): returns first of two central elements (type T)
        static object FindMedian<T>(IEnumerable<T> items) where T : IComparable<T>
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            List<T> list = items.ToList();
            if (list.Count == 0) throw new ArgumentException("Collection is empty");

            // sort using default comparer
            list.Sort();

            int n = list.Count;
            if (n % 2 == 1)
            {
                // odd = middle element
                return list[n / 2];
            }
            else
            {
                // even = two middle elements
                int mid = n / 2;
                T left = list[mid - 1];
                T right = list[mid];

                if (IsNumericType(typeof(T)))
                {
                    // convert to double and return average (double)
                    double a = Convert.ToDouble(left);
                    double b = Convert.ToDouble(right);
                    return (a + b) / 2.0;
                }
                else
                {
                    // for strings (or other comparables) return first of two central
                    return left;
                }
            }
        }

        // helper to detect numeric types
        static bool IsNumericType(Type t)
        {
            TypeCode tc = Type.GetTypeCode(t);
            switch (tc)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }
    }
}