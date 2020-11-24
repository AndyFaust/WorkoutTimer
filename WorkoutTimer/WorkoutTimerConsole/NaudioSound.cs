using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkoutTimerConsole
{
    class NaudioSound : ISound
    {
        private readonly string fileName;

        public NaudioSound(string fileName)
        {
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            this.Duration = GetDuration(fileName);
        }

        public TimeSpan Duration { get; }

        public async Task PlayAsync()
        {
            await Task.Run(() => Play());
        }

        private void Play()
        {
            using (var audioFile = new AudioFileReader(fileName))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }

        private TimeSpan GetDuration(string fileName)
        {
            using (var reader = new AudioFileReader(fileName))
            {
                return reader.TotalTime;
            }
        }
    }
}
