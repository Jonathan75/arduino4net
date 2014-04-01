using System;
using Arduino4Net.Interfaces;

namespace Arduino4Net.Tests.Fakes
{
    public class FakeTimer : ITimer
    {
        public FakeTimer()
        {
            IsDisposed = false;
        }

        private Action _action;
        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void Start(Action action, TimeSpan dueTime, TimeSpan period)
        {
            _action = action;
            Tick();
        }

        public void Tick()
        {
            _action();
        }
    }
}