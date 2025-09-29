// Homework 4.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <windows.h>
using namespace std;

// Двозв'язний список
class DoublyLinkedList {
    // Внутрішній клас для вузла списку
    class Node {
    public:
        int value;   
        Node* prev;  
        Node* next;   

        Node(int val) : value(val), prev(nullptr), next(nullptr) {}
    };

    Node* head; 
    Node* tail; 
    int count;  

public:
    // Конструктор
    DoublyLinkedList() : head(nullptr), tail(nullptr), count(0) {}

    // Деструктор
    ~DoublyLinkedList() {
        clear();
    }

    // 1. Додати елемент на початок
    void push_front(int value) {
        Node* newNode = new Node(value);
        newNode->next = head;
        if (head) head->prev = newNode;
        head = newNode;
        if (!tail) tail = newNode;
        count++;
    }

    // 2. Додати елемент в кінець
    void push_back(int value) {
        Node* newNode = new Node(value);
        newNode->prev = tail;
        if (tail) tail->next = newNode;
        tail = newNode;
        if (!head) head = newNode;
        count++;
    }

    // 3. Видалити елемент з початку
    void pop_front() {
        if (!head) return;
        Node* temp = head;
        head = head->next;
        if (head) head->prev = nullptr;
        else tail = nullptr; // якщо був останній елемент
        delete temp;
        count--;
    }

    // 4. Видалити елемент з кінця
    void pop_back() {
        if (!tail) return;
        Node* temp = tail;
        tail = tail->prev;
        if (tail) tail->next = nullptr;
        else head = nullptr;
        delete temp;
        count--;
    }

    // 5. Вставити елемент у позицію (0 — це початок)
    void insert(int position, int value) {
        if (position < 0 || position > count) return;
        if (position == 0) {
            push_front(value);
            return;
        }
        if (position == count) {
            push_back(value);
            return;
        }
        Node* current = head;
        for (int i = 0; i < position - 1; ++i)
            current = current->next;
        Node* newNode = new Node(value);
        newNode->next = current->next;
        newNode->prev = current;
        current->next->prev = newNode;
        current->next = newNode;
        count++;
    }

    // 6. Видалити елемент з позиції
    void erase(int position) {
        if (position < 0 || position >= count) return;
        if (position == 0) {
            pop_front();
            return;
        }
        if (position == count - 1) {
            pop_back();
            return;
        }
        Node* current = head;
        for (int i = 0; i < position; ++i)
            current = current->next;
        current->prev->next = current->next;
        current->next->prev = current->prev;
        delete current;
        count--;
    }

    // 7. Пошук першого елемента зі значенням value (повертає позицію або -1)
    int find(int value) {
        Node* current = head;
        int idx = 0;
        while (current) {
            if (current->value == value) return idx;
            current = current->next;
            idx++;
        }
        return -1;
    }

    // 8. Очищення списку
    void clear() {
        while (head) {
            pop_front();
        }
    }

    // 9. Кількість елементів
    int size() const {
        return count;
    }

    // 10. Чи порожній список?
    bool empty() const {
        return count == 0;
    }

    // 11. Вивід елементів від голови до хвоста
    void print_forward() const {
        Node* current = head;
        cout << "Список (з голови): ";
        while (current) {
            cout << current->value << " ";
            current = current->next;
        }
        cout << endl;
    }

    // 12. Вивід елементів від хвоста до голови
    void print_backward() const {
        Node* current = tail;
        cout << "Список (з хвоста): ";
        while (current) {
            cout << current->value << " ";
            current = current->prev;
        }
        cout << endl;
    }
};

// Тестування
int main() {
    SetConsoleOutputCP(1251);
    SetConsoleCP(1251);

    DoublyLinkedList list;

    list.push_back(5);
    list.push_front(2);
    list.push_back(7);
    list.insert(1, 3); 

    list.print_forward();    
    list.print_backward();   

    cout << "Розмір: " << list.size() << endl;

    cout << "Індекс значення 5: " << list.find(5) << endl;

    list.erase(2); 
    list.print_forward();    

    list.pop_front();
    list.pop_back();
    list.print_forward();    

    cout << "Порожній? " << (list.empty() ? "так" : "ні") << endl;

    list.clear();
    cout << "Після очищення: ";
    list.print_forward();

    return 0;
}