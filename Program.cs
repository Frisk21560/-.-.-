class Program
{
    static void Main(string[] args)
    {
        // 2
        Console.WriteLine(" Reading List Demo ");
        ReadingList myList = new ReadingList();

        Book book1 = new Book("The Hobbit", "J. R. R. Tolkien");
        Book book2 = new Book("Clean Code", "Robert C. Martin");
        Book book3 = new Book("The Hobbit", "J. R. R. Tolkien"); // same as book1

        // add books using Add and operator +
        myList.Add(book1);
        myList = myList + book2; // using operator
        myList += book3; // trying to add duplicate

        Console.WriteLine($"Count: {myList.Count}");
        Console.WriteLine($"Has 'The Hobbit' ? {myList["The Hobbit"]}"); // string indexer -> bool

        // access by indexer int
        for (int i = 0; i < myList.Count; i++)
        {
            Console.WriteLine($"Book {i}: {myList[i]}");
        }

        // remove using operator -
        myList = myList - book1;
        Console.WriteLine("After remove:");
        Console.WriteLine($"Count: {myList.Count}");

        Console.WriteLine();

        // 2
        Console.WriteLine("=== Fraction Demo ===");

        Fraction f1 = new Fraction(1, 2); // 1/2
        Fraction f2 = new Fraction(3, 4); // 3/4

        Fraction sum = f1 + f2;
        Fraction diff = f1 - f2;
        Fraction prod = f1 * f2;
        Fraction quot = f1 / f2;

        Console.WriteLine($"{f1} + {f2} = {sum}");
        Console.WriteLine($"{f1} - {f2} = {diff}");
        Console.WriteLine($"{f1} * {f2} = {prod}");
        Console.WriteLine($"{f1} / {f2} = {quot}");

        Fraction f3 = new Fraction(2, 4);
        Console.WriteLine($"{f3} reduced is {f3.ToReducedString()}");

        Console.WriteLine($"Is {f1} == {new Fraction(2, 4)}? {(f1 == new Fraction(2, 4))}");
        Console.WriteLine($"Is {f3} == {new Fraction(1, 2)}? {(f3 == new Fraction(1, 2))}");

        // small pause
        Console.WriteLine("Done. Press any key to exit.");
        Console.ReadKey();
    }
}