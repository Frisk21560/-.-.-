#pragma once
#ifndef GROUP_H
#define GROUP_H

#include <string>
#include <vector>
#include "Student.h"

class Group {
private:
    std::string gname;           
    std::vector<Student> studs;  

public:
    Group();                  
    Group(const std::string& name); 

    void setName(const std::string& name); 
    std::string getName() const;           

    void addStudent(const Student& s);   
    bool removeStudentByName(const std::string& name);

    void printAll() const;                 
    std::size_t size() const;             
};

#endif 