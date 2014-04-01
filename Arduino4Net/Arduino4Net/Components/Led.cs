using Arduino4Net.Interfaces;
using Arduino4Net.Models;

namespace Arduino4Net.Components
{
    public class Led : LedBase
    {
        public Led(IArduino board, int pin, ITimer timer = null)
            : base(board, pin, timer)
        {
            SetPinMode(PinMode.Output);
        }
    }
}