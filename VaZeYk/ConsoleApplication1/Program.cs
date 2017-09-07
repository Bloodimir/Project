using System;
using System.Threading;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static int frequency = 10000;
        private static int duration = 100;

        private static void Main(string[] args)
        {
            Other();
            Console.Clear();
            Matrix.MatrixEffect();
        }

        private static void Other()
        {
            var Progresbar = "VaZeYk Banned - R.I.P";
            var title = "";
            while (true)
            {
                for (var i = 0; i < Progresbar.Length; i++)
                {
                    title += Progresbar[i];
                    Console.Title = title;
                    Thread.Sleep(100);
                }
                title = "";

                for (var i = 1; i < 40; i++)
                {
                    Console.SetWindowSize(i, i);
                    Thread.Sleep(50);
                }
                {
                    Console.WriteLine("Klavye Ok Tuşlarıyla Frekansı Ayarlayabilirsiniz, Sad Violin Çalmak Yasak");
                    do
                    {
                        while (!Console.KeyAvailable)
                        {
                            Console.Beep(frequency, duration);
                        }

                        var key = Console.ReadKey(true);

                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                frequency += 100;
                                frequency = Math.Min(frequency, 15000);
                                break;
                            case ConsoleKey.DownArrow:
                                frequency -= 100;
                                frequency = Math.Max(frequency, 1000);
                                break;
                            case ConsoleKey.RightArrow:
                                duration += 100;
                                duration = Math.Min(duration, 1000);
                                break;
                            case ConsoleKey.LeftArrow:
                                duration -= 100;
                                duration = Math.Max(duration, 100);
                                break;
                        }
                    } while (true);
                }
            }
        }
    }
}