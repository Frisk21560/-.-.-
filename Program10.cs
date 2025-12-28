using System;

namespace ConsoleApp10
{
    class Program
    {
        // Task1
        static Action showTime;
        static Action showDate;
        static Action showDay;

        // predicate to check numbers are positive (used before area calc)
        static Predicate<double> isPosit = (x) => x > 0;

        // Funcs for area calculations
        // triangle area with base and height
        static Func<double, double, double> areaTriangle;
        // rectangle area with width and height
        static Func<double, double, double> areaRectangle;

        static void Main(string[] args)
        {
            // setup Task1 delegates
            showTime = () => Console.WriteLine("Current time is: " + DateTime.Now.ToString("T"));
            showDate = () => Console.WriteLine("Current date is: " + DateTime.Now.ToString("d"));
            showDay = () => Console.WriteLine("Current day of week: " + DateTime.Now.DayOfWeek);

            areaTriangle = (b, h) =>
            {
                if (!isPosit(b) || !isPosit(h))
                {
                    Console.WriteLine("Oops: base or height not positive!");
                    return -1.0;
                }
                return 0.5 * b * h;
            };

            areaRectangle = (w, h) =>
            {
                if (!isPosit(w) || !isPosit(h))
                {
                    Console.WriteLine("Oops: width or height not positive!");
                    return -1.0;
                }
                return w * h;
            };

            Console.WriteLine("Task 1");
            // call actions
            showTime();
            showDate();
            showDay();

            // compute areas with student-like var names
            double my_bass = 5;   // base for triangle 
            double my_heit = 4;   // height
            double triRes = areaTriangle(my_bass, my_heit);
            Console.WriteLine("Triangle area (base " + my_bass + ", height " + my_heit + "): " + triRes);

            double rect_w = 6;
            double rect_h = 3;
            double rectRes = areaRectangle(rect_w, rect_h);
            Console.WriteLine("Rectangle area (w " + rect_w + ", h " + rect_h + "): " + rectRes);

            // try invalid inputs
            double bad = areaTriangle(-1, 2);

            Console.WriteLine("\nTask 2 (Credit Card)");

            // Task2
            // create card
            CreditCard lol_card = new CreditCard("1111-2222-3333-4444", "Ivan Student", DateTime.Now.AddYears(3), "0000", 1000.0, 50.0);

            // subscribe to events
            lol_card.OnDeposit += (msg) => Console.WriteLine("[Event Deposit] " + msg);
            lol_card.OnWithdraw += (msg) => Console.WriteLine("[Event Withdraw] " + msg);
            lol_card.OnCreditStart += (amt) => Console.WriteLine("[Event CreditStart] Used credit amount: " + amt);
            lol_card.OnGoalReached += (msg) => Console.WriteLine("[Event GoalReached] " + msg);
            lol_card.OnPINChanged += (msg) => Console.WriteLine("[Event PINChanged] " + msg);

            // set a goal: when balance reach >= 200
            lol_card.SetGoalAmount(200.0);

            Console.WriteLine("\n-- do deposit 150 --");
            lol_card.Deposit(150.0);

            Console.WriteLine("\n-- try withdraw 100 with wrong pin --");
            bool ok = lol_card.Withdraw(100.0, "1234"); // wrong pin

            Console.WriteLine("\n-- withdraw 30 with correct pin --");
            ok = lol_card.Withdraw(30.0, "0000"); // correct pin

            Console.WriteLine("\n-- withdraw big 500 (will use credit) --");
            ok = lol_card.Withdraw(500.0, "0000"); // should start using credit because balance < amount

            Console.WriteLine("\n-- deposit to reach goal (100) --");
            lol_card.Deposit(200.0); // this should trigger goal reached

            Console.WriteLine("\n-- change PIN with wrong old pin --");
            bool changed = lol_card.ChangePIN("1111", "1234");

            Console.WriteLine("\n-- change PIN with correct old pin --");
            changed = lol_card.ChangePIN("0000", "1234");

            Console.WriteLine("\nProgram finished. Press any key.");
            Console.ReadKey();
        }
    }
}