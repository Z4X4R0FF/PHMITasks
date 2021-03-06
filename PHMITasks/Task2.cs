using System;
using System.Collections.Generic;
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

        private static void PrintMessage(ConsoleColor color = ConsoleColor.White, params string[] text)
        {
            Console.ForegroundColor = color;
            foreach (var message in text) Console.WriteLine(message);
            Console.ResetColor();
        }

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
                ProcessCommands(Console.ReadLine());
            }
        }

        private static void ProcessCommands(string command)
        {
            var str = command.Split(new[] {' '}, 2);
            switch (str[0])
            {
                case "-help":
                    PrintMessage(ConsoleColor.Cyan, "Available commands:");
                    PrintMessage(ConsoleColor.Cyan, AvailableCommands);
                    break;
                case "-changeDir":
                    if (Regex.IsMatch(str[1], @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$"))
                    {
                        if (Directory.Exists(str[1]))
                        {
                            _currentDirAbsolutePath = str[1];
                            _currentDirDirectories = Directory.GetDirectories(_currentDirAbsolutePath);
                            _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
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
                        File.Move(_currentDirAbsolutePath + '\\' + file,
                            Path.ChangeExtension(_currentDirAbsolutePath + '\\' + file, '.' + extension));
                    }

                    _currentDirFiles = Directory.GetFiles(_currentDirAbsolutePath);
                    PrintMessage(ConsoleColor.Cyan, "Files changed successfully");
                    break;
                case "-dirInfo":
                    PrintMessage(ConsoleColor.Cyan, $"Directory {_currentDirAbsolutePath} content:");
                    foreach (var directory in _currentDirDirectories)
                    {
                        PrintMessage(ConsoleColor.Green, $"{directory}     Folder");
                    }

                    foreach (var file in _currentDirFiles)
                    {
                        PrintMessage(ConsoleColor.Green, $"{file}     {file.Split('.').Last()}");
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


        private static void LoadMenuInterface()
        {
            PrintMessage(ConsoleColor.Cyan, "Welcome to FolderWorker: menu version!",
                "Write an absolute path to folder to work with.");
            var path = Console.ReadLine();
            //TODO: Implement
        }
    }
}