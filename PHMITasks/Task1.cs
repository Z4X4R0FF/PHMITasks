using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PHMITasks
{
    ///<summary>Генерация тестов для арабских чисел и пиктограмм с вариативностью цвета</summary>
    public class Task1
    {
        public Task1()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PrintMessage(ConsoleColor.White, "1 for ArabNumbersTest, 2 for CharTest");
            var task = Console.ReadKey();
            Console.Clear();
            PrintMessage(ConsoleColor.White, "Write test count");
            _testCount = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            switch (task.Key)
            {
                case ConsoleKey.D1:
                    CreateArabNumbersTests();
                    break;
                case ConsoleKey.D2:
                    CreateCharTests();
                    break;
            }
        }

        #region Private fields

        //Список пиктограмм для теста
        private static readonly List<char> PrintableChars = new List<char>
        {
            '\u263A', '\u263B', '\u2665',
            '\u2666', '\u2663', '\u2660',
            '\u2642', '\u2640',
            '\u266A', '\u266B',
        };
        
        //Памятка пользователю
        private static readonly string CharDescription =
            $"1 = {PrintableChars[0]} | 2 = {PrintableChars[1]} |" +
            $" 3 = {PrintableChars[2]} | 4 = {PrintableChars[3]} |" +
            $" 5 = {PrintableChars[4]} | 6 = {PrintableChars[5]} |" +
            $" 11 = {PrintableChars[6]} | 12 = {PrintableChars[7]} |" +
            $" 13 = {PrintableChars[8]} | 14 = {PrintableChars[9]}";

        //Список цифр для теста
        private readonly List<int> _printableNumbers = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        //Список цветов консоли
        private readonly Array _colors = Enum.GetValues(typeof(ConsoleColor));

        //Количество чисел в тесте
        private const int CharCount = 10;

        //Количество тестов
        private readonly int _testCount;

        //Пройденные тесты за сессию
        private readonly List<Tuple<string, string, ConsoleColor>> _tests =
            new List<Tuple<string, string, ConsoleColor>>();

        //Результаты тестов за сессию
        private readonly List<string> _results = new List<string>();

        #endregion

        #region Test Processing

        //Обработка тестов с арабскими цифрами
        private void ProcessArabTests()
        {
            foreach (var (test, answer, color) in _tests)
            {
                var correctNumbers = 0;
                for (var i = 0; i < CharCount * 2; i += 2)
                {
                    //Сравниваем строки посимвольно
                    try
                    {
                        if (test[i] == answer[i]) correctNumbers++;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                //Считаем процент выполнения теста
                var result = correctNumbers / ((float) CharCount / 100);
                _results.Add($"Arab Test With {color.ToString()} color result {result}%.");
                PrintMessage(ConsoleColor.White, $"test result is {result}%.");
            }

            SaveResults();
        }

        //Обработка тестов с пиктограммами
        private void ProcessCharTests()
        {
            foreach (var (test, answer, color) in _tests)
            {
                var correctChars = 0;
                for (var i = 0; i < CharCount * 2; i += 2)
                {
                    //Сравниваем строки посимвольно
                    try
                    {
                        if (test[i] == answer[i]) correctChars++;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                //Считаем процент выполнения теста
                var result = correctChars / ((float) CharCount / 100);
                _results.Add($"Char Test With {color.ToString()} color result {result}%.");
                PrintMessage(ConsoleColor.White, $"test result is {result}%.");
            }

            SaveResults();
        }

        #endregion

        #region Test Creation

        //Создание тестов с арабскими цифрами
        private void CreateArabNumbersTests()
        {
            PrintMessage(
                ConsoleColor.White,
                $"You will see {CharCount} numbers {_testCount} times, try to remember and reproduce them. U have 10 seconds to remember");
            var rnd = new Random();
            for (var i = 0; i < _testCount; i++)
            {
                //Перемешиваем массив
                var numbers = _printableNumbers.OrderBy(x => rnd.Next()).ToArray();
                var color = (ConsoleColor) _colors.GetValue(rnd.Next(1, _colors.Length));
                var print = "";
                //Собираем строку теста
                for (var j = 0; j < CharCount; j++)
                {
                    print += $"{numbers[j]},";
                }

                PrintMessage(ConsoleColor.White, $"Test {i + 1}");
                PrintMessage(color, print);
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
                PrintMessage(ConsoleColor.White, "Now repeat numbers, split with ',' ");
                //Получаем результат теста
                var answer = Console.ReadLine();
                _tests.Add(new Tuple<string, string, ConsoleColor>(print, answer, color));
            }

            ProcessArabTests();
        }

        //Создание тестов с пиктограммами
        private void CreateCharTests()
        {
            PrintMessage(
                ConsoleColor.White,
                $"You will see {CharCount} chars {_testCount} times, try to remember and reproduce them. U have 10 seconds to remember");
            var rnd = new Random();
            for (var i = 0; i < _testCount; i++)
            {
                //Перемешиваем массив
                var chars = PrintableChars.OrderBy(x => rnd.Next()).ToArray();
                var color = (ConsoleColor) _colors.GetValue(rnd.Next(1, _colors.Length));
                var print = "";
                //Собираем строку теста
                for (var j = 0; j < CharCount; j++)
                {
                    print += $"{chars[j]},";
                }

                PrintMessage(ConsoleColor.White, $"Test {i + 1}");
                PrintMessage(color, print);
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
                PrintMessage(ConsoleColor.White, "Now repeat numbers, split with ','. Use Alt+NumPad keycode");
                PrintMessage(ConsoleColor.White, CharDescription);

                //Получаем результат теста
                var answer = Console.ReadLine();
                _tests.Add(new Tuple<string, string, ConsoleColor>(print, answer, color));
            }

            ProcessCharTests();
        }

        #endregion

        private static void PrintMessage(ConsoleColor color, params string[] text)
        {
            Console.ForegroundColor = color;
            foreach (var message in text) Console.WriteLine(message);
            Console.ResetColor();
        }

        private void SaveResults()
        {
            using (var writer = File.AppendText("../../results.txt"))
            {
                foreach (var result in _results)
                {
                    writer.WriteLine(result);
                }
            }
        }
    }
}