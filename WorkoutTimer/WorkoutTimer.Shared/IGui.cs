using System;
using System.Collections.Generic;

namespace WorkoutTimer.Shared
{
    public interface IGui
    {
        void AskToContinue();
        void DisplayScript(IEnumerable<IWorkoutCommand> commands);
        void UpdateNowAndNext(IWorkoutCommand now, IWorkoutCommand next);
        void DisplayException(Exception e);
        void UpdateTimer(int seconds);
    }
}
