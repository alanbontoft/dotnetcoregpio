﻿using System;
using System.Device.Gpio;
using System.Threading;
using System.IO.Ports;
using System.Device.Spi;
using System.Device.Spi.Drivers;

namespace gpiotest
{
    class Program
    {
        static void Main(string[] args)
        {
            var pin = 17;
            var lightTimeInMilliseconds = 1000;
            var dimTimeInMilliseconds = 1200;

            // Serial Port
            System.IO.Ports.SerialPort port = new SerialPort();
            
            // SPI
            var busId = 1;
            var chipSelectLine = 0;
            var settings = new SpiConnectionSettings(busId, chipSelectLine);

            SpiDevice spi = new UnixSpiDevice(settings);
            
            Console.WriteLine($"Let's blink an LED!");
            using (GpioController controller = new GpioController())
            {
                controller.OpenPin(pin, PinMode.Output);
                Console.WriteLine($"GPIO pin enabled for use: {pin}");

                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) =>
                {
                    
                    controller.Dispose();
                };

                while (true)
                {
                    Console.WriteLine($"Light for {lightTimeInMilliseconds}ms");
                    controller.Write(pin, PinValue.High);
                    Thread.Sleep(lightTimeInMilliseconds);
                    Console.WriteLine($"Dim for {dimTimeInMilliseconds}ms");
                    controller.Write(pin, PinValue.Low);
                    Thread.Sleep(dimTimeInMilliseconds);
                }
            }

        }
    }
}

