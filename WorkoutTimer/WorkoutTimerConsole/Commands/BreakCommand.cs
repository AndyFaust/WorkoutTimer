using System;
using WorkoutTimer.Shared;

namespace WorkoutTimerConsole.Commands
{
    class BreakCommand : IWorkoutCommand
    {
        private readonly IGui gui;

        public BreakCommand(IGui gui)
        {
            this.gui = gui;
        }

        public void Run()
        {
            this.gui.AskToContinue();
        }

        public override string ToString()
        {
            return $"Break";
        }
    }
}
