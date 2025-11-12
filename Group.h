#pragma once
#ifndef GROUP_H
#define GROUP_H

#include <string>
#include <vector>
#include "Student.h"

// Клас група: має імя групи і вектор студентів
class Group {
private:
    std::string gname;           // назва групи
    std::vector<Student> studs;  // вектор студентів, замість динамічного масиву

public:
    Group();                    // конструктор за замовчуванням
    Group(const std::string& name); // конструктор з іменем

    void setName(const std::string& name); // встановити імя групи
    std::string getName() const;           // отримати імя групи

    void addStudent(const Student& s);     // додати студента
    bool removeStudentByName(const std::string& name); // видалити студента по імені

    void printAll() const;                 // вивести інформацію про всіх студентів
    std::size_t size() const;              // кількість студентів
};

#endif // GROUP_H