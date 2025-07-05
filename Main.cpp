#include <iostream>
#include <Windows.h>
#include "Group.h"

using namespace std;

int main() {
    SetConsoleOutputCP(1251);

    Group* zminna_grupa = nullptr; // �������� �� �����

    int fukciya = -1;

    while (true) {
        cout << "\n����\n";
        cout << "1 - �������� ���� �����\n";
        cout << "2 - ������ ��������\n";
        cout << "3 - �������� ��������\n";
        cout << "4 - �������� ��������\n";
        cout << "0 - �����\n";
        cout << "������ �������: ";
        cin >> fukciya;
        cin.ignore(); // ������ �����

        if (fukciya == 0) {
            break;
        }
        if (fukciya == 1) {
            if (zminna_grupa != nullptr) delete zminna_grupa; // ���� ��� �, �������
            char buf[100];
            cout << "����� ����� �����: ";
            cin.getline(buf, 100); // ������ �����
            zminna_grupa = new Group(buf); // ������� �����
            cout << "����� ��������!\n";
        }
        else if (fukciya == 2) {
            if (zminna_grupa == nullptr) {
                cout << "�������� ������ �����!\n";
                continue;
            }
            char buf[100];
            cout << "����� ��'� ��������: ";
            cin.getline(buf, 100); // ������ ��'�
            zminna_grupa->DodatyStudent(buf); // ����� ��������
            cout << "�������� ������\n";
        }
        else if (fukciya == 3) {
            if (zminna_grupa == nullptr || zminna_grupa->GetKilkist() == 0) {
                cout << "���� ���� ��������!\n";
                continue;
            }
            int nomer;
            cout << "����� ����� �������� ��� ���������: ";
            cin >> nomer;
            cin.ignore();
            zminna_grupa->VudalutyStudenta(nomer - 1); // ������� �� ������
            cout << "���� ��� - ������� :)\n";
        }
        else if (fukciya == 4) {
            if (zminna_grupa == nullptr) {
                cout << "����� �� �� �������� :(\n";
                continue;
            }
            zminna_grupa->VivestyStudentiv(); // ������� ��� ��������
        }
        else {
            cout << "���� ������� ����\n";
        }
    }

    if (zminna_grupa != nullptr) delete zminna_grupa; // ������ � ����

    cout << "���, ����� �����, � ���� ��� ����\n";
    return 0;
}