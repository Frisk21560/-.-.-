#pragma once

// ���� �������
class Student {
private:
    char* imya; // ��'� ��������
public:
    Student(); // �����������
    Student(const char* zmina); // ����������� � ��'��
    Student(const Student& zmina); // ��������������
    ~Student(); // ����������

    void SetImya(const char* zmina); // ������ ��'�
    char* GetImya() const; // ������� ��'�
};