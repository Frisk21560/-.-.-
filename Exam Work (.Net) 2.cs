
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

// Простi моделі даних

public class zminnaUser
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = ""; 
    public string BirthDate { get; set; } = ""; 
    public bool IsEditor { get; set; } = false; // якщо true - може редагувати вiкторини
    public List<zminnaResult> Results { get; set; } = new();
}

public class zminnaResult
{
    public string QuizName { get; set; } = "";
    public int Score { get; set; }
    public int Total { get; set; }
    public string DateTimeISO { get; set; } = "";
}

public class zminnaQuiz
{
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public List<zminnaQuestion> Questions { get; set; } = new();
}

public class zminnaQuestion
{
    public string Text { get; set; } = "";
    public List<string> Options { get; set; } = new();
    public List<int> CorrectIndexes { get; set; } = new(); // iндекси правильних варiантiв (0-based)
}

// Клас для роботи з файлами та даними 
public static class zminnaStorage
{
    private static string folderData = Path.Combine("data");
    private static string folderQuizzes = Path.Combine(folderData, "quizzes");
    private static string fileUsers = Path.Combine(folderData, "users.json");

    // Iнiцiалiзацiя тек i стартових файлів
    public static void Init()
    {
        if (!Directory.Exists(folderData)) Directory.CreateDirectory(folderData);
        if (!Directory.Exists(folderQuizzes)) Directory.CreateDirectory(folderQuizzes);

        // якщо користувачiв нема - створимо дефолтного admin (пароль admin)
        if (!File.Exists(fileUsers))
        {
            var lst = new List<zminnaUser>();
            var admin = new zminnaUser
            {
                Login = "admin",
                Password = "admin",
                BirthDate = "2000-01-01",
                IsEditor = true
            };
            lst.Add(admin);
            SaveUsers(lst);
        }

        // Якщо нема жодної вiкторини - створити прикладнi украiнською (пo 10 питань)
        var files = Directory.GetFiles(folderQuizzes, "*.json");
        if (files.Length == 0)
        {
            CreateSampleQuizzesUA();
        }
    }

    // Завантажити всiх користувачiв
    public static List<zminnaUser> LoadUsers()
    {
        try
        {
            var txt = File.ReadAllText(fileUsers);
            var lst = JsonSerializer.Deserialize<List<zminnaUser>>(txt);
            if (lst == null) return new List<zminnaUser>();
            return lst;
        }
        catch { return new List<zminnaUser>(); }
    }

    public static void SaveUsers(List<zminnaUser> lst)
    {
        var txt = JsonSerializer.Serialize(lst, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileUsers, txt);
    }

    // Завантажити всi вiкторини
    public static List<zminnaQuiz> LoadQuizzes()
    {
        var res = new List<zminnaQuiz>();
        foreach (var f in Directory.GetFiles(folderQuizzes, "*.json"))
        {
            try
            {
                var txt = File.ReadAllText(f);
                var q = JsonSerializer.Deserialize<zminnaQuiz>(txt);
                if (q != null) res.Add(q);
            }
            catch { }
        }
        return res;
    }

    public static void SaveQuiz(zminnaQuiz q)
    {
        var name = SanitizeFileName(q.Name);
        var path = Path.Combine(folderQuizzes, name + ".json");
        var txt = JsonSerializer.Serialize(q, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, txt);
    }

    public static void DeleteQuiz(zminnaQuiz q)
    {
        var name = SanitizeFileName(q.Name);
        var path = Path.Combine(folderQuizzes, name + ".json");
        if (File.Exists(path)) File.Delete(path);
    }

    private static string SanitizeFileName(string s)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            s = s.Replace(c, '_');
        return s.Replace(' ', '_');
    }

    private static void CreateSampleQuizzesUA()
    {
        // Категорiя: Istoriya
        var qIstoriya = new zminnaQuiz
        {
            Name = "Zagalna Istoriya - osnovy",
            Category = "Istoriya",
            Questions = new List<zminnaQuestion>
            {
                new zminnaQuestion {
                    Text = "Хто був першим президентом США?",
                    Options = new List<string>{"Джордж Вашингтон","Томас Джефферсон","Абрахам Лiнкольн","Джон Адамс"},
                    CorrectIndexes = new List<int>{0}
                },
                new zminnaQuestion {
                    Text = "В якому роцi впав Захiдний Рим?",
                    Options = new List<string>{"410","476","532","1453"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Хто керував СРСР у часи Другоi свiтовоi вiйни?",
                    Options = new List<string>{"Володимир Ленiн","Йосип Сталiн","Микита Хрущов","Михайло Горбачов"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Де була підписана Magna Carta?",
                    Options = new List<string>{"Вестмінстер","Раннимiд","Кентерберi","Оксфорд"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Який рiк вважають початком Французької революцiї?",
                    Options = new List<string>{"1776","1789","1804","1815"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Яка цивiлiзацiя побудувала Мачу-Пiчу?",
                    Options = new List<string>{"Майя","Ацтеки","Інки","Ольмеки"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Який корабель потонув у 1912 роцi?",
                    Options = new List<string>{"Lusitania","Britannic","Titanic","Olympic"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Хто вiйшов у союзники у Другiй свiтовiй вiйнi з СРСР?",
                    Options = new List<string>{"США","Нiмеччина","Францiя","Японiя"},
                    CorrectIndexes = new List<int>{0}
                },
                new zminnaQuestion {
                    Text = "Хто 'вiдкрив' Америку у 1492 (європейський навiгатор)?",
                    Options = new List<string>{"Васко да Гама","Фернан Магеллан","Крiстофор Колумб","Амерiго Веспуччi"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Якi з перелiчених були великими iмперiями на початку 20 ст? (вибрати всi правильнi)",
                    Options = new List<string>{"Османська iмперiя","Британська iмперiя","Росiйська iмперiя","Королiвство Iспанiя"},
                    CorrectIndexes = new List<int>{0,1,2} // мультi-вiдповiдь
                }
            }
        };

        // Категорiя: Geografiya
        var qGeografiya = new zminnaQuiz
        {
            Name = "Zagalna Geografiya - osnovy",
            Category = "Geografiya",
            Questions = new List<zminnaQuestion>
            {
                new zminnaQuestion {
                    Text = "Який океан найбiльший на Землi?",
                    Options = new List<string>{"Атлантичний океан","Тихий океан","Iндiйський океан","Пiвнiчний Льодовитий океан"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Яка рiчка часто вважається найдовшою?",
                    Options = new List<string>{"Амазонка","Нiл","Янцзи","Мiссiсiпi"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Яка гора є найвищою над рiвнем моря?",
                    Options = new List<string>{"К2","Канченджанга","Еверест","Лхоцзе"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Яка столиця Францiї?",
                    Options = new List<string>{"Лiон","Марсель","Париж","Нiца"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Якi з перелiчених країн є членами ЄС? (вибрати всi правильнi)",
                    Options = new List<string>{"Нiмеччина","Швейцарiя","Францiя","Бразилiя"},
                    CorrectIndexes = new List<int>{0,2}
                },
                new zminnaQuestion {
                    Text = "На якому континентi переважно знаходиться Єгипет?",
                    Options = new List<string>{"Азія","Африка","Європа","Пiвденна Америка"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Яка країна має найбiльше населення?",
                    Options = new List<string>{"Iндія","США","Китай","Iндонезiя"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Який штат США на захiдному узбережжi?",
                    Options = new List<string>{"Техас","Флорида","Калiфорнiя","Огайо"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Які країни кордонять з Нiмеччиною? (вибрати всi правильнi)",
                    Options = new List<string>{"Францiя","Польща","Iспанiя","Австрiя"},
                    CorrectIndexes = new List<int>{0,1,3}
                },
                new zminnaQuestion {
                    Text = "Де розташована пустеля Сахара?",
                    Options = new List<string>{"Азія","Африка","Австралiя","Пiвнiчна Америка"},
                    CorrectIndexes = new List<int>{1}
                }
            }
        };

        // Категорiя: Biologiya
        var qBiologiya = new zminnaQuiz
        {
            Name = "Osnovy Biologii",
            Category = "Biologiya",
            Questions = new List<zminnaQuestion>
            {
                new zminnaQuestion {
                    Text = "Яка базова одиниця життя?",
                    Options = new List<string>{"Атом","Молекула","Клетина","Тканина"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Що означає абревiатура DNA?",
                    Options = new List<string>{"Деоксирибонуклеїнова кислота","Рибонуклеїнова кислота","Деоксирибонуклеїновий амiн","Дикарбоксилова кислота"},
                    CorrectIndexes = new List<int>{0}
                },
                new zminnaQuestion {
                    Text = "Де в рослиннiй клетинi вiдбувається фотосинтез?",
                    Options = new List<string>{"Мiтoхондрiя","Хлоропласт","Ядро","Рибосома"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Скiльки хромосом мають бiльшiсть людських соматичних клетин?",
                    Options = new List<string>{"23","46","92","44"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Які з перелiчених тварин є ссавцями? (вибрати всi правильнi)",
                    Options = new List<string>{"Кит","Акула","Лiтаюча миш","Жаба"},
                    CorrectIndexes = new List<int>{0,2}
                },
                new zminnaQuestion {
                    Text = "Яка група кровi вважається унiверсальним донором (негативна)?",
                    Options = new List<string>{"A+","O-","AB+","B-"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Який газ видiляють рослини пiд час фотосинтезу?",
                    Options = new List<string>{"Дiоксид вуглецю","Кисень","Азот","Метан"},
                    CorrectIndexes = new List<int>{1}
                },
                new zminnaQuestion {
                    Text = "Який орган всмоктує бiльшiсть поживних речовин з травленої їжi?",
                    Options = new List<string>{"Шлунок","Товста кишка","Тонка кишка","Печiнка"},
                    CorrectIndexes = new List<int>{2}
                },
                new zminnaQuestion {
                    Text = "Якi органiзми є прокарiотами? (вибрати всi правильнi)",
                    Options = new List<string>{"Бактерiї","Гриби","Археї","Рослини"},
                    CorrectIndexes = new List<int>{0,2}
                },
                new zminnaQuestion {
                    Text = "Який фермент допомагає розщеплювати бiлки?",
                    Options = new List<string>{"Лiпаза","Амилаза","Протеаза","Лактаза"},
                    CorrectIndexes = new List<int>{2}
                }
            }
        };

        // Зберегти всi прикладнi вiкторини
        SaveQuiz(qIstoriya);
        SaveQuiz(qGeografiya);
        SaveQuiz(qBiologiya);
    }
}

// Головна програма: меню, логiн/рeєстрацiя, користувацький iнтерфейс i редактор
class Program
{
    static void Main()
    {
        zminnaStorage.Init(); // пiдготувати текi та дефолтних користувачiв
        var users = zminnaStorage.LoadUsers();
        var quizzes = zminnaStorage.LoadQuizzes();

        Console.WriteLine("Ласкаво просимо до вiкторини!");
        // Вхід або реєстрацiя
        zminnaUser curr = null;
        while (curr == null)
        {
            Console.WriteLine();
            Console.WriteLine("1) Увiйти");
            Console.WriteLine("2) Зарeєструватися");
            Console.WriteLine("0) Вихiд");
            Console.Write("Вибiр: ");
            var ch = Console.ReadLine()?.Trim();
            if (ch == "1")
            {
                curr = FuncLogin(users);
            }
            else if (ch == "2")
            {
                curr = FuncRegister(users);
                if (curr != null)
                {
                    users.Add(curr);
                    zminnaStorage.SaveUsers(users);
                }
            }
            else if (ch == "0")
            {
                Console.WriteLine("До побачення!");
                return;
            }
            else Console.WriteLine("Невiрний вибiр.");
        }

        // Головне меню пiсля входу
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"Привiт, {curr.Login}!");
            Console.WriteLine("1) Стартувати нову вiкторину");
            Console.WriteLine("2) Мої минулi результати");
            Console.WriteLine("3) Top-20 за вiкториною");
            Console.WriteLine("4) Налаштування (змiнити пароль/дату)");
            if (curr.IsEditor) Console.WriteLine("5) Редактор вiкторин (створити/редагувати)");
            Console.WriteLine("0) Вийти (вихiд з акаунту)");
            Console.Write("Вибiр: ");
            var zch = Console.ReadLine()?.Trim();
            if (zch == "1")
            {
                quizzes = zminnaStorage.LoadQuizzes(); // оновити
                FuncStartQuiz(curr, users, quizzes);
            }
            else if (zch == "2")
            {
                FuncShowMyResults(curr);
            }
            else if (zch == "3")
            {
                quizzes = zminnaStorage.LoadQuizzes();
                FuncShowTop20(quizzes, users);
            }
            else if (zch == "4")
            {
                FuncSettings(curr, users);
            }
            else if (zch == "5" && curr.IsEditor)
            {
                quizzes = zminnaStorage.LoadQuizzes();
                FuncEditor(curr, users, quizzes);
                // пiсля роботи в редакторi оновимо локальнi данi
                quizzes = zminnaStorage.LoadQuizzes();
                users = zminnaStorage.LoadUsers();
            }
            else if (zch == "0")
            {
                Console.WriteLine("Вихiд з аккаунта.");
                break;
            }
            else
            {
                Console.WriteLine("Невiрний вибiр.");
            }
        }
    }

    // Функцiя входу
    static zminnaUser FuncLogin(List<zminnaUser> users)
    {
        Console.Write("Логiн: ");
        var login = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Пароль: ");
        var pass = ReadPassword();
        var u = users.FirstOrDefault(x => x.Login.Equals(login, StringComparison.OrdinalIgnoreCase) && x.Password == pass);
        if (u == null) { Console.WriteLine("Невiрний логiн або пароль."); return null; }
        Console.WriteLine("Вдало.");
        return u;
    }

    // Функцiя рeєстрацiї
    static zminnaUser FuncRegister(List<zminnaUser> users)
    {
        Console.Write("Введiть бажаний логiн: ");
        var login = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrEmpty(login)) { Console.WriteLine("Порожнiй логiн."); return null; }
        if (users.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase))) { Console.WriteLine("Такий логiн вже зареєстровано."); return null; }
        Console.Write("Пароль: ");
        var pass = ReadPassword();
        Console.Write("Пiдтвердження пароля: ");
        var pass2 = ReadPassword();
        if (pass != pass2) { Console.WriteLine("Паролi не збiгаються."); return null; }
        Console.Write("Дата народження (yyyy-MM-dd): ");
        var bd = Console.ReadLine()?.Trim() ?? "";
        // простий синтакс вiдсiв
        if (!DateTime.TryParse(bd, out var tmp)) { Console.WriteLine("Невiрна дата."); return null; }
        var newu = new zminnaUser { Login = login, Password = pass, BirthDate = tmp.ToString("yyyy-MM-dd"), IsEditor = false };
        Console.WriteLine("Реєстрацiя пройшла успiшно.");
        return newu;
    }

    // Почати вiкторину
    static void FuncStartQuiz(zminnaUser curr, List<zminnaUser> users, List<zminnaQuiz> quizzes)
    {
        if (quizzes.Count == 0) { Console.WriteLine("Нема вiкторин. Редактор може створити."); return; }
        Console.WriteLine("Оберiть роздiл або 0 для змiшаної вiкторини:");
        var cats = quizzes.Select(q => q.Category).Distinct().ToList();
        for (int i = 0; i < cats.Count; i++) Console.WriteLine($"{i + 1}) {cats[i]}");
        Console.Write("Вибiр: ");
        var ch = Console.ReadLine()?.Trim();
        List<zminnaQuestion> pool = new();
        if (ch == "0")
        {
            // взяти питання з усiх вiкторин
            foreach (var qz in quizzes) pool.AddRange(qz.Questions);
        }
        else if (int.TryParse(ch, out int idx) && idx >= 1 && idx <= cats.Count)
        {
            var sel = cats[idx - 1];
            foreach (var qz in quizzes.Where(q => q.Category == sel)) pool.AddRange(qz.Questions);
        }
        else { Console.WriteLine("Невiрний вибiр."); return; }

        if (pool.Count == 0) { Console.WriteLine("Нема питань у вибраному роздiлi."); return; }

        // Вибрати 20 випадкових питань (без повторiв, або з повтором якщо недостатньо)
        var rnd = new Random();
        var selected = new List<zminnaQuestion>();
        var poolCopy = new List<zminnaQuestion>(pool);
        while (selected.Count < 20)
        {
            if (poolCopy.Count == 0) poolCopy = new List<zminnaQuestion>(pool); // повторити пул якщо закiнчився
            int i = rnd.Next(poolCopy.Count);
            selected.Add(poolCopy[i]);
            poolCopy.RemoveAt(i);
        }

        // Проходження питань
        int correct = 0;
        for (int qi = 0; qi < selected.Count; qi++)
        {
            var q = selected[qi];
            Console.WriteLine();
            Console.WriteLine($"Питання {qi + 1}: {q.Text}");
            for (int o = 0; o < q.Options.Count; o++)
            {
                Console.WriteLine($"  {o + 1}) {q.Options[o]}");
            }
            Console.WriteLine("Введiть номери вiдповiдей через кому (наприклад 1,3). Якщо одна вiдповiдь - просто номер:");
            Console.Write("Ваша вiдповiдь: ");
            var ansLine = Console.ReadLine() ?? "";
            var parts = ansLine.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => int.TryParse(s, out _))
                .Select(s => int.Parse(s) - 1)
                .Where(i => i >= 0 && i < q.Options.Count)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var correctSorted = q.CorrectIndexes.OrderBy(x => x).ToList();
            // вважаємо вiдповiдь правильною лише коли точно вказано всi правильнi i нiчого зайвого
            if (parts.SequenceEqual(correctSorted)) { correct++; Console.WriteLine("Правильно!"); }
            else { Console.WriteLine("Неправильно."); }
        }

        Console.WriteLine();
        Console.WriteLine($"Результат: {correct} / {selected.Count}");

        // Збережемо результат у користувача
        var res = new zminnaResult { QuizName = "(сесiя)", Score = correct, Total = selected.Count, DateTimeISO = DateTime.UtcNow.ToString("o") };

        // Для кращого відображення збережемо назву вiкторини як "Mixed" або конкретну кат.
        string quizLabel = "Mixed";
        var catsOfSelected = quizzes
            .Where(qz => qz.Questions.Intersect(selected).Any())
            .Select(qz => qz.Category)
            .Distinct()
            .ToList();
        if (catsOfSelected.Count == 1) quizLabel = catsOfSelected[0];
        res.QuizName = quizLabel;

        curr.Results.Add(res);
        // оновити файл користувачiв
        var allUsers = zminnaStorage.LoadUsers();
        var idxu = allUsers.FindIndex(u => u.Login.Equals(curr.Login, StringComparison.OrdinalIgnoreCase));
        if (idxu >= 0)
        {
            allUsers[idxu] = curr;
            zminnaStorage.SaveUsers(allUsers);
        }
        else
        {
            // якщо з якоюсь причиною не знайдено - додати
            allUsers.Add(curr);
            zminnaStorage.SaveUsers(allUsers);
        }

        // Показати мiсце у таблицi результатiв для цiєї вiкторини
        var all = zminnaStorage.LoadUsers();
        var listResultsForQuiz = new List<(string login, int score, string dt)>();
        foreach (var u in all)
        {
            foreach (var r in u.Results)
            {
                if (r.QuizName == res.QuizName)
                {
                    listResultsForQuiz.Add((u.Login, r.Score, r.DateTimeISO));
                }
            }
        }
        // сортирування: за результатом (score desc), потiм за датою (ранiше краще)
        var sorted = listResultsForQuiz.OrderByDescending(x => x.score).ThenBy(x => x.dt).ToList();
        int place = sorted.FindIndex(x => x.login == curr.Login && x.score == res.Score && x.dt == res.DateTimeISO) + 1;
        Console.WriteLine($"Ваше мiсце у таблицi для \"{res.QuizName}\": {place} з {sorted.Count}");
    }

    // Показати мої результати
    static void FuncShowMyResults(zminnaUser curr)
    {
        if (curr.Results == null || curr.Results.Count == 0) { Console.WriteLine("Нема минулих результатiв."); return; }
        Console.WriteLine("Мої результати:");
        foreach (var r in curr.Results.OrderByDescending(x => x.DateTimeISO))
        {
            Console.WriteLine($"- {r.DateTimeISO} | {r.QuizName} | {r.Score}/{r.Total}");
        }
    }

    // Показати топ-20 для конкретної вiкторини
    static void FuncShowTop20(List<zminnaQuiz> quizzes, List<zminnaUser> users)
    {
        Console.WriteLine("Оберiть вiкторину або 0 для змiшаної:");
        Console.WriteLine("Доступнi категорiї:");
        var cats = quizzes.Select(q => q.Category).Distinct().ToList();
        for (int i = 0; i < cats.Count; i++) Console.WriteLine($"{i + 1}) {cats[i]}");
        Console.Write("Вибiр: ");
        var ch = Console.ReadLine()?.Trim();
        string quizLabel = "Mixed";
        if (ch == "0") quizLabel = "Mixed";
        else if (int.TryParse(ch, out int idx) && idx >= 1 && idx <= cats.Count) quizLabel = cats[idx - 1];
        else { Console.WriteLine("Невiрний вибiр."); return; }

        // Збираємо результати
        var list = new List<(string login, int score, string dt)>();
        foreach (var u in users)
            foreach (var r in u.Results)
                if (r.QuizName == quizLabel) list.Add((u.Login, r.Score, r.DateTimeISO));
        if (list.Count == 0) { Console.WriteLine("Нема результатiв для цiєї вiкторини."); return; }
        var sorted = list.OrderByDescending(x => x.score).ThenBy(x => x.dt).Take(20).ToList();
        Console.WriteLine($"Top-20 для \"{quizLabel}\":");
        for (int i = 0; i < sorted.Count; i++)
        {
            var it = sorted[i];
            Console.WriteLine($"{i + 1}. {it.login} - {it.score} ({it.dt})");
        }
    }

    // Налаштування: змiнити пароль та дату народження
    static void FuncSettings(zminnaUser curr, List<zminnaUser> users)
    {
        Console.WriteLine("1) Змiнити пароль");
        Console.WriteLine("2) Змiнити дату народження");
        Console.Write("Вибiр: ");
        var ch = Console.ReadLine()?.Trim();
        if (ch == "1")
        {
            Console.Write("Старий пароль: ");
            var oldp = ReadPassword();
            if (oldp != curr.Password) { Console.WriteLine("Невiрний пароль."); return; }
            Console.Write("Новий пароль: ");
            var np = ReadPassword();
            Console.Write("Пiдтвердження: ");
            var np2 = ReadPassword();
            if (np != np2) { Console.WriteLine("Паролi не збiгаються."); return; }
            curr.Password = np;
            // зберегти
            var all = zminnaStorage.LoadUsers();
            var idx = all.FindIndex(u => u.Login.Equals(curr.Login, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0) { all[idx] = curr; zminnaStorage.SaveUsers(all); Console.WriteLine("Пароль змiнено."); }
        }
        else if (ch == "2")
        {
            Console.Write("Нова дата (yyyy-MM-dd): ");
            var bd = Console.ReadLine()?.Trim() ?? "";
            if (!DateTime.TryParse(bd, out var tmp)) { Console.WriteLine("Невiрна дата."); return; }
            curr.BirthDate = tmp.ToString("yyyy-MM-dd");
            var all = zminnaStorage.LoadUsers();
            var idx = all.FindIndex(u => u.Login.Equals(curr.Login, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0) { all[idx] = curr; zminnaStorage.SaveUsers(all); Console.WriteLine("Дата змiнена."); }
        }
        else Console.WriteLine("Невiрний вибiр.");
    }

    // Утилiта редактора вiкторин (потрiбний акаунт з IsEditor)
    static void FuncEditor(zminnaUser curr, List<zminnaUser> users, List<zminnaQuiz> quizzes)
    {
        Console.WriteLine("Ви в редакторi вiкторин.");
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1) Показати всi вiкторини");
            Console.WriteLine("2) Створити нову вiкторину");
            Console.WriteLine("3) Редагувати вiкторину");
            Console.WriteLine("4) Видалити вiкторину");
            Console.WriteLine("0) Назад");
            Console.Write("Вибiр: ");
            var ch = Console.ReadLine()?.Trim();
            if (ch == "1")
            {
                quizzes = zminnaStorage.LoadQuizzes();
                if (quizzes.Count == 0) Console.WriteLine("Нема вiкторин.");
                else
                {
                    for (int i = 0; i < quizzes.Count; i++)
                        Console.WriteLine($"{i + 1}) {quizzes[i].Name} [{quizzes[i].Category}] ({quizzes[i].Questions.Count} пит.)");
                }
            }
            else if (ch == "2")
            {
                Console.Write("Назва нової вiкторини: ");
                var name = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Категорiя: ");
                var cat = Console.ReadLine()?.Trim() ?? "";
                var q = new zminnaQuiz { Name = name, Category = cat };
                zminnaStorage.SaveQuiz(q);
                Console.WriteLine("Вiкторина створена.");
                quizzes = zminnaStorage.LoadQuizzes();
            }
            else if (ch == "3")
            {
                quizzes = zminnaStorage.LoadQuizzes();
                if (quizzes.Count == 0) { Console.WriteLine("Нема вiкторин."); continue; }
                for (int i = 0; i < quizzes.Count; i++)
                    Console.WriteLine($"{i + 1}) {quizzes[i].Name} [{quizzes[i].Category}]");
                Console.Write("Номер вiкторини для редагування: ");
                if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > quizzes.Count) { Console.WriteLine("Невiрний номер."); continue; }
                var sel = quizzes[idx - 1];
                EditQuizLoop(sel);
                zminnaStorage.SaveQuiz(sel);
                quizzes = zminnaStorage.LoadQuizzes();
            }
            else if (ch == "4")
            {
                quizzes = zminnaStorage.LoadQuizzes();
                if (quizzes.Count == 0) { Console.WriteLine("Нема вiкторин."); continue; }
                for (int i = 0; i < quizzes.Count; i++)
                    Console.WriteLine($"{i + 1}) {quizzes[i].Name} [{quizzes[i].Category}]");
                Console.Write("Номер вiкторини для видалення: ");
                if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > quizzes.Count) { Console.WriteLine("Невiрний номер."); continue; }
                var sel = quizzes[idx - 1];
                zminnaStorage.DeleteQuiz(sel);
                quizzes.RemoveAt(idx - 1);
                Console.WriteLine("Вiкторина видалена.");
            }
            else if (ch == "0") break;
            else Console.WriteLine("Невiрний вибiр.");
        }
    }

    // Цикл редагування однiєї вiкторини
    static void EditQuizLoop(zminnaQuiz sel)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"Редагуємо: {sel.Name} [{sel.Category}] - питань: {sel.Questions.Count}");
            Console.WriteLine("1) Додати питання");
            Console.WriteLine("2) Редагувати питання");
            Console.WriteLine("3) Видалити питання");
            Console.WriteLine("4) Змiнити назву/категорiю");
            Console.WriteLine("0) Назад");
            Console.Write("Вибiр: ");
            var ch = Console.ReadLine()?.Trim();
            if (ch == "1")
            {
                var q = BuildQuestionInteractive();
                if (q != null) { sel.Questions.Add(q); Console.WriteLine("Питання додано."); }
            }
            else if (ch == "2")
            {
                if (sel.Questions.Count == 0) { Console.WriteLine("Нема питань."); continue; }
                for (int i = 0; i < sel.Questions.Count; i++) Console.WriteLine($"{i + 1}) {sel.Questions[i].Text}");
                Console.Write("Номер питання для редагування: ");
                if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > sel.Questions.Count) { Console.WriteLine("Невiрний номер."); continue; }
                var q = sel.Questions[idx - 1];
                EditQuestionInteractive(q);
                Console.WriteLine("Питання оновлено.");
            }
            else if (ch == "3")
            {
                if (sel.Questions.Count == 0) { Console.WriteLine("Нема питань."); continue; }
                for (int i = 0; i < sel.Questions.Count; i++) Console.WriteLine($"{i + 1}) {sel.Questions[i].Text}");
                Console.Write("Номер питання для видалення: ");
                if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > sel.Questions.Count) { Console.WriteLine("Невiрний номер."); continue; }
                sel.Questions.RemoveAt(idx - 1);
                Console.WriteLine("Питання видалено.");
            }
            else if (ch == "4")
            {
                Console.Write("Нова назва (залишити порожнiм щоб не змiнити): ");
                var n = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(n)) sel.Name = n.Trim();
                Console.Write("Нова категорiя (залишити порожнiм щоб не змiнити): ");
                var c = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(c)) sel.Category = c.Trim();
                Console.WriteLine("Оновлено.");
            }
            else if (ch == "0") break;
            else Console.WriteLine("Невiрний вибiр.");
        }
    }

    // Побудова питання через iнтерфейс
    static zminnaQuestion BuildQuestionInteractive()
    {
        Console.Write("Текст питання: ");
        var txt = Console.ReadLine() ?? "";
        var opts = new List<string>();
        while (true)
        {
            Console.Write($"Додати варiант вiдповiдi #{opts.Count + 1} (або пусто для завершення): ");
            var o = Console.ReadLine();
            if (string.IsNullOrEmpty(o)) break;
            opts.Add(o);
        }
        if (opts.Count < 2) { Console.WriteLine("Потрiбно хоча б 2 варiанти."); return null; }
        Console.WriteLine("Вкажiть номери правильних варiантiв через кому (наприклад 1 або 1,3): ");
        var line = Console.ReadLine() ?? "";
        var idxs = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => int.TryParse(s, out _))
            .Select(s => int.Parse(s) - 1)
            .Where(i => i >= 0 && i < opts.Count)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
        if (idxs.Count == 0) { Console.WriteLine("Потрiбно хоча б один правильний варiант."); return null; }
        return new zminnaQuestion { Text = txt, Options = opts, CorrectIndexes = idxs };
    }

    // Редагування питання
    static void EditQuestionInteractive(zminnaQuestion q)
    {
        Console.WriteLine($"Питання: {q.Text}");
        Console.WriteLine("1) Змiнити текст");
        Console.WriteLine("2) Додати варiант");
        Console.WriteLine("3) Видалити варiант");
        Console.WriteLine("4) Вказати правильнi варiанти");
        Console.Write("Вибiр: ");
        var ch = Console.ReadLine()?.Trim();
        if (ch == "1")
        {
            Console.Write("Новий текст: ");
            var t = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(t)) q.Text = t;
        }
        else if (ch == "2")
        {
            Console.Write("Текст нового варiанту: ");
            var t = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(t)) q.Options.Add(t);
        }
        else if (ch == "3")
        {
            for (int i = 0; i < q.Options.Count; i++) Console.WriteLine($"{i + 1}) {q.Options[i]}");
            Console.Write("Номер для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > q.Options.Count) { Console.WriteLine("Невiрний номер."); return; }
            q.Options.RemoveAt(idx - 1);
            // Оновити індекси правильних варiантiв
            q.CorrectIndexes = q.CorrectIndexes.Where(ci => ci != idx - 1).Select(ci => ci > idx - 1 ? ci - 1 : ci).ToList();
        }
        else if (ch == "4")
        {
            for (int i = 0; i < q.Options.Count; i++) Console.WriteLine($"{i + 1}) {q.Options[i]}");
            Console.WriteLine("Вкажiть номери правильних варiантiв через кому:");
            var line = Console.ReadLine() ?? "";
            var idxs = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => int.TryParse(s, out _))
                .Select(s => int.Parse(s) - 1)
                .Where(i => i >= 0 && i < q.Options.Count)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            if (idxs.Count == 0) { Console.WriteLine("Потрiбно хоча б один."); return; }
            q.CorrectIndexes = idxs;
        }
        else Console.WriteLine("Невiрний вибiр.");
    }

    // Читання пароля без echo
    static string ReadPassword()
    {
        var pass = "";
        ConsoleKeyInfo key;
        while (true)
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace)
            {
                if (pass.Length > 0)
                {
                    pass = pass.Substring(0, pass.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                pass += key.KeyChar;
                Console.Write("*");
            }
        }
        Console.WriteLine();
        return pass;
    }
}