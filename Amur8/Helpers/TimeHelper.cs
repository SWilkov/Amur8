using Amur8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Amur8.Helpers
{
    /// <summary>
    /// TimeHelper is a Singleton class that ensures the Timer will run across pages in current app.
    /// Contains a TimeFinished event that is wired up in CountdownTimer.cs so that various logic can be done
    /// </summary>
    public class TimeHelper
    {
        private static readonly TimeHelper _instance = new TimeHelper();

        public static TimeHelper Instance
        {
            get { return _instance; }
        }

        private int _loadedCount = 0;
        public int LoadedCount
        {
            get { return _loadedCount; }
            set { _loadedCount = value; }
        }

        private DispatcherTimer _timer;
        public DispatcherTimer Timer
        {
            get { return _timer; }
        }

        public TimeDetails TimeDetails { get; set; }

        private TimeHelper()
        {
            TimeDetails = new TimeDetails();
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(1000);
                _timer.Tick += (s, args) =>
                {
                    if (TimeDetails.Hours == 0 && TimeDetails.Minutes == 0 && TimeDetails.Seconds == 0)
                    {
                        //Stop the timer
                        _timer.Stop();

                        //Raise TimerFinished event
                        OnTimerFinished();

                        return;
                    }

                    if (TimeDetails.Seconds == 0 && TimeDetails.Minutes > 0)
                    {
                        TimeDetails.Minutes--;
                        TimeDetails.Seconds = 60;
                    }

                    if (TimeDetails.Minutes == 0 && TimeDetails.Hours > 0)
                    {
                        TimeDetails.Hours--;
                        TimeDetails.Minutes = 59;
                        TimeDetails.Seconds = 60;
                    }
                    TimeDetails.Seconds--;
                };
            }

        }

        public void StartTimer()
        {
            if (_timer != null)
                _timer.Start();
        }

        public void PauseTimer()
        {
            if (_timer != null)
                _timer.Stop();
        }

        public event EventHandler TimerFinishedEvent;

        public void OnTimerFinished()
        {
            if (TimerFinishedEvent != null)
            {
                TimerFinishedEvent(this, new EventArgs());
            }
        }
    }
}
