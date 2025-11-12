#include "Student.h"
#include <iostream>

Student::Student()
    : name("noName"), grades()
{

}

Student::Student(const std::string& name)
    : name(name), grades()
{

}


Student::Student(const std::string& name, const std::vector<int>& grades)
    : name(name), grades(grades)
{
   
}


void Student::addGrade(int g)
{
    grades.push_back(g);
}

std::string Student::getName() const
{
    return name;
}

std::vector<int> Student::getGrades() const
{
    return grades;
}

void Student::print() const
{
    std::cout << "²ìÿ: " << name << " | Îö³íêè: ";
    for (int x : grades) {        
        std::cout << x << " ";
    }
    std::cout << std::endl;
}