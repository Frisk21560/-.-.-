#include "Group.h"
#include <iostream>
#include <algorithm> 
Group::Group()
    : gname("noGroup"), studs()
{

    studs.reserve(4);
}


Group::Group(const std::string& name)
    : gname(name), studs()
{
    studs.reserve(4); 
}

void Group::setName(const std::string& name)
{
    gname = name;
}

std::string Group::getName() const
{
    return gname;
}

void Group::addStudent(const Student& s)
{
    studs.push_back(s); 
    std::cout << "Додав студента. size " << studs.size() << " cap " << studs.capacity() << std::endl;
}

bool Group::removeStudentByName(const std::string& name)
{
    auto it = std::find_if(studs.begin(), studs.end(),
        [&](const Student& st) { return st.getName() == name; });
    if (it != studs.end()) {
        studs.erase(it); 
        std::cout << "Видалив студента " << name << std::endl;
        return true;
    }
    std::cout << "Не знайшов студента " << name << std::endl;
    return false;
}

void Group::printAll() const
{
    std::cout << "Група: " << gname << " | Кількість студентів: " << studs.size() << std::endl;
    for (const auto& s : studs) { 
        s.print();
    }
}

std::size_t Group::size() const
{
    return studs.size();
}