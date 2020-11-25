using System;
using WorkoutTimerConsole.Consoles;

namespace WorkoutTimerConsole.Commands
{
    class BreakCommand : ICommand
    {
        private readonly IConsole console;

        public BreakCommand(IConsole console)
        {
            this.console = console;
        }

        public void Run()
        {
            this.console.PressAnyKeyToContinue();
        }

        public override string ToString()
        {
            return $"Break";
        }
    }
}
