// HomeWork 10.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>
#include <vector>
#include <windows.h>
#include "Group.h"
#include "Student.h"
int main()
{
    SetConsoleOutputCP(1251);
    Group g;                // тут наша група
    bool haveGroup = false; // чи створена група
    while (true) {
        // меню
        std::cout << "\nМеню:\n";
        std::cout << "1 - створити нову групу\n";
        std::cout << "2 - додати студента\n";
        std::cout << "3 - видалити студента\n";
        std::cout << "4 - показати всіх студентів\n";
        std::cout << "0 - вихід\n";
        std::cout << "Вибір: ";

        int choice;
        if (!(std::cin >> choice)) { // якщо ввели не число
            std::cin.clear();
            std::cin.ignore(10000, '\n');
            std::cout << "Неправильне введення\n";
            continue;
        }

        if (choice == 0) break;

        if (choice == 1) {
            // створення групи
            std::cout << "Введи назву групи: ";
            std::string name;
            std::cin.ignore();               // прибираю залишок від попереднього вводу
            std::getline(std::cin, name);    // читаю повністю рядок
            g = Group(name);                 // створюю групу
            haveGroup = true;
            std::cout << "Група створена: " << g.getName() << std::endl;
        }
        else if (choice == 2) {
            // додавання студента
            if (!haveGroup) {
                std::cout << "Спочатку створіть групу (опція 1)\n";
                continue;
            }
            std::cout << "Введи ім'я студента: ";
            std::string sname;
            std::cin.ignore();
            std::getline(std::cin, sname);

            // читаємо кількість оцінок
            std::cout << "Скільки оцінок додати? ";
            int n;
            if (!(std::cin >> n) || n < 0) {
                std::cout << "Неправильна кількість\n";
                std::cin.clear();
                std::cin.ignore(10000, '\n');
                continue;
            }
            std::vector<int> grades;         // збираю оцінки тут
            for (int i = 0; i < n; ++i) {
                std::cout << "Оцінка " << (i + 1) << ": ";
                int gval;
                std::cin >> gval;
                grades.push_back(gval);      // додаю як в векторі вчителя
            }

            Student s(sname, grades);       // створюю студента
            g.addStudent(s);                // додаю в групу
        }
        else if (choice == 3) {
            // видалення студента по імені
            if (!haveGroup) {
                std::cout << "Спочатку створіть групу (опція 1)\n";
                continue;
            }
            std::cout << "Введи ім'я студента для видалення: ";
            std::string sname;
            std::cin.ignore();
            std::getline(std::cin, sname);
            g.removeStudentByName(sname);   // пробую видалити
        }
        else if (choice == 4) {
            // показати всіх студентів
            if (!haveGroup) {
                std::cout << "Група не створена\n";
                continue;
            }
            g.printAll(); // виводить всіх студентів
        }
        else {
            std::cout << "Невідомий вибір\n";
        }
    }

    std::cout << "Пока! (закриваю програму)\n";
    return 0;
}