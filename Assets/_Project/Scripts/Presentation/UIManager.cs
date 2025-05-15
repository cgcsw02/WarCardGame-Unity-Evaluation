using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Networking;
using Game.Model;

namespace Game.Presentation
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel, gameScreenPanel, endScreenPanel;
        [SerializeField] private Image deckImage; // Fixed deck image
        [SerializeField] private Image playerCardImage, botCardImage;
        [SerializeField] private TextMeshProUGUI playerScoreText, botScoreText, messageText, resultText, roundText; // Added roundText
        [SerializeField] private Button drawButton;
        [SerializeField] private Sprite deckSprite; // Card back for deck
        [SerializeField] private Sprite emptyPlaceholderSprite; // White/transparent placeholder
        [SerializeField] private SimpleSoundPlayer soundPlayer; // Reference to play sounds

        private void Awake()
        {
            // Validate assignments
            if (deckSprite == null)
                Debug.LogError("Deck sprite not assigned in UIManager!");
            if (emptyPlaceholderSprite == null)
                Debug.LogError("Empty placeholder sprite not assigned in UIManager!");
            if (deckImage == null)
                Debug.LogError("Deck image not assigned in UIManager!");
            if (playerCardImage == null)
                Debug.LogError("Player card image not assigned in UIManager!");
            if (botCardImage == null)
                Debug.LogError("Bot card image not assigned in UIManager!");
            if (messageText == null)
                Debug.LogError("MessageText not assigned in UIManager!");
            if (resultText == null)
                Debug.LogError("ResultText not assigned in UIManager!");
            if (roundText == null)
                Debug.LogError("RoundText not assigned in UIManager!");
            if (soundPlayer == null)
                Debug.LogError("SoundPlayer not assigned in UIManager!");
            if (drawButton == null)
                Debug.LogError("DrawButton not assigned in UIManager!");

            // Set initial sprites and ensure visibility
            if (deckImage != null)
            {
                deckImage.sprite = deckSprite;
                deckImage.color = Color.white;
                deckImage.transform.localScale = Vector3.one;
            }
            if (playerCardImage != null)
            {
                playerCardImage.sprite = emptyPlaceholderSprite;
                playerCardImage.color = Color.white;
                playerCardImage.transform.localScale = Vector3.one;
            }
            if (botCardImage != null)
            {
                botCardImage.sprite = emptyPlaceholderSprite;
                botCardImage.color = Color.white;
                botCardImage.transform.localScale = Vector3.one;
            }
            if (messageText != null)
            {
                messageText.text = "";
            }
            if (roundText != null)
            {
                roundText.text = "";
            }
        }

        public void ShowMainMenu()
        {
            SetActivePanel(mainMenuPanel);
        }

        public void ShowGameScreen()
        {
            SetActivePanel(gameScreenPanel);
            ResetCardImages(); // Reset to empty placeholders
            if (roundText != null)
            {
                roundText.text = "";
                Debug.Log("Cleared round text");
            }
        }

        public async UniTask ShowEndScreen(string result)
        {
            if (resultText != null)
            {
                resultText.text = result;
            }
            var cg = endScreenPanel.GetComponent<CanvasGroup>() ?? endScreenPanel.AddComponent<CanvasGroup>();
            cg.alpha = 0;
            SetActivePanel(endScreenPanel);
            await cg.DOFade(1f, 0.5f).AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask DisplayCards(Card playerCard, Card botCard)
        {
            Debug.Log($"Displaying Player card: {playerCard.Value}_{playerCard.Suit}, URL: {playerCard.Image}");
            Debug.Log($"Displaying Bot card: {botCard.Value}_{botCard.Suit}, URL: {botCard.Image}");

            // Load card textures from API URLs
            Sprite playerSprite = await LoadCardSpriteAsync(playerCard.Image);
            Sprite botSprite = await LoadCardSpriteAsync(botCard.Image);

            Debug.Log($"Player sprite: {(playerSprite != null ? "Loaded" : "Null")}");
            Debug.Log($"Bot sprite: {(botSprite != null ? "Loaded" : "Null")}");

            // Animate Player card with popup effect and play card flip sound
            if (playerSprite != null && playerCardImage != null)
            {
                playerCardImage.sprite = playerSprite;
                playerCardImage.color = Color.white;
                playerCardImage.transform.localScale = Vector3.zero;
                if (soundPlayer != null)
                {
                    soundPlayer.PlayCardFlip();
                    Debug.Log("Playing card flip sound for Player card reveal");
                }
                Sequence playerSequence = DOTween.Sequence();
                playerSequence.Append(playerCardImage.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
                playerSequence.Append(playerCardImage.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine));
                await playerSequence.AsyncWaitForCompletion().AsUniTask();
            }
            else
            {
                Debug.LogWarning("Player card sprite not updated due to null sprite or image!");
                if (playerCardImage != null)
                    playerCardImage.sprite = emptyPlaceholderSprite; // Fallback
            }

            // Simulate bot "thinking" delay with text animation
            int thinkingDelay = UnityEngine.Random.Range(500, 1001); // 500–1000ms
            Debug.Log($"Bot thinking for {thinkingDelay}ms");
            if (messageText != null)
            {
                await ShowThinkingAnimationAsync(thinkingDelay);
            }
            else
            {
                await UniTask.Delay(thinkingDelay); // Fallback if messageText is null
            }

            // Animate Bot card with popup effect and play card flip sound
            if (botSprite != null && botCardImage != null)
            {
                botCardImage.sprite = botSprite;
                botCardImage.color = Color.white;
                botCardImage.transform.localScale = Vector3.zero;
                if (soundPlayer != null)
                {
                    soundPlayer.PlayCardFlip();
                    Debug.Log("Playing card flip sound for Bot card reveal");
                }
                Sequence botSequence = DOTween.Sequence();
                botSequence.Append(botCardImage.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));
                botSequence.Append(botCardImage.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine));
                await botSequence.AsyncWaitForCompletion().AsUniTask();
            }
            else
            {
                Debug.LogWarning("Bot card sprite not updated due to null sprite or image!");
                if (botCardImage != null)
                    botCardImage.sprite = emptyPlaceholderSprite; // Fallback
            }
        }

        private async UniTask ShowThinkingAnimationAsync(int durationMs)
        {
            if (messageText == null)
            {
                Debug.LogWarning("MessageText is null, cannot show thinking animation!");
                await UniTask.Delay(durationMs);
                return;
            }

            string baseText = "Bot is thinking";
            int cycleTimeMs = 250; // Update every 250ms
            int cycles = durationMs / cycleTimeMs; // Number of text updates
            for (int i = 0; i <= cycles; i++)
            {
                int dotCount = i % 3 + 1; // Cycle through 1, 2, 3 dots
                messageText.text = baseText + new string('.', dotCount);
                Debug.Log($"Showing thinking text: {messageText.text}");
                await UniTask.Delay(cycleTimeMs);
                if (i * cycleTimeMs >= durationMs) break; // Ensure we don't exceed duration
            }

            // Clear text after animation
            messageText.text = "";
            Debug.Log("Cleared thinking text");
        }

        private async UniTask<Sprite> LoadCardSpriteAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("Card image URL is empty!");
                return emptyPlaceholderSprite; // Fallback to placeholder
            }

            using (var request = UnityWebRequestTexture.GetTexture(url))
            {
                var operation = await request.SendWebRequest().ToUniTask();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load texture from {url}: {request.error}");
                    return emptyPlaceholderSprite; // Fallback to placeholder
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
        }

        public void ResetCardImages()
        {
            if (playerCardImage != null)
            {
                playerCardImage.sprite = emptyPlaceholderSprite;
                playerCardImage.color = Color.white;
                playerCardImage.transform.localScale = Vector3.one;
            }
            if (botCardImage != null)
            {
                botCardImage.sprite = emptyPlaceholderSprite;
                botCardImage.color = Color.white;
                botCardImage.transform.localScale = Vector3.one;
            }
            Debug.Log("Card images reset to empty placeholders");
        }

        public void UpdateScores(int playerScore, int botScore)
        {
            if (playerScoreText != null)
                playerScoreText.text = $"Player: {playerScore}";
            if (botScoreText != null)
                botScoreText.text = $"Bot: {botScore}";
        }

        public void ShowMessage(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
                Debug.Log($"Showing message: {message}");
            }
            else
            {
                Debug.LogWarning("MessageText is null, cannot show message!");
            }
        }

        public void ShowRoundNumber(int roundCount)
        {
            if (roundText != null)
            {
                roundText.text = $"Round {roundCount}";
                Debug.Log($"Showing round number: Round {roundCount}");
            }
            else
            {
                Debug.LogWarning("RoundText is null, cannot show round number!");
            }
        }

        public void EnableDrawButton(bool enabled)
        {
            if (drawButton != null)
            {
                drawButton.interactable = enabled;
                Debug.Log($"Draw button {(enabled ? "enabled" : "disabled")}");
            }
            else
                Debug.LogError("Draw button not assigned in UIManager!");
        }

        private void SetActivePanel(GameObject active)
        {
            mainMenuPanel.SetActive(active == mainMenuPanel);
            gameScreenPanel.SetActive(active == gameScreenPanel);
            endScreenPanel.SetActive(active == endScreenPanel);
        }
    }
}