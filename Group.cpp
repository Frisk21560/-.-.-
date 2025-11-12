#include "Group.h"
#include <iostream>
#include <algorithm> // для find_if

// Конструктор за замовчуванням
Group::Group()
    : gname("noGroup"), studs()
{
    // резервнемо трохи місця, щоб було як у вчителя з reserve
    studs.reserve(4);
}

// Конструктор з іменем групи
Group::Group(const std::string& name)
    : gname(name), studs()
{
    studs.reserve(4); // трохи зарезервували, щоб зменшити перевиділення
}

// Встановлюю імя групи
void Group::setName(const std::string& name)
{
    gname = name;
}

// Повертаю імя групи
std::string Group::getName() const
{
    return gname;
}

// Додаю студента в кінець вектора
void Group::addStudent(const Student& s)
{
    studs.push_back(s); // push_back як у вчителя
    // трохи дебагу для учня, показую розмір і capacity
    std::cout << "Додав студента. size " << studs.size() << " cap " << studs.capacity() << std::endl;
}

// Видаляю першого студента з таким ім'ям
bool Group::removeStudentByName(const std::string& name)
{
    // знаходимо студента за іменем
    auto it = std::find_if(studs.begin(), studs.end(),
        [&](const Student& st) { return st.getName() == name; });
    if (it != studs.end()) {
        studs.erase(it); // erase як у вчителя (erase на позицію)
        std::cout << "Видалив студента " << name << std::endl;
        return true;
    }
    // не знайшли
    std::cout << "Не знайшов студента " << name << std::endl;
    return false;
}

// Виводжу всю інфу по групі
void Group::printAll() const
{
    std::cout << "Група: " << gname << " | Кількість студентів: " << studs.size() << std::endl;
    for (const auto& s : studs) { // range-for виводить кожного студента
        s.print();
    }
}

// Повертає кількість студентів
std::size_t Group::size() const
{
    return studs.size();
}