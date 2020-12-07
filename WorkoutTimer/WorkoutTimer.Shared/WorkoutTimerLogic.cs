using System.Collections.Generic;
using System.Linq;
using WorkoutTimer.Shared.Interfaces;
using WorkoutTimer.Shared.Commands;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using WorkoutTimer.Shared.FileHelpers;
using WorkoutTimer.Shared.Sounds;

[assembly: InternalsVisibleTo("WorkoutTimer.Tests")]

namespace WorkoutTimer.Shared
{
    public class WorkoutTimerLogic
    {
        private readonly IGui gui;

        public WorkoutTimerLogic(IGui gui)
        {
            this.gui = gui;
        }

        public IEnumerable<IWorkoutCommand> ReadScript(string path)
        {
            var fileRepository = new FileRepository();
            var soundFactory = new SoundFactory();
            var commandFactory = new CommandFactory(gui, fileRepository, soundFactory);

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
