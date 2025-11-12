#include "Student.h"
#include <iostream>

// Конструктор за замовчуванням, імя "noName", пусті оцінки
Student::Student()
    : name("noName"), grades()
{
    // нічого особливого, просто ініціалізація
}

// Конструктор з іменем
Student::Student(const std::string& name)
    : name(name), grades()
{
    // записали імя
}

// Конструктор з іменем і оцінками
Student::Student(const std::string& name, const std::vector<int>& grades)
    : name(name), grades(grades)
{
    // скопіювали все
}

// Додаємо оцінку в вектор оцінок
void Student::addGrade(int g)
{
    grades.push_back(g); // push_back як у вчителя
}

// Повертаємо імя
std::string Student::getName() const
{
    return name;
}

// Повертаємо копію вектору оцінок
std::vector<int> Student::getGrades() const
{
    return grades;
}

// Виводимо імя і всі оцінки
void Student::print() const
{
    std::cout << "Імя: " << name << " | Оцінки: ";
    for (int x : grades) {          // перебираємо оцінки як у вчителя (range-for)
        std::cout << x << " ";
    }
    std::cout << std::endl;
}