using System;

namespace Inheritence
{
    public class Money
    {
        // public properties
        public int Hryvnias { get; private set; }
        public int Kopecks { get; private set; }

        // internal total kopecks
        private long TotalKopecks => (long)Hryvnias * 100 + Kopecks;

        // ctor - normalise kopecks and forbids negative total
        public Money(int h, int k)
        {
            long total = (long)h * 100 + k;
            if (total < 0)
            {
                throw new BankruptException("Bankrupt: money cannot be negative in constructor");
            }

            // normalise
            Hryvnias = (int)(total / 100);
            Kopecks = (int)(total % 100);
        }

        // convenience ctor from total kopecks (private)
        private Money(long totalKopecks)
        {
            if (totalKopecks < 0)
                throw new BankruptException("Bankrupt: operation produced negative money");

            Hryvnias = (int)(totalKopecks / 100);
            Kopecks = (int)(totalKopecks % 100);
        }

        // override ToString
        public override string ToString()
        {
            return $"{Hryvnias} UAH {Kopecks:00} kop";
        }

        // + operator
        public static Money operator +(Money a, Money b)
        {
            return new Money(a.TotalKopecks + b.TotalKopecks);
        }

        // - operator
        public static Money operator -(Money a, Money b)
        {
            long res = a.TotalKopecks - b.TotalKopecks;
            if (res < 0)
                throw new BankruptException("Bankrupt: subtraction produced negative sum");
            return new Money(res);
        }

        // * operator (multiply by integer)
        public static Money operator *(Money a, int m)
        {
            long res = a.TotalKopecks * m;
            if (res < 0)
                throw new BankruptException("Bankrupt: multiplication produced negative sum");
            return new Money(res);
        }

        public static Money operator *(int m, Money a) => a * m;

        // / operator 
        public static Money operator /(Money a, int d)
        {
            if (d == 0)
                throw new DivideByZeroException("Division by zero is not allowed");
            long res = a.TotalKopecks / d; // integer division, kopecks truncated
            if (res < 0)
                throw new BankruptException("Bankrupt: division produced negative sum");
            return new Money(res);
        }

        // ++ and -- increase/decrease by 1 kopeck
        public static Money operator ++(Money a)
        {
            return new Money(a.TotalKopecks + 1);
        }

        public static Money operator --(Money a)
        {
            long res = a.TotalKopecks - 1;
            if (res < 0)
                throw new BankruptException("Bankrupt: decrement produced negative sum");
            return new Money(res);
        }

        // comparison operators
        public static bool operator <(Money a, Money b) => a.TotalKopecks < b.TotalKopecks;
        public static bool operator >(Money a, Money b) => a.TotalKopecks > b.TotalKopecks;
        public static bool operator ==(Money a, Money b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.TotalKopecks == b.TotalKopecks;
        }
        public static bool operator !=(Money a, Money b) => !(a == b);

        // override Equals and GetHashCode when ==/!= overloaded
        public override bool Equals(object? obj)
        {
            if (obj is Money m)
                return this.TotalKopecks == m.TotalKopecks;
            return false;
        }

        public override int GetHashCode()
        {
            return TotalKopecks.GetHashCode();
        }
    }
}