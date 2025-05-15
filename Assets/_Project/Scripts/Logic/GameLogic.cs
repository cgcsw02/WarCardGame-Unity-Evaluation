using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Logic
{
    public class GameLogic : IGameLogic
    {
        private IDeckApiService _deckApiService;
        private string _deckId;
        private int _playerPoints, _botPoints, _roundCount;
        public int PlayerPoints => _playerPoints;
        public int BotPoints => _botPoints;
        public int RoundCount => _roundCount; // Ensures RoundCount is implemented

        public GameLogic()
        {
            // Parameterless constructor for initialization
        }

        public void Initialize(IDeckApiService deckApiService, string deckId)
        {
            _deckApiService = deckApiService;
            _deckId = deckId;
            _playerPoints = 0;
            _botPoints = 0;
            _roundCount = 0;
            Debug.Log("GameLogic state reset: points and round count set to 0, new deck ID assigned");
        }

        public async UniTask<GameRoundResult> PlayGameRoundAsync()
        {
            GameRoundResult roundResult = new GameRoundResult();
            _roundCount++;

            if (await _deckApiService.IsDeckEmptyAsync(_deckId))
            {
                roundResult.isGameOver = true;
                roundResult.gameResult = DetermineWinner();
                return roundResult;
            }

            var (playerCard, botCard) = await _deckApiService.DrawTwoCardsAsync(_deckId);
            if (playerCard == null || botCard == null)
            {
                roundResult.isGameOver = true;
                roundResult.gameResult = "Error drawing cards!";
                return roundResult;
            }

            roundResult.playerCard = playerCard;
            roundResult.botCard = botCard;
            int comparison = Game.Model.CardComparer.CompareCards(playerCard, botCard);

            if (comparison > 0)
            {
                _playerPoints++;
                roundResult.outcome = "Player wins this round!";
            }
            else if (comparison < 0)
            {
                _botPoints++;
                roundResult.outcome = "Bot wins this round!";
            }
            else
            {
                roundResult.outcome = "It's a tie!";
            }

            roundResult.isGameOver = _roundCount >= 8 || _playerPoints >= 5 || _botPoints >= 5;
            if (roundResult.isGameOver)
            {
                roundResult.gameResult = DetermineWinner();
            }

            return roundResult;
        }

        private string DetermineWinner()
        {
            if (_playerPoints > _botPoints) return "Player Wins the Game!";
            if (_botPoints > _playerPoints) return "Bot Wins the Game!";
            return "Game Tied!";
        }
    }
}