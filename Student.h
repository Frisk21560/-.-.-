#pragma once
#ifndef STUDENT_H
#define STUDENT_H

#include <string>
#include <vector>

// Простий студент: імя і список оцінок
class Student {
private:
    std::string name;           // тут ім'я студента
    std::vector<int> grades;    // і тут його оцінки

public:
    Student();                                  // конструктор за замовчуванням
    Student(const std::string& name);           // конструктор з іменем
    Student(const std::string& name, const std::vector<int>& grades); // з іменем і оцінками

    void addGrade(int g);           // додати оцінку
    std::string getName() const;    // отримати імя
    std::vector<int> getGrades() const; // отримати вектор оцінок

    void print() const;             // вивести інфу про студента
};

#endif // STUDENT_H
