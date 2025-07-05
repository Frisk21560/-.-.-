// Student.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include "Student.h"
#include <iostream>

// рахую довжину строки
int my_strlen(const char* s) {
    int k = 0;
    if (!s) return 0;
    while (s[k] != '\0') k++;
    return k;
}

// копіюю строку
void my_strcpy(char* dst, const char* src) {
    int i = 0;
    while (src && src[i] != '\0') {
        dst[i] = src[i];
        i++;
    }
    dst[i] = '\0';
}

// тут просто нуль
Student::Student() {
    imya = nullptr;
}

// створюю строку для імені
Student::Student(const char* zmina) {
    if (zmina != nullptr) {
        int len = my_strlen(zmina);
        imya = new char[len + 1];
        my_strcpy(imya, zmina);
    }
    else {
        imya = nullptr;
    }
}

// копіюю студента
Student::Student(const Student& zmina) {
    if (zmina.imya != nullptr) {
        int len = my_strlen(zmina.imya);
        imya = new char[len + 1];
        my_strcpy(imya, zmina.imya);
    }
    else {
        imya = nullptr;
    }
}

// видаляю пам'ять
Student::~Student() {
    if (imya != nullptr) delete[] imya;
}

// змінити ім'я
void Student::SetImya(const char* zmina) {
    if (imya != nullptr) {
        delete[] imya;
    }
    if (zmina != nullptr) {
        int len = my_strlen(zmina);
        imya = new char[len + 1];
        my_strcpy(imya, zmina);
    }
    else {
        imya = nullptr;
    }
}

// повертаю ім'я
char* Student::GetImya() const {
    return imya;
}