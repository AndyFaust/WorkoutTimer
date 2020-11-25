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

        public ICommand LoadScriptCommand { get; set; }
        public bool CanLoadScript { get; set; }

        public ICommand StartCommand { get; set; }
        public bool CanStart { get; set; }

        public string NowCommand { get; set; }
        public string NextCommand { get; set; }
        public int Seconds { get; set; }
        public ObservableCollection<string> Commands { get; set; }

        public string ScriptPath { get; set; }

        public MainWindowViewModel()
        {
            CanLoadScript = true;

            workoutTimer = new WorkoutTimerLogic(this);

            LoadScriptCommand = new RelayCommand(_ => {
                var scriptPath = GetScriptFilePath();
                if (scriptPath == null) return;
                script = workoutTimer.ReadScript(scriptPath);
                ScriptPath = scriptPath;
                Commands = new ObservableCollection<string>(script.Select(c => c.ToString()));
                CanStart = true;
            }, _ => CanLoadScript);

            StartCommand = new RelayCommand(async _ => {
                CanLoadScript = false;
                CanStart = false;
                await workoutTimer.RunCommands(script);
                CanLoadScript = true;
                CanStart = true;
            }, _ => CanStart);
        }

        public string GetScriptFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else
                return null;
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
