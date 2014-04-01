using Arduino4Net.Components;
using Arduino4Net.Interfaces;
using Arduino4Net.Models;
using Arduino4Net.Tests.Fakes;
using FakeItEasy;
using NUnit.Framework;

namespace Arduino4Net.Tests.Components
{
    public class LedRGBTests
    {
        private static readonly int[] Pins = {9, 10, 11};
        private IArduino _arduino;
        private LedRGB _led;
        private FakeTimer _timer;

        [SetUp]
        public void Setup()
        {
            _arduino = A.Fake<IArduino>();
            _timer = new FakeTimer();
            _led = new LedRGB(_arduino, Pins, _timer);
        }

        [Test]
        public void All_Pins_should_be_PWM()
        {
            foreach (var pin in Pins)
            {
                var p = pin;
                A.CallTo(() => _arduino.PinMode(p, PinMode.Pwm)).MustHaveHappened();
            }
        }

        [Test]
        public void On_should_analogwrite_255()
        {
            const int value = 255;
            FakeAnalogWrite(value);
            _led.On();
            FakeVerifyAnalogWrite(value);
        }

        [Test]
        public void Off_should_analogwrite_0() {
            const int value = 0;
            FakeAnalogWrite(value);
            _led.Off();
            FakeVerifyAnalogWrite(value);
        }

        [Test]
        public void Color_should_analogwrite_values()
        {
            const int value = 255;
            FakeAnalogWrite(value);
            _led.Color(value, value, value);
            FakeVerifyAnalogWrite(value);
        }

        [Test]
        public void Color_should_not_allow_values_greater_than_255()
        {
            const int invalidValue = 256;
            const int value = 255;
            FakeAnalogWrite(value);
            _led.Color(invalidValue, invalidValue, invalidValue);
            FakeVerifyAnalogWrite(value);
        }

        [Test]
        public void Color_should_not_allow_values_lower_than_0()
        {
            const int invalidValue = -1;
            const int value = 0;
            FakeAnalogWrite(value);
            _led.Color(invalidValue, invalidValue, invalidValue);
            FakeVerifyAnalogWrite(value);
        }

        private void FakeVerifyAnalogWrite(int value)
        {
            foreach (var pin in Pins)
            {
                var p = pin;
                A.CallTo(() => _arduino.AnalogWrite(p, value)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        private void FakeAnalogWrite(int value)
        {
            foreach (var pin in Pins)
            {
                var p = pin;
                A.CallTo(() => _arduino.AnalogWrite(p, value)).DoesNothing();
            }
        }
    }
}