namespace Legacy_Code_Homework_6
{
    // ЗАВДАННЯ 1 
    public class Povdomlennya
    {
        // Статичний метод - належить класу, а не об'єкту
        // Тому викликаємо його як Povidomlennya.VivestiPovidomlennya(), без new
        public static void VivestiPovidomlennya(string zmist)
        {
            // Просто виводимо те, що передали в метод
            Console.WriteLine($"Повідомлення: {zmist}");
        }
    }


    //  ЗАВДАННЯ 2

    public class Matematyka
    {
        // Статичний метод для підрахунку факторіалу
        public static long Faktorijal(int chyslo)
        {
            // Якщо число менше або рівне 1, то факторіал = 1
            if (chyslo <= 1)
                return 1;

            // Інакше множимо все від 1 до числа
            long rezultat = 1;
            for (int i = 1; i <= chyslo; i++)
            {
                rezultat *= i;
            }
            return rezultat;
        }

        // Статичний метод для перевірки, чи число просте
        public static bool JeProstim(int chyslo)
        {
            // Якщо число менше 2, то воно не просте
            if (chyslo < 2)
                return false;

            // Перевіряємо, чи число ділиться на будь-яке число від 2 до його половини
            // Якщо ділиться, то воно не просте
            for (int i = 2; i <= chyslo / 2; i++)
            {
                if (chyslo % i == 0)
                    return false;
            }

            // Якщо ні на що не ділиться, то число просте
            return true;
        }

        // Статичний метод для перевірки, чи число парне
        public static bool JeParnym(int chyslo)
        {
            // Якщо остача від ділення на 2 дорівнює 0, то число парне
            return chyslo % 2 == 0;
        }

        // Статичний метод для перевірки, чи число непарне
        public static bool JeNeparnbim(int chyslo)
        {
            // Якщо остача від ділення на 2 дорівнює 1, то число непарне
            return chyslo % 2 != 0;
        }
    }


    //  ЗАВДАННЯ 3 

    public class Drub
    {
        // Змінні класу - чисельник і знаменник
        public int chyselnyk;
        public int znamenyk;
        public Drub(int ch, int zn)
        {
            chyselnyk = ch;
            znamenyk = zn;
        }

        // Метод для виведення дробу на екран
        public void Vyvesty()
        {
            Console.WriteLine($"Дріб: {chyselnyk}/{znamenyk}");
        }

        // Метод для додавання двох дробів
        public void Dodaty(Drub inshyy_drub)
        {
            // Підраховуємо новий чисельник за формулою
            int novyy_chyselnyk = (chyselnyk * inshyy_drub.znamenyk) + (inshyy_drub.chyselnyk * znamenyk);
            // Підраховуємо новий знаменник за формулою
            int novyy_znamenyk = znamenyk * inshyy_drub.znamenyk;

            // Оновлюємо чисельник і знаменник поточного дробу
            chyselnyk = novyy_chyselnyk;
            znamenyk = novyy_znamenyk;
        }

        // Метод для віднімання двох дробів
        public void Vidnyaty(Drub inshyy_drub)
        {
            // Підраховуємо новий чисельник за формулою
            int novyy_chyselnyk = (chyselnyk * inshyy_drub.znamenyk) - (inshyy_drub.chyselnyk * znamenyk);
            // Підраховуємо новий знаменник за формулою
            int novyy_znamenyk = znamenyk * inshyy_drub.znamenyk;

            // Оновлюємо чисельник і знаменник поточного дробу
            chyselnyk = novyy_chyselnyk;
            znamenyk = novyy_znamenyk;
        }

        // Метод для множення двох дробів
        public void Mnozhyty(Drub inshyy_drub)
        {
            // Множимо чисельники між собою
            chyselnyk = chyselnyk * inshyy_drub.chyselnyk;
            // Множимо знаменники між собою
            znamenyk = znamenyk * inshyy_drub.znamenyk;
        }

        // Метод для ділення двох дробів
        public void Derty(Drub inshyy_drub)
        {
            // Множимо перший дріб на обернений другий дріб
            chyselnyk = chyselnyk * inshyy_drub.znamenyk;
            znamenyk = znamenyk * inshyy_drub.chyselnyk;
        }
    }

}