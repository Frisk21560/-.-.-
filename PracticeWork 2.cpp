// PracticeWork 2.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <windows.h>
using namespace std;

// Агрегація
// Клас BookShelf просто зберігає масив книжок, але не створює їх сам.
// Книги можуть існувати окремо від полиці.
class Book {
private:
    string author;
    string title;
public:
    Book(string author, string title) : author(author), title(title) {}
    void Print() const {
        cout << title << " by " << author << endl;
    }
};

class BookShelf {
private:
    Book* books[10];
    int count;
public:
    BookShelf() : count(0) {}

    void AddBook(Book* book) {
        if (count < 10) {
            books[count] = book;
            count++;
        }
    }
    void PrintBooks() {
        for (int i = 0; i < count; i++) {
            books[i]->Print();
        }
    }
};

// Композиція
// Клас Car створює Engine як свою частину, без Engine машина не працює.
class Engine {
private:
    bool incar;
    int hp;
public:
    Engine() : incar(true), hp(100) {}
    Engine(bool incar, int hp) : incar(incar), hp(hp) {}
    bool GetEngine() {
        return incar;
    }
};

class Car {
private:
    Engine engine;
public:
    Car(const Engine& engine) : engine(engine) {}
    void Start() {
        if (!engine.GetEngine()) {
            cout << "Cannot start without engine" << endl;
        }
        else {
            cout << "Engine Started..." << endl;
        }
    }
};

// Наслідування
// Клас Tiger успадковує Cat, можна змінити поведінку.
class Cat {
protected:
    string name;
    string breed;
    int age;
public:
    Cat() : name("Barsyk"), breed("StreetCat"), age(10) {}
    Cat(string name, string breed, int age) : name(name), breed(breed), age(age) {}
    virtual void MakeSound() {
        cout << "Meow" << endl;
    }
    void PrintInfo() {
        cout << "Name: " << name << endl;
        cout << "Breed: " << breed << endl;
        cout << "Age: " << age << endl;
    }
};

class Tiger : public Cat {
public:
    Tiger() {
        name = "Druzhok";
        breed = "Tiger";
        age = 30;
    }
    void MakeSound() override {
        cout << "MEOW" << endl;
    }
};

    int main()
    {
        SetConsoleOutputCP(1251);

        // Агрегація
        cout << "--- Aggregation ---" << endl;
        Book book1("Author1", "Book1");
        Book book2("Author2", "Book2");
        BookShelf shelf;
        shelf.AddBook(&book1);
        shelf.AddBook(&book2);
        shelf.PrintBooks();

        // Композиция
        cout << "\n--- Composition ---" << endl;
        Engine eng(true, 120);
        Car car(eng);
        car.Start();

        // Наследование
        cout << "\n--- Inheritance ---" << endl;
        Cat cat;
        Tiger tiger;
        cat.PrintInfo();
        cat.MakeSound();
        tiger.PrintInfo();
        tiger.MakeSound();

        return 0;
    }