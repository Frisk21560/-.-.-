#pragma once
#include "Student.h"

// Клас група
class Group {
private:
    char* nazva; 
    Student* massiv; 
    int kilkist; 
public:
    Group(); // конструктор
    Group(const char* zmina); // конструктор з назвою
    Group(const Group& zmina); // копіконструктор
    ~Group(); // деструктор

    void SetNazva(const char* zmina); // змінити назву
    char* GetNazva() const; // повертає назву

    void DodatyStudent(const char* imya); // додає студента
    void VudalutyStudenta(int nomer); // видаляє студента
    void VivestyStudentiv() const; // показати студентів

    int GetKilkist() const; // скільки студентів
};