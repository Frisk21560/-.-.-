// Exam work 2.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <windows.h>    
#include <io.h>         
#include <fcntl.h>      

#include <iostream>     
#include <fstream>     
#include <string>
#include <vector>
#include <set>
#include <random>
#include <chrono>
#include <ctime>
#include <cwctype>    
#include <sstream>
#include <iomanip>
#include <algorithm>

using namespace std;
using wstr = std::wstring;

// UTF-8

static bool utf8ToW(const string& s, wstr& out)
{
    out.clear();
    if (s.empty()) return true;
    int need = MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS, s.c_str(), (int)s.size(), nullptr, 0);
    if (!need) return false;
    out.resize(need);
    int got = MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS, s.c_str(), (int)s.size(), &out[0], need);
    return got != 0;
}

static bool wToUtf8(const wstr& s, string& out)
{
    out.clear();
    if (s.empty()) return true;
    int need = WideCharToMultiByte(CP_UTF8, WC_ERR_INVALID_CHARS, s.c_str(), (int)s.size(), nullptr, 0, nullptr, nullptr);
    if (!need) return false;
    out.resize(need);
    int got = WideCharToMultiByte(CP_UTF8, WC_ERR_INVALID_CHARS, s.c_str(), (int)s.size(), &out[0], need, nullptr, nullptr);
    return got != 0;
}

// ---------------- Алфавіти для цезаря ----------------

const wstr ukrLo = L"абвгґдеєжзийіклмнопрстуфхцчшщьюя";
const wstr ukrUp = L"АБВГҐДЕЄЖЗИЙІКЛМНОПРСТУФХЦЧШЩЬЮЯ";
const wstr engLo = L"abcdefghijklmnopqrstuvwxyz";
const wstr engUp = L"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

// ----------------  Цезар для wide  ----------------

wstr caesarW(const wstr& s, int shift)
{
    wstr r = s;

    for (size_t i = 0; i < r.size(); ++i)
    {
        wchar_t c = r[i];

        auto pU = ukrUp.find(c);
        if (pU != wstr::npos)
        {
            int n = (int)ukrUp.size();
            int np = (pU + shift) % n;
            if (np < 0) np += n;
            r[i] = ukrUp[np];
            continue;
        }

        auto pL = ukrLo.find(c);
        if (pL != wstr::npos)
        {
            int n = (int)ukrLo.size();
            int np = (pL + shift) % n;
            if (np < 0) np += n;
            r[i] = ukrLo[np];
            continue;
        }

        auto pEU = engUp.find(c);
        if (pEU != wstr::npos)
        {
            int n = (int)engUp.size();
            int np = (pEU + shift) % n;
            if (np < 0) np += n;
            r[i] = engUp[np];
            continue;
        }

        auto pEL = engLo.find(c);
        if (pEL != wstr::npos)
        {
            int n = (int)engLo.size();
            int np = (pEL + shift) % n;
            if (np < 0) np += n;
            r[i] = engLo[np];
            continue;
        }
    }

    return r;
}

// ---------------- Файли ----------------

bool existsFile(const string& name)
{
    std::ifstream f(name, std::ios::binary);
    return f.good();
}

void makeDefaultWords(const string& fname, int shift)
{
    vector<wstr> list =
    {
        L"програмування", L"компютер", L"алгоритм", L"функція", L"масив",
        L"шаблон", L"обєкт", L"клас", L"спадкування", L"поліморфізм",
        L"інтерфейс", L"архітектура", L"мова", L"компілятор", L"змінна",
        L"виконання", L"оптимізація", L"сервер", L"клієнт", L"база",
        L"дані", L"запит", L"мережа", L"пакет", L"протокол",
        L"безпека", L"шифрування", L"ключ", L"сертифікат", L"тестування",
        L"деплой", L"сценарій", L"процес", L"потік", L"синхронізація",
        L"модуль", L"збірка", L"контейнер", L"віртуалізація", L"хмара",
        L"проєкт", L"задача", L"результат", L"логіка", L"інструкція",
        L"команда", L"помилка", L"відладка", L"документація", L"пакування"
    };

    std::ofstream fo(fname, std::ios::out | std::ios::binary);

    if (!fo)
    {
        wcerr << L"Cannot create words file\n";
        return;
    }

    for (auto& w : list)
    {
        wstr e = caesarW(w, shift);
        string u8;
        if (!wToUtf8(e, u8))
        {
            continue;
        }
        fo << u8 << "\n";
    }

    fo.close();
}

vector<wstr> loadWords(const string& fname, int shift)
{
    vector<wstr> out;
    std::ifstream fi(fname, std::ios::in | std::ios::binary);

    if (!fi)
    {
        wcerr << L"Cannot open words file\n";
        return out;
    }

    string line;
    while (std::getline(fi, line))
    {
        if (line.empty()) continue;
        wstr w;
        if (!utf8ToW(line, w)) continue;
        wstr dec = caesarW(w, -shift);

        for (wchar_t& c : dec) c = towlower(c);

        out.push_back(dec);
    }

    fi.close();
    return out;
}

// ----------------  Ігрові допоміжні функції  ----------------

wstr pickWord(const vector<wstr>& arr)
{
    if (arr.empty()) return L"";
    static std::random_device rd;
    static std::mt19937 gen(rd());
    std::uniform_int_distribution<> d(0, (int)arr.size() - 1);
    return arr[d(gen)];
}

bool wordGuessed(const wstr& w, const set<wchar_t>& used)
{
    for (wchar_t c : w)
    {
        if (!iswalpha(c)) continue;
        if (used.find(c) == used.end()) return false;
    }
    return true;
}

wstr showState(const wstr& w, const set<wchar_t>& used)
{
    wstr r;
    for (wchar_t c : w)
    {
        if (!iswalpha(c))
        {
            r.push_back(c);
            r.push_back(L' ');
        }
        else
        {
            if (used.find(c) != used.end()) r.push_back(c);
            else r.push_back(L'_');
            r.push_back(L' ');
        }
    }
    return r;
}

void saveStat(const string& fname, const wstr& word, int tries, int wrong, double sec, const set<wchar_t>& used)
{
    std::ofstream fo(fname, std::ios::app | std::ios::binary);
    if (!fo) return;

    time_t t = chrono::system_clock::to_time_t(chrono::system_clock::now());
    std::tm tm;
#if defined(_MSC_VER)
    localtime_s(&tm, &t);
#else
    tm = *std::localtime(&t);
#endif

    char buf[128];
    std::string dateStr;
    if (std::strftime(buf, sizeof(buf), "%c", &tm) == 0)
    {
        dateStr = "unknown";
    }
    else
    {
        dateStr = buf;
    }

    string w8;
    if (!wToUtf8(word, w8)) w8 = "<bad>";

    fo << "Date: " << dateStr << "\n";
    fo << "Word: " << w8 << "\n";
    fo << "Tries: " << tries << "\n";
    fo << "Wrong: " << wrong << "\n";
    fo << "Time: " << sec << "\n";
    fo << "Used: ";
    for (wchar_t c : used)
    {
        wstr tmp(1, c);
        string s;
        if (!wToUtf8(tmp, s)) s = "?";
        fo << s << " ";
    }
    fo << "\n---\n";
    fo.close();
}

wstr toLowerW(const wstr& s)
{
    wstr r = s;
    for (wchar_t& c : r) c = towlower(c);
    return r;
}

// ---------------- main ----------------

int main()
{
    _setmode(_fileno(stdout), _O_U16TEXT);
    _setmode(_fileno(stdin), _O_U16TEXT);
    SetConsoleOutputCP(CP_UTF8);
    SetConsoleCP(CP_UTF8);

    wcout << L"Шибениця (Hangman)\n";
    wcout << L"Введіть 'q' або 'вихід' щоб вийти у будь-який момент.\n";

    string wordsFile = "words.enc";
    string statsFile = "hangman_stats.txt";
    int shift = 3;

    if (!existsFile(wordsFile))
    {
        makeDefaultWords(wordsFile, shift);
        wcout << L"Створено words.enc з прикладовим словником.\n";
    }

    vector<wstr> list = loadWords(wordsFile, shift);
    if (list.empty())
    {
        wcout << L"Словник порожній. Завершення.\n";
        return 1;
    }

    bool again = true;

    while (again)
    {
        wstr word = pickWord(list);
        if (word.empty()) break;

        set<wchar_t> good;
        set<wchar_t> bad;
        int maxBad = 6;
        int tries = 0;
        auto start = chrono::steady_clock::now();

        while (true)
        {
            wcout << L"\nСлово: " << showState(word, good) << L"\n";
            wcout << L"Промахи (" << (int)bad.size() << L"/" << maxBad << L"): ";
            for (wchar_t c : bad) wcout << c << L' ';
            wcout << L"\n";

            if (wordGuessed(word, good))
            {
                auto end = chrono::steady_clock::now();
                double sec = chrono::duration<double>(end - start).count();
                wcout << L"\nВітаю! Ви відгадали: " << word << L"\n";
                wcout << L"Час: " << sec << L" с, Спроб: " << tries << L", Невірних: " << bad.size() << L"\n";

                set<wchar_t> used = good;
                used.insert(bad.begin(), bad.end());
                saveStat(statsFile, word, tries, (int)bad.size(), sec, used);
                break;
            }

            if ((int)bad.size() >= maxBad)
            {
                auto end = chrono::steady_clock::now();
                double sec = chrono::duration<double>(end - start).count();
                wcout << L"\nВи програли. Слово: " << word << L"\n";
                wcout << L"Час: " << sec << L" с, Спроб: " << tries << L", Невірних: " << bad.size() << L"\n";

                set<wchar_t> used = good;
                used.insert(bad.begin(), bad.end());
                saveStat(statsFile, word, tries, (int)bad.size(), sec, used);
                break;
            }

            wcout << L"Введіть літеру або слово (q - вихід): ";
            wstr in;
            std::getline(wcin, in);

            if (in.empty())
            {
                wcout << L"Порожній ввід\n";
                continue;
            }

            in = toLowerW(in);

            if (in == L"q" || in == L"вихід" || in == L"exit")
            {
                wcout << L"Вихід...\n";
                again = false;
                break;
            }

            if (in.size() > 1)
            {
                tries++;
                if (in == word)
                {
                    for (wchar_t c : word) if (iswalpha(c)) good.insert(c);
                    continue;
                }
                else
                {
                    wcout << L"Невірне слово\n";
                    bad.insert(L'*');
                    continue;
                }
            }

            wchar_t ch = in[0];

            if (!iswalpha(ch))
            {
                wcout << L"Вводьте тільки літери\n";
                continue;
            }

            tries++;

            if (good.find(ch) != good.end() || bad.find(ch) != bad.end())
            {
                wcout << L"Ви вже вводили цю букву: " << ch << L"\n";
                continue;
            }

            if (word.find(ch) != wstr::npos)
            {
                wcout << L"Правильно: " << ch << L"\n";
                good.insert(ch);
            }
            else
            {
                wcout << L"Ні такої букви\n";
                bad.insert(ch);
            }
        } 

        if (!again) break;

        wcout << L"\nГрати ще? (y/так/n/ні): ";
        wstr a;
        std::getline(wcin, a);
        a = toLowerW(a);

        if (a.empty()) again = false;
        else if (a == L"y" || a == L"так" || a == L"t") again = true;
        else again = false;
    } 

    wcout << L"Дякую за гру. Статистика у файлі: " << statsFile.c_str() << L"\n";
    return 0;
}