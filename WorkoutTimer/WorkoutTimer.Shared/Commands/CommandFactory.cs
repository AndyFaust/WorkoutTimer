using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WorkoutTimer.Shared.Interfaces;
using WorkoutTimer.Shared.Sounds;

namespace WorkoutTimer.Shared.Commands
{
    internal class CommandFactory
    {
        private readonly IGui gui;
        private readonly IFileRepository fileRepo;
        private readonly ISoundFactory soundFactory;
        private readonly Regex startSetRegex = new Regex(@"(\d+).*?{");
        private readonly Regex endSetRegex = new Regex(@"}.*");


        public CommandFactory(IGui gui, IFileRepository fileRepo, ISoundFactory soundFactory)
        {
            this.gui = gui ?? throw new ArgumentNullException(nameof(gui));
            this.fileRepo = fileRepo ?? throw new ArgumentNullException(nameof(fileRepo));
            this.soundFactory = soundFactory ?? throw new ArgumentNullException(nameof(soundFactory));
        }

        public IEnumerable<IWorkoutCommand> GetCommands(string scriptPath)
        {
            var scriptFile = fileRepo.GetFile(scriptPath);
            return GetCommands(scriptFile);
        }

        public IEnumerable<IWorkoutCommand> GetCommands(IFile script)
        {
            var numberOfSets = 1;
            var set = new List<IWorkoutCommand>();

            foreach (var line in script.ReadLines())
            {
                if (string.IsNullOrEmpty(line) || line.Trim()[0] == '#')
                    continue;

                if (startSetRegex.IsMatch(line))
                {
                    numberOfSets = int.Parse(startSetRegex.Match(line).Groups[1].Value);
                    continue;
                }

                if (endSetRegex.IsMatch(line))
                {
                    for (int i = 0; i < numberOfSets - 1; i++)
                    {
                        foreach (var command in set)
                        {
                            yield return command;
                        }
                    }
                    numberOfSets = 1;
                    set.Clear();
                    continue;
                }

                IWorkoutCommand workoutCommand;
                try
                {
                    workoutCommand = GetCommand(line, script.Directory);
                }
                catch (Exception e)
                {
                    throw new Exception($"Unable to create command from line '{line}' due to exception '{e.Message}'.", e);
                }

                set.Add(workoutCommand);

                yield return workoutCommand;
            }
        }

        private IWorkoutCommand GetCommand(string line, string scriptDirectory)
        {
            var items = line.Split(',').Select(n => n.Trim()).ToList();

            IWorkoutCommand command;
            switch (items[0].ToLower())
            {
                case "break":
                    command = new BreakCommand(gui);
                    break;
                default:
                    if (items.Count < 2)
                        throw new Exception($"Unable to interpret '{line}'.");

                    var commandName = items[0];

                    var exerciseDuration = TimeSpan.FromSeconds(Convert.ToInt32(items[1]));

                    ISound startSound = new NullSound();
                    if (items.Count >= 3 && items[2] != "-" && !string.IsNullOrWhiteSpace(items[2]))
                    {
                        var path = Path.Combine(scriptDirectory, items[2]);
                        var file = fileRepo.GetFile(path);
                        startSound = soundFactory.GetSoundFromFile(file);
                    }

                    ISound endSound = new NullSound();
                    if (items.Count >= 4 && items[3] != "-" && !string.IsNullOrWhiteSpace(items[3]))
                    {
                        var path = Path.Combine(scriptDirectory, items[3]);
                        var file = fileRepo.GetFile(path);
                        endSound = soundFactory.GetSoundFromFile(file);
                    }

                    command = new ExerciseCommand(gui, commandName, exerciseDuration, startSound, endSound);

                    break;
            }

            return command;
        }
    }
}
