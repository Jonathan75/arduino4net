using Arduino4Net.Components.Buttons;
using Arduino4Net.Interfaces;
using Arduino4Net.Tests.Fakes;
using FakeItEasy;
using NUnit.Framework;
using Should;

namespace Arduino4Net.Tests.Components.Buttons
{
    public class PushButtonTests
    {
        private const int Pin = 5;
        private IArduino _arduino;
        private PushButton _button;
        private FakeTimer _timer;

        [SetUp]
        public void Setup()
        {
            _arduino = A.Fake<IArduino>();
            _timer = new FakeTimer();
            _button = new PushButton(_arduino, Pin, _timer);
        }

        [Test]
        public void DigitalRead_1_should_trigger_Down()
        {
            var wasCalled = false;
            _button.Down = () =>
            {
                wasCalled = true;
            };
            A.CallTo(() => _arduino.DigitalRead(Pin)).Returns(0);
            wasCalled.ShouldBeFalse();
            _button.IsDown.ShouldBeFalse();

            _timer.Tick();

            A.CallTo(() => _arduino.DigitalRead(Pin)).Returns(1);
            _timer.Tick();

            wasCalled.ShouldBeTrue();
            _button.IsDown.ShouldBeTrue();
        }

        [Test]
        public void DigitalRead_1_then_0_should_trigger_Up()
        {
            var wasCalled = false;
            _button.Up = () =>
            {
                wasCalled = true;
            };

            A.CallTo(() => _arduino.DigitalRead(Pin)).Returns(1);
            _timer.Tick();
            wasCalled.ShouldBeFalse();
            _button.IsDown.ShouldBeTrue();

            A.CallTo(() => _arduino.DigitalRead(Pin)).Returns(0);
            _timer.Tick();

            wasCalled.ShouldBeTrue();
            _button.IsDown.ShouldBeFalse();
        }
    }
}