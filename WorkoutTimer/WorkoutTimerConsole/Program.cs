using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
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

                Task.WaitAll(RunCommands(commands));

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
                var items = line.Split(',');
                if (items.Length != 4)
                    throw new Exception($"Could not read line: '{line}'");

                var name = items[0].Trim();
                var timeSeconds = items[1].Trim();
                var startAudioPath = items[2].Trim();
                var endAudioPath = items[3].Trim();

                yield return new ExerciseCommand(
                    name,
                    TimeSpan.FromSeconds(Convert.ToInt32(timeSeconds)),
                    startAudioPath == "-" ? new NullSound() : (ISound) new NaudioSound(Path.Combine(script.DirectoryName, startAudioPath)),
                    endAudioPath == "-" ? new NullSound() : (ISound) new NaudioSound(Path.Combine(script.DirectoryName, endAudioPath))
                );
            }
        }

        static async Task RunCommands(IEnumerable<ICommand> commands)
        {
            foreach(var command in commands)
            {
                await command.RunAsync();
            }
        }

        static string GetFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
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
