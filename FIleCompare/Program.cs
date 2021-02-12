using System;
using System.Collections.Generic;
using System.IO;

namespace FIleCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            OnStart();
        }
        private static void RerunComparison( string key)
        {
            if (key == "Spacebar")
            {
                Console.Clear();
                OnStart();
            }
            else
            {
                Console.Clear();

                Console.WriteLine("INVALID INPUT, Please enter a valid Key(SPACE key)");

                key = Console.ReadKey().Key.ToString();
                RerunComparison(key);
            }
        }
        private static void OnStart()
        {
            Console.Title = "File Comparer";
            Console.BufferHeight = 9999;
            Console.BufferWidth = 3000;
            Console.SetBufferSize(300, 32766);
            Console.WindowHeight = 50;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            List<string> paths = new List<string>();

            Console.WriteLine("Type the Comparer Path :");
            string input = Path();
            if (input.Length < 1)
            {
                Console.WriteLine("Please Type the Comparer Path :");
                while (input.Length < 1)
                {
                    input = Path();
                }
            }
            paths.Add(input);
            input = "";
            Console.WriteLine("Type the Comparing Path :");
            input = Path();
            if (input.Length < 1)
            {
                Console.WriteLine("Please Type the Comparing Path :");

                while (input.Length < 1)
                {
                    input = Path();
                }
            }
            paths.Add(input);
            Console.WriteLine("Press ENTER to compare or Press SPACE to add another a new comparison path");
            ConsoleKey consoleKey = Console.ReadKey().Key;
            string key = consoleKey.ToString();
            if (!string.IsNullOrEmpty(Input(key)))
            {
                paths.Add(Input(key));
            }
            string[] p = paths.ToArray();

            GetFile(paths);
            Console.WriteLine("Press the SPACE key to initiate a new Comparison.");
            string rkey = Console.ReadKey().Key.ToString();
            RerunComparison(rkey);
        }
        private static string Input(string input)
        {
            string output = "";
            if (input == "Spacebar" || input == "Enter")
            {
                if (input == "Spacebar")
                {
                    Console.WriteLine("Add a new comparison path");
                    output = Path();
                    Console.WriteLine("Press ENTER to compare");
                    ConsoleKey consoleKey = Console.ReadKey().Key;
                    Input(consoleKey.ToString());
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("INVALID INPUT, Please input a valid key");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                ConsoleKey consoleKey = Console.ReadKey().Key;
                Input(consoleKey.ToString());
            }
            return output;
        }
        private static string Path()
        {
            string output = "";
            output = Console.ReadLine();

            return output;
        }
        private static void GetFile(List<string> paths)
        {

            ComparableFolders folders = new ComparableFolders();

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    FolderContent folderContent = new FolderContent();

                    string[] dir = Directory.GetDirectories(path);
                    if (dir.Length > 0)
                    {
                        foreach (string d in dir)
                        {
                            string folderName = d.Replace($"{path}\\", null);
                            folderContent.PathSubDirectories.Add(folderName);
                        }
                    }
                    foreach (string s in Directory.GetFiles(path))
                    {
                        string fileName = s.Replace($"{path}\\", null);
                        folderContent.Files.Add(fileName);
                    }
                    folders.Comparables.Add(folderContent);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{path} DOES NOT EXIST");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
            }
            CompareFile(folders, paths);
        }
        private static void CompareFile(ComparableFolders folders, List<string> paths)
        {
            if (folders?.Comparables?.Count > 1)
            {
                List<string> s = new List<string>();
                FolderContent folderContent = new FolderContent();
                folderContent.Files = folders.Comparables[0].Files;
                for (int i = 1; i < folders.Comparables.Count ; i++)
                {
                    bool complete = true;
                    if (folderContent.Files.Count == folders.Comparables[i].Files.Count)
                    {
                        foreach (string file in folders.Comparables[i].Files)
                        {
                            if (!folderContent.Files.Contains(file))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"{paths[0]} and {paths[i]} FILES DO NOT MATCH. @(Different File = {file})");
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                complete = false;
                                //break;
                            }
                        } 
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{paths[0]} and {paths[i]} FILES DO NOT MATCH. The number of files contained in each are not equal");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        complete = false;
                    }
                    if (complete)
                    {
                        Console.WriteLine($"{paths[0]} and {paths[i]} Files match.");
                        CompareSubFolders(folders, paths);
                    }
                   
                }
            }
        }
        private static void CompareSubFolders(ComparableFolders folders, List<string> paths)
        {
            if (folders?.Comparables?.Count > 1)
            {
                FolderContent folderContent = new FolderContent();
                folderContent.PathSubDirectories = folders.Comparables[0].PathSubDirectories;

                bool complete = true;
                for (int i = 1; i < folders.Comparables.Count; i++)
                {
                    if (folderContent.PathSubDirectories.Count == folders.Comparables[i].PathSubDirectories.Count)
                    {
                        foreach (string path in folders.Comparables[i].PathSubDirectories)
                        {
                            if (!folderContent.PathSubDirectories.Contains(path))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"{paths[0]} and {paths[i]} Subfolders DO NOT MATCH. @(Different Subfolder = {paths[i]}\\{path})");
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                complete = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Atleast one Subfolder is NOT IN BOTH PATHS.");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        complete = false;
                    }
                    if (complete)
                    {
                        Console.WriteLine($"{paths[0]} and {paths[i]} Subfolder Names matches.");
                        foreach (string s in folderContent.PathSubDirectories)
                        {
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            GetFile(new List<string>
                            {
                                $"{paths[0]}\\{s}",
                                $"{paths[i]}\\{s}"
                            });
                        }
                    } 
                }
            }
        }
        private static ComparableFolders Folders(List<string> folderName, string path)
        {
            ComparableFolders output = new ComparableFolders();
            foreach (string content in folderName)
            {
                output.Comparables.Add(GetAllFIlesnSubfolders($"{path}\\{content}"));
            }

            return output;
        }
        private static FolderContent GetAllFIlesnSubfolders(string path)
        {
            FolderContent output = new FolderContent();
            output.Path = path;
            foreach (string s in Directory.GetFiles(path))
            {
                output.Files.Add(s);
            }
            foreach (string s in Directory.GetDirectories(path))
            {
                output.PathSubDirectories.Add(s);
            }
            return output;
        }
    }
}
