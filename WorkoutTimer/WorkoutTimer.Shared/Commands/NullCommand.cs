using System.Threading.Tasks;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Commands
{
    class NullCommand : IWorkoutCommand
    {
        public Task RunAsync()
        {
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return "None";
        }
    }
}
