using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;
using WMPLib;

namespace SpeechCommands;

internal class HueLights
{
    private bool _partyTime;

    //Edit to us your Hue Bridge IP.
    private readonly ILocalHueClient _client = new LocalHueClient("192.168.50.131"); 

    //Edit to use your own API key from your Hue Bridge.
    private readonly string _appKey = File.ReadAllText(@"appKeyHueLights.txt");

    private readonly LightCommand _lightCommand;
    private readonly SpeechSynthesizer _bot;
    private readonly WindowsMediaPlayer _player;
    private readonly Random _random;
    private readonly string[] _lights;

    public HueLights(SpeechSynthesizer bot)
    {
        _client.Initialize(_appKey);

        _lightCommand = new LightCommand()
        {
            On = true
        };

        _lights = new[]
        {
            "4", //Floor Light by Erik's Table
            "5", //Kristian's Tablelight
            "6", //Roof
            "7", //Shelf
            "13", //Kristian Under Table
            "14" //Erik Under Table
        };

        _bot = bot;
        _player = new WindowsMediaPlayer();
        _random = new Random();
    }

    public void TurnOn()
    {
        _bot.SpeakAsync("Turning on lights.");
        Console.WriteLine("Bot: Turning on lights.");
        _lightCommand.TurnOn();
        _client.SendCommandAsync(_lightCommand, _lights);

    }
    public void TurnOff()
    {
        _bot.SpeakAsync("Turning off lights.");
        Console.WriteLine("Bot: Turning off lights.");
        _lightCommand.TurnOff();
        _client.SendCommandAsync(_lightCommand, _lights);
    }

    public void ChangeColor(string color)
    {
        _bot.SpeakAsync($"Changing light color.");
        Console.WriteLine($"Bot: Changing light color.");
        _lightCommand.TurnOn().SetColor(new RGBColor(color));
        _client.SendCommandAsync(_lightCommand, _lights);
    }

    public void RandomColors()
    {
        _bot.SpeakAsync($"Changing to random colors.");
        Console.WriteLine($"Bot: Changing to random colors.");
        foreach (var light in _lights)
        {
            int red = _random.Next(256);
            int blue = _random.Next(256);
            int green = _random.Next(256);
            _lightCommand.TurnOn().SetColor(new RGBColor(red, green, blue));
            _client.SendCommandAsync(_lightCommand, new List<string> { light });
        }
    }

    public void PartyTime()
    {
        _bot.SpeakAsync("The time is:" + DateTime.Now.ToString("h mm") + "and it is time to party.");
        Console.WriteLine("Bot: The time is: " + DateTime.Now.ToString("h mm") + " and it is time to party.");
        Thread.Sleep(4000);

        //Starting a new thread for the while loop so it loops in the background
        var ts = new ThreadStart(BackgroundParty);
        var backgroundThread = new Thread(ts);
        backgroundThread.Start();

        _player.URL = @"D:\Code\GET Academy\Modul 3\SpeechCommands\HomeAssistantProject\SpeechCommands\sound\rickroll.wav";
        _player.settings.volume = 30;
        _player.controls.play();
    }

    private void BackgroundParty()
    {
        _partyTime = true;
        while (_partyTime)
        {
            //_lightCommand.Alert = Alert.Once;
            //_client.SendCommandAsync(_lightCommand, _lights);
            Thread.Sleep(500);
            foreach (var light in _lights)
            {
                int red = _random.Next(256);
                int blue = _random.Next(256);
                int green = _random.Next(256);
                _lightCommand.TurnOn().SetColor(new RGBColor(red, green, blue));
                _client.SendCommandAsync(_lightCommand, new List<string> { light });
            }
        }
    }

    public void StopPartyTime()
    {
        _player.controls.stop();
        _partyTime = false;
        _bot.SpeakAsync("Turning off party time.");
        Console.WriteLine("Bot: Turning off party time.");
        _lightCommand.TurnOff();
        _client.SendCommandAsync(_lightCommand, _lights);
    }
}
