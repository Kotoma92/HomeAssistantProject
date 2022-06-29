using System;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechCommands;

internal class SpeechRecognition
{
    private readonly SpeechRecognitionEngine _recognition = new();
    private readonly SpeechSynthesizer _bot;
    private readonly SpeechModel _model;
    private SpeechModelContext _modelContext;
    public bool Enabled = true;
    private readonly HueLights _hueLights;
    private readonly CoinFlip _coinFlip;

    public SpeechRecognition(SpeechSynthesizer bot, CoinFlip coinFlip, HueLights hueLights)
    {
        _bot = bot;
        _hueLights = hueLights;
        _coinFlip = coinFlip;
        _model = new SpeechModel();

        _recognition.SetInputToDefaultAudioDevice();
        _recognition.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
        _recognition.SpeechRecognized += SpeechRecognized;
        _recognition.RecognizeAsync(RecognizeMode.Multiple);
    }

    private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        string color;
        var speech = e.Result.Text;
        Console.WriteLine($"User: {speech}");

        if (speech == "Hello there")
        {
            _bot.SpeakAsync("Hello.");
            Console.WriteLine("Bot: Hello.");
        }
        if (speech == "How are you")
        {
            _bot.SpeakAsync("I am good, though I am a bot so I don't have feelings.");
            Console.WriteLine("Bot: I am good, though I am a bot so I don't have feelings.");
        }
        if (speech == "Turn on lights") _hueLights.TurnOn();
        if (speech == "Turn off lights") _hueLights.TurnOff();
        if (speech == "Change color to red") 
        {
            color = "FF0000"; 
            _hueLights.ChangeColor(color);
        }
        if (speech == "Change color to blue")
        {
            color = "0000FF";
            _hueLights.ChangeColor(color);
        }
        if (speech == "Change color to green")
        {
            color = "00FF00";
            _hueLights.ChangeColor(color);
        }
        if (speech == "Random colors") _hueLights.RandomColors();
        if (speech == "Party time") _hueLights.PartyTime();
        if (speech == "Stop party time") _hueLights.StopPartyTime();
        if (speech == "Flip a coin") _coinFlip.Flip();
        if (speech == "Shutdown") ShutDown(speech);
        _ = LoggingSpeechToDb(speech);
    }

    private void ShutDown(string speech)
    {
        _bot.SpeakAsync("Shutting down.");
        Console.WriteLine("Bot: Shutting Down...");
        Thread.Sleep(1000);
        Enabled = false;
    }

    private async Task LoggingSpeechToDb(string speech)
    {
        _modelContext = new SpeechModelContext();
        _model.CommandId = Guid.NewGuid().ToString();
        _model.DateTimeNow = DateTime.Now;
        _model.Commands = speech;
        _modelContext.Speech.Add(_model);
        await _modelContext.SaveChangesAsync();
    }
}