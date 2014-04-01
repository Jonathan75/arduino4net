using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Arduino4Net.Components;
using Arduino4Net.Extensions;
using Arduino4Net.Models;

namespace Arduino4Net.Sample
{
    public static class Program
    {
        private static readonly Random Random = new Random();

        private static void Main()
        {
            Console.WriteLine("1 - Led (Pin 13)");
            Console.WriteLine("2 - LedPwm (Pin 9)");
            Console.WriteLine("3 - LedRGB (Pins 9, 10, 11)");
            switch (Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture))
            {
                case "1":
                    Led();
                    break;
                case "2":
                    LedPwm();
                    break;
                case "3":
                    LedRGB();
                    break;
            }
        }

        private static void LedRGB()
        {
            using (var board = new Arduino {Debug = true})
            {
                var led = new LedRGB(board, 9, 10, 11);
                Action wait = () => Thread.Sleep(2.Seconds());
                Action hold = () => Thread.Sleep(50.Milliseconds());
                led.On();
                wait();
                led.Off();
                wait();
                led.Color(255, 0, 0);
                wait();
                led.Color(0, 255, 0);
                wait();
                led.Color(0, 0, 255);
                wait();
                led.Off();
                wait();
                for (int i = 0; i < 100; i++)
                {
                    led.Color(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255));
                    hold();
                }
                led.Off();
            }
        }

        private static void LedPwm()
        {
            using (var board = new Arduino {Debug = true})
            {
                var led = new LedPwm(board, 9);
                led.Fade(255, 1.Second());
                Thread.Sleep(2.Seconds());
                led.Fade(0, 1.Second());
                led.Off();
            }
        }

        private static void Led()
        {
            using (var board = new Arduino {Debug = true})
            {
                var led = new Led(board, 13);
                led.StrobeOn(20);
                Thread.Sleep(3.Seconds());
                led.Off();
            }
        }
    }
}