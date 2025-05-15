using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Model
{
    public class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            CreateDeck();
            RandomizeDeck();
        }

        private void CreateDeck()
        {
            string[] suitArray = { "SPADES", "HEARTS", "DIAMONDS", "CLUBS" };
            string[] valueArray = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "JACK", "QUEEN", "KING", "ACE" };
            cards.Clear();
            foreach (string suit in suitArray)
            foreach (string val in valueArray)
                cards.Add(new Card(val, suit));
        }

        public void RandomizeDeck()
        {
            cards = cards.OrderBy(x => UnityEngine.Random.value).ToList();
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                Debug.LogError("Deck is empty! No more cards to draw.");
                return null;
            }
            Card drawnCard = cards[0];
            cards.RemoveAt(0);
            return drawnCard;
        }

        public bool IsDeckEmpty()
        {
            return cards.Count == 0;
        }
    }
}