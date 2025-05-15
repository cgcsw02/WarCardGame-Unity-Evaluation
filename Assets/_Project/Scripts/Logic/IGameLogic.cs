using Cysharp.Threading.Tasks;

namespace Game.Logic
{
    public interface IGameLogic
    {
        int PlayerPoints { get; }
        int BotPoints { get; }
        int RoundCount { get; } // RoundCount property
        UniTask<GameRoundResult> PlayGameRoundAsync();
        void Initialize(IDeckApiService deckApiService, string deckId);
    }
}