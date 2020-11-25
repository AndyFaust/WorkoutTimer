using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTimer.Shared;

namespace WorkoutTimerConsole.Sounds
{
    class SampleSound : ISound
    {
        private readonly int seconds;

        public SampleSound(int seconds)
        {
            this.seconds = seconds;
        }

        public TimeSpan Duration => TimeSpan.FromSeconds(seconds);

        public async Task PlayAsync()
        {
            await Task.Run(() => Play());
        }

        private void Play()
        {
            using (var player = new System.Media.SoundPlayer("file_example_WAV_1MG.wav"))
            {
                player.Play();
                Thread.Sleep(Duration);
                player.Stop();
            }
        }
    }
}
