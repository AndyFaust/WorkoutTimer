﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace WorkoutTimerConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var scriptPath = GetFilePath();
                if (scriptPath is null) return;
                var script = new FileInfo(scriptPath);

                var commands = GetCommands(script).ToList();

                Console.WriteLine("Press any key to start.");
                Console.ReadKey();

                RunCommands(commands);

                Console.WriteLine("Finished");
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static IEnumerable<ICommand> GetCommands(FileInfo script)
        {
            foreach (var line in File.ReadLines(script.FullName))
            {
                if(string.IsNullOrEmpty(line))
                    continue;

                var items = line.Split(',').Select(n => n.Trim()).ToList();

                var command = items[0];
                switch (command.ToLower())
                {
                    case "break":
                        yield return new BreakCommand();
                        break;
                    default:
                        if (items.Count < 2) 
                            throw new Exception($"Unable to interpret '{line}'.");
                        yield return new ExerciseCommand(
                            items[0],
                            TimeSpan.FromSeconds(Convert.ToInt32(items[1])),
                            items.Count < 3 || items[2] == "-" || string.IsNullOrWhiteSpace(items[2])
                                ? new NullSound() 
                                : (ISound) new NaudioSound(Path.Combine(script.DirectoryName, items[2])),
                            items.Count < 4 || items[3] == "-" || string.IsNullOrWhiteSpace(items[3])
                                ? new NullSound() 
                                : (ISound) new NaudioSound(Path.Combine(script.DirectoryName, items[3]))
                        );
                        break;
                }
            }
        }

        static void RunCommands(IEnumerable<ICommand> commands)
        {
            var commandsList = commands.ToList();
            for (int i = 0; i < commandsList.Count; i++)
            {
                var command = commandsList[i];
                var nextCommand = i < commandsList.Count - 1 ? commandsList[i + 1].ToString() : "End";
                Console.WriteLine($"{command} [Next: {nextCommand}]");
                command.Run();
            }
        }

        static string GetFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "Scripts|*.txt;*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    return openFileDialog.FileName;
                }

                return null;
            }
        }
    }
}
