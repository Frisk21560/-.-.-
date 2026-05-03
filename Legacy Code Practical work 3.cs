namespace Legacy_Code_Homework_3
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            // Виводимо час коли почалось все, щоб побачити на скільки це прискорило роботу
            Console.WriteLine($"Час початку: {DateTime.Now}");

            // Тут ми робимо всі задачи одночасно замість того щоб робити їх по черзі
            Task kafu = KafuAsync();
            Task yajcya = YajcyaAsync();
            Task bekon = BekonAsync();
            Task toast = ToastAsync();
            Task sok = SokAsync();

            // WhenAll чекає поки всі задачи закінчаться, а не чекає їх по одній
            await Task.WhenAll(kafu, yajcya, bekon, toast, sok);

            Console.WriteLine($"Час закінчення: {DateTime.Now}");
            Console.WriteLine("Снідданок готовий!");
        }

        // Метод для приготування кави, використовуємо Task.Delay щоб імітувати реальний час готування
        static async Task KafuAsync()
        {
            Console.WriteLine("Почали готувати каву");
            await Task.Delay(2000);
            Console.WriteLine("Кава готова");
        }

        // Метод для яєць, спочатку береми яйця потім кладемо на сковороду, це два кроки але один метод
        static async Task YajcyaAsync()
        {
            Console.WriteLine("Беремо яйця");
            await Task.Delay(1000);
            Console.WriteLine("Кладемо яйця на сковороду");
            await Task.Delay(3000);
            Console.WriteLine("Яйця готові");
        }

        // Метод для бекону, його довше готувати тому більше затримка
        static async Task BekonAsync()
        {
            Console.WriteLine("Клаємо беконь на сковороду");
            await Task.Delay(4000);
            Console.WriteLine("Беконь готовий");
        }

        // Метод для тосту, спочатку беремо хлеб потім кладемо в тостер
        static async Task ToastAsync()
        {
            Console.WriteLine("Беремо хлеб");
            await Task.Delay(500);
            Console.WriteLine("Кладемо в тостер");
            await Task.Delay(2000);
            Console.WriteLine("Тост готовий");
        }

        // Метод для соку, просто наливаємо в склянку
        static async Task SokAsync()
        {
            Console.WriteLine("Наливаємо сік");
            await Task.Delay(1500);
            Console.WriteLine("Сік готовий");
        }
    }
};