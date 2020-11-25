using System;
using System.Threading.Tasks;

namespace WorkoutTimer.Shared.Interfaces
{
    public interface ISound
    {
        TimeSpan Duration { get; }
        Task PlayAsync();
    }
}
