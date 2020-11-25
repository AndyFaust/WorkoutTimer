using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkoutTimer.Shared.Interfaces
{
    public interface IGui
    {
        Task AskToContinue();
        Task UpdateNowAndNext(IWorkoutCommand now, IWorkoutCommand next);
        Task UpdateTimer(int seconds);
    }
}
