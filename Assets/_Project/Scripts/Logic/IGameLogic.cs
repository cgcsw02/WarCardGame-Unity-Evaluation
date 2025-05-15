using Cysharp.Threading.Tasks;

namespace Game.Logic
{
    public interface IGameLogic
    {
        UniTask<GameRoundResult> PlayGameRoundAsync();
        int PlayerPoints { get; }
        int BotPoints { get; }
    }
}