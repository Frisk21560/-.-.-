using System;
using System.Collections.Generic;

public class ReadingList
{
    private List<Book> _books = new List<Book>();

    public int Count => _books.Count;

    // indexer by integer to get book
    public Book this[int index]
    {
        get
        {
            if (index < 0 || index >= _books.Count)
                throw new IndexOutOfRangeException();
            return _books[index];
        }
        set
        {
            if (index < 0 || index >= _books.Count)
                throw new IndexOutOfRangeException();
            _books[index] = value;
        }
    }

    // indexer by title to check if exists
    public bool this[string title]
    {
        get
        {
            foreach (var b in _books)
            {
                if (b.Title == title) return true;
            }
            return false;
        }
    }

    public void Add(Book book)
    {
        if (!_books.Contains(book))
        {
            _books.Add(book);
        }
        else
        {
            // just ignore duplicates
        }
    }

    public bool Remove(Book book)
    {
        return _books.Remove(book);
    }

    public bool Contains(Book book)
    {
        return _books.Contains(book);
    }

    // operator overloads to add/remove books more student-like
    public static ReadingList operator +(ReadingList list, Book book)
    {
        list.Add(book);
        return list;
    }

    public static ReadingList operator -(ReadingList list, Book book)
    {
        list.Remove(book);
        return list;
    }

    // also convenience compound assignment
    public static ReadingList operator +(ReadingList list, ReadingList other)
    {
        foreach (var b in other._books)
        {
            list.Add(b);
        }
        return list;
    }
}