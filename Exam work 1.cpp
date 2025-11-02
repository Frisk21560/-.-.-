// Exam work 1.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <windows.h>
#include <io.h>
#include <fcntl.h>

#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <algorithm>
#include <iomanip>
#include <ctime>

using std::string;
using std::wstring;
using std::vector;
using std::wcout;
using std::wcin;
using std::wostringstream;
using std::getline;

//--------- Файли --------- 
static const string FILE_USERS = "users.dat";
static const string FILE_ACC = "wallets.dat";
static const string FILE_EXP = "expenses.dat";
static const string FILE_REPORT = "report.txt";

// --------- UTF-8 --------- 
static bool utf8ToWstring(const string& s, wstring& out)
{
    out.clear();

    if (s.empty())
    {
        return true;
    }

    int need = MultiByteToWideChar(CP_UTF8,
        MB_ERR_INVALID_CHARS,
        s.c_str(),
        (int)s.size(),
        nullptr,
        0);

    if (!need)
    {
        return false;
    }

    out.resize(need);

    int got = MultiByteToWideChar(CP_UTF8,
        MB_ERR_INVALID_CHARS,
        s.c_str(),
        (int)s.size(),
        &out[0],
        need);

    return got != 0;
}

static bool wstringToUtf8(const wstring& s, string& out)
{
    out.clear();

    if (s.empty())
    {
        return true;
    }

    int need = WideCharToMultiByte(CP_UTF8,
        WC_ERR_INVALID_CHARS,
        s.c_str(),
        (int)s.size(),
        nullptr,
        0,
        nullptr,
        nullptr);

    if (!need)
    {
        return false;
    }

    out.resize(need);

    int got = WideCharToMultiByte(CP_UTF8,
        WC_ERR_INVALID_CHARS,
        s.c_str(),
        (int)s.size(),
        &out[0],
        need,
        nullptr,
        nullptr);

    return got != 0;
}

// --------- Дата (YYYY-MM-DD) ---------
static wstring todayW()
{
    time_t now = time(nullptr);
    std::tm t;

#if defined(_MSC_VER)
    localtime_s(&t, &now);
#else
    t = *localtime(&now);
#endif

    wostringstream os;
    os << std::setw(4) << std::setfill(L'0') << (t.tm_year + 1900) << L'-'
        << std::setw(2) << std::setfill(L'0') << (t.tm_mon + 1) << L'-'
        << std::setw(2) << std::setfill(L'0') << t.tm_mday;

    return os.str();
}

// --------- Структури ---------

enum class CardType
{
    Debit,
    Credit
};

struct User
{
    wstring login;
    wstring pass;
    wstring name;
    int age = 0;
    wstring mainBank = L"mono";
    wstring cardEnd;

    string saveLine() const
    {
        auto esc = [&](const wstring& s) -> string
            {
                string tmp;
                wstringToUtf8(s, tmp);

                for (char& c : tmp)
                {
                    if (c == '|')
                    {
                        c = '/';
                    }
                }

                return tmp;
            };

        std::ostringstream os;
        os << esc(login) << "|" << esc(pass) << "|" << esc(name) << "|" << age << "|" << esc(mainBank) << "|" << esc(cardEnd);
        return os.str();
    }

    static User loadFromLine(const string& line)
    {
        User u;
        vector<string> parts;
        std::istringstream is(line);
        string p;

        while (std::getline(is, p, '|'))
        {
            parts.push_back(p);
        }

        if (parts.size() >= 6)
        {
            utf8ToWstring(parts[0], u.login);
            utf8ToWstring(parts[1], u.pass);
            utf8ToWstring(parts[2], u.name);

            try
            {
                u.age = std::stoi(parts[3]);
            }
            catch (...)
            {
                u.age = 0;
            }

            utf8ToWstring(parts[4], u.mainBank);
            utf8ToWstring(parts[5], u.cardEnd);
        }

        return u;
    }
};

struct Account
{
    wstring id;
    wstring owner;
    double balance = 0.0;

    virtual ~Account() {}

    Account() {}

    Account(const wstring& i, const wstring& o, double b)
        : id(i), owner(o), balance(b)
    {
    }

    virtual bool take(double amt)
    {
        if (balance >= amt)
        {
            balance -= amt;
            return true;
        }

        return false;
    }

    virtual void add(double amt)
    {
        balance += amt;
    }

    virtual wstring typeName() const
    {
        return L"Acc";
    }

    virtual string saveLine() const
    {
        return "";
    }
};

struct Wallet : public Account
{
    Wallet() {}

    Wallet(const wstring& i, const wstring& o, double b)
        : Account(i, o, b)
    {
    }

    virtual wstring typeName() const override
    {
        return L"Wallet";
    }

    virtual string saveLine() const override
    {
        string idu;
        string ou;

        wstringToUtf8(id, idu);
        wstringToUtf8(owner, ou);

        std::ostringstream os;
        os << "WAL|" << idu << "|" << ou << "|" << balance;
        return os.str();
    }
};

struct Card : public Account
{
    CardType type = CardType::Debit;
    double limit = 0;
    wstring bank = L"mono";
    wstring expiry;

    Card() {}

    Card(const wstring& i, const wstring& o, CardType t, double b, double lim, const wstring& bk, const wstring& ex)
        : Account(i, o, b), type(t), limit(lim), bank(bk), expiry(ex)
    {
    }

    virtual wstring typeName() const override
    {
        return (type == CardType::Debit) ? L"Debit" : L"Credit";
    }

    virtual bool take(double amt) override
    {
        if (type == CardType::Debit)
        {
            return Account::take(amt);
        }

        double nb = balance - amt;

        if (nb >= -limit)
        {
            balance = nb;
            return true;
        }

        return false;
    }

    virtual string saveLine() const override
    {
        string idu;
        string ou;
        string bu;
        string exu;

        wstringToUtf8(id, idu);
        wstringToUtf8(owner, ou);
        wstringToUtf8(bank, bu);
        wstringToUtf8(expiry, exu);

        std::ostringstream os;
        os << "CARD|" << idu << "|" << ou << "|" << bu << "|" << exu << "|" << limit << "|" << balance;
        return os.str();
    }
};

struct Expense
{
    wstring accId;
    wstring owner;
    wstring cat;
    double amount = 0;
    wstring date; // YYYY-MM-DD
    wstring note;

    string saveLine() const
    {
        string a, o, c, d, n;

        wstringToUtf8(accId, a);
        wstringToUtf8(owner, o);
        wstringToUtf8(cat, c);
        wstringToUtf8(date, d);
        wstringToUtf8(note, n);

        std::ostringstream os;
        os << a << "|" << o << "|" << c << "|" << amount << "|" << d << "|" << n;
        return os.str();
    }

    static Expense loadFromLine(const string& line)
    {
        Expense e;
        vector<string> parts;
        std::istringstream is(line);
        string p;

        while (std::getline(is, p, '|'))
        {
            parts.push_back(p);
        }

        if (parts.size() >= 6)
        {
            utf8ToWstring(parts[0], e.accId);
            utf8ToWstring(parts[1], e.owner);
            utf8ToWstring(parts[2], e.cat);

            try
            {
                e.amount = std::stod(parts[3]);
            }
            catch (...)
            {
                e.amount = 0;
            }

            utf8ToWstring(parts[4], e.date);
            utf8ToWstring(parts[5], e.note);
        }

        return e;
    }
};

/* --------- Основна програма --------- */

class SimpleFin
{
    vector<User> users;
    vector<Account*> accs;
    vector<Expense> exps;
    User* cur = nullptr;

    Account* findAcc(const wstring& id)
    {
        for (auto p : accs)
        {
            if (p->id == id)
            {
                return p;
            }
        }

        return nullptr;
    }

    User* findUser(const wstring& login)
    {
        for (auto& u : users)
        {
            if (u.login == login)
            {
                return &u;
            }
        }

        return nullptr;
    }

    void printExpense(const Expense& e)
    {
        wcout << e.date << L" | " << e.owner << L" | " << e.accId << L" | " << e.cat << L" | "
            << std::fixed << std::setprecision(2) << e.amount;

        if (!e.note.empty())
        {
            wcout << L" | " << e.note;
        }

        wcout << L"\n";
    }

public:
    SimpleFin() {}

    ~SimpleFin()
    {
        for (auto p : accs)
        {
            delete p;
        }

        accs.clear();
    }

    // Регiстрацiя
    void regUser()
    {
        wcout << L"\n=== Реєстрація ===\n";
        wstring lg;

        while (true)
        {
            wcout << L"Логін: ";
            getline(wcin, lg);

            if (lg.empty())
            {
                wcout << L"Логін не може бути пустим\n";
                continue;
            }

            if (findUser(lg))
            {
                wcout << L"Такий логін вже є\n";
                continue;
            }

            break;
        }

        wcout << L"Пароль: ";
        wstring pw;
        getline(wcin, pw);

        wcout << L"ПІБ: ";
        wstring nm;
        getline(wcin, nm);

        wcout << L"Вік: ";
        wstring as;
        getline(wcin, as);

        int age = 0;

        try
        {
            age = std::stoi(std::string(as.begin(), as.end()));
        }
        catch (...)
        {
            age = 0;
        }

        wcout << L"Дата закінчення картки (YYYY-MM-DD) або пусто: ";
        wstring ex;
        getline(wcin, ex);

        User u;
        u.login = lg;
        u.pass = pw;
        u.name = nm;
        u.age = age;
        u.cardEnd = ex;

        users.push_back(u);

        wcout << L"Зареєстровано\n";
    }

    // Вхід
    void login()
    {
        wcout << L"\n=== Вхід ===\n";
        wcout << L"Логін: ";
        wstring lg;
        getline(wcin, lg);

        wcout << L"Пароль: ";
        wstring pw;
        getline(wcin, pw);

        User* u = findUser(lg);

        if (!u)
        {
            wcout << L"Користувача не знайдено\n";
            return;
        }

        if (u->pass != pw)
        {
            wcout << L"Невірний пароль\n";
            return;
        }

        cur = u;
        wcout << L"Вітаю, " << cur->name << L"\n";
    }

    void logout()
    {
        cur = nullptr;
        wcout << L"Вихід\n";
    }

    void showProfile()
    {
        if (!cur)
        {
            wcout << L"Увійдіть спочатку\n";
            return;
        }

        wcout << L"Логін: " << cur->login << L"\n";
        wcout << L"ПІБ: " << cur->name << L"\n";
        wcout << L"Вік: " << cur->age << L"\n";
        wcout << L"Банк: " << cur->mainBank << L"\n";
        wcout << L"Термін: " << (cur->cardEnd.empty() ? L"(не вказано)" : cur->cardEnd) << L"\n";
    }

    // Акаунти
    void createWallet()
    {
        if (!cur)
        {
            wcout << L"Потрібно увійти\n";
            return;
        }

        wcout << L"Назва гаманця: ";
        wstring id;
        getline(wcin, id);

        if (id.empty())
        {
            wcout << L"Ім'я пусте\n";
            return;
        }

        if (findAcc(id))
        {
            wcout << L"Такий акаунт вже є\n";
            return;
        }

        wcout << L"Початковий баланс: ";
        wstring bs;
        getline(wcin, bs);

        double bal = 0;

        try
        {
            bal = std::stod(std::string(bs.begin(), bs.end()));
        }
        catch (...)
        {
            bal = 0;
        }

        accs.push_back(new Wallet(id, cur->login, bal));
        wcout << L"Гаманець створено\n";
    }

    void createCard()
    {
        if (!cur)
        {
            wcout << L"Потрібно увійти\n";
            return;
        }

        wcout << L"Назва картки: ";
        wstring id;
        getline(wcin, id);

        if (id.empty() || findAcc(id))
        {
            wcout << L"Неправильне ім'я або вже існує\n";
            return;
        }

        wcout << L"Тип (1-дебет,2-кредит): ";
        wstring t;
        getline(wcin, t);

        int it = 1;

        try
        {
            it = std::stoi(std::string(t.begin(), t.end()));
        }
        catch (...)
        {
            it = 1;
        }

        CardType ct = (it == 2) ? CardType::Credit : CardType::Debit;

        wcout << L"Початковий баланс: ";
        wstring bs;
        getline(wcin, bs);

        double bal = 0;

        try
        {
            bal = std::stod(std::string(bs.begin(), bs.end()));
        }
        catch (...)
        {
            bal = 0;
        }

        double lim = 0;

        if (ct == CardType::Credit)
        {
            wcout << L"Кредитний ліміт: ";
            wstring ls;
            getline(wcin, ls);

            try
            {
                lim = std::stod(std::string(ls.begin(), ls.end()));
            }
            catch (...)
            {
                lim = 0;
            }
        }

        wcout << L"Банк (Enter - mono): ";
        wstring bank;
        getline(wcin, bank);

        if (bank.empty())
        {
            bank = L"mono";
        }

        wcout << L"Дата кінця (YYYY-MM-DD) або пусто: ";
        wstring ex;
        getline(wcin, ex);

        accs.push_back(new Card(id, cur->login, ct, bal, lim, bank, ex));

        if (cur->mainBank.empty() || cur->mainBank == L"mono")
        {
            cur->mainBank = bank;
            cur->cardEnd = ex;
        }

        wcout << L"Картка додана\n";
    }

    void deposit()
    {
        if (!cur)
        {
            wcout << L"Потрібно увійти\n";
            return;
        }

        wcout << L"Акаунт для поповнення: ";
        wstring id;
        getline(wcin, id);

        Account* a = findAcc(id);

        if (!a || a->owner != cur->login)
        {
            wcout << L"Акаунт не знайдено або не ваш\n";
            return;
        }

        wcout << L"Сума: ";
        wstring s;
        getline(wcin, s);

        double v = 0;

        try
        {
            v = std::stod(std::string(s.begin(), s.end()));
        }
        catch (...)
        {
            v = 0;
        }

        a->add(v);
        wcout << L"Готово. Новий баланс: " << a->balance << L"\n";
    }

    void addExpense()
    {
        if (!cur)
        {
            wcout << L"Потрібно увійти\n";
            return;
        }

        wcout << L"Акаунт списання: ";
        wstring id;
        getline(wcin, id);

        Account* a = findAcc(id);

        if (!a || a->owner != cur->login)
        {
            wcout << L"Акаунт не знайдено або не ваш\n";
            return;
        }

        wcout << L"Категорія: ";
        wstring cat;
        getline(wcin, cat);

        wcout << L"Сума: ";
        wstring s;
        getline(wcin, s);

        double v = 0;

        try
        {
            v = std::stod(std::string(s.begin(), s.end()));
        }
        catch (...)
        {
            v = 0;
        }

        wcout << L"Дата (YYYY-MM-DD) або Enter для сьогодні: ";
        wstring d;
        getline(wcin, d);

        if (d.empty())
        {
            d = todayW();
        }

        wcout << L"Нотатка: ";
        wstring note;
        getline(wcin, note);

        if (v <= 0)
        {
            wcout << L"Сума має бути > 0\n";
            return;
        }

        if (!a->take(v))
        {
            wcout << L"Не вистачає коштів або ліміт\n";
            return;
        }

        Expense e;
        e.accId = a->id;
        e.owner = cur->login;
        e.cat = cat;
        e.amount = v;
        e.date = d;
        e.note = note;

        exps.push_back(e);

        wcout << L"Витрата додана\n";
    }

    void showMyAccs()
    {
        if (!cur)
        {
            wcout << L"Потрібно увійти\n";
            return;
        }

        wcout << L"=== Ваші акаунти ===\n";

        for (auto p : accs)
        {
            if (p->owner != cur->login)
            {
                continue;
            }

            Card* c = dynamic_cast<Card*>(p);

            if (c)
            {
                wcout << p->id << L" (" << c->typeName() << L") Баланс: " << p->balance << L" Банк: " << c->bank << L" Термін: " << c->expiry;

                if (c->type == CardType::Credit)
                {
                    wcout << L" Ліміт: " << c->limit;
                }

                wcout << L"\n";
            }
            else
            {
                wcout << p->id << L" (Wallet) Баланс: " << p->balance << L"\n";
            }
        }
    }

    // Фільтри і ТОП
    vector<Expense> filterByDay(const wstring& d, const wstring& owner = L"")
    {
        vector<Expense> out;
        string ds;
        wstringToUtf8(d, ds);

        for (auto& e : exps)
        {
            string ed;
            wstringToUtf8(e.date, ed);

            if ((owner.empty() || e.owner == owner) && ed == ds)
            {
                out.push_back(e);
            }
        }

        return out;
    }

    vector<Expense> filterByMonth(const wstring& d, const wstring& owner = L"")
    {
        vector<Expense> out;
        string ds;
        wstringToUtf8(d, ds);

        string month = ds.size() >= 7 ? ds.substr(0, 7) : ds;

        for (auto& e : exps)
        {
            string ed;
            wstringToUtf8(e.date, ed);

            if ((owner.empty() || e.owner == owner) && ed.size() >= 7 && ed.substr(0, 7) == month)
            {
                out.push_back(e);
            }
        }

        return out;
    }

    vector<Expense> topExpenses(const vector<Expense>& list, int n = 3)
    {
        vector<Expense> tmp = list;

        std::sort(tmp.begin(), tmp.end(), [](const Expense& a, const Expense& b)
            {
                return a.amount > b.amount;
            });

        if ((int)tmp.size() > n)
        {
            tmp.resize(n);
        }

        return tmp;
    }

    vector<std::pair<wstring, double>> topCategories(const vector<Expense>& list, int n = 3)
    {
        vector<std::pair<string, double>> sums;

        for (auto& e : list)
        {
            string cu;
            wstringToUtf8(e.cat, cu);

            bool found = false;

            for (auto& p : sums)
            {
                if (p.first == cu)
                {
                    p.second += e.amount;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                sums.emplace_back(cu, e.amount);
            }
        }

        std::sort(sums.begin(), sums.end(), [](auto& a, auto& b)
            {
                return a.second > b.second;
            });

        if ((int)sums.size() > n)
        {
            sums.resize(n);
        }

        vector<std::pair<wstring, double>> out;

        for (auto& p : sums)
        {
            wstring w;
            utf8ToWstring(p.first, w);
            out.emplace_back(w, p.second);
        }

        return out;
    }

    wstring buildReport(const vector<Expense>& list, const wstring& title)
    {
        wostringstream os;
        os << L"=== " << title << L" ===\n";

        double total = 0;

        for (auto& e : list)
        {
            os << e.date << L" | " << e.accId << L" | " << e.cat << L" | " << std::fixed << std::setprecision(2) << e.amount;

            if (!e.note.empty())
            {
                os << L" | " << e.note;
            }

            os << L"\n";
            total += e.amount;
        }

        os << L"---\nВсього: " << std::fixed << std::setprecision(2) << total << L" грн\n\n";

        auto top = topExpenses(list, 3);

        os << L"ТОП-3 витрат:\n";

        int i = 1;

        for (auto& t : top)
        {
            os << i++ << L". " << t.date << L" | " << t.accId << L" | " << t.cat << L" | " << t.amount << L" грн\n";
        }

        if (top.empty())
        {
            os << L"(немає)\n";
        }

        os << L"\nТОП-3 категорій:\n";

        i = 1;
        auto cats = topCategories(list, 3);

        for (auto& c : cats)
        {
            os << i++ << L". " << c.first << L" | " << c.second << L" грн\n";
        }

        if (cats.empty())
        {
            os << L"(немає)\n";
        }

        os << L"\n";
        return os.str();
    }

    void saveReportToFile(const wstring& txt)
    {
        string u8;

        if (!wstringToUtf8(txt, u8))
        {
            wcout << L"Помилка конвертації\n";
            return;
        }

        std::ofstream f(FILE_REPORT, std::ios::app | std::ios::binary);

        if (!f)
        {
            wcout << L"Не відкрив файл звіту\n";
            return;
        }

        f << u8 << "\n";
        wcout << L"Звіт збережено у " << std::wstring(FILE_REPORT.begin(), FILE_REPORT.end()) << L"\n";
    }

    // Меню звітів
    void reportDay()
    {
        wcout << L"Введіть дату (YYYY-MM-DD) або Enter: ";
        wstring d;
        getline(wcin, d);

        if (d.empty())
        {
            d = todayW();
        }

        wstring owner = cur ? cur->login : L"";
        auto list = filterByDay(d, owner);
        wstring title = L"Звіт за день: " + d;
        wstring txt = buildReport(list, title);

        wcout << txt;
        saveReportToFile(txt);
    }

    void reportMonth()
    {
        wcout << L"Введіть місяць (YYYY-MM) або дата: ";
        wstring d;
        getline(wcin, d);

        if (d.size() == 7)
        {
            d += L"-01";
        }

        if (d.empty())
        {
            d = todayW();
        }

        wstring owner = cur ? cur->login : L"";
        auto list = filterByMonth(d, owner);
        wstring title = L"Звіт за місяць: " + (d.size() >= 7 ? d.substr(0, 7) : d);
        wstring txt = buildReport(list, title);

        wcout << txt;
        saveReportToFile(txt);
    }

    // Імпорт / Експорт CSV 
    void exportCsv(const string& fname)
    {
        std::ofstream f(fname, std::ios::binary);

        if (!f)
        {
            wcout << L"Не можу відкрити файл для експорту\n";
            return;
        }

        f << "account,owner,category,amount,date,note\n";

        for (auto& e : exps)
        {
            string a, o, c, d, n;
            wstringToUtf8(e.accId, a);
            wstringToUtf8(e.owner, o);
            wstringToUtf8(e.cat, c);
            wstringToUtf8(e.date, d);
            wstringToUtf8(e.note, n);

            for (char& ch : n)
            {
                if (ch == '\n' || ch == '\r')
                {
                    ch = ' ';
                }
            }

            f << a << "," << o << "," << c << "," << e.amount << "," << d << "," << n << "\n";
        }

        wcout << L"Експорт завершено\n";
    }

    void importCsv(const string& fname)
    {
        std::ifstream f(fname, std::ios::binary);

        if (!f)
        {
            wcout << L"Не можу відкрити файл для імпорту\n";
            return;
        }

        string header;

        if (!std::getline(f, header))
        {
            wcout << L"Пустий файл\n";
            return;
        }

        string line;
        int cnt = 0;

        while (std::getline(f, line))
        {
            if (line.empty())
            {
                continue;
            }

            vector<string> p;
            std::istringstream is(line);
            string part;

            while (std::getline(is, part, ','))
            {
                p.push_back(part);
            }

            if (p.size() < 6)
            {
                continue;
            }

            wstring aid;
            wstring owner;
            wstring cat;
            wstring date;
            wstring note;

            utf8ToWstring(p[0], aid);
            utf8ToWstring(p[1], owner);
            utf8ToWstring(p[2], cat);
            utf8ToWstring(p[4], date);
            utf8ToWstring(p[5], note);

            double amt = 0;

            try
            {
                amt = std::stod(p[3]);
            }
            catch (...)
            {
                amt = 0;
            }

            Account* a = findAcc(aid);

            if (a && a->owner == owner && a->take(amt))
            {
                Expense e;
                e.accId = aid;
                e.owner = owner;
                e.cat = cat;
                e.amount = amt;
                e.date = date;
                e.note = note;

                exps.push_back(e);
                ++cnt;
            }
            else
            {
                wcout << L"Пропущено: " << amt << L" з " << aid << L"\n";
            }
        }

        wcout << L"Імпортовано: " << cnt << L"\n";
    }

    // Збереження / Завантаження всіх даних
    void saveAll()
    {
        std::ofstream fu(FILE_USERS, std::ios::binary);

        if (fu)
        {
            for (auto& u : users)
            {
                fu << u.saveLine() << "\n";
            }
        }

        std::ofstream fa(FILE_ACC, std::ios::binary);

        if (fa)
        {
            for (auto p : accs)
            {
                fa << p->saveLine() << "\n";
            }
        }

        std::ofstream fe(FILE_EXP, std::ios::binary);

        if (fe)
        {
            for (auto& e : exps)
            {
                fe << e.saveLine() << "\n";
            }
        }

        wcout << L"Всі дані збережено\n";
    }

    void loadAll()
    {
        users.clear();

        for (auto p : accs)
        {
            delete p;
        }

        accs.clear();
        exps.clear();

        // users
        std::ifstream fu(FILE_USERS, std::ios::binary);

        if (fu)
        {
            string line;

            while (std::getline(fu, line))
            {
                if (!line.empty())
                {
                    users.push_back(User::loadFromLine(line));
                }
            }
        }

        // accounts
        std::ifstream fa(FILE_ACC, std::ios::binary);

        if (fa)
        {
            string line;

            while (std::getline(fa, line))
            {
                if (line.empty())
                {
                    continue;
                }

                vector<string> p;
                std::istringstream is(line);
                string part;

                while (std::getline(is, part, '|'))
                {
                    p.push_back(part);
                }

                if (p.size() >= 4 && p[0] == "WAL")
                {
                    wstring id;
                    wstring owner;
                    utf8ToWstring(p[1], id);
                    utf8ToWstring(p[2], owner);

                    double bal = 0;

                    try
                    {
                        bal = std::stod(p[3]);
                    }
                    catch (...)
                    {
                        bal = 0;
                    }

                    accs.push_back(new Wallet(id, owner, bal));
                }
                else if (p.size() >= 7 && p[0] == "CARD")
                {
                    wstring id;
                    wstring owner;
                    wstring bank;
                    wstring ex;

                    utf8ToWstring(p[1], id);
                    utf8ToWstring(p[2], owner);
                    utf8ToWstring(p[3], bank);
                    utf8ToWstring(p[4], ex);

                    double lim = 0;
                    double bal = 0;

                    try
                    {
                        lim = std::stod(p[5]);
                    }
                    catch (...)
                    {
                        lim = 0;
                    }

                    try
                    {
                        bal = std::stod(p[6]);
                    }
                    catch (...)
                    {
                        bal = 0;
                    }

                    CardType ct = (lim > 0) ? CardType::Credit : CardType::Debit;
                    accs.push_back(new Card(id, owner, ct, bal, lim, bank, ex));
                }
            }
        }

        // expenses
        std::ifstream fe(FILE_EXP, std::ios::binary);

        if (fe)
        {
            string line;

            while (std::getline(fe, line))
            {
                if (!line.empty())
                {
                    exps.push_back(Expense::loadFromLine(line));
                }
            }
        }

        wcout << L"Дані завантажено (якщо файли існували)\n";
    }

    void showAllExp()
    {
        if (exps.empty())
        {
            wcout << L"Немає витрат\n";
            return;
        }

        wcout << L"=== Всі витрати ===\n";

        for (auto& e : exps)
        {
            printExpense(e);
        }
    }

public:
    // Обгортки для меню 
    void menuReg() { regUser(); }
    void menuLogin() { login(); }
    void menuLogout() { logout(); }
    void menuProfile() { showProfile(); }
    void menuCreateWallet() { createWallet(); }
    void menuCreateCard() { createCard(); }
    void menuDeposit() { deposit(); }
    void menuAddExpense() { addExpense(); }
    void menuShowMyAccs() { showMyAccs(); }
    void menuShowAllExp() { showAllExp(); }
    void menuReportDay() { reportDay(); }
    void menuReportMonth() { reportMonth(); }
    void menuExportCsv()
    {
        wcout << L"Ім'я файлу для експорту (export.csv): ";
        wstring fn;
        getline(wcin, fn);

        string f;

        if (fn.empty())
        {
            f = "export.csv";
        }
        else
        {
            wstringToUtf8(fn, f);
        }

        exportCsv(f);
    }

    void menuImportCsv()
    {
        wcout << L"Ім'я файлу для імпорту: ";
        wstring fn;
        getline(wcin, fn);

        if (fn.empty())
        {
            wcout << L"Файл не вказано\n";
            return;
        }

        string f;
        wstringToUtf8(fn, f);
        importCsv(f);
    }

    void menuSaveAll() { saveAll(); }
    void menuLoadAll() { loadAll(); }

    User* getCur() { return cur; }
};

/* --------- Меню головне --------- */

static inline wstring trimW(const wstring& s)
{
    size_t a = 0;

    while (a < s.size() && iswspace(s[a]))
    {
        ++a;
    }

    size_t b = s.size();

    while (b > a && iswspace(s[b - 1]))
    {
        --b;
    }

    return s.substr(a, b - a);
}

void printMenu(bool logged)
{
    wcout << L"\n=== Меню Системи ===\n";
    wcout << L"1. Реєстрація\n";
    wcout << L"2. Вхід\n";
    wcout << L"3. Вихід (logout)\n";
    wcout << L"4. Профіль\n";
    wcout << L"5. Додати гаманець\n";
    wcout << L"6. Додати картку\n";
    wcout << L"7. Поповнити акаунт\n";
    wcout << L"8. Додати витрату\n";
    wcout << L"9. Показати мої акаунти\n";
    wcout << L"10. Показати всі витрати\n";
    wcout << L"11. Звіт за день\n";
    wcout << L"12. Звіт за місяць\n";
    wcout << L"13. Експорт CSV\n";
    wcout << L"14. Імпорт CSV\n";
    wcout << L"15. Зберегти всі дані\n";
    wcout << L"16. Завантажити всі дані\n";
    wcout << L"0. Вихід з програми\n";
    wcout << L"Введіть номер: ";
}

int wmain()
{
    // Налаштування консолі
    _setmode(_fileno(stdout), _O_U16TEXT);
    _setmode(_fileno(stdin), _O_U16TEXT);

    SetConsoleOutputCP(CP_UTF8);
    SetConsoleCP(CP_UTF8);

    SimpleFin app;
    app.menuLoadAll();

    while (true)
    {
        bool logged = (app.getCur() != nullptr);
        printMenu(logged);

        wstring choiceLine;

        if (!getline(wcin, choiceLine))
        {
            break;
        }

        choiceLine = trimW(choiceLine);

        int choice = -1;

        try
        {
            choice = std::stoi(std::string(choiceLine.begin(), choiceLine.end()));
        }
        catch (...)
        {
            choice = -1;
        }

        switch (choice)
        {
        case 0:
        {
            wcout << L"Вихід...\n";
            app.menuSaveAll();
            return 0;
        }

        case 1:  app.menuReg();         break;
        case 2:  app.menuLogin();       break;
        case 3:  app.menuLogout();      break;
        case 4:  app.menuProfile();     break;
        case 5:  app.menuCreateWallet(); break;
        case 6:  app.menuCreateCard();  break;
        case 7:  app.menuDeposit();     break;
        case 8:  app.menuAddExpense();  break;
        case 9:  app.menuShowMyAccs();  break;
        case 10: app.menuShowAllExp();  break;
        case 11: app.menuReportDay();   break;
        case 12: app.menuReportMonth(); break;
        case 13: app.menuExportCsv();   break;
        case 14: app.menuImportCsv();   break;
        case 15: app.menuSaveAll();     break;
        case 16: app.menuLoadAll();     break;

        default:
        {
            wcout << L"Невірна команда\n";
            break;
        }
        }
    }

    app.menuSaveAll();
    return 0;
}