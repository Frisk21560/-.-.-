namespace Legacy_Code_Homework_6._2
{
    // ЗАВДАННЯ 1
    public class Geometriya
    {
        public static double PloshchaKvadrata(double storona)
        {
            // Площа квадрата = сторона * сторона
            return storona * storona;
        }

        // Статичний метод для підрахунку площі прямокутника
        // Принимає довжину і ширину
        public static double PloshchaPryamokutnika(double dovzhyna, double shyryna)
        {
            // Площа прямокутника = довжина * ширина
            return dovzhyna * shyryna;
        }

        // Статичний метод для підрахунку площі трикутника
        // Принимає основу і висоту
        public static double PloshchaTrykutnyka(double osnova, double vysota)
        {
            // Площа трикутника = (основа * висота) / 2
            return (osnova * vysota) / 2;
        }
    }

    // Викликаємо методи з завдання 1
    Console.WriteLine("ЗАВДАННЯ 1 - Площі фігур:");
    double plosh_kv = Geometriya.PloshchaKvadrata(5);
    Console.WriteLine($"Площа квадрата зі стороною 5 = {plosh_kv}");

    double plosh_pryam = Geometriya.PloshchaPryamokutnika(4, 6);
    Console.WriteLine($"Площа прямокутника 4x6 = {plosh_pryam}");

    double plosh_tryk = Geometriya.PloshchaTrykutnyka(5, 8);
    Console.WriteLine($"Площа трикутника з основою 5 та висотою 8 = {plosh_tryk}");


    // ЗАВДАННЯ 2
    public class Tekst
    {
        // Статичний метод для перевірки, чи рядок є паліндромом
        // Паліндром - це рядок, який однаково читається в обох напрямках
        public static bool JePalindrnom(string ryadok)
        {
            // Видаляємо пробіли і переводимо в малі літери для перевірки
            string ochyshchenyy = ryadok.Replace(" ", "").ToLower();

            // Створюємо розвернутий рядокк
            char[] chary = ochyshchenyy.ToCharArray();
            Array.Reverse(chary);
            string rozvernutyy = new string(chary);

            // Порівнюемо оригінальний рядок з розвернутим
            return ochyshchenyy == rozvernutyy;
        }

        // Статичний метод для підрахунку кількості пропозицій в тексті
        public static int Pidrahunok_Propozyciy(string tekst)
        {
            // Лічильник для пропозицій
            int kilkist = 0;

            // Проходе покожному символу в тексті
            for (int i = 0; i < tekst.Length; i++)
            {
                // Якщо символ є крапкою, питальним або оклицним знаком, то це кінець пропозиції
                if (tekst[i] == '.' || tekst[i] == '?' || tekst[i] == '!')
                {
                    kilkist++;
                }
            }

            // Повертаємо кількість пропозицій
            return kilkist;
        }

        // Статичний метод для розвороту рядка
        public static string RozvernutyRyadok(string ryadok)
        {
            // Переводимо рядок в масив символів
            char[] chary = ryadok.ToCharArray();

            // Розвертаємо масив
            Array.Reverse(chary);

            // Перетворюємо масив назад в рядок
            return new string(chary);
        }
    }

    // Викликаємо методи з завдання 2
    Console.WriteLine("\nЗАВДАННЯ 2 - Робота з текстом:");

    string test_palindrom = "А роза упала на лапу Азора";
    bool je_palindrom = Tekst.JePalindrnom(test_palindrom);
    Console.WriteLine($"Чи '{test_palindrom}' є паліндромом? {je_palindrom}");

    string test_tekst = "Привіт. Як справи? Все добре! Це чудово.";
    int propo = Tekst.Pidrahunok_Propozyciy(test_tekst);
    Console.WriteLine($"Кількість пропозицій в тексті: {propo}");

    string test_ryadok = "Привіт";
    string rozver = Tekst.RozvernutyRyadok(test_ryadok);
    Console.WriteLine($"Рядок '{test_ryadok}' розвернутий: '{rozver}'");


    // ЗАВДАННЯ 3
    // Клас для перевірки контактних даних користувача
    public class Pereverka_Danykh
    {
        // Статичний метод для перевірки ПІБ
        public static bool PereverkaFIO(string fio)
        {
            // Проходимо по кожному символу в ПІБ
            for (int i = 0; i < fio.Length; i++)
            {
                // Перевіряє, чи символ є літерою або пробілом
                if (!char.IsLetter(fio[i]) && fio[i] != ' ')
                {
                    // Якщо символ не літера і не пробіл, то ПІБ невалідне
                    return false;
                }
            }

            // Якщо всі символи - літери або пробіли, то ПІБ валідне
            return true;
        }

        // Статичний метод для перевірки віку
        public static bool PereverkaViku(string vik)
        {
            // Проходимо по кожному символу у віці
            for (int i = 0; i < vik.Length; i++)
            {
                // Перевіряємо, чи символ є цифрою
                if (!char.IsDigit(vik[i]))
                {
                    return false;
                }
            }

            // Якщо всі символи - цифри, то вік валідний
            return true;
        }

        // Статичний метод для перевірки телефону
        public static bool PereverkaTelepfonu(string telefon)
        {
            // Перевіряємо довжину телефону
            if (telefon.Length != 13)
                return false;

            // Перевіряємо першу частину: +38(0
            if (telefon[0] != '+' || telefon[1] != '3' || telefon[2] != '8' ||
                telefon[3] != '(' || telefon[4] != '0')
                return false;

            // Перевіряємо, що символи на позиціях 5, 6, 7 - цифри
            if (!char.IsDigit(telefon[5]) || !char.IsDigit(telefon[6]) || !char.IsDigit(telefon[7]))
                return false;

            // Перевіряємо закриваючу дужку
            if (telefon[8] != ')')
                return false;

            // Перевіряємо, що наступні 3 символи - цифри
            if (!char.IsDigit(telefon[9]) || !char.IsDigit(telefon[10]) || !char.IsDigit(telefon[11]))
                return false;

            // Перевіряємо першу дефіс
            if (telefon[12] != '-')
                return false;

            // Якщо всі перевірки пройшли, телефон валідний
            return true;
        }

        // Статичний метод для перевірки email
        public static bool PereverkaEmail(string email)
        {
            // Перевіряємо, чи email містить символ @
            if (!email.Contains("@"))
                return false;

            // Перевіряємо, чи email містить крапку після @
            int index_at = email.IndexOf("@");
            if (index_at == -1 || index_at == email.Length - 1)
                return false;

            // Перевіряємо, чи є крапка після @
            string chastyna_pisla_at = email.Substring(index_at + 1);
            if (!chastyna_pisla_at.Contains("."))
                return false;

            // Перевіряємо, чи є символи перед @
            if (index_at == 0)
                return false;

            // Якщо всі перевірки пройшли, email валідний
            return true;
        }
    }

    // Викликаємо методи з завдання 3
    Console.WriteLine("\nЗАВДАННЯ 3 - Перевірка контактних даних:");

    string test_fio = "Іван Петренко";
    bool fio_ok = Pereverka_Danykh.PereverkaFIO(test_fio);
    Console.WriteLine($"ПІБ '{test_fio}' валідне? {fio_ok}");

    string test_vik = "25";
    bool vik_ok = Pereverka_Danykh.PereverkaViku(test_vik);
    Console.WriteLine($"Вік '{test_vik}' валідний? {vik_ok}");

    string test_tel = "+38(095)123-45-67";
    bool tel_ok = Pereverka_Danykh.PereverkaTelepfonu(test_tel);
    Console.WriteLine($"Телефон '{test_tel}' валідний? {tel_ok}");

    string test_email = "ivan@example.com";
    bool email_ok = Pereverka_Danykh.PereverkaEmail(test_email);
    Console.WriteLine($"Email '{test_email}' валідний? {email_ok}");
}