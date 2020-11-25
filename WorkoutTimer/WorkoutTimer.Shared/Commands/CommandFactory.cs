using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WorkoutTimer.Shared.Interfaces;
using WorkoutTimer.Shared.Sounds;

namespace WorkoutTimer.Shared.Commands
{
    class CommandFactory
    {
        private readonly IGui gui;

        public CommandFactory(IGui gui)
        {
            this.gui = gui;
        }

        public IEnumerable<IWorkoutCommand> GetCommands(string scriptPath)
        {
            var script = new FileInfo(scriptPath);

            foreach (var line in File.ReadLines(script.FullName))
            {
                if (string.IsNullOrEmpty(line) || line.Trim()[0] == '#')
                    continue;

                var items = line.Split(',').Select(n => n.Trim()).ToList();

                var command = items[0];
                switch (command.ToLower())
                {
                    case "break":
                        yield return new BreakCommand(gui);
                        break;
                    default:
                        if (items.Count < 2)
                            throw new Exception($"Unable to interpret '{line}'.");
                        yield return new ExerciseCommand(
                            gui,
                            items[0],
                            TimeSpan.FromSeconds(Convert.ToInt32(items[1])),
                            items.Count < 3 || items[2] == "-" || string.IsNullOrWhiteSpace(items[2])
                                ? new NullSound()
                                : (ISound)new NaudioSound(Path.Combine(script.DirectoryName, items[2])),
                            items.Count < 4 || items[3] == "-" || string.IsNullOrWhiteSpace(items[3])
                                ? new NullSound()
                                : (ISound)new NaudioSound(Path.Combine(script.DirectoryName, items[3]))
                        );
                        break;
                }
            }
        }
    }
}
