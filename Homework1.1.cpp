// Homework1.1.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <Windows.h>
using namespace std;

class String {
private:
    char* zminna; // масив для символів
    int dovzhyna; // довжина рядка

    // функція щоб порахувати довжину рядка
    static int porahuyDovzhynu(const char* str) {
        int k = 0;
        while (str[k] != '\0') {
            k++;
        }
        return k;
    }

public:
    // конструктор за замовчуванням, створює пустий рядок
    String() {
        dovzhyna = 0;
        zminna = new char[1];
        zminna[0] = '\0';
        // тут нічого нема, але масив все одно є
    }

    // конструктор з параметром, приймає const char*
    String(const char* str) {
        dovzhyna = porahuyDovzhynu(str);
        zminna = new char[dovzhyna + 1];
        for (int i = 0; i < dovzhyna; i++) {
            zminna[i] = str[i];
        }
        zminna[dovzhyna] = '\0';
        // тут копіюю символи з str у свій масив
    }

    // конструктор копіювання
    String(const String& other) {
        dovzhyna = other.dovzhyna;
        zminna = new char[dovzhyna + 1];
        for (int i = 0; i < dovzhyna; i++) {
            zminna[i] = other.zminna[i];
        }
        zminna[dovzhyna] = '\0';
        // тут копіюю з іншого об'єкта
    }

    // конструктор переміщення
    String(String&& other) noexcept {
        dovzhyna = other.dovzhyna;
        zminna = other.zminna;
        other.zminna = nullptr;
        other.dovzhyna = 0;
        // тут просто забираю чужий масив і роблю той об'єкт пустим
    }

    // оператор присвоєння з копіюванням
    String& operator=(const String& other) {
        if (this == &other) return *this; // перевіряю на самоприсвоєння
        if (zminna) delete[] zminna;
        dovzhyna = other.dovzhyna;
        zminna = new char[dovzhyna + 1];
        for (int i = 0; i < dovzhyna; i++) {
            zminna[i] = other.zminna[i];
        }
        zminna[dovzhyna] = '\0';
        // тут копіюю символи з іншого рядка
        return *this;
    }

    // оператор присвоєння з переміщенням
    String& operator=(String&& other) noexcept {
        if (this == &other) return *this; // перевіряю на самоприсвоєння
        if (zminna) delete[] zminna;
        dovzhyna = other.dovzhyna;
        zminna = other.zminna;
        other.zminna = nullptr;
        other.dovzhyna = 0;
        // просто забираю чужий масив і роблю той об'єкт пустим
        return *this;
    }

    // деструктор
    ~String() {
        delete[] zminna;
        // тут видаляю пам'ять, щоб не було утечки
    }

    // метод щоб отримати довжину
    int GetDovzhyna() const {
        return dovzhyna;
    }

    // метод щоб отримати масив символів
    const char* GetConstChar() const {
        return zminna;
    }
};

// функція щоб показати рядок
void printString(const String& str) {
    cout << "рядок: " << str.GetConstChar() << endl;
}

int main() {
    SetConsoleOutputCP(1251);

    // створюю рядки
    String z1("привіт");
    printString(z1);

    // копіюю рядок (копіювання)
    String z2 = z1;
    printString(z2);

    // присвоєння з копіюванням
    String z3;
    z3 = z1;
    printString(z3);

    // переміщення через конструктор
    String z4 = std::move(z3);
    printString(z4);

    // присвоєння з переміщенням
    String z5;
    z5 = std::move(z4);
    printString(z5);

    // показую довжину
    cout << "довжина: " << z5.GetDovzhyna() << endl;

}
