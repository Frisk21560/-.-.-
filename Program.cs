namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Виберіть завдання:");
            Console.WriteLine("1 Арифметичний вираз (+, -)");
            Console.WriteLine("2 Шифр Цезаря через CLI");
            Console.WriteLine("3 Перевірка тексту на неприпустимі слова");
            char[] taskChoice = new char[2];
            for (int i = 0; i < 2; i++)
            {
                int input = Console.Read();
                if (input == 10 || input == 13) break;
                taskChoice[i] = (char)input;
            }
            Console.ReadLine(); 

            if (taskChoice[0] == '1')
            {
                // 1
                Console.WriteLine("Введіть арифметичний вираз (наприклад: 23+2-3):");
                char[] inputBuffer = new char[100];
                int inputLength = 0;
                int tempChar = 0;
                while ((tempChar = Console.Read()) != 10 && tempChar != 13 && inputLength < 100)
                {
                    inputBuffer[inputLength++] = (char)tempChar;
                }
                if (tempChar != 10 && tempChar != 13)
                {
                    Console.ReadLine();
                }

                int[] numbers = new int[50];
                char[] operators = new char[50];
                int numbersCount = 0, operatorsCount = 0;
                int currentNumber = 0;
                bool hasNumber = false;
                for (int i = 0; i < inputLength; i++)
                {
                    if (inputBuffer[i] >= '0' && inputBuffer[i] <= '9')
                    {
                        currentNumber = currentNumber * 10 + (inputBuffer[i] - '0');
                        hasNumber = true;
                    }
                    else if (inputBuffer[i] == '+' || inputBuffer[i] == '-')
                    {
                        if (hasNumber)
                        {
                            numbers[numbersCount++] = currentNumber;
                            currentNumber = 0;
                            hasNumber = false;
                        }
                        operators[operatorsCount++] = inputBuffer[i];
                    }
                }
                if (hasNumber)
                {
                    numbers[numbersCount++] = currentNumber;
                }
                int result = numbers[0];
                for (int i = 0; i < operatorsCount; i++)
                {
                    if (operators[i] == '+')
                        result = result + numbers[i + 1];
                    else if (operators[i] == '-')
                        result = result - numbers[i + 1];
                }
                Console.WriteLine("Результат: " + result);
            }
            else if (taskChoice[0] == '2')
            {
                // 2
                char[] text = new char[100];
                int textLength = 0;
                int offset = 0;
                if (args.Length >= 2)
                {
                    string argText = args[0];
                    string argOffset = args[1];
                    textLength = argText.Length;
                    for (int i = 0; i < textLength; i++) text[i] = argText[i];
                    offset = 0;
                    for (int i = 0; i < argOffset.Length; i++)
                        offset = offset * 10 + (argOffset[i] - '0');
                }
                else
                {
                    Console.WriteLine("Введіть текст для шифрування:");
                    int t = 0;
                    while ((t = Console.Read()) != 10 && t != 13 && textLength < 100)
                    {
                        text[textLength++] = (char)t;
                    }
                    if (t != 10 && t != 13) Console.ReadLine();
                    Console.WriteLine("Введіть ключ зсуву:");
                    offset = 0;
                    int temp;
                    while ((temp = Console.Read()) >= '0' && temp <= '9')
                        offset = offset * 10 + (temp - '0');
                    Console.ReadLine();
                }
                char[] encryptedText = new char[textLength];
                for (int i = 0; i < textLength; i++)
                {
                    char c = text[i];
                    if (c >= 'a' && c <= 'z')
                        encryptedText[i] = (char)('a' + (c - 'a' + offset) % 26);
                    else if (c >= 'A' && c <= 'Z')
                        encryptedText[i] = (char)('A' + (c - 'A' + offset) % 26);
                    else
                        encryptedText[i] = c;
                }
                Console.Write("Зашифровано: ");
                for (int i = 0; i < textLength; i++)
                    Console.Write(encryptedText[i]);
                Console.WriteLine();
            }
            else if (taskChoice[0] == '3')
            {
                // 3
                Console.WriteLine("Введіть текст:");
                char[] inputText = new char[800];
                int inputTextLength = 0;
                int textChar = 0;
                while ((textChar = Console.Read()) != 10 && textChar != 13 && inputTextLength < 800)
                {
                    inputText[inputTextLength++] = (char)textChar;
                }
                if (textChar != 10 && textChar != 13) Console.ReadLine();
                Console.WriteLine("Введіть неприпустиме слово:");
                char[] forbiddenWord = new char[30];
                int forbiddenWordLength = 0;
                int forbiddenChar = 0;
                while ((forbiddenChar = Console.Read()) != 10 && forbiddenChar != 13 && forbiddenWordLength < 30)
                {
                    forbiddenWord[forbiddenWordLength++] = (char)forbiddenChar;
                }
                if (forbiddenChar != 10 && forbiddenChar != 13) Console.ReadLine();

                int replacements = 0;
                for (int i = 0; i <= inputTextLength - forbiddenWordLength; i++)
                {
                    bool match = true;
                    for (int j = 0; j < forbiddenWordLength; j++)
                    {
                        if (inputText[i + j] != forbiddenWord[j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        for (int j = 0; j < forbiddenWordLength; j++)
                        {
                            inputText[i + j] = '*';
                        }
                        replacements++;
                        i = i + forbiddenWordLength - 1;
                    }
                }
                Console.WriteLine("Результат:");
                for (int i = 0; i < inputTextLength; i++) Console.Write(inputText[i]);
                Console.WriteLine();
                Console.WriteLine("Статистика: " + replacements + " заміни(а) слова.");
            }
            else
            {
                Console.WriteLine("Вибране невірно!");
            }
        }
    }
}