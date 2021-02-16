using System;
using System.Collections.Generic;
using System.IO;

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
            PrintMessage(ConsoleColor.White, "Write symbols count");
            _numCount = Convert.ToInt32(Console.ReadLine());
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
        private readonly List<char> _printableChars = new List<char>
        {
            '\u263A', '\u263B', '\u2665',
            '\u2666', '\u2663', '\u2660', '\u2022',
            '\u25D8', '\u25CB', '\u25D9', '\u2642',
            '\u2640', '\u266A', '\u266B', '\u263C'
        };

        //Список цветов консоли
        private readonly Array _colors = Enum.GetValues(typeof(ConsoleColor));

        //Количество чисел в тесте
        private readonly int _numCount;

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
                for (var i = 0; i < _numCount * 2; i += 2)
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
                var result = correctNumbers / ((float) _numCount / 100);
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
                for (var i = 0; i < _numCount * 2; i += 2)
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
                var result = correctChars / ((float) _numCount / 100);
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
                $"You will see {_numCount} numbers {_testCount} times, try to remember and reproduce them. U have 10 seconds to remember");
            var rnd = new Random();
            for (var i = 0; i < _testCount; i++)
            {
                var color = (ConsoleColor) _colors.GetValue(rnd.Next(1, _colors.Length));
                var print = "";
                //Собираем строку теста
                for (var j = 0; j < _numCount; j++)
                {
                    print += $"{rnd.Next(0, 10)},";
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
                $"You will see {_numCount} chars {_testCount} times, try to remember and reproduce them. U have 10 seconds to remember");
            var rnd = new Random();
            for (var i = 0; i < _testCount; i++)
            {
                var color = (ConsoleColor) _colors.GetValue(rnd.Next(1, _colors.Length));
                var print = "";
                //Собираем строку теста
                for (var j = 0; j < _numCount; j++)
                {
                    print += $"{_printableChars[rnd.Next(_printableChars.Count)]},";
                }

                PrintMessage(ConsoleColor.White, $"Test {i + 1}");
                PrintMessage(color, print);
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
                PrintMessage(ConsoleColor.White, "Now repeat numbers, split with ',' ");
                for (int j = 0; j < _printableChars.Count; j++)
                {
                    PrintMessage(ConsoleColor.White, $"{j+1} for {_printableChars[j]}");
                }

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