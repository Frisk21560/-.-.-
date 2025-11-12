#pragma once
#ifndef STUDENT_H
#define STUDENT_H

#include <string>
#include <vector>
class Student {
private:
    std::string name;          
    std::vector<int> grades;    

public:
    Student();                                  
    Student(const std::string& name);          
    Student(const std::string& name, const std::vector<int>& grades); 

    void addGrade(int g);           
    std::string getName() const;   
    std::vector<int> getGrades() const; 

    void print() const;            
};

#endif 
