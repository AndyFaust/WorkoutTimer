using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Sounds
{
    class NaudioSound : ISound
    {
        private readonly string fileName;

        public NaudioSound(string fileName)
        {
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            var file = new System.IO.FileInfo(fileName);
            if (file.Exists == false)
                throw new Exception($"Unable to find file '{fileName}'.");
            Duration = GetDuration(fileName);
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
