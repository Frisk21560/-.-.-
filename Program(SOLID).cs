using System;
using System.Globalization;

// Завдання 1
public interface IShape
{
    void Draw();
};

public class Circle : IShape
{
    public void Draw()
    {
        Console.WriteLine("Drawing a circle");
    }
};

public class Rectangle : IShape
{
    public void Draw()
    {
        Console.WriteLine("Drawing a rectangle");
    }
};

public abstract class ShapeFactory
{
    public abstract IShape CreateShape();
};

public class CircleFactory : ShapeFactory
{
    public override IShape CreateShape()
    {
        return new Circle();
    }
};

public class RectangleFactory : ShapeFactory
{
    public override IShape CreateShape()
    {
        return new Rectangle();
    }
};

// Завдання 2

// Клас для повернення координат у вигляді об'єкта
public class Coordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
};

// Інтерфейс, який очікує програма
public interface ICoordinatesService
{
    Coordinates GetCoordinates();
};

// Сторонній клас, який повертає координати як рядок
public class GeoLocation
{
    public string GetCoordinatesString()
    {
        return "37.7749, -122.4194";
    }
};

// Адаптер, який перетворює рядок у Coordinates і реалізує ICoordinatesService
public class GeoLocationAdapter : ICoordinatesService
{
    private readonly GeoLocation _geoLocation;

    public GeoLocationAdapter(GeoLocation geoLocation)
    {
        _geoLocation = geoLocation;
    }

    public Coordinates GetCoordinates()
    {
        var s = _geoLocation.GetCoordinatesString();
        // Розділяемо по комі і парсимо, використовуючи InvariantCulture
        var parts = s.Split(',');
        double lat = 0;
        double lng = 0;
        if (parts.Length >= 2)
        {
            double.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out lat);
            double.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out lng);
        }
        return new Coordinates { Latitude = lat, Longitude = lng };
    }
};

public class Program
{
    public static void Main()
    {
        // Демонстрація Завдання 1
        ShapeFactory circleFactory = new CircleFactory();
        IShape circle = circleFactory.CreateShape();
        circle.Draw(); // Drawing a circle

        ShapeFactory rectangleFactory = new RectangleFactory();
        IShape rectangle = rectangleFactory.CreateShape();
        rectangle.Draw(); // Drawing a rectangle

        Console.WriteLine();

        // Демонстрація Завдання 2
        GeoLocation geoLocation = new GeoLocation();
        ICoordinatesService coordinatesService = new GeoLocationAdapter(geoLocation);
        var coordinates = coordinatesService.GetCoordinates();
        Console.WriteLine($"Latitude: {coordinates.Latitude}, Longitude: {coordinates.Longitude}");
        // Вивід: Latitude: 37.7749, Longitude: -122.4194
    }
};