using System;
using System.Threading.Tasks;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Commands
{
    class BreakCommand : IWorkoutCommand
    {
        private readonly IGui gui;

        public BreakCommand(IGui gui)
        {
            this.gui = gui;
        }

        public async Task RunAsync()
        {
            await gui.AskToContinue();
        }

        public override string ToString()
        {
            return $"Break";
        }
    }
}
