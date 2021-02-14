using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PHMITasks
{
    public class Task1
    {
        public Task1()
        {
            PrintMessage("1 for ArabNumbersTest, 2 for CharTest", ConsoleColor.White);
            var task = Console.ReadKey();
            Console.Clear();
            PrintMessage("Write symbols count", ConsoleColor.White);
            _numCount = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            PrintMessage("Write test count", ConsoleColor.White);
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

        private readonly List<char> _printableChars = Enumerable.Range(0, 126)
            .Select(i => (char) i)
            .Where(c => !char.IsControl(c))
            .ToList();

        private readonly Array _colors = Enum.GetValues(typeof(ConsoleColor));
        private readonly int _numCount;
        private readonly int _testCount;

        private readonly List<Tuple<string, string, ConsoleColor>> _tests =
            new List<Tuple<string, string, ConsoleColor>>();

        private readonly List<string> _results = new List<string>();

        #endregion

        #region Test Processing

        private void ProcessArabTests()
        {
            foreach (var (test, answer, color) in _tests)
            {
                var correctNumbers = 0;
                for (var i = 0; i < _numCount * 2; i += 2)
                {
                    try
                    {
                        if (test[i] == answer[i]) correctNumbers++;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                var result = correctNumbers / ((float) _numCount / 100);
                _results.Add($"Arab Test With {color.ToString()} color result {result}%.");
                PrintMessage($"test result is {result}%.", ConsoleColor.White);
            }

            SaveResults();
        }

        private void ProcessCharTests()
        {
            foreach (var (test, answer, color) in _tests)
            {
                var correctChars = 0;
                for (var i = 0; i < _numCount * 2; i += 2)
                {
                    try
                    {
                        if (test[i] == answer[i]) correctChars++;
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

                var result = correctChars / ((float) _numCount / 100);
                _results.Add($"Char Test With {color.ToString()} color result {result}%.");
                PrintMessage($"test result is {result}%.", ConsoleColor.White);
            }

            SaveResults();
        }

        #endregion

        #region Test Creation

        private void CreateArabNumbersTests()
        {
            PrintMessage(
                $"U will see {_numCount} numbers {_testCount} times, try to remember and reproduce them. U have 10 seconds to remember",
                ConsoleColor.White);
            var rnd = new Random();
            for (var i = 0; i < _testCount; i++)
            {
                var color = (ConsoleColor) _colors.GetValue(rnd.Next(1, _colors.Length));
                var print = "";
                for (var j = 0; j < _numCount; j++)
                {
                    print += $"{rnd.Next(0, 10)},";
                }

                PrintMessage($"Test {i + 1}", ConsoleColor.White);
                PrintMessage(print, color);
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
                PrintMessage("Now repeat numbers, split with ',' ", ConsoleColor.White);
                var answer = Console.ReadLine();
                _tests.Add(new Tuple<string, string, ConsoleColor>(print, answer, color));
            }

            ProcessArabTests();
        }

        private void CreateCharTests()
        {
            PrintMessage(
                $"U will see {_numCount} chars {_testCount} times, try to remember and reproduce them. U have 10 seconds to remember",
                ConsoleColor.White);
            var rnd = new Random();
            for (var i = 0; i < _testCount; i++)
            {
                var color = (ConsoleColor) _colors.GetValue(rnd.Next(1, _colors.Length));
                var print = "";
                for (var j = 0; j < _numCount; j++)
                {
                    print += $"{_printableChars[rnd.Next(_printableChars.Count)]},";
                }

                PrintMessage($"Test {i + 1}", ConsoleColor.White);
                PrintMessage(print, color);
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
                PrintMessage("Now repeat numbers, split with ',' ", ConsoleColor.White);
                var answer = Console.ReadLine();
                _tests.Add(new Tuple<string, string, ConsoleColor>(print, answer, color));
            }

            ProcessCharTests();
        }

        #endregion

        private static void PrintMessage(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
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