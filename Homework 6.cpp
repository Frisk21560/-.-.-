// Homework 6.cpp : Этот файл содержит функцию "main". Здесь начинается и заканчивается выполнение программы.
//

#include <iostream>
#include <string>
#include <Windows.h>

using namespace std;

class Audio {
public:
    virtual void Play() const {
        cout << "Я абстрактний аудіо! Не знаю як грати." << endl;
    }
    // Віртуальний деструктор, щоб не було утечок пам'яті
    virtual ~Audio() {}
};

// Клас Song, наслідується від Audio
class Song : public Audio {
public:
    string title;
    string artist;

    Song(string t, string a) : title(t), artist(a) {}

    void Play() const override {
        cout << "Playing song: " << title << " by " << artist << endl;
    }
};

// Клас Podcast, наслідується від Audio
class Podcast : public Audio {
public:
    string host;
    string topic;

    Podcast(string h, string tp) : host(h), topic(tp) {}

    void Play() const override {
        cout << "Podcast on " << topic << " hosted by " << host << endl;
    }
};

// Клас Audiobook, наслідується від Audio
class Audiobook : public Audio {
public:
    string bookTitle;
    string author;
    string voice;

    Audiobook(string bt, string a, string v) : bookTitle(bt), author(a), voice(v) {}

    void Play() const override {
        cout << "Listening to audiobook: " << bookTitle << " by " << author << ". Read by " << voice << "." << endl;
    }
};

int main()
{
    SetConsoleOutputCP(1251); 
    cout << "Віртуальні функції аудіо\n";

    Audio* audio = nullptr;

    cout << "Оберіть тип аудіо:\n";
    cout << "1. Song\n";
    cout << "2. Podcast\n";
    cout << "3. Audiobook\n> ";

    short choice;
    cin >> choice;
    cin.ignore();

    if (choice == 1) {
        string title, artist;
        cout << "Введіть назву пісні: ";
        getline(cin, title);
        cout << "Введіть виконавця: ";
        getline(cin, artist);
        audio = new Song(title, artist);
    }
    else if (choice == 2) {
        string topic, host;
        cout << "Введіть тему подкасту: ";
        getline(cin, topic);
        cout << "Введіть ведучого: ";
        getline(cin, host);
        audio = new Podcast(host, topic);
    }
    else if (choice == 3) {
        string bookTitle, author, voice;
        cout << "Введіть назву книги: ";
        getline(cin, bookTitle);
        cout << "Введіть автора: ";
        getline(cin, author);
        cout << "Введіть ім'я диктора: ";
        getline(cin, voice);
        audio = new Audiobook(bookTitle, author, voice);
    }
    else {
        audio = new Audio();
    }

    audio->Play();

    delete audio;
}