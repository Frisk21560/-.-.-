// Homework 5.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <windows.h>
using namespace std;

// Базовий клас Домашня тварина
class HomePet {
protected:
    string name;
    int age;
public:
    HomePet(string name, int age) : name(name), age(age) {}
    virtual void PrintInfo() {
        cout << "Name: " << name << endl;
        cout << "Age: " << age << endl;
    }
    virtual void MakeSound() {
        cout << "Pet sound" << endl;
    }
};

// Похідний клас Собака
class Dog : public HomePet {
private:
    string breed;
public:
    Dog(string name, int age, string breed) : HomePet(name, age), breed(breed) {}
    void PrintInfo() override {
        cout << "Dog\n";
        cout << "Name: " << name << endl;
        cout << "Age: " << age << endl;
        cout << "Breed: " << breed << endl;
    }
    void MakeSound() override {
        cout << "Woof" << endl;
    }
};

// Похідний клас Кішка
class Cat : public HomePet {
private:
    string color;
public:
    Cat(string name, int age, string color) : HomePet(name, age), color(color) {}
    void PrintInfo() override {
        cout << "Cat\n";
        cout << "Name: " << name << endl;
        cout << "Age: " << age << endl;
        cout << "Color: " << color << endl;
    }
    void MakeSound() override {
        cout << "Meow" << endl;
    }
};

// Похідний клас Папуга
class Parrot : public HomePet {
private:
    string country;
public:
    Parrot(string name, int age, string country) : HomePet(name, age), country(country) {}
    void PrintInfo() override {
        cout << "Parrot\n";
        cout << "Name: " << name << endl;
        cout << "Age: " << age << endl;
        cout << "Country: " << country << endl;
    }
    void MakeSound() override {
        cout << "Kra" << endl;
    }
};

int main() {
    SetConsoleOutputCP(1251);
    // Створення об'єктів
    Dog dog("Bobik", 5, "Dvornyaga");
    Cat cat("Myrka", 3, "White");
    Parrot parrot("Kesha", 2, "Brazil");

    // Вивід інформації та звуку
    dog.PrintInfo();
    dog.MakeSound();

    cat.PrintInfo();
    cat.MakeSound();

    parrot.PrintInfo();
    parrot.MakeSound();

    return 0;
}
