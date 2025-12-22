using System;
using Interfaces;

class Program
{
    static void Main()
    {
        Console.WriteLine($"Max generation: {GC.MaxGeneration}");
        Console.WriteLine($"Memory before GC: {GC.GetTotalMemory(false)}");

        Console.WriteLine("\n--- Test finalizer (no Dispose) ---");
        // create book and do not call Dispose = finalizer should run on GC
        var bk1 = new Book("NoDisposeBook", "Some Author", 1999, 123);
        bk1.DisplayInfo();
        bk1 = null; // remove reference
        Console.WriteLine("bk1 set to null, calling GC.Collect()");
        GC.Collect();
        GC.WaitForPendingFinalizers(); // wait finalizers
        Console.WriteLine($"Memory after collect: {GC.GetTotalMemory(false)}");

        Console.WriteLine("\n--- Test Dispose with using (book) ---");
        using (var bk2 = new Book("UsingBook", "Author2", 2005, 200))
        {
            bk2.DisplayInfo();
        } // here Dispose is called for bk2

        Console.WriteLine("\n--- Library test (using) ---");
        using (var lib = new Library())
        {
            lib.AddBook(new Book("B1", "Auth1", 2010, 150));
            lib.AddBook(new Book("B2", "Auth2", 2015, 250));
            lib.ShowAllBooks();

            Console.WriteLine("Leaving using block for library (Dispose will be called)");
        } // library.Dispose() called = disposes inner books

        Console.WriteLine("\nForce another GC to show any remaining finalizers:");
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("\nEnd of program. Bye!");
        Console.WriteLine($"Memory at end: {GC.GetTotalMemory(false)}");
    }
}