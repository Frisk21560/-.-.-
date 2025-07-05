#include <iostream>
#include <Windows.h>
#include "Group.h"

using namespace std;

int main() {
    SetConsoleOutputCP(1251);

    Group* zminna_grupa = nullptr; // покажчик на групу

    int fukciya = -1;

    while (true) {
        cout << "\nМЕНЮ\n";
        cout << "1 - створити нову групу\n";
        cout << "2 - додати студента\n";
        cout << "3 - видалити студента\n";
        cout << "4 - показати студентів\n";
        cout << "0 - вихід\n";
        cout << "Вибери функцію: ";
        cin >> fukciya;
        cin.ignore(); // очищаю буфер

        if (fukciya == 0) {
            break;
        }
        if (fukciya == 1) {
            if (zminna_grupa != nullptr) delete zminna_grupa; // якщо вже є, видаляю
            char buf[100];
            cout << "Введи назву групи: ";
            cin.getline(buf, 100); // зчитую назву
            zminna_grupa = new Group(buf); // створюю групу
            cout << "Групу створено!\n";
        }
        else if (fukciya == 2) {
            if (zminna_grupa == nullptr) {
                cout << "Спочатку створи групу!\n";
                continue;
            }
            char buf[100];
            cout << "Введи ім'я студента: ";
            cin.getline(buf, 100); // зчитую ім'я
            zminna_grupa->DodatyStudent(buf); // додаю студента
            cout << "Студента додано\n";
        }
        else if (fukciya == 3) {
            if (zminna_grupa == nullptr || zminna_grupa->GetKilkist() == 0) {
                cout << "Нема кого видаляти!\n";
                continue;
            }
            int nomer;
            cout << "Введи номер студента для видалення: ";
            cin >> nomer;
            cin.ignore();
            zminna_grupa->VudalutyStudenta(nomer - 1); // видаляю по номеру
            cout << "Якщо був - видалив :)\n";
        }
        else if (fukciya == 4) {
            if (zminna_grupa == nullptr) {
                cout << "Групу ще не створили :(\n";
                continue;
            }
            zminna_grupa->VivestyStudentiv(); // показую всіх студентів
        }
        else {
            cout << "такої функції нема\n";
        }
    }

    if (zminna_grupa != nullptr) delete zminna_grupa; // очищаю в кінці

    cout << "Все, кінець проги, я пішов чай пити\n";
    return 0;
}