using System.Speech.Synthesis;

namespace SpeechCommands;

internal class SpeechController
{
    public SpeechRecognition Recognition;
    public SpeechSynthesizer Bot;
    public CoinFlip CoinFlip;
    public HueLights HueLights;
        
    public SpeechController()
    {
        Bot = new SpeechSynthesizer();
        CoinFlip = new CoinFlip(Bot);
        HueLights = new HueLights(Bot);
        Recognition = new SpeechRecognition(Bot, CoinFlip, HueLights);
    }
}