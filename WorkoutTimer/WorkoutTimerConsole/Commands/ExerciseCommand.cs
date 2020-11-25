using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTimerConsole.Sounds;

namespace WorkoutTimerConsole.Commands
{
    class ExerciseCommand : ICommand
    {
        private readonly string name;
        private readonly TimeSpan time;
        private readonly ISound startingSound;
        private readonly ISound endingSound;

        public ExerciseCommand(string name, TimeSpan time, ISound startingSound, ISound endingSound)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.time = time;
            this.startingSound = startingSound ?? throw new ArgumentNullException(nameof(startingSound));
            this.endingSound = endingSound ?? throw new ArgumentNullException(nameof(endingSound));

            if (time - startingSound.Duration - endingSound.Duration < TimeSpan.FromSeconds(0))
                throw new Exception($"Cannot create '{name}' command because there isn't enough time to play the audio.");
        }

        public void Run()
        {
            Task.WaitAll(RunAsync());
        }

        private async Task RunAsync()
        {
            _ = startingSound.PlayAsync();
            _ = PrintTimerAsync(time);
            await Task.Delay(time - endingSound.Duration);
            await endingSound.PlayAsync();
        }

        private async Task PrintTimerAsync(TimeSpan time)
        {
            await Task.Run(() => PrintTimer(time));
        }

        private void PrintTimer(TimeSpan time)
        {
            var remaining = time.TotalSeconds;
            while (remaining > 0)
            {
                ConsoleHelper.WriteAndResetCursor(remaining.ToString());
                Thread.Sleep(1000);
                ConsoleHelper.WriteAndResetCursor(new string(' ', remaining.ToString().Length));
                remaining--;
            }
        }

        public override string ToString()
        {
            return $"{name} ({time.TotalSeconds}s)";
        }
    }
}
