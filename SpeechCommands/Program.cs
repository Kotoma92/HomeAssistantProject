using System;
using System.Threading;

namespace SpeechCommands;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Initializing SpeechBot...");
        var speechController = new SpeechController();
        Console.WriteLine("Speech Controller enabled...");
        while (true)
        {
            Thread.Sleep(500);
            if (speechController.Recognition.Enabled == false)
            {
                Environment.Exit(0);
            }
        }
    }
}