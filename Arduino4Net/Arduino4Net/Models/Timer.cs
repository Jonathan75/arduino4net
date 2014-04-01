using System;
using Arduino4Net.Interfaces;

namespace Arduino4Net.Models
{
    public class Timer : ITimer
    {
        private System.Threading.Timer _timer;

        public void Start(Action action, TimeSpan dueTime, TimeSpan period)
        {
            _timer = new System.Threading.Timer(s => action(), null, dueTime, period);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}