using System;
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

                Console.WriteLine("Script");
                Console.WriteLine("------");
                ConsoleHelper.PrintItems(commands);
                Console.WriteLine();
                ConsoleHelper.PressAnyKeyToContinue();
                ConsoleHelper.WriteColumns("Now", "Next");
                ConsoleHelper.WriteColumns("---", "----");

                RunCommands(commands);

                Console.WriteLine("End");
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
                if(string.IsNullOrEmpty(line) || line[0] == '#')
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
            var queue = new Queue<ICommand>(commands);
            while (queue.Count > 0)
            {
                var command = queue.Dequeue();
                var nextCommand = queue.Count > 0 ? queue.Peek() : new NullCommand();
                ConsoleHelper.WriteColumns(command.ToString(), nextCommand.ToString());
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
