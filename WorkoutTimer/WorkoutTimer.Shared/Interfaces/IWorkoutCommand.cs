using System.Threading.Tasks;

namespace WorkoutTimer.Shared.Interfaces
{
    public interface IWorkoutCommand
    {
        Task RunAsync();
    }
}
