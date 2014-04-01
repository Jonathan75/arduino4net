Arduino wrapper for .NET based on Firmata protocol.

Also, this project has the concept of Components, for abstracting boring stuff from you!

You can create a new Led instance and start strobing right away!

```csharp
using (var board = new Arduino()))
{
    var led = new Led(board, 13);
    led.StrobeOn(20);
    Thread.Sleep(3.Seconds());
    led.StrobeOff();
}
```