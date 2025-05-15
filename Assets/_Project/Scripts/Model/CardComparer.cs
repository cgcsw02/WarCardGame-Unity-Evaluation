using System.Collections.Generic;

namespace Game.Model
{
    public static class CardComparer
    {
        public static Dictionary<string, int> rankDictionary = new Dictionary<string, int>
        {
            {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6}, {"7", 7}, {"8", 8}, {"9", 9},
            {"10", 10}, {"JACK", 11}, {"QUEEN", 12}, {"KING", 13}, {"ACE", 14}
        };

        public static int CompareCards(Card a, Card b)
        {
            return a.GetCardRank() - b.GetCardRank();
        }
    }
}