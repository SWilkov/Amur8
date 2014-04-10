using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amur8.Events
{
    public class CountdownTimerEventArgs : EventArgs
    {
        public TimeSpan StartedTime { get; set; }
        public TimeSpan PausedTime { get; set; }
    }
}
