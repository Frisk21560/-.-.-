using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

// Модель словника (один файл = один словник)
public class Slownik
{
    public string Name { get; set; } 
    public string Type { get; set; } 
    public Dictionary<string, List<string>> Entries { get; set; } = new();
}

// Клас для роботи з файлами і операціями над словниками
public static class UpravlinnyaSlovnykamy
{
    private static string zminnaFolder = "dictionaries"; // папка для словників

    // Ініціалізація: створити папку, якщо нема
    public static void Init()
    {
        if (!Directory.Exists(zminnaFolder))
            Directory.CreateDirectory(zminnaFolder);
        // створити папку для експорту
        if (!Directory.Exists("exports"))
            Directory.CreateDirectory("exports");
    }

    // Завантажити всі словники з папки
    public static List<Slownik> ZavantazhytVsi()
    {
        var zminnaRes = new List<Slownik>();
        foreach (var zminnaFile in Directory.GetFiles(zminnaFolder, "*.json"))
        {
            try
            {
                var zminnaText = File.ReadAllText(zminnaFile);
                var zminnaObj = JsonSerializer.Deserialize<Slownik>(zminnaText);
                if (zminnaObj != null) zminnaRes.Add(zminnaObj);
            }
            catch { /* ігнор помилок читання */ }
        }
        return zminnaRes;
    }

    // Зберегти (або оновити) словник в файл
    public static void Zberihaty(Slownik zminnaSlownik)
    {
        var zminnaPath = Path.Combine(zminnaFolder, SanitizeFileName(zminnaSlownik.Name) + ".json");
        var zminnaText = JsonSerializer.Serialize(zminnaSlownik, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(zminnaPath, zminnaText);
    }

    // Видалити файл словника
    public static void ВидалитиФайл(Slownik zminnaSlownik)
    {
        var zminnaPath = Path.Combine(zminnaFolder, SanitizeFileName(zminnaSlownik.Name) + ".json");
        if (File.Exists(zminnaPath)) File.Delete(zminnaPath);
    }

    // Допоміжна функція: очистити ім'я файлу
    private static string SanitizeFileName(string zminnaName)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            zminnaName = zminnaName.Replace(c, '_');
        return zminnaName;
    }
}

// Головна програма: меню і взаємодія з користувачем
class Program
{
    static void Main()
    {
        UpravlinnyaSlovnykamy.Init(); // підготувати папки
        var zminnaVsi = UpravlinnyaSlovnykamy.ZavantazhytVsi();

        while (true) // головне меню
        {
            Console.WriteLine(" Програма: Словники ");
            Console.WriteLine("1. Створити словник");
            Console.WriteLine("2. Вiдкрити словник");
            Console.WriteLine("3. Показати список словникiв");
            Console.WriteLine("4. Видалити словник");
            Console.WriteLine("0. Вийти");
            Console.Write("Вибір: ");
            var zminnaChoice = Console.ReadLine();

            if (zminnaChoice == "1")
            {
                FuncCreate(zminnaVsi);
            }
            else if (zminnaChoice == "2")
            {
                FuncOpen(zminnaVsi);
            }
            else if (zminnaChoice == "3")
            {
                FuncList(zminnaVsi);
            }
            else if (zminnaChoice == "4")
            {
                FuncDelete(zminnaVsi);
            }
            else if (zminnaChoice == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("Невiрний вибiр. Спробуйте ще.");
            }
            Console.WriteLine();
        }
    }

    // Створення нового словника
    static void FuncCreate(List<Slownik> zminnaVsi)
    {
        Console.Write("Введiть назву словника: ");
        var zminnaName = Console.ReadLine()?.Trim() ?? "";
        if (zminnaVsi.Exists(s => s.Name.Equals(zminnaName, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Словник з такою назвою вже iснує.");
            return;
        }
        Console.Write("Введiть тип словника (наприклад 'український'): ");
        var zminnaType = Console.ReadLine()?.Trim() ?? "";
        var zminnaNew = new Slownik { Name = zminnaName, Type = zminnaType };
        zminnaVsi.Add(zminnaNew);
        UpravlinnyaSlovnykamy.Zberihaty(zminnaNew);
        Console.WriteLine("Словниик створено.");
    }

    // Показати список словників
    static void FuncList(List<Slownik> zminnaVsi)
    {
        if (zminnaVsi.Count == 0) { Console.WriteLine("Немає словникiв."); return; }
        Console.WriteLine("Словники:");
        for (int i = 0; i < zminnaVsi.Count; i++)
        {
            var s = zminnaVsi[i];
            Console.WriteLine($"{i + 1}. {s.Name} ({s.Type}) - {s.Entries.Count} слів");
        }
    }

    // Видалити словник (з файлу і з пам'яті)
    static void FuncDelete(List<Slownik> zminnaVsi)
    {
        FuncList(zminnaVsi);
        Console.Write("Номер словника для видалення: ");
        if (!int.TryParse(Console.ReadLine(), out int zminnaNum) || zminnaNum < 1 || zminnaNum > zminnaVsi.Count)
        {
            Console.WriteLine("Невiрний номер.");
            return;
        }
        var zminnaSel = zminnaVsi[zminnaNum - 1];
        UpravlinnyaSlovnykamy.ВидалитиФайл(zminnaSel);
        zminnaVsi.RemoveAt(zminnaNum - 1);
        Console.WriteLine("Словник видалено.");
    }

    // Відкрити словник
    static void FuncOpen(List<Slownik> zminnaVsi)
    {
        FuncList(zminnaVsi);
        Console.Write("Номер словника для вiдкриття: ");
        if (!int.TryParse(Console.ReadLine(), out int zminnaNum) || zminnaNum < 1 || zminnaNum > zminnaVsi.Count)
        {
            Console.WriteLine("Невiрний номер.");
            return;
        }
        var zminnaSel = zminnaVsi[zminnaNum - 1];

        while (true) // підменю словника
        {
            Console.WriteLine();
            Console.WriteLine($" Словник: {zminnaSel.Name} ({zminnaSel.Type}) ---");
            Console.WriteLine("1. Додати слово та переклади");
            Console.WriteLine("2. Замiнити слово");
            Console.WriteLine("3. Додати переклад до iснуючого слова");
            Console.WriteLine("4. Замiнити переклад");
            Console.WriteLine("5. Видалити переклад");
            Console.WriteLine("6. Видалити слово");
            Console.WriteLine("7. Шукати переклад слова");
            Console.WriteLine("8. Експортувати слово i переклади у файл");
            Console.WriteLine("0. Назад");
            Console.Write("Вибір: ");
            var zminnaChoice = Console.ReadLine();

            if (zminnaChoice == "1") { AddWord(zminnaSel); UpravlinnyaSlovnykamy.Zberihaty(zminnaSel); }
            else if (zminnaChoice == "2") { ReplaceWord(zminnaSel); UpravlinnyaSlovnykamy.Zberihaty(zminnaSel); }
            else if (zminnaChoice == "3") { AddTranslation(zminnaSel); UpravlinnyaSlovnykamy.Zberihaty(zminnaSel); }
            else if (zminnaChoice == "4") { ReplaceTranslation(zminnaSel); UpravlinnyaSlovnykamy.Zberihaty(zminnaSel); }
            else if (zminnaChoice == "5") { DeleteTranslation(zminnaSel); UpravlinnyaSlovnykamy.Zberihaty(zminnaSel); }
            else if (zminnaChoice == "6") { DeleteWord(zminnaSel, zminnaVsi); }
            else if (zminnaChoice == "7") { SearchWord(zminnaSel); }
            else if (zminnaChoice == "8") { ExportWord(zminnaSel); }
            else if (zminnaChoice == "0") { break; }
            else Console.WriteLine("Невiрний вибiр.");
        }
    }

    // Додати слово з кількома перекладами (через кому)
    static void AddWord(Slownik zminnaSel)
    {
        Console.Write("Введiть слово: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (zminnaWord == "") { Console.WriteLine("Пусто."); return; }
        if (zminnaSel.Entries.ContainsKey(zminnaWord))
        {
            Console.WriteLine("Слово вже є. Використайте додати переклад.");
            return;
        }
        Console.Write("Введiть переклади (через кому): ");
        var zminnaTrans = Console.ReadLine() ?? "";
        var zminnaList = new List<string>();
        foreach (var t in zminnaTrans.Split(',', StringSplitOptions.RemoveEmptyEntries))
            zminnaList.Add(t.Trim());
        if (zminnaList.Count == 0) zminnaList.Add("");
        zminnaSel.Entries[zminnaWord] = zminnaList;
        Console.WriteLine("Слово додано.");
    }

    // Додати переклад до існуючого слова
    static void AddTranslation(Slownik zminnaSel)
    {
        Console.Write("Введiть слово: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (!zminnaSel.Entries.ContainsKey(zminnaWord))
        {
            Console.WriteLine("Слово вiдсутнє.");
            return;
        }
        Console.Write("Введiть новий переклад: ");
        var zminnaNewTrans = Console.ReadLine()?.Trim() ?? "";
        if (zminnaNewTrans == "") { Console.WriteLine("Пустий переклад."); return; }
        zminnaSel.Entries[zminnaWord].Add(zminnaNewTrans);
        Console.WriteLine("Переклад додано.");
    }

    // Замінити слово (ключ) - всі переклади залишаються
    static void ReplaceWord(Slownik zminnaSel)
    {
        Console.Write("Введiть старе слово: ");
        var zminnaOld = Console.ReadLine()?.Trim() ?? "";
        if (!zminnaSel.Entries.ContainsKey(zminnaOld)) { Console.WriteLine("Нема такого слова."); return; }
        Console.Write("Введiть нове слово: ");
        var zminnaNew = Console.ReadLine()?.Trim() ?? "";
        if (zminnaNew == "") { Console.WriteLine("Неможливо."); return; }
        if (zminnaSel.Entries.ContainsKey(zminnaNew)) { Console.WriteLine("Нове слово вже iснує."); return; }
        var zminnaVals = zminnaSel.Entries[zminnaOld];
        zminnaSel.Entries.Remove(zminnaOld);
        zminnaSel.Entries[zminnaNew] = zminnaVals;
        Console.WriteLine("Слово замiнено.");
    }

    // Замінити один переклад на інший для слова
    static void ReplaceTranslation(Slownik zminnaSel)
    {
        Console.Write("Введiть слово: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (!zminnaSel.Entries.ContainsKey(zminnaWord)) { Console.WriteLine("Нема такого слова."); return; }
        var zminnaList = zminnaSel.Entries[zminnaWord];
        Console.WriteLine("Існуючi переклади:");
        for (int i = 0; i < zminnaList.Count; i++) Console.WriteLine($"{i + 1}. {zminnaList[i]}");
        Console.Write("Номер перекладу для замiни: ");
        if (!int.TryParse(Console.ReadLine(), out int zminnaIdx) || zminnaIdx < 1 || zminnaIdx > zminnaList.Count) { Console.WriteLine("Невірний номер."); return; }
        Console.Write("Введiть новий текст перекладу: ");
        var zminnaNew = Console.ReadLine() ?? "";
        zminnaList[zminnaIdx - 1] = zminnaNew.Trim();
        Console.WriteLine("Переклад замiнено.");
    }

    // Видалити переклад, але не можна видалити останній переклад
    static void DeleteTranslation(Slownik zminnaSel)
    {
        Console.Write("Введiть слово: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (!zminnaSel.Entries.ContainsKey(zminnaWord)) { Console.WriteLine("Нема такого слова."); return; }
        var zminnaList = zminnaSel.Entries[zminnaWord];
        if (zminnaList.Count <= 1) { Console.WriteLine("Не можна видалити останнiй переклад."); return; }
        Console.WriteLine("Переклади:");
        for (int i = 0; i < zminnaList.Count; i++) Console.WriteLine($"{i + 1}. {zminnaList[i]}");
        Console.Write("Номер перекладу для видалення: ");
        if (!int.TryParse(Console.ReadLine(), out int zminnaIdx) || zminnaIdx < 1 || zminnaIdx > zminnaList.Count) { Console.WriteLine("Невiрний номер."); return; }
        zminnaList.RemoveAt(zminnaIdx - 1);
        Console.WriteLine("Переклад видалено.");
    }

    // Видалити слово разом з усіма перекладами
    static void DeleteWord(Slownik zminnaSel, List<Slownik> zminnaVsi)
    {
        Console.Write("Введiть слово для видалення: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (!zminnaSel.Entries.ContainsKey(zminnaWord)) { Console.WriteLine("Нема такого слова."); return; }
        zminnaSel.Entries.Remove(zminnaWord);
        UpravlinnyaSlovnykamy.Zberihaty(zminnaSel); // зберегти після видалення
        Console.WriteLine("Слово видалено.");
    }

    // Шукати переклад слова
    static void SearchWord(Slownik zminnaSel)
    {
        Console.Write("Введiть слово для пошуку: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (zminnaSel.Entries.TryGetValue(zminnaWord, out var zminnaList))
        {
            Console.WriteLine("Знайдено переклади:");
            foreach (var t in zminnaList) Console.WriteLine($"- {t}");
        }
        else
        {
            Console.WriteLine("Переклад не знайдено.");
        }
    }

    // Експортувати слово і його переклади у текстовий файл
    static void ExportWord(Slownik zminnaSel)
    {
        Console.Write("Введiть слово для експорту: ");
        var zminnaWord = Console.ReadLine()?.Trim() ?? "";
        if (!zminnaSel.Entries.ContainsKey(zminnaWord)) { Console.WriteLine("Нема такого слова."); return; }
        var zminnaList = zminnaSel.Entries[zminnaWord];
        var zminnaFileName = $"{SanitizeName(zminnaSel.Name)}_{SanitizeName(zminnaWord)}_export.txt";
        var zminnaPath = Path.Combine("exports", zminnaFileName);
        using (var w = new StreamWriter(zminnaPath))
        {
            w.WriteLine($"Словник: {zminnaSel.Name} ({zminnaSel.Type})");
            w.WriteLine($"Слово: {zminnaWord}");
            w.WriteLine("Переклади:");
            foreach (var t in zminnaList) w.WriteLine($"- {t}");
        }
        Console.WriteLine($"Експортовано у файл: {zminnaPath}");
    }

    // Допоміжна очистка для імен файлів
    static string SanitizeName(string s)
    {
        foreach (var c in Path.GetInvalidFileNameChars()) s = s.Replace(c, '_');
        return s.Replace(' ', '_');
    }
}