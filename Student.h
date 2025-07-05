#pragma once

// Клас студент
class Student {
private:
    char* imya; // ім'я студента
public:
    Student(); // конструктор
    Student(const char* zmina); // конструктор з ім'ям
    Student(const Student& zmina); // копіконструктор
    ~Student(); // деструктор

    void SetImya(const char* zmina); // змінити ім'я
    char* GetImya() const; // повертає ім'я
};