using System;
using Arduino4Net.Interfaces;
using Arduino4Net.Models;

namespace Arduino4Net.Components.Buttons
{
    public class PushButton : SinglePinComponent
    {
        public PushButton(IArduino board, int pin, ITimer timer = null)
            : base(board, pin, timer)
        {
            SetPinMode(PinMode.Input);
            SetDefaults();
            Setup(board, pin);
        }

        private void Setup(IArduino board, int pin)
        {
            SetInterval(() =>
            {
                var isDown = board.DigitalRead(pin) == 1;
                var wasDown = IsDown;

                if (!wasDown && isDown)
                {
                    IsDown = true;
                    Down();
                }
                else if (wasDown && !isDown)
                {
                    IsDown = false;
                    Up();
                }
            }, 5);
        }

        private void SetDefaults()
        {
            Down = () => { };
            Up = () => { };
        }

        public bool IsDown { get; set; }
        public Action Down { get; set; }
        public Action Up { get; set; }
    }
}