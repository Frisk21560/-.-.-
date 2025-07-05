#include "Group.h"
#include <iostream>

// ����� ������� ������
int my_strlen_g(const char* s) {
    int k = 0;
    if (!s) return 0;
    while (s[k] != '\0') k++;
    return k;
}

// ����� ������
void my_strcpy_g(char* dst, const char* src) {
    int i = 0;
    while (src && src[i] != '\0') {
        dst[i] = src[i];
        i++;
    }
    dst[i] = '\0';
}

// ������� ����� �����
Group::Group() {
    nazva = nullptr;
    massiv = nullptr;
    kilkist = 0;
}

// ������� ����� � ������
Group::Group(const char* zmina) {
    if (zmina != nullptr) {
        int len = my_strlen_g(zmina);
        nazva = new char[len + 1];
        my_strcpy_g(nazva, zmina);
    }
    else nazva = nullptr;
    massiv = nullptr;
    kilkist = 0;
}

// ����� �����
Group::Group(const Group& zmina) {
    if (zmina.nazva != nullptr) {
        int len = my_strlen_g(zmina.nazva);
        nazva = new char[len + 1];
        my_strcpy_g(nazva, zmina.nazva);
    }
    else nazva = nullptr;
    kilkist = zmina.kilkist;
    if (kilkist > 0) {
        massiv = new Student[kilkist];
        for (int i = 0; i < kilkist; i++) {
            massiv[i] = zmina.massiv[i];
        }
    }
    else {
        massiv = nullptr;
    }
}

// ������� ��� ���'���
Group::~Group() {
    if (nazva != nullptr) delete[] nazva;
    if (massiv != nullptr) delete[] massiv;
}

// ������ ����� �����
void Group::SetNazva(const char* zmina) {
    if (nazva != nullptr) delete[] nazva;
    if (zmina != nullptr) {
        int len = my_strlen_g(zmina);
        nazva = new char[len + 1];
        my_strcpy_g(nazva, zmina);
    }
    else nazva = nullptr;
}

// �������� �����
char* Group::GetNazva() const {
    return nazva;
}

// ����� ��������
void Group::DodatyStudent(const char* imya) {
    Student* noviyMassiv = new Student[kilkist + 1];
    for (int i = 0; i < kilkist; i++) {
        noviyMassiv[i] = massiv[i];
    }
    noviyMassiv[kilkist].SetImya(imya);
    if (massiv != nullptr) delete[] massiv;
    massiv = noviyMassiv;
    kilkist++;
}

// ������� �������� �� ������
void Group::VudalutyStudenta(int nomer) {
    if (nomer < 0 || nomer >= kilkist) {
        std::cout << "������ �������� ����\n";
        return;
    }
    if (kilkist == 1) {
        delete[] massiv;
        massiv = nullptr;
        kilkist = 0;
        return;
    }
    Student* noviyMassiv = new Student[kilkist - 1];
    for (int i = 0, j = 0; i < kilkist; i++) {
        if (i == nomer) continue;
        noviyMassiv[j++] = massiv[i];
    }
    delete[] massiv;
    massiv = noviyMassiv;
    kilkist--;
}

// ������� ��� ��������
void Group::VivestyStudentiv() const {
    if (kilkist == 0) {
        std::cout << "� ���� ���� ��������\n";
        return;
    }
    std::cout << "�������� ����� " << (nazva ? nazva : "��� �����") << ":\n";
    for (int i = 0; i < kilkist; i++) {
        std::cout << i + 1 << ". " << massiv[i].GetImya() << "\n";
    }
}

// ������ ��������
int Group::GetKilkist() const {
    return kilkist;
}