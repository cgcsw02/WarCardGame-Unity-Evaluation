using System;

namespace Game.Model
{
    public class Card
    {
        public string value;
        public string suit;

        public Card(string v, string s)
        {
            value = v;
            suit = s;
        }

        public int GetCardRank()
        {
            if (CardComparer.rankDictionary.ContainsKey(value.ToUpper()))
                return CardComparer.rankDictionary[value.ToUpper()];
            return 0;
        }
    }
}