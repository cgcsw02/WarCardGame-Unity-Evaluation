namespace Game.Model
{
    public class Card
    {
        public string Value { get; set; }
        public string Suit { get; set; }
        public string Image { get; set; } // URL for card image from API

        public Card(string value, string suit, string image = null)
        {
            Value = value;
            Suit = suit;
            Image = image;
        }

        public int GetCardRank()
        {
            if (CardComparer.rankDictionary.ContainsKey(Value.ToUpper()))
                return CardComparer.rankDictionary[Value.ToUpper()];
            return 0;
        }
    }
}