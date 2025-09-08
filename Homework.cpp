// Homework.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <Windows.h>
using namespace std;

// тут я роблю клас String
class String {
private:
    char* zminna; 
    int dovzhyna;

public:
    // конструктор по замовчуванню, створює пустий рядок
    String() {
        dovzhyna = 0;
        zminna = new char[1];
        zminna[0] = '\0';
        // тут створюю пустий рядок
    }

    // конструктор з параметром, приймає const char*
    String(const char* str) {
        // рахую довжину
        dovzhyna = 0;
        while (str[dovzhyna] != '\0') {
            dovzhyna++;
        }
        zminna = new char[dovzhyna + 1];
        // копіюю по одному символу
        for (int i = 0; i < dovzhyna; i++) {
            zminna[i] = str[i];
        }
        zminna[dovzhyna] = '\0';
    }

    // конструктор копіювання
    String(const String& other) {
        dovzhyna = other.dovzhyna;
        zminna = new char[dovzhyna + 1];
        for (int i = 0; i < dovzhyna; i++) {
            zminna[i] = other.zminna[i];
        }
        zminna[dovzhyna] = '\0';
        // тут просто копіюю все з іншого String
    }

    // деструктор
    ~String() {
        delete[] zminna;
        // тут видаляю той масив, бо інакше буде утечка пам'яті
    }

    // метод щоб отримати довжину рядка
    int GetDovzhyna() const {
        return dovzhyna;
        // повертаю довжину
    }

    // метод щоб отримати сам рядок
    String GetString() const {
        return *this;
        // повертаю цей рядок як новий String
    }

    // метод щоб отримати рядок як const char*
    const char* GetConstChar() const {
        return zminna;
        // повертаю просто масив символів
    }

    // оператор + для конкатенації рядків
    String operator+(const String& right) const {
        int newLen = dovzhyna + right.dovzhyna;
        char* novaZminna = new char[newLen + 1];
        // копіюю свій рядок
        for (int i = 0; i < dovzhyna; i++) {
            novaZminna[i] = zminna[i];
        }
        // копіюю другий рядок
        for (int i = 0; i < right.dovzhyna; i++) {
            novaZminna[dovzhyna + i] = right.zminna[i];
        }
        novaZminna[newLen] = '\0';
        // тут з'єдную два рядки в один

        String result(novaZminna);
        delete[] novaZminna;
        // створюю новий рядок, а потім видаляю тимчасовий масив
        return result;
    }
};

// функція щоб показати рядок
void printString(const String& str) {
    cout << "рядок: " << str.GetConstChar() << endl;
    // просто виводить рядок на екран
}

int main() {
    SetConsoleOutputCP(1251);

    // створюю пару рядків
    String z1("привіт");
    String z2(" світ!");

    printString(z1); // показую перший
    printString(z2); // показую другий

    // конкатеную рядки
    String z3 = z1 + z2;
    printString(z3); // показую результат

    // показую довжину
    cout << "довжина: " << z3.GetDovzhyna() << endl;

    // отримую рядок як const char*
    cout << "рядок як const char*: " << z3.GetConstChar() << endl;

    // копіюю рядок
    String z4 = z3;
    printString(z4); // показую копію
}
