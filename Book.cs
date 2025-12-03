using System;

public class Book
{
    private string _title;
    private string _author;

    public Book(string title, string author)
    {
        _title = title;
        _author = author;
    }

    public string Title
    {
        get { return _title; }
        set { _title = value ?? ""; }
    }

    public string Author
    {
        get { return _author; }
        set { _author = value ?? ""; }
    }

    public override string ToString()
    {
        return $"{Title} by {Author}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Book b)
        {
            return this.Title == b.Title && this.Author == b.Author;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (Title + "|" + Author).GetHashCode();
    }

    public static bool operator ==(Book a, Book b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(Book a, Book b)
    {
        return !(a == b);
    }
}