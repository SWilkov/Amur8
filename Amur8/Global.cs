using Amur8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Amur8
{
    /// <summary>
    /// Global is a singleton used to hold items that need persisting across pages in application
    /// </summary>
    public class Global
    {
        private static readonly Global _instance = new Global();

        /// <summary>
        /// Timers is used to store DispatcherTimers used by CountdownTimer
        /// </summary>
        private List<DispatcherTimer> _timers;
        public List<DispatcherTimer> Timers
        {
            get { return _timers; }
        }
        
        private Global()
        {
            _timers = new List<DispatcherTimer>();
        }

        public static Global Instance
        {
            get { return _instance; }
        }

        public int CreateTimer()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            
            _timers.Add(timer);
            return _timers.IndexOf(timer);
        }
    }
}
