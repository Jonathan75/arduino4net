using System;
using Arduino4Net.Components.Leds;
using Arduino4Net.Interfaces;
using Arduino4Net.Tests.Fakes;
using FakeItEasy;
using NUnit.Framework;
using Should;

namespace Arduino4Net.Tests.Components.Leds
{
    public class LedPwmTests
    {
        private LedPwm _led;
        private IArduino _arduino;
        private FakeTimer _timer;
        private const int Pin = 13;

        [SetUp]
        public void Setup()
        {
            _arduino = A.Fake<IArduino>();
            _timer = new FakeTimer();
            _led = new LedPwm(_arduino, Pin, _timer);
            _led.Intensity.ShouldEqual(0);
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void Cant_set_intensity_over_255()
        {
            _led.Intensity = 256;
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void Cant_set_intensity_below_0()
        {
            _led.Intensity = -1;
        }

        [Test]
        public void Fade_with_high_value_should_fadeIn()
        {
            var times = 0;
            const int toIntensity = 255;
            A.CallTo(() => _arduino.AnalogWrite(Pin, A<int>._))
                .Invokes(() => { times++; });
            _led.Fade(toIntensity);
            for (var i = 0; i < toIntensity - 1; i++)
            {
                _timer.Tick();
            }
            _led.Intensity.ShouldEqual(toIntensity);
            times.ShouldEqual(255);
        }

        [Test]
        public void Fade_with_low_value_should_fadeIn()
        {
            var times = 0;
            const int toIntensity = 0;
            A.CallTo(() => _arduino.AnalogWrite(Pin, A<int>._))
                .Invokes(() => { times++; });
            _led.Intensity = 255;

            _led.Fade(toIntensity);
            for (int i = 255; i > toIntensity - +1; i--)
            {
                _timer.Tick();
            }

            _led.Intensity.ShouldEqual(toIntensity);
            times.ShouldEqual(255 + 1);
        }
    }
}