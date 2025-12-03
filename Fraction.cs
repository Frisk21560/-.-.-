using System;

public class Fraction
{
    private int _numerator;
    private int _denominator;

    public Fraction(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator; // property checks zero
        // reduce maybe in constructor
        Reduce();
    }

    public int Numerator
    {
        get { return _numerator; }
        set { _numerator = value; }
    }

    public int Denominator
    {
        get { return _denominator; }
        set
        {
            if (value == 0)
            {
                throw new ArgumentException("Denominator cannot be zero");
            }
            _denominator = value;
        }
    }

    // reduce to simplest form
    public void Reduce()
    {
        int a = Math.Abs(_numerator);
        int b = Math.Abs(_denominator);
        if (a == 0)
        {
            _denominator = 1;
            return;
        }
        int g = Gcd(a, b);
        _numerator /= g;
        _denominator /= g;

        // keep sign only in numerator
        if (_denominator < 0)
        {
            _denominator = -_denominator;
            _numerator = -_numerator;
        }
    }

    private static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int t = a % b;
            a = b;
            b = t;
        }
        return Math.Abs(a);
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public string ToReducedString()
    {
        var copy = new Fraction(Numerator, Denominator);
        copy.Reduce();
        return copy.ToString();
    }

    // arithmetic operators
    public static Fraction operator +(Fraction a, Fraction b)
    {
        int num = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
        int den = a.Denominator * b.Denominator;
        var res = new Fraction(num, den);
        res.Reduce();
        return res;
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        int num = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
        int den = a.Denominator * b.Denominator;
        var res = new Fraction(num, den);
        res.Reduce();
        return res;
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        var res = new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        res.Reduce();
        return res;
    }

    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.Numerator == 0) throw new DivideByZeroException("Cannot divide by zero fraction");
        var res = new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        res.Reduce();
        return res;
    }

    // equality operators
    public static bool operator ==(Fraction a, Fraction b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        // cross multiply to compare
        return a.Numerator * b.Denominator == b.Numerator * a.Denominator;
    }

    public static bool operator !=(Fraction a, Fraction b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Fraction f)
        {
            return this == f;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // reduce copy to create stable hash
        var c = new Fraction(Numerator, Denominator);
        c.Reduce();
        return (c.Numerator, c.Denominator).GetHashCode();
    }
}