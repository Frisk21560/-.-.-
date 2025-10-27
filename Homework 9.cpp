// Homework 9.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>
#include <vector>
#include <Windows.h>
#include <cstdio> 

using namespace std;

class GameRecord {
private:
    int number;
    int tries;
    string timestr;
public:
    GameRecord(int number, int tries, const string& timestr)
        : number(number), tries(tries), timestr(timestr) {
    }

    // Повертає рядок для виводу або запису у файл
    string ToString() const {
        return "Number " + to_string(number) + " guessed in " + to_string(tries) + " tries at " + timestr;
    }

    void Print() const {
        cout << ToString() << endl;
    }
};

class StatsShelf {
private:
    vector<GameRecord> records;
public:
    void AddRecord(const GameRecord& rec) {
        records.push_back(rec); 
    }

    void PrintAll() const {
        if (records.empty()) {
            cout << "Статистика порожня (в цій сесії)." << endl;
            return;
        }
        cout << "=== Статистика в цій сесії ===" << endl;
        for (auto rec : records) {
            rec.Print();
        }
    }
};

// FileWorker - клас для роботи з файлом
class FileWorker {
private:
    string fileName;
public:
    FileWorker(const string& fname) : fileName(fname) {}

    // Додає один запис у файл (додаємо в кінець)
    void AppendRecord(const GameRecord& rec) {
        FILE* f = nullptr;
        try {
            f = fopen(fileName.c_str(), "a"); // a - додавати в кінець
            if (!f) {
                throw ios_base::failure("Failed to open file for append.");
            }
            string line = rec.ToString();
            fwrite(line.c_str(), sizeof(char), line.size(), f);
            fwrite("\n", sizeof(char), 1, f);
            fclose(f);
            f = nullptr;
        }
        catch (ios_base::failure& ex) {
            cout << "Помилка запису у файл: " << ex.what() << endl;
            if (f) fclose(f);
        }
    }

    // Показує вміст файлу (статистика з попередніх ігор)
    void ShowFile() {
        FILE* f = fopen(fileName.c_str(), "r");
        if (!f) {
            cout << "Статистика поки відсутня (файлу " << fileName << " немає)." << endl;
            return;
        }
        cout << "=== Статистика з файлу (" << fileName << ") ===" << endl;
        const int bufsize = 512;
        char buf[bufsize];
        while (fgets(buf, bufsize, f)) {
            cout << buf; // fgets вже містить '\n'
        }
        cout << "=== Кінець файлу ===" << endl;
        fclose(f);
    }
};

// Допоміжні функції
// Отримуємо поточний час як рядок
string GetLocalTimeString() {
    SYSTEMTIME st;
    GetLocalTime(&st);
    char buf[64];
    sprintf(buf, "%04d-%02d-%02d %02d:%02d:%02d",
        st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
    return string(buf);
}

unsigned long long nextSeed(unsigned long long seed) {
    return (seed * 1103515245ULL + 12345ULL) & 0x7fffffffULL;
}
int GetRandom1to500(unsigned long long& seed) {
    seed = nextSeed(seed);
    return static_cast<int>(seed % 500ULL) + 1;
}

// Логіка гри
void PlayGame(FileWorker& fw, StatsShelf& shelf, unsigned long long& seed) {
    int secret = GetRandom1to500(seed);
    int guess = 0;
    int tries = 0;

    cout << "Я загадав число від 1 до 500. Спробуй вгадати!" << endl;
    while (true) {
        cout << "Введи число: ";
        if (!(cin >> guess)) {
            cin.clear();
            string trash;
            getline(cin, trash);
            cout << "Введи, будь ласка, ціле число." << endl;
            continue;
        }
        tries++;
        if (guess == secret) {
            cout << "Вітаю! Ти вгадав за " << tries << " спроб." << endl;
            string timestr = GetLocalTimeString();
            GameRecord rec(secret, tries, timestr);
            shelf.AddRecord(rec);
            fw.AppendRecord(rec);
            break;
        }
        else if (guess < secret) {
            cout << "Загадане число більше." << endl;
        }
        else {
            cout << "Загадане число менше." << endl;
        }
    }
}

int main() {
    SetConsoleOutputCP(1251);

    cout << "Гра: Вгадай число (класами, запис у файл)" << endl;

    const string fileName = "games.txt";
    FileWorker fileWorker(fileName);
    StatsShelf statsShelf;

    // seed беремо з GetTickCount (WinAPI) 
    unsigned long long seed = static_cast<unsigned long long>(GetTickCount());

    while (true) {
        cout << endl;
        cout << "Меню:" << endl;
        cout << "1 - Нова гра" << endl;
        cout << "2 - Показати статистику з файлу" << endl;
        cout << "3 - Показати статистику в цій сесії" << endl;
        cout << "4 - Вийти" << endl;
        cout << "Оберіть пункт: ";

        int choice = 0;
        if (!(cin >> choice)) {
            cin.clear();
            string trash;
            getline(cin, trash);
            cout << "Некоректний ввід." << endl;
            continue;
        }

        if (choice == 1) {
            PlayGame(fileWorker, statsShelf, seed);
        }
        else if (choice == 2) {
            fileWorker.ShowFile();
        }
        else if (choice == 3) {
            statsShelf.PrintAll();
        }
        else if (choice == 4) {
            cout << "До побачення!" << endl;
            break;
        }
        else {
            cout << "Невідомий вибір." << endl;
        }
    }

    return 0;
}