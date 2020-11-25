using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkoutTimer.Shared;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.WPF
{
    class MainWindowViewModel : INotifyPropertyChanged, IGui
    {
        private readonly WorkoutTimerLogic workoutTimer;
        private IEnumerable<IWorkoutCommand> script;

        public ICommand ReadScriptCommand { get; set; }
        public bool CanReadScript { get; set; }

        public ICommand StartCommand { get; set; }
        public bool CanStart { get; set; }


        public string NowCommand { get; set; }
        public string NextCommand { get; set; }
        public int Seconds { get; set; }
        public ObservableCollection<string> Commands { get; set; }

        public string ScriptPath { get; set; }

        public MainWindowViewModel()
        {
            NowCommand = "Now";
            NextCommand = "Next";
            CanReadScript = true;

            workoutTimer = new WorkoutTimerLogic(this);

            ReadScriptCommand = new RelayCommand(_ => {
                var scriptPath = GetScriptFilePath();
                ScriptPath = scriptPath;
                script = workoutTimer.ReadScript(scriptPath);
                Commands = new ObservableCollection<string>(script.Select(c => c.ToString()));
                CanStart = true;
            }, _ => CanReadScript);

            StartCommand = new RelayCommand(async _ => {
                CanReadScript = false;
                CanStart = false;
                await workoutTimer.RunCommands(script);
                CanReadScript = true;
                CanStart = true;
            }, _ => CanStart);
        }

        public string GetScriptFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else
                throw new Exception("No file selected.");
        }

        public Task UpdateNowAndNext(IWorkoutCommand now, IWorkoutCommand next)
        {
            NowCommand = now.ToString();
            NextCommand = next.ToString();
            Commands.Remove(NowCommand.ToString());
            return Task.CompletedTask;
        }

        public Task UpdateTimer(int seconds)
        {
            Seconds = seconds;
            return Task.CompletedTask;
        }

        public Task AskToContinue()
        {
            MessageBox.Show("Continue?");
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
