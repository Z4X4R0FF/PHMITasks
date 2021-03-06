using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PHMITasks
{
    public class Task2
    {
        public Task2()
        {
            PrintMessage(ConsoleColor.White, "Welcome to DirectoryWorker!",
                "Press 1 for console interface, press 2 for Menu Interface");
            var interfaceNumber = Console.ReadKey();
            Console.Clear();
            switch (interfaceNumber.Key)
            {
                case ConsoleKey.D1:
                    LoadConsoleInterface();
                    break;
                case ConsoleKey.D2:
                    LoadMenuInterface();
                    break;
                default: throw new ArgumentException("Wrong key pressed");
            }
        }

        private static string _currentDirAbsolutePath = "";
        private static string[] _currentDirDirectories;
        private static string[] _currentDirFiles;

        private static readonly string[] AvailableCommands =
        {
            "-help     Displays available commands",
            "-changeDir <new path>     Changes working directory",
            "-changeExtension <filename1>|<filename2>|... <new extension>     Changes extension of specified files",
            "-dirInfo     Displays content of current folder",
            "-exit     Exit application"
        };

        private static readonly string[] Menu =
        {
            "1 - Help how to use",
            "2 - Changes working directory",
            "3 - Changes extension of specified files",
            "4 - Displays content of current folder",
            "0 - Exit application"
        };

        private static void PrintMessage(ConsoleColor color = ConsoleColor.White, params string[] text)
        {
            Console.ForegroundColor = color;
            foreach (var message in text) Console.WriteLine(message);
            Console.ResetColor();
        }

        #region InterfaceLoading

        private static void LoadConsoleInterface()
        {
            PrintMessage(ConsoleColor.Cyan, "Welcome to DirectoryWorker: console version!",
                "Write an absolute path to folder to work with.");
            while (true)
            {
                var absolutePath = Console.ReadLine();
                if (Regex.IsMatch(absolutePath,
                    @"^[a-zA-Z]:\\[\\\S|*\S]?.*$"))
                {
                    if (Directory.Exists(absolutePath))
                    {
                        _currentDirAbsolutePath = absolutePath;
                        break;
                    }

                    PrintMessage(ConsoleColor.Red, $"Path {absolutePath} does not exist.");
                }
                else PrintMessage(ConsoleColor.Red, $"Path {absolutePath} is not valid.");
            }

            Console.Clear();
            _currentDirDirectories = Directory.GetDirectories(_currentDirAbsolutePath);
            _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
            PrintMessage(ConsoleColor.Cyan, $"Current work directory is {_currentDirAbsolutePath}",
                "Type -help to see available commands");
            while (true)
            {
                ProcessConsoleCommands(Console.ReadLine());
                Console.WriteLine();
            }
        }

        private static void LoadMenuInterface()
        {
            PrintMessage(ConsoleColor.Cyan, "Welcome to FolderWorker: menu version!",
                "Write an absolute path to folder to work with.");
            while (true)
            {
                var absolutePath = Console.ReadLine();
                if (Regex.IsMatch(absolutePath,
                    @"^[a-zA-Z]:\\[\\\S|*\S]?.*$"))
                {
                    if (Directory.Exists(absolutePath))
                    {
                        _currentDirAbsolutePath = absolutePath;
                        break;
                    }

                    PrintMessage(ConsoleColor.Red, $"Path {absolutePath} does not exist.");
                }
                else PrintMessage(ConsoleColor.Red, $"Path {absolutePath} is not valid.");
            }

            Console.Clear();
            _currentDirDirectories = Directory.GetDirectories(_currentDirAbsolutePath);
            _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
            PrintMessage(ConsoleColor.Cyan, $"Current work directory is {_currentDirAbsolutePath}");
            while (true)
            {
                PrintMessage(ConsoleColor.Cyan, "Menu:");
                PrintMessage(ConsoleColor.Cyan, Menu);
                ProcessMenuCommands(Console.ReadKey());
                Console.WriteLine();
            }
        }

        #endregion

        #region CommandProcessing

        private static void ProcessConsoleCommands(string command)
        {
            var str = command.Split(new[] {' '}, 2);
            switch (str[0])
            {
                case "-help":
                    PrintMessage(ConsoleColor.Cyan, "Available commands:");
                    PrintMessage(ConsoleColor.Cyan, AvailableCommands);
                    break;
                case "-changeDir":
                    if (Regex.IsMatch(str[1], @"^[a-zA-Z]:\\[\\\S|*\S]?.*$"))
                    {
                        if (Directory.Exists(str[1]))
                        {
                            _currentDirAbsolutePath = str[1];
                            _currentDirDirectories = Directory.GetDirectories(_currentDirAbsolutePath);
                            _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
                            PrintMessage(ConsoleColor.Cyan, $"Current work directory is {_currentDirAbsolutePath}");
                        }
                        else PrintMessage(ConsoleColor.Red, $"Path {str[1]} does not exist.");
                    }
                    else PrintMessage(ConsoleColor.Red, $"Path {str[1]} is not valid.");

                    break;
                case "-changeExt":
                    var extension = str[1].Substring(str[1].LastIndexOf(' '));
                    var files = str[1].Substring(0, str[1].LastIndexOf(' ')).Split('|');
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Move(_currentDirAbsolutePath + '\\' + file,
                                Path.ChangeExtension(_currentDirAbsolutePath + '\\' + file, '.' + extension));
                            PrintMessage(ConsoleColor.Green,
                                $"File {_currentDirAbsolutePath + '\\' + file} extension changed.");
                        }
                        catch (Exception)
                        {
                            PrintMessage(ConsoleColor.Red,
                                $"Couldn't change file {_currentDirAbsolutePath + '\\' + file} extension.");
                            throw;
                        }
                    }

                    _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
                    break;
                case "-dirInfo":
                    PrintMessage(ConsoleColor.Cyan, $"Directory {_currentDirAbsolutePath} content:");
                    foreach (var directory in _currentDirDirectories)
                    {
                        PrintMessage(ConsoleColor.Magenta, $"{directory}     Folder");
                    }

                    foreach (var file in _currentDirFiles)
                    {
                        PrintMessage(ConsoleColor.Yellow, $"{file}     {file.Split('.').Last()}");
                    }

                    break;
                case "-exit":
                    Environment.Exit(0);
                    break;
                default:
                    PrintMessage(ConsoleColor.Red, "Wrong Command");
                    break;
            }
        }

        private static void ProcessMenuCommands(ConsoleKeyInfo consoleKeyInfo)
        {
            Console.WriteLine();
            string inputString;
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.D1:
                    PrintMessage(ConsoleColor.Cyan, "Press key for command you want to execute");
                    break;
                case ConsoleKey.D2:
                    PrintMessage(ConsoleColor.Cyan, "Write new absolute path to change working directory");
                    inputString = Console.ReadLine();
                    if (Regex.IsMatch(inputString, @"^[a-zA-Z]:\\[\\\S|*\S]?.*$"))
                    {
                        if (Directory.Exists(inputString))
                        {
                            _currentDirAbsolutePath = inputString;
                            _currentDirDirectories = Directory.GetDirectories(_currentDirAbsolutePath);
                            _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
                            PrintMessage(ConsoleColor.Cyan, $"Current work directory is {_currentDirAbsolutePath}");
                        }
                        else PrintMessage(ConsoleColor.Red, $"Path {inputString} does not exist.");
                    }
                    else PrintMessage(ConsoleColor.Red, $"Path {inputString} is not valid.");

                    break;
                case ConsoleKey.D3:
                    PrintMessage(ConsoleColor.Cyan, "Choose file(s) and write it index(es), split with '|'.");
                    inputString = Console.ReadLine();
                    var fileIndexes = inputString.Split('|').Select(r => Convert.ToInt32(r)).ToArray();
                    PrintMessage(ConsoleColor.Cyan, "Type extension you want files to have (for example 'txt').");
                    var extension = Console.ReadLine();

                    foreach (var file in fileIndexes)
                    {
                        try
                        {
                            File.Move(_currentDirFiles[file],
                                Path.ChangeExtension(_currentDirFiles[file], '.' + extension));
                            PrintMessage(ConsoleColor.Green, $"File {_currentDirFiles[file]} extension changed.");
                        }
                        catch (Exception)
                        {
                            PrintMessage(ConsoleColor.Red, $"Couldn't change file {_currentDirFiles[file]} extension.");
                            throw;
                        }
                    }

                    _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
                    break;
                case ConsoleKey.D4:
                    PrintMessage(ConsoleColor.Cyan, $"Directory {_currentDirAbsolutePath} content:");
                    foreach (var directory in _currentDirDirectories)
                    {
                        PrintMessage(ConsoleColor.Magenta, $"{directory}     Folder");
                    }

                    for (var i = 0; i < _currentDirFiles.Length; i++)
                    {
                        var file = _currentDirFiles[i];
                        PrintMessage(ConsoleColor.Yellow, $"{i} - {file}     {file.Split('.').Last()}");
                    }

                    break;
                case ConsoleKey.D0:
                    Environment.Exit(0);
                    break;
                default:
                    PrintMessage(ConsoleColor.Red, "Wrong key pressed");
                    break;
            }
        }

        #endregion
    }
}