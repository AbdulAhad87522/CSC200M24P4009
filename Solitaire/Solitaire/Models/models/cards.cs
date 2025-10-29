//namespace Solitaire.Models.models
//{
//    public class Card
//    {
//        public string Suit { get; set; }
//        public int Rank { get; set; }
//        public bool IsFaceUp { get; set; }

//        // Constructor with optional parameter for IsFaceUp
//        public Card(string suit, int rank, bool isFaceUp = false)
//        {
//            Suit = suit;
//            Rank = rank;
//            IsFaceUp = isFaceUp;
//        }

//        // Property to get rank name (Ace, King, Queen, Jack)
//        public string RankName
//        {
//            get
//            {
//                return Rank switch
//                {
//                    1 => "Ace",
//                    11 => "Jack",
//                    12 => "Queen",
//                    13 => "King",
//                    _ => Rank.ToString()
//                };
//            }
//        }

//        // Property to get suit symbol
//        public string SuitSymbol
//        {
//            get
//            {
//                return Suit.ToLower() switch
//                {
//                    "hearts" => "♥",
//                    "diamonds" => "♦",
//                    "clubs" => "♣",
//                    "spades" => "♠",
//                    _ => Suit[0].ToString().ToUpper()
//                };
//            }
//        }

//        // Method to flip the card
//        public void Flip()
//        {
//            IsFaceUp = !IsFaceUp;
//        }

//        // Override ToString for better display
//        public override string ToString()
//        {
//            if (!IsFaceUp)
//                return "🂠 (Face Down)";

//            return $"{RankName} of {Suit} {SuitSymbol}";
//        }

//        // Method to check if card is a face card
//        public bool IsFaceCard => Rank >= 11 && Rank <= 13;

//        // Method to check if card is an Ace
//        public bool IsAce => Rank == 1;
//    }
//}


namespace Solitaire.Models.models
{
    public class Card
    {
        public string Suit { get; set; }
        public int Rank { get; set; }
        public bool IsFaceUp { get; set; }
        public string Color { get; set; } // ADDED: Color property to store card color

        // UPDATED: Constructor now calculates and sets color
        public Card(string suit, int rank, bool isFaceUp = false)
        {
            Suit = suit;
            Rank = rank;
            IsFaceUp = isFaceUp;
            Color = DetermineColor(suit); // ADDED: Set color based on suit
        }

        // ADDED: Helper method to determine color from suit
        private string DetermineColor(string suit)
        {
            string suitLower = suit.ToLower();
            if (suitLower == "hearts" || suitLower == "diamonds")
                return "Red";
            else if (suitLower == "clubs" || suitLower == "spades")
                return "Black";
            return "Unknown";
        }

        // ADDED: Property to check if card is red
        public bool IsRed => Color == "Red";

        // ADDED: Property to check if card is black
        public bool IsBlack => Color == "Black";

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

        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }

        // UPDATED: ToString now includes color information
        public override string ToString()
        {
            if (!IsFaceUp)
                return "🂠 (Face Down)";

            return $"{RankName} of {Suit} {SuitSymbol} ({Color})"; // UPDATED: Added color display
        }

        // ADDED: Method to get colored display for console with ANSI color codes
        public string GetColoredDisplay()
        {
            if (!IsFaceUp)
                return "🂠";

            string colorCode = IsRed ? "\u001b[31m" : "\u001b[30m";
            string reset = "\u001b[0m";
            return $"{colorCode}{RankName}{SuitSymbol}{reset}";
        }

        // ADDED: Method to get HTML color for web display
        public string GetHtmlColor()
        {
            return IsRed ? "#DC143C" : "#000000";
        }

        public bool IsFaceCard => Rank >= 11 && Rank <= 13;

        public bool IsAce => Rank == 1;
    }
}