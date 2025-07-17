// Homework 4.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <cstring>
#include <Windows.h>
using namespace std;

// Типи водойм
enum TypeOfReservoir {
    Lake,
    Sea,
    Pool,
    Pond
};

// Клас Reservoir (водойма)
class Reservoir {
private:
    char* nazva; // назва водойми
    float shirina; // ширина
    float dovzhina; // довжина
    float maxHlybyna; // максимальна глибина
    TypeOfReservoir typ; // тип водойми

public:
    // Конструктор за замовчуванням
    Reservoir() : Reservoir("Без назви", 0, 0, 0, Lake) {}

    // explicit конструктор з параметрами
    explicit Reservoir(const char* zminnaNazva, float zminnaShirina, float zminnaDovzhina, float zminnaHlybyna, TypeOfReservoir zminnaTyp)
        : shirina(zminnaShirina), dovzhina(zminnaDovzhina), maxHlybyna(zminnaHlybyna), typ(zminnaTyp) {
        nazva = new char[strlen(zminnaNazva) + 1];
        strcpy_s(nazva, strlen(zminnaNazva) + 1, zminnaNazva);
    }

    // Конструктор копіювання
    Reservoir(const Reservoir& other)
        : shirina(other.shirina), dovzhina(other.dovzhina), maxHlybyna(other.maxHlybyna), typ(other.typ) {
        nazva = new char[strlen(other.nazva) + 1];
        strcpy_s(nazva, strlen(other.nazva) + 1, other.nazva);
    }

    // Деструктор
    ~Reservoir() {
        delete[] nazva;
    }

    // Метод для обсягу водойми
    float GetObsyag() const {
        return shirina * dovzhina * maxHlybyna;
    }

    // Метод для площі поверхні
    float GetPloshcha() const {
        return shirina * dovzhina;
    }

    // Перевірка чи однаковий тип водойм
    bool IsSameType(const Reservoir& other) const {
        return typ == other.typ;
    }

    // Порівняння площі водної поверхні (тільки якщо типи однакові)
    bool IsLargerThan(const Reservoir& other) const {
        if (typ != other.typ) return false;
        return GetPloshcha() > other.GetPloshcha();
    }

    // Копіювання об'єкта
    Reservoir Copy() const {
        return Reservoir(*this); // викликаю конструктор копіювання
    }

    // Методи set
    Reservoir& SetNazva(const char* newNazva) {
        if (nazva) delete[] nazva;
        nazva = new char[strlen(newNazva) + 1];
        strcpy_s(nazva, strlen(newNazva) + 1, newNazva);
        return *this;
    }
    Reservoir& SetShirina(float zminna) { shirina = zminna; return *this; }
    Reservoir& SetDovzhina(float zminna) { dovzhina = zminna; return *this; }
    Reservoir& SetMaxHlybyna(float zminna) { maxHlybyna = zminna; return *this; }
    Reservoir& SetTyp(TypeOfReservoir t) { typ = t; return *this; }

    // Методи get
    const char* GetNazva() const { return nazva; }
    float GetShirina() const { return shirina; }
    float GetDovzhina() const { return dovzhina; }
    float GetMaxHlybyna() const { return maxHlybyna; }
    TypeOfReservoir GetTyp() const { return typ; }

    // Вивід інформації (константна функція)
    void Print() const {
        cout << "Назва: " << nazva << endl;
        cout << "Тип: ";
        switch (typ) {
        case Lake: cout << "Озеро"; break;
        case Sea: cout << "Море"; break;
        case Pool: cout << "Басейн"; break;
        case Pond: cout << "Ставок"; break;
        }
        cout << endl;
        cout << "Ширина: " << shirina << endl;
        cout << "Довжина: " << dovzhina << endl;
        cout << "Макс. глибина: " << maxHlybyna << endl;
        cout << "Площа водної поверхні: " << GetPloshcha() << endl;
        cout << "Об'єм: " << GetObsyag() << endl;
        cout << "Адреса об'єкта: " << this << endl << endl;
    }
};

// Клас для роботи з масивом водойм
class ReservoirArray {
private:
    Reservoir* masyv; // массив водойм
    int kilkist; // скільки їх всього

public:
    ReservoirArray() : masyv(nullptr), kilkist(0) {}

    ~ReservoirArray() {
        delete[] masyv;
    }

    // Додаю водойму
    void Add(const Reservoir& newReservoir) {
        Reservoir* temp = new Reservoir[kilkist + 1];
        for (int i = 0; i < kilkist; ++i)
            temp[i] = masyv[i];
        temp[kilkist] = newReservoir;
        delete[] masyv;
        masyv = temp;
        kilkist++;
    }

    // Видалити водойму за індексом
    void Remove(int idx) {
        if (idx < 0 || idx >= kilkist) return;
        Reservoir* temp = new Reservoir[kilkist - 1];
        int j = 0;
        for (int i = 0; i < kilkist; ++i) {
            if (i != idx) {
                temp[j] = masyv[i];
                j++;
            }
        }
        delete[] masyv;
        masyv = temp;
        kilkist--;
    }

    // Вивести всі водойми
    void PrintAll() const {
        for (int i = 0; i < kilkist; ++i) {
            cout << "Водойма #" << i + 1 << ":" << endl;
            masyv[i].Print();
        }
    }

    // Записати в текстовий файл
    void SaveToTextFileFake(const char* fname) const {
        cout << fname << "\n";
        for (int i = 0; i < kilkist; ++i) {
            cout << masyv[i].GetNazva() << endl
                << masyv[i].GetTyp() << endl
                << masyv[i].GetShirina() << endl
                << masyv[i].GetDovzhina() << endl
                << masyv[i].GetMaxHlybyna() << endl;
        }
        cout << "Кінець даних\n";
    }

    // "Записати" у бінарний файл (те саме, просто текст)
    void SaveToBinaryFileFake(const char* fname) const {
        cout << fname << "\n";
        for (int i = 0; i < kilkist; ++i) {
            cout << "[Бінарні дані] Назва: " << masyv[i].GetNazva() << ", Тип: " << masyv[i].GetTyp()
                << ", Ширина: " << masyv[i].GetShirina() << ", Довжина: " << masyv[i].GetDovzhina()
                << ", Глибина: " << masyv[i].GetMaxHlybyna() << endl;
        }
        cout << "Кінець бінарних даних\n";
    }

    // Геттер кількості
    int GetKilkist() const { return kilkist; }

    // Геттер водойми
    const Reservoir& Get(int idx) const { return masyv[idx]; }
};

void PrintMenu() {
    cout << "\nМеню:\n";
    cout << "1. Додати водойму\n";
    cout << "2. Видалити водойму\n";
    cout << "3. Вивести всі водойми\n";
    cout << "4. записати в текстовий файл\n";
    cout << "5. записати в бінарний файл\n";
    cout << "0. Вихід\n";
}

TypeOfReservoir InputType() {
    cout << "Введіть тип водойми (0 - Озеро, 1 - Море, 2 - Басейн, 3 - Ставок): ";
    int t;
    cin >> t;
    cin.ignore();
    if (t < 0 || t > 3) t = 0;
    return (TypeOfReservoir)t;
}

Reservoir InputReservoir() {
    char buffer[100];
    cout << "Введіть назву водойми: ";
    cin.getline(buffer, 100);
    cout << "Введіть ширину: ";
    float sh;
    cin >> sh;
    cout << "Введіть довжину: ";
    float d;
    cin >> d;
    cout << "Введіть максимальну глибину: ";
    float h;
    cin >> h;
    TypeOfReservoir typ = InputType();
    cin.ignore();
    return Reservoir(buffer, sh, d, h, typ);
}

int main() {
    SetConsoleOutputCP(1251);

    ReservoirArray arr;

    int vibor = -1;
    do {
        PrintMenu();
        cout << "Ваш вибір: ";
        cin >> vibor;
        cin.ignore();
        switch (vibor) {
        case 1:
            arr.Add(InputReservoir());
            break;
        case 2:
            cout << "Введіть номер для видалення: ";
            int idx;
            cin >> idx;
            cin.ignore();
            arr.Remove(idx - 1);
            break;
        case 3:
            arr.PrintAll();
            break;
        case 4:
            arr.SaveToTextFileFake("reservoirs.txt");
            break;
        case 5:
            arr.SaveToBinaryFileFake("reservoirs.dat");
            break;
        }
    } while (vibor != 0);

    return 0;
}