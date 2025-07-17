// Homework 3.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <Windows.h>
using namespace std;
// Клас Людина, тут я зберігаю ПІБ ті вік
class Lyudyna {
private:
    char* fio; // ПІБ
    int vik; // Вік

public:
    // Конструктор по замовчуванню
    Lyudyna() : Lyudyna(nullptr, 0) {}

    // Конструктор з параметрами
    Lyudyna(const char* zminnaFio, int zminnaVik) : vik(zminnaVik) {
        if (zminnaFio == nullptr)
            fio = nullptr;
        else {
            fio = new char[strlen(zminnaFio) + 1];
            strcpy_s(fio, strlen(zminnaFio) + 1, zminnaFio);
        }
    }

    // Конструктор копіювання
    Lyudyna(const Lyudyna& other) {
        if (other.fio == nullptr)
            fio = nullptr;
        else {
            fio = new char[strlen(other.fio) + 1];
            strcpy_s(fio, strlen(other.fio) + 1, other.fio);
        }
        vik = other.vik;
    }

    // Деструктор
    ~Lyudyna() {
        if (fio) delete[] fio;
    }

    // Метод для зміни ПІБ
    Lyudyna& SetFio(const char* newFio) {
        if (fio) delete[] fio;
        if (newFio == nullptr)
            fio = nullptr;
        else {
            fio = new char[strlen(newFio) + 1];
            strcpy_s(fio, strlen(newFio) + 1, newFio);
        }
        return *this;
    }

    // Метод для зміни віку
    Lyudyna& SetVik(int newVik) {
        vik = newVik;
        return *this;
    }

    // Вивід інформації
    void Print() {
        cout << "ПІБ: " << (fio ? fio : "нема") << endl;
        cout << "Вік: " << vik << endl;
        cout << "Адреса об'єкту: " << this << endl;
    }
};

// Клас Квартира, тут масив людей
class Kvartyra {
private:
    Lyudyna* masyvLyudey; // Массив людей
    int kilkistLyudey; // Скільки людей у квартирі

public:
    // Конструктор по замовчуванню
    Kvartyra() : Kvartyra(nullptr, 0) {}

    // Конструктор з параметрами
    Kvartyra(Lyudyna* masyv, int k) : kilkistLyudey(k) {
        if (k == 0 || masyv == nullptr) {
            masyvLyudey = nullptr;
            kilkistLyudey = 0;
        }
        else {
            masyvLyudey = new Lyudyna[k];
            for (int i = 0; i < k; ++i) {
                masyvLyudey[i] = masyv[i]; // Викликається конструктор копіювання для Lyudyna
            }
        }
    }

    // Конструктор копіювання
    Kvartyra(const Kvartyra& other) {
        kilkistLyudey = other.kilkistLyudey;
        if (kilkistLyudey == 0 || other.masyvLyudey == nullptr) {
            masyvLyudey = nullptr;
        }
        else {
            masyvLyudey = new Lyudyna[kilkistLyudey];
            for (int i = 0; i < kilkistLyudey; ++i) {
                masyvLyudey[i] = other.masyvLyudey[i];
            }
        }
    }

    // Деструктор
    ~Kvartyra() {
        if (masyvLyudey) delete[] masyvLyudey;
    }

    // Вивід інформації про всіх людей
    void Print() {
        cout << "Квартира (адреса об'єкту: " << this << ") містить " << kilkistLyudey << " людей:" << endl;
        for (int i = 0; i < kilkistLyudey; ++i) {
            cout << "  Людина #" << i + 1 << ":" << endl;
            masyvLyudey[i].Print();
        }
    }
};

// Клас Дім, тут масив квартир
class Dim {
private:
    Kvartyra* masyvKvartyr; // Массів квартир
    int kilkistKvartyr; // Скільки квартир у домі

public:
    // Конструктор по замовчуванню
    Dim() : Dim(nullptr, 0) {}

    // Конструктор з параметрами
    Dim(Kvartyra* masyv, int k) : kilkistKvartyr(k) {
        if (k == 0 || masyv == nullptr) {
            masyvKvartyr = nullptr;
            kilkistKvartyr = 0;
        }
        else {
            masyvKvartyr = new Kvartyra[k];
            for (int i = 0; i < k; ++i) {
                masyvKvartyr[i] = masyv[i]; // Викликається конструктор копіювання для Kvartyra
            }
        }
    }

    // Конструктор копіювання
    Dim(const Dim& other) {
        kilkistKvartyr = other.kilkistKvartyr;
        if (kilkistKvartyr == 0 || other.masyvKvartyr == nullptr) {
            masyvKvartyr = nullptr;
        }
        else {
            masyvKvartyr = new Kvartyra[kilkistKvartyr];
            for (int i = 0; i < kilkistKvartyr; ++i) {
                masyvKvartyr[i] = other.masyvKvartyr[i];
            }
        }
    }

    // Деструктор
    ~Dim() {
        if (masyvKvartyr) delete[] masyvKvartyr;
    }

    // Вивід інформації про всі квартири
    void Print() {
        cout << "Дім (адреса об'єкту: " << this << ") містить " << kilkistKvartyr << " квартир:" << endl;
        for (int i = 0; i < kilkistKvartyr; ++i) {
            cout << "\nКвартира #" << i + 1 << ":" << endl;
            masyvKvartyr[i].Print();
        }
    }
};

int main() {
    SetConsoleOutputCP(1251);
    // Створюю людей
    Lyudyna lyudynyVkvartiri1[] = {
        Lyudyna("Іван Іванов", 20),
        Lyudyna("Петро Петренко", 25)
    };
    Lyudyna lyudynyVkvartiri2[] = {
        Lyudyna("Оля Іванова", 30)
    };

    // Створюю квартири, передаю масив людей
    Kvartyra kvartira1(lyudynyVkvartiri1, 2);
    Kvartyra kvartira2(lyudynyVkvartiri2, 1);

    // Створюю масив квартир для дому
    Kvartyra kvartiriVDomi[] = { kvartira1, kvartira2 };

    // Створюю дім
    Dim dim1(kvartiriVDomi, 2);

    // Вивожу все
    dim1.Print();

    // Тестую копіювання
    cout << "\n--- Тест копіювання дому ---\n";
    Dim dim2(dim1);
    dim2.Print();

    // Тут все має видалитись нормально, бо є деструктори
    return 0;
}