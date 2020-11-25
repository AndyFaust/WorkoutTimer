namespace WorkoutTimerConsole.Commands
{
    class NullCommand : ICommand
    {
        public void Run()
        {
            // Do nothing
        }

        public override string ToString()
        {
            return "None";
        }
    }
}
