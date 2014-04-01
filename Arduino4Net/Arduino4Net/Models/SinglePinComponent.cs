using Arduino4Net.Interfaces;

namespace Arduino4Net.Models
{
    public abstract class SinglePinComponent : Component
    {
        protected readonly int Pin;

        public SinglePinComponent(IArduino board, int pin, ITimer timer = null)
            : base(board, timer)
        {
            Pin = pin;
        }

        protected void SetPinMode(PinMode mode)
        {
            Board.PinMode(Pin, mode);
        }
    }
}