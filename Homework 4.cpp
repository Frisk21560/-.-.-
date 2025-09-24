// Homework 4.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <windows.h>
using namespace std;

class String {
private:
    char* zminna; // тут стрічка
    int rozmir;  

public:
    // Конструктор
    String(const char* tekst = "") {
        if (tekst == nullptr) {
            zminna = nullptr;
            rozmir = 0;
            return;
        }
        rozmir = 0;
        while (tekst[rozmir] != '\0')
            rozmir++;
        zminna = new char[rozmir + 1];
        for (int i = 0; i < rozmir; i++) {
            zminna[i] = tekst[i];
        }
        zminna[rozmir] = '\0';
    }

    // Деструктор
    ~String() {
        delete[] zminna;
    }

    // Метод empty() перевіряє, чи пустий рядок
    bool empty() const {
        return rozmir == 0;
    }

    // Метод substr повертає підрядок [vidkuda; dokuda)
    String substr(int vidkuda, int dokuda) const {
        if (vidkuda < 0) vidkuda = 0;
        if (dokuda > rozmir) dokuda = rozmir;
        if (vidkuda >= dokuda) return String("");
        int novRozmir = dokuda - vidkuda;
        char* novaStr = new char[novRozmir + 1];
        for (int i = 0; i < novRozmir; i++) {
            novaStr[i] = zminna[vidkuda + i];
        }
        novaStr[novRozmir] = '\0';
        String res(novaStr);
        delete[] novaStr;
        return res;
    }

    // Метод insert вставляє tekst на poz
    void insert(int poz, const char* tekst) {
        if (poz < 0) poz = 0;
        if (poz > rozmir) poz = rozmir;
        int addLen = 0;
        while (tekst[addLen] != '\0') addLen++;
        char* novaStr = new char[rozmir + addLen + 1];
        for (int i = 0; i < poz; i++)
            novaStr[i] = zminna[i];
        for (int i = 0; i < addLen; i++)
            novaStr[poz + i] = tekst[i];
        for (int i = poz; i < rozmir; i++)
            novaStr[addLen + i] = zminna[i];
        novaStr[rozmir + addLen] = '\0';
        delete[] zminna;
        zminna = novaStr;
        rozmir += addLen;
    }

    // Метод replace заміняє count символів з poz на tekst
    void replace(int poz, int count, const char* tekst) {
        if (poz < 0) poz = 0;
        if (poz > rozmir) poz = rozmir;
        if (count < 0) count = 0;
        if (poz + count > rozmir) count = rozmir - poz;

        int addLen = 0;
        while (tekst[addLen] != '\0') addLen++;
        int novRozmir = rozmir - count + addLen;
        char* novaStr = new char[novRozmir + 1];

        for (int i = 0; i < poz; i++)
            novaStr[i] = zminna[i];
        for (int i = 0; i < addLen; i++)
            novaStr[poz + i] = tekst[i];
        for (int i = poz + count; i < rozmir; i++)
            novaStr[addLen + i - count] = zminna[i];
        novaStr[novRozmir] = '\0';

        delete[] zminna;
        zminna = novaStr;
        rozmir = novRozmir;
    }

    // Для виводу
    void print() const {
        cout << zminna << endl;
    }
};

// Головна функція
int main() {
    SetConsoleOutputCP(1251);
    // Створюємо стрінг
    String tupoStr("Privit, ya string!");

    // Показуємо рядок
    tupoStr.print();

    // Перевірка empty
    cout << "empty: " << tupoStr.empty() << endl;

    // substr
    String pidstr = tupoStr.substr(2, 7);
    cout << "pidstr: ";
    pidstr.print();

    // insert
    tupoStr.insert(8, "QWER");
    cout << "pislya insert: ";
    tupoStr.print();

    // replace
    tupoStr.replace(2, 5, "ZZZ");
    cout << "pislya replace: ";
    tupoStr.print();

    // Перевірка на пустий
    String porozhniy("");
    cout << "porozhniy empty: " << porozhniy.empty() << endl;

    return 0;
}

