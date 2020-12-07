using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutTimer.Shared.Interfaces;
using WorkoutTimer.Shared.Commands;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using WorkoutTimer.Shared.FileHelpers;

[assembly: InternalsVisibleTo("WorkoutTimer.Tests")]

namespace WorkoutTimer.Shared
{
    public class WorkoutTimerLogic
    {
        private readonly IGui gui;
        private readonly IFileRepository fileRepository;

        public WorkoutTimerLogic(IGui gui, IFileRepository fileRepository = null)
        {
            this.gui = gui;
            this.fileRepository = fileRepository ?? new FileRepository();
        }

        public IEnumerable<IWorkoutCommand> ReadScript(string path)
        {
            var commandFactory = new CommandFactory(gui, fileRepository);
            return commandFactory.GetCommands(path).ToList();
        }

        public async Task RunCommands(IEnumerable<IWorkoutCommand> commands)
        {
            var queue = new Queue<IWorkoutCommand>(commands);
            while (queue.Count > 0)
            {
                var command = queue.Dequeue();
                var nextCommand = queue.Count > 0 ? queue.Peek() : new NullCommand();
                await gui.UpdateNowAndNext(command, nextCommand);
                await command.RunAsync();
            }
        }
    }
}
