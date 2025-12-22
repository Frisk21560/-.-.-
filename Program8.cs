using System;
using Interfaces;

class Program
{
    static void Main()
    {
        var tvRem = new TvRemoteControl();
        tvRem.TurnOn();
        tvRem.SetChannel(5);
        tvRem.SetChannel(0); // bad
        tvRem.TurnOff();

        Console.WriteLine();

        var radRem = new RadioRemoteControl();
        radRem.TurnOn();
        radRem.SetChannel(95);
        radRem.SetChannel(200); // out of range but allowed
        radRem.TurnOff();

        Console.WriteLine("\n--- Validators demo ---\n");

        var ev = new EmailValidator("student@example.com");
        bool eok = ev.Validate();
        Console.WriteLine($"Email valid? {eok}");

        var ev2 = new EmailValidator("bademail@com");
        Console.WriteLine($"Email valid? {ev2.Validate()}");

        Console.WriteLine();

        var pv = new PasswordValidator("abc123");
        Console.WriteLine($"Password valid? {pv.Validate()}");

        var pv2 = new PasswordValidator("short");
        Console.WriteLine($"Password valid? {pv2.Validate()}");
    }
}