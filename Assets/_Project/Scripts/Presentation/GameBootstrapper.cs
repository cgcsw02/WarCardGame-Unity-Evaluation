using UnityEngine;
using Game.Logic;
using Game.Presentation;

namespace Game
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SimpleSoundPlayer soundPlayer;
        [SerializeField] private UnityEngine.UI.Button startButton;
        [SerializeField] private UnityEngine.UI.Button drawButton;
        [SerializeField] private UnityEngine.UI.Button restartButton;
        [SerializeField] private GameManager gameManager; // Serialized field for GameManager

        private void Awake()
        {
            // Validate serialized fields
            if (gameManager == null)
            {
                // Fallback: Try to find GameManager
                gameManager = FindObjectOfType<GameManager>();
                if (gameManager == null)
                {
                    // Create new GameManager GameObject
                    GameObject gameManagerObj = new GameObject("GameManager");
                    gameManager = gameManagerObj.AddComponent<GameManager>();
                    Debug.LogWarning("GameManager not assigned in Inspector or found in scene. Created new GameManager GameObject.");
                }
                else
                {
                    Debug.Log("GameManager found via FindObjectOfType.");
                }
            }

            if (uiManager == null) Debug.LogError("UIManager not assigned in GameBootstrapper!");
            if (soundPlayer == null) Debug.LogError("SimpleSoundPlayer not assigned in GameBootstrapper!");
            if (startButton == null) Debug.LogError("StartButton not assigned in GameBootstrapper!");
            if (drawButton == null) Debug.LogError("DrawButton not assigned in GameBootstrapper!");
            if (restartButton == null) Debug.LogError("RestartButton not assigned in GameBootstrapper!");

            // Initialize services
            IDeckApiService deckApiService = new DeckApiService();
            IGameLogic gameLogic = new GameLogic();

            // Initialize GameManager with dependencies
            gameManager.Initialize(gameLogic, deckApiService, uiManager, soundPlayer, startButton, drawButton, restartButton);
            Debug.Log("GameBootstrapper initialized dependencies successfully");
        }
    }
}