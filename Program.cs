using System;
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
            var lightTimeInMilliseconds = 250;
            var dimTimeInMilliseconds = 250;
            byte data = 1;

            // Serial Port
            System.IO.Ports.SerialPort port = new SerialPort();
            
            // SPI
            // using '/dev/spidev0.0' (use CS = 1 for spidev0.1)
            var busId = 0;
            var chipSelect = 0;
            var settings = new SpiConnectionSettings(busId, chipSelect);

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
                    
                    // send byte via SPI and increment value
                    spi.WriteByte(data++);
                }
            }

        }
    }
}

