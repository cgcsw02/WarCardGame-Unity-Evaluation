using Cysharp.Threading.Tasks;
    using UnityEngine;

    namespace Game.Logic
    {
        public class GameLogic : IGameLogic
        {
            private Game.Model.Deck gameDeck;
            private int playerPoints, botPoints, roundCount;
            public int PlayerPoints => playerPoints;
            public int BotPoints => botPoints;

            public GameLogic()
            {
                gameDeck = new Game.Model.Deck();
                playerPoints = 0;
                botPoints = 0;
                roundCount = 0;
            }

            public async UniTask<GameRoundResult> PlayGameRoundAsync()
            {
                GameRoundResult roundResult = new GameRoundResult();
                roundCount++;

                if (gameDeck.IsDeckEmpty())
                {
                    roundResult.isGameOver = true;
                    roundResult.gameResult = DetermineWinner();
                    return roundResult;
                }

                var playerCard = gameDeck.DrawCard();
                var botCard = gameDeck.DrawCard();

                roundResult.playerCard = playerCard;
                roundResult.botCard = botCard;
                int comparison = Game.Model.CardComparer.CompareCards(playerCard, botCard);

                if (comparison > 0)
                {
                    playerPoints++;
                    roundResult.outcome = "Player wins this round!";
                }
                else if (comparison < 0)
                {
                    botPoints++;
                    roundResult.outcome = "Bot wins this round!";
                }
                else
                {
                    roundResult.outcome = "It's a tie!";
                }

                roundResult.isGameOver = roundCount >= 8 || playerPoints >= 5 || botPoints >= 5;
                if (roundResult.isGameOver)
                {
                    roundResult.gameResult = DetermineWinner();
                }

                await UniTask.Delay(UnityEngine.Random.Range(300, 1001)); // Random bot "thinking" delay
                return roundResult;
            }

            private string DetermineWinner()
            {
                if (playerPoints > botPoints) return "Player Wins the Game!";
                if (botPoints > playerPoints) return "Bot Wins the Game!";
                return "Game Tied!";
            }
        }
    }