// Homework2.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <Windows.h>
using namespace std;
// Однозв'язний список - вузол
template<typename T>
struct Node {
    T zminna; // значення
    Node* next; // посилання на наступний вузол
    Node(T znach) : zminna(znach), next(nullptr) {}
};

// СТЕК на основі однозв'язного списку
template<typename T>
class Stack {
private:
    Node<T>* golova; // вершина стеку

public:
    Stack() : golova(nullptr) {}

    ~Stack() {
        clear();
    }

    // Додає елемент до стеку
    void push(T znach) {
        Node<T>* noviy = new Node<T>(znach);
        noviy->next = golova;
        golova = noviy;
    }

    // Видаляє верхній елемент зі стеку
    void pop() {
        if (isEmpty()) {
            cout << "Stack pustiy" << endl;
            return;
        }
        Node<T>* temp = golova;
        golova = golova->next;
        delete temp;
    }

    // Повертає верхній елемент стеку
    T top() {
        if (isEmpty()) {
            throw out_of_range("Stack pustiy");
        }
        return golova->zminna;
    }

    // Чи пустий стек
    bool isEmpty() {
        return golova == nullptr;
    }

    // Очищає стек
    void clear() {
        while (!isEmpty()) {
            pop();
        }
    }
};

// черга на основі однозв'язного списку
template<typename T>
class QueueList {
private:
    Node<T>* front; // початок черги
    Node<T>* last;  // кінець черги
    int rozmir;

public:
    QueueList() : front(nullptr), last(nullptr), rozmir(0) {}

    ~QueueList() {
        clear();
    }

    // Додає елемент в кінець черги
    void enqueue(T znach) {
        Node<T>* noviy = new Node<T>(znach);
        if (isEmpty()) {
            front = last = noviy;
        }
        else {
            last->next = noviy;
            last = noviy;
        }
        rozmir++;
    }

    // Видаляє елемент з початку черги
    T dequeue() {
        if (isEmpty()) {
            throw out_of_range("Queue pustiy");
        }
        Node<T>* temp = front;
        T znach = front->zminna;
        front = front->next;
        delete temp;
        rozmir--;
        if (rozmir == 0) last = nullptr;
        return znach;
    }

    // Повертає перший елемент черги
    T peek() {
        if (isEmpty()) {
            throw out_of_range("Queue pustiy");
        }
        return front->zminna;
    }

    // Чи пуста черга
    bool isEmpty() const {
        return rozmir == 0;
    }

    // Повертає кількість елементів
    int getSize() const {
        return rozmir;
    }

    // Очищае чергу
    void clear() {
        while (!isEmpty()) {
            dequeue();
        }
    }
};

int main() {
    SetConsoleOutputCP(1251);
    // Приклад використання стеку
    Stack<int> stack;
    stack.push(10);
    stack.push(20);
    cout << "Verkh steka: " << stack.top() << endl; // 20
    stack.pop();
    cout << "Verkh steka: " << stack.top() << endl; // 10

    // Приклад використання черги
    QueueList<int> queue;
    queue.enqueue(100);
    queue.enqueue(200);
    cout << "Pershiy v cherzi: " << queue.peek() << endl; // 100
    cout << "Vidalyayemo: " << queue.dequeue() << endl; // 100
    cout << "Zalishilosya v cherzi: " << queue.getSize() << endl; // 1
    return 0;
}