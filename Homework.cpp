// Homework.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>
#include <Windows.h>

using namespace std;

// 1: шаблонна функція для виводу імені типу
template<typename T>
void printTypeName(const T& value) {
    cout << "Type is: " << typeid(value).name() << endl;
}

// Перевантаження для void pointer
void printTypeName(void* ptr) {
    cout << "Type is: void pointer" << endl;
}

// 2: перетворюємо int у char через reinterpret_cast
void intToCharReinterpret(int number) {
    char* charPtr = reinterpret_cast<char*>(&number);
    cout << "First byte as char (reinterpret_cast): " << *charPtr << endl;

    // Більш очікуване перетворення значенння number - char
    char charViaStatic = static_cast<char>(number);
    cout << "Value converted to char (static_cast): " << charViaStatic << endl;
}

int main() {
    SetConsoleOutputCP(1251);

    // 1
    int num = 42;
    double dbl = 3.14;
    string nameStr = "student";

    cout << "Task 1 examples:" << endl;
    printTypeName(num);
    printTypeName(dbl);
    printTypeName(nameStr);

    void* someptr = nullptr;
    printTypeName(someptr);

    cout << endl;

    // 2
    cout << "Task 2 examples:" << endl;
    int numberForChar = 65; // 65 -> 'A'
    cout << "Original number = " << numberForChar << endl;
    intToCharReinterpret(numberForChar);

    int numberForChar2 = 0x00000061; // младший байт 0x61 -> 'a'
    cout << "Another number = " << numberForChar2 << endl;
    intToCharReinterpret(numberForChar2);

    return 0;
}
