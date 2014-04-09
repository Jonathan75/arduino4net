using Arduino4Net.Components.Leds;
using Arduino4Net.Interfaces;
using Arduino4Net.Models;
using Arduino4Net.Tests.Fakes;
using FakeItEasy;
using NUnit.Framework;
using Should;

namespace Arduino4Net.Tests.Components.Leds
{
    public class LedTests
    {
        private const int Pin = 13;
        private IArduino _arduino;
        private Led _led;
        private FakeTimer _timer;

        [SetUp]
        public void Setup()
        {
            _arduino = A.Fake<IArduino>();
            _timer = new FakeTimer();
            _led = new Led(_arduino, Pin, _timer);
        }

        [Test]
        public void Off_should_digitalwrite_low()
        {
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.Low)).DoesNothing();
            var ledState = LedState.On;
            _led.OnStateChanged = state => { ledState = state; };

            _led.Off();

            _led.State.ShouldEqual(LedState.Off);
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.Low)).MustHaveHappened(Repeated.Exactly.Twice);
            ledState.ShouldEqual(LedState.Off);
        }

        [Test]
        public void On_should_digitalwrite_high()
        {
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.High)).DoesNothing();
            var ledState = LedState.On;
            _led.OnStateChanged = state => { ledState = state; };

            _led.On();

            _led.State.ShouldEqual(LedState.On);
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.High)).MustHaveHappened(Repeated.Exactly.Once);
            ledState.ShouldEqual(LedState.On);
        }

        [Test]
        public void Toggle_should_toggle_LedState_and_digitalwrite()
        {
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.High)).DoesNothing();
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.Low)).DoesNothing();
            _led.On();

            _led.Toggle();
            _led.State.ShouldEqual(LedState.Off);
            _led.Toggle();
            _led.State.ShouldEqual(LedState.On);
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.High)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.Low)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void StrobeOn_should_start_timer()
        {
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.High)).DoesNothing();
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.Low)).DoesNothing();
            _led.On();

            _led.StrobeOn();
            _led.State.ShouldEqual(LedState.Off);
            _timer.Tick();
            _led.State.ShouldEqual(LedState.On);
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.High)).MustHaveHappened(Repeated.Exactly.Twice);
            A.CallTo(() => _arduino.DigitalWrite(Pin, DigitalPin.Low)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void StrobeOff_should_stop_timer()
        {
            _led.StrobeOff();
            _timer.IsDisposed.ShouldEqual(true);
        }
    }
}