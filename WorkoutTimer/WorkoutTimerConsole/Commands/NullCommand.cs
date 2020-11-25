using WorkoutTimer.Shared;

namespace WorkoutTimerConsole.Commands
{
    class NullCommand : IWorkoutCommand
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
