using Cysharp.Threading.Tasks;
using Game.Model;

namespace Game.Logic
{
    public interface IDeckApiService
    {
        UniTask<(Card, Card)> DrawTwoCardsAsync(string deckId);
        UniTask<bool> IsDeckEmptyAsync(string deckId);
        UniTask<string> CreateNewDeckAsync(); // Added for new deck creation
    }
}