using UnityEngine;
using Cysharp.Threading.Tasks;
using Game.Logic;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class GameManager : MonoBehaviour
    {
        private IGameLogic _gameLogic;
        private IDeckApiService _deckApiService;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private SimpleSoundPlayer _soundPlayer;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _DrawButton;
        [SerializeField] private Button _RestartButtonn;

        public void Initialize(IGameLogic gameLogic, IDeckApiService deckApiService, UIManager uiManager, SimpleSoundPlayer soundPlayer, Button start_Button, Button Draw_Button, Button Restart_Buttonn)
        {
            _gameLogic = gameLogic;
            _deckApiService = deckApiService;
            _uiManager = uiManager;
            _soundPlayer = soundPlayer;
            
            _startButton = start_Button;
            _startButton.onClick.AddListener(StartGame);
            
            _DrawButton = Draw_Button;
            _DrawButton.onClick.AddListener(PlayRound);
            
            _RestartButtonn = Restart_Buttonn;
            _RestartButtonn.onClick.AddListener(StartGame);
            
            Debug.Log("GameManager initialized");
        }

        private void Start()
        {
            if (_uiManager == null)
            {
                Debug.LogError("UIManager not initialized in Start!");
                return;
            }
            
            _uiManager.ShowMainMenu();
            
            if (_soundPlayer != null)
                _soundPlayer.PlayloopMusic();
            else
                Debug.LogError("SoundPlayer not initialized!");
        }

        public async void StartGame()
        {
            Debug.Log("Starting game");
            
            // Fetch a new deck ID
            string newDeckId = await _deckApiService.CreateNewDeckAsync();
            if (string.IsNullOrEmpty(newDeckId))
            {
                Debug.LogError("Failed to create new deck!");
                _uiManager.ShowMessage("Error: Could not create deck!");
                return;
            }
            
            // Reinitialize GameLogic with new deck
            _gameLogic.Initialize(_deckApiService, newDeckId);
            Debug.Log($"GameLogic initialized with new deck ID: {newDeckId}");
            
            // Reset UI
            _uiManager.ShowGameScreen();
            _uiManager.UpdateScores(0, 0);
            _uiManager.EnableDrawButton(true);
            await UniTask.Yield();
        }

        public async void PlayRound()
        {
            Debug.Log("Playing round");
            _uiManager.EnableDrawButton(false);
            // Show round number
            _uiManager.ShowRoundNumber(_gameLogic.RoundCount + 1); // +1 because roundCount increments after
            _soundPlayer.PlayCardFlip();
            var roundResult = await _gameLogic.PlayGameRoundAsync();
            if (roundResult.playerCard != null && roundResult.botCard != null)
            {
                Debug.Log("Displaying cards");
                await _uiManager.DisplayCards(roundResult.playerCard, roundResult.botCard);
            }
            else
            {
                Debug.LogError("Player or Bot card is null!");
            }

            // Play sounds for round outcomes
            if (roundResult.outcome.Contains("Player wins"))
            {
                _soundPlayer.PlayWin();
                Debug.Log("Playing win sound for Player round win");
            }
            else if (roundResult.outcome.Contains("Bot wins"))
            {
                _soundPlayer.PlayNotCorrect();
                Debug.Log("Playing notCorrect sound for Bot round win");
            }

            _uiManager.ShowMessage(roundResult.outcome);
            _uiManager.UpdateScores(_gameLogic.PlayerPoints, _gameLogic.BotPoints);

            if (roundResult.isGameOver)
            {
                // Play win or lose sound for game result
                if (roundResult.gameResult.Contains("Player Wins"))
                {
                    _soundPlayer.PlayWin();
                    Debug.Log("Playing win sound for Player game win");
                }
                else if (roundResult.gameResult.Contains("Bot Wins") || roundResult.gameResult.Contains("Game Tied"))
                {
                    _soundPlayer.PlayLose();
                    Debug.Log("Playing lose sound for Bot game win or tie");
                }
                await _uiManager.ShowEndScreen(roundResult.gameResult);
            }
            else
            {
                // Delay to keep cards visible, then reset
                await UniTask.Delay(1500); // 1.5s to view cards
                _uiManager.ResetCardImages();
                _uiManager.EnableDrawButton(true);
            }
        }

        public void Cleanup()
        {
            _gameLogic = null;
            _deckApiService = null;
            _uiManager = null;
            _soundPlayer = null;
            if (_startButton != null)
                _startButton.onClick.RemoveListener(StartGame);
            if (_DrawButton != null)
                _DrawButton.onClick.RemoveListener(PlayRound);
            if (_RestartButtonn != null)
                _RestartButtonn.onClick.RemoveListener(StartGame);
            Debug.Log("GameManager cleaned up");
        }

        void OnDestroy()
        {
            Cleanup();
        }
    }
}