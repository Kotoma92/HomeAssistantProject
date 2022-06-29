using System;
using WMPLib;
using System.Speech.Synthesis;
using System.Threading;

namespace SpeechCommands;

internal class CoinFlip
{
    private readonly Random _rnd = new();
    private readonly SpeechSynthesizer _bot;
    private readonly WindowsMediaPlayer _player;
    public CoinFlip(SpeechSynthesizer bot)
    {
        _bot = bot;
        _player = new WindowsMediaPlayer();
    }
    public void Flip()
    {
        var coinFlip = _rnd.Next(2);
        _bot.SpeakAsync("Flipping a coin.");
        Console.WriteLine("Bot: Flipping a coin...");

        Thread.Sleep(1500);
        _player.URL = @"D:\Code\GET Academy\Modul 3\SpeechCommands\HomeAssistantProject\SpeechCommands\sound\coin-flip.wav";
        _player.settings.volume = 50;
        _player.controls.play();
        Thread.Sleep(3000);

        if (coinFlip == 0)
        {
            _bot.SpeakAsync("It was Heads");
            Console.WriteLine("It was Heads!");
        }
        if (coinFlip == 1)
        {
            _bot.SpeakAsync("It was Tails!");
            Console.WriteLine("It was Tails!");
        }
    }
}