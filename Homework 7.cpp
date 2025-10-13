// Homework 7.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>
#include <Windows.h>

using namespace std;

class Shape {
public:
    virtual void Show() const = 0; // вивести інфу
    virtual void Save(FILE* file) const = 0; // записати у файл
    virtual void Load(FILE* file) = 0; // зчитати з файла
    virtual ~Shape() {}
};

class Square : public Shape {
public:
    float side;
    Square(float s = 0) : side(s) {}

    void Show() const override {
        cout << "Square with side: " << side << endl;
    }
    void Save(FILE* file) const override {
        fprintf(file, "Square %f\n", side);
    }
    void Load(FILE* file) override {
        fscanf(file, "%f", &side);
    }
};

class Rectangle : public Shape {
public:
    float firstSide;
    float secondSide;
    Rectangle(float fs = 0, float ss = 0) : firstSide(fs), secondSide(ss) {}

    void Show() const override {
        cout << "Rectangle with sides: " << firstSide << " and " << secondSide << endl;
    }
    void Save(FILE* file) const override {
        fprintf(file, "Rectangle %f %f\n", firstSide, secondSide);
    }
    void Load(FILE* file) override {
        fscanf(file, "%f %f", &firstSide, &secondSide);
    }
};

class Circle : public Shape {
public:
    float radius;
    Circle(float r = 0) : radius(r) {}

    void Show() const override {
        cout << "Circle with radius: " << radius << endl;
    }
    void Save(FILE* file) const override {
        fprintf(file, "Circle %f\n", radius);
    }
    void Load(FILE* file) override {
        fscanf(file, "%f", &radius);
    }
};

int main()
{
    SetConsoleOutputCP(1251); 
    cout << " Абстрактні фігури\n";

    // Роблю масив фігур просто через Shape* і кількість
    const int size = 3;
    Shape* shapes[size];

    // Додаю фігури у масив
    shapes[0] = new Square(5);
    shapes[1] = new Rectangle(3, 7);
    shapes[2] = new Circle(4);

    // Зберігаю фігури у файл (через FILE*)
    FILE* file = fopen("shapes.txt", "w");
    for (int i = 0; i < size; i++) {
        shapes[i]->Save(file);
    }
    fclose(file);
    cout << "Фігури збережені у файл.\n";

    // Видаляю старі фігури
    for (int i = 0; i < size; i++) {
        delete shapes[i];
    }

    // Читаю фігури з файла у новий масив
    Shape* loadedShapes[size];
    file = fopen("shapes.txt", "r");
    char type[20];
    for (int i = 0; i < size; i++) {
        fscanf(file, "%s", type);
        if (strcmp(type, "Square") == 0) {
            loadedShapes[i] = new Square();
        }
        else if (strcmp(type, "Rectangle") == 0) {
            loadedShapes[i] = new Rectangle();
        }
        else if (strcmp(type, "Circle") == 0) {
            loadedShapes[i] = new Circle();
        }
        loadedShapes[i]->Load(file);
    }
    fclose(file);
    cout << "Фігури зчитані з файла.\n";

    // Виводжу всі фігури на екран
    for (int i = 0; i < size; i++) {
        loadedShapes[i]->Show();
    }

    // Очищаю пам'ять
    for (int i = 0; i < size; i++) {
        delete loadedShapes[i];
    }