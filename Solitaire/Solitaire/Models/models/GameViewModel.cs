namespace Solitaire.Models.models
{
    public class GameViewModel
    {
        public List<List<Card>> Tableau { get; set; }
        public Dictionary<string, List<Card>> Foundations { get; set; }
        public List<Card> Waste { get; set; }
        public int StockCount { get; set; }
        public bool IsGameWon { get; set; }
    }

    public class MoveRequest
    {
        public string MoveType { get; set; } // "draw", "tableau", "foundation"
        public int FromColumn { get; set; }
        public int ToColumn { get; set; }
        public string FoundationSuit { get; set; }
        public int CardIndex { get; set; }
    }
}
        