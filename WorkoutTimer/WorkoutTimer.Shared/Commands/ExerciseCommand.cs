using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Commands
{
    class ExerciseCommand : IWorkoutCommand
    {
        private readonly IGui gui;
        private readonly string name;
        private readonly TimeSpan time;
        private readonly ISound startingSound;
        private readonly ISound endingSound;

        public ExerciseCommand(IGui gui, string name, TimeSpan time, ISound startingSound, ISound endingSound)
        {
            this.gui = gui;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.time = time;
            this.startingSound = startingSound ?? throw new ArgumentNullException(nameof(startingSound));
            this.endingSound = endingSound ?? throw new ArgumentNullException(nameof(endingSound));

            if (time - startingSound.Duration - endingSound.Duration < TimeSpan.FromSeconds(0))
                throw new Exception($"Cannot create '{name}' command because there isn't enough time to play the audio.");
        }

        public async Task RunAsync()
        {
            var startSoundTask = startingSound.PlayAsync();
            var timerTask = PrintTimerAsync(time);
            await Task.Delay(time - endingSound.Duration);
            await endingSound.PlayAsync();
            await startSoundTask;
            await timerTask;
        }

        private async Task PrintTimerAsync(TimeSpan time)
        {
            var remaining = (int)time.TotalSeconds;
            await gui.UpdateTimer(remaining);
            while (remaining > 0)
            {
                await Task.Delay(1000);
                remaining--;
                await gui.UpdateTimer(remaining);
            }
        }

        public override string ToString()
        {
            return $"{name} ({time.TotalSeconds}s)";
        }
    }
}
