#pragma once
#include "Student.h"

// ���� �����
class Group {
private:
    char* nazva; 
    Student* massiv; 
    int kilkist; 
public:
    Group(); // �����������
    Group(const char* zmina); // ����������� � ������
    Group(const Group& zmina); // ��������������
    ~Group(); // ����������

    void SetNazva(const char* zmina); // ������ �����
    char* GetNazva() const; // ������� �����

    void DodatyStudent(const char* imya); // ���� ��������
    void VudalutyStudenta(int nomer); // ������� ��������
    void VivestyStudentiv() const; // �������� ��������

    int GetKilkist() const; // ������ ��������
};