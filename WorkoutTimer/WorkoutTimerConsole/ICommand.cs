using System.Threading.Tasks;

namespace WorkoutTimerConsole
{
    interface ICommand
    {
        Task RunAsync();
    }
}
