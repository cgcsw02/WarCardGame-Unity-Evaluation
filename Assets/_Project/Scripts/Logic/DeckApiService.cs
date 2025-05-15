using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Game.Model;
using UnityEngine;

namespace Game.Logic
{
    public class DeckApiService : IDeckApiService
    {
        public async UniTask<string> CreateNewDeckAsync()
        {
            string url = "https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1";
            using (var request = UnityWebRequest.Get(url))
            {
                var operation = await request.SendWebRequest().ToUniTask();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to create deck: {request.error}");
                    return null;
                }

                string json = request.downloadHandler.text;
                var deckResponse = JsonConvert.DeserializeObject<DeckResponse>(json);
                Debug.Log($"Created new deck with ID: {deckResponse.deck_id}");
                return deckResponse.deck_id;
            }
        }

        public async UniTask<(Card, Card)> DrawTwoCardsAsync(string deckId)
        {
            if (string.IsNullOrEmpty(deckId))
            {
                Debug.LogError("Deck ID is empty!");
                return (null, null);
            }

            string url = $"https://deckofcardsapi.com/api/deck/{deckId}/draw/?count=2";
            using (var request = UnityWebRequest.Get(url))
            {
                var operation = await request.SendWebRequest().ToUniTask();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to draw cards: {request.error}");
                    return (null, null);
                }

                string json = request.downloadHandler.text;
                var drawResponse = JsonConvert.DeserializeObject<DrawResponse>(json);
                if (drawResponse.cards == null || drawResponse.cards.Length < 2)
                {
                    Debug.LogError("Failed to draw two cards!");
                    return (null, null);
                }

                Debug.Log($"Drew cards: Player={drawResponse.cards[0].Value}_{drawResponse.cards[0].Suit}, Bot={drawResponse.cards[1].Value}_{drawResponse.cards[1].Suit}");
                return (drawResponse.cards[0], drawResponse.cards[1]);
            }
        }

        public async UniTask<bool> IsDeckEmptyAsync(string deckId)
        {
            if (string.IsNullOrEmpty(deckId))
            {
                Debug.LogError("Deck ID is empty!");
                return true;
            }

            string url = $"https://deckofcardsapi.com/api/deck/{deckId}/";
            using (var request = UnityWebRequest.Get(url))
            {
                var operation = await request.SendWebRequest().ToUniTask();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to check deck: {request.error}");
                    return true;
                }

                string json = request.downloadHandler.text;
                var deckResponse = JsonConvert.DeserializeObject<DeckResponse>(json);
                bool isEmpty = deckResponse.remaining <= 0;
                Debug.Log($"Deck {deckId} has {deckResponse.remaining} cards remaining. Is empty: {isEmpty}");
                return isEmpty;
            }
        }
    }

    // Helper classes for JSON deserialization
    public class DeckResponse
    {
        public string deck_id { get; set; }
        public bool shuffled { get; set; }
        public int remaining { get; set; }
    }

    public class DrawResponse
    {
        public Card[] cards { get; set; }
    }
}