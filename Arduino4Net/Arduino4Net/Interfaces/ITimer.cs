using System;

namespace Arduino4Net.Interfaces
{
    public interface ITimer : IDisposable
    {
        void Start(Action action, TimeSpan dueTime, TimeSpan period);
    }
}