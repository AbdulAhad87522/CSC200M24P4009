namespace Solitaire.Models.models
{
    public class Card
    {
        public string Suit { get; set; }
        public int Rank { get; set; }
        public bool IsFaceUp { get; set; }

        // Constructor with optional parameter for IsFaceUp
        public Card(string suit, int rank, bool isFaceUp = false)
        {
            Suit = suit;
            Rank = rank;
            IsFaceUp = isFaceUp;
        }

        // Property to get rank name (Ace, King, Queen, Jack)
        public string RankName
        {
            get
            {
                return Rank switch
                {
                    1 => "Ace",
                    11 => "Jack",
                    12 => "Queen",
                    13 => "King",
                    _ => Rank.ToString()
                };
            }
        }

        // Property to get suit symbol
        public string SuitSymbol
        {
            get
            {
                return Suit.ToLower() switch
                {
                    "hearts" => "♥",
                    "diamonds" => "♦",
                    "clubs" => "♣",
                    "spades" => "♠",
                    _ => Suit[0].ToString().ToUpper()
                };
            }
        }

        // Method to flip the card
        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }

        // Override ToString for better display
        public override string ToString()
        {
            if (!IsFaceUp)
                return "🂠 (Face Down)";

            return $"{RankName} of {Suit} {SuitSymbol}";
        }

        // Method to check if card is a face card
        public bool IsFaceCard => Rank >= 11 && Rank <= 13;

        // Method to check if card is an Ace
        public bool IsAce => Rank == 1;
    }
}
