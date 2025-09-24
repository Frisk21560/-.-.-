// Homework 3.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <Windows.h>
using namespace std;

class String {
private:
    char* zminna; // тут зберігається текст
    int rozmir;   // розмір стрічки

public:
    // Конструктор з C-стрічки
    String(const char* tekst) {
        if (tekst == nullptr) {
            zminna = nullptr;
            rozmir = 0;
            return;
        }
        // шукаємо розмір
        rozmir = 0;
        while (tekst[rozmir] != '\0') {
            rozmir++;
        }
        zminna = new char[rozmir + 1];
        for (int i = 0; i < rozmir; i++) {
            zminna[i] = tekst[i];
        }
        zminna[rozmir] = '\0';
    }

    // Деструктор (щоб не було утечки пам'яті)
    ~String() {
        if (zminna) {
            delete[] zminna;
        }
    }

    // Перевантаження оператору [] для доступу до символів
    char& operator[](int indeks) {
        // Перевіряємо на вихід за межі
        if (indeks < 0 || indeks >= rozmir) {
            cout << "Oshibka: indeks za mezhami!" << endl;
            // Тут повертаємо перший символ, ну бо треба щось повертати
            // В реальному коді краще викидати exception
            return zminna[0];
        }
        return zminna[indeks];
    }

    // Для константних об'єктів
    char operator[](int indeks) const {
        if (indeks < 0 || indeks >= rozmir) {
            cout << "Oshibka: indeks za mezhami!" << endl;
            return zminna[0];
        }
        return zminna[indeks];
    }

    // Проста функція для виводу рядка
    void Vivesti() const {
        if (zminna) {
            cout << zminna << endl;
        }
    }
};

int main() {
    SetConsoleOutputCP(1251);
    // Створюємо свій стрінг
    String str("privit, ya string!");

    // Виводимо весь рядок
    str.Vivesti();

    // Доступ до символу через []
    cout << "Simvol na 3 mistsi: " << str[2] << endl;

    // Змінюємо символ через []
    str[0] = 'P';
    str.Vivesti();

    // Некоректний індекс
    cout << str[99] << endl;

    return 0;
}