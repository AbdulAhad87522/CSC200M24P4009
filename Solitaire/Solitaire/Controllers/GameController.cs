using Microsoft.AspNetCore.Mvc;
using Solitaire.Models.models;
using System.Text.Json;

namespace Solitaire.Controllers
{
    public class GameController : Controller
    {
        private static GameEngine _game;

        public IActionResult Index()
        {
            _game = new GameEngine();
            var model = CreateViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult MakeMove([FromBody] MoveRequest request)
        {
            try
            {
                Console.WriteLine($"=== MOVE REQUEST ===");
                Console.WriteLine($"MoveType: {request.MoveType}");
                Console.WriteLine($"FromColumn: {request.FromColumn}");
                Console.WriteLine($"ToColumn: {request.ToColumn}");
                Console.WriteLine($"CardIndex: {request.CardIndex}");
                Console.WriteLine($"FoundationSuit: {request.FoundationSuit}");

                bool moveSuccessful = false;

                switch (request.MoveType?.ToLower())
                {
                    case "draw":
                        Console.WriteLine("Drawing from stock...");
                        _game.DrawFromStock();
                        moveSuccessful = true;
                        break;

                    case "tableau_to_tableau":
                        Console.WriteLine($"Moving tableau: {request.FromColumn} -> {request.ToColumn} at index {request.CardIndex}");
                        moveSuccessful = _game.MoveTableauCards(
                            request.FromColumn ?? 0,
                            request.ToColumn ?? 0,
                            request.CardIndex ?? 0
                        );
                        Console.WriteLine($"Tableau move result: {moveSuccessful}");
                        break;

                    case "waste_to_foundation":
                        Console.WriteLine($"Moving waste to {request.FoundationSuit} foundation");
                        moveSuccessful = _game.MoveWasteToFoundation(request.FoundationSuit);
                        Console.WriteLine($"Waste to foundation result: {moveSuccessful}");
                        break;

                    case "tableau_to_foundation":
                        Console.WriteLine($"Moving tableau to {request.FoundationSuit} foundation");
                        moveSuccessful = _game.MoveTableauToFoundation(
                            request.FromColumn ?? 0,
                            request.CardIndex ?? 0,
                            request.FoundationSuit
                        );
                        Console.WriteLine($"Tableau to foundation result: {moveSuccessful}");
                        break;

                    case "waste_to_tableau":
                        Console.WriteLine($"Moving waste to tableau column {request.ToColumn}");
                        moveSuccessful = MoveWasteToTableau(request.ToColumn ?? 0);
                        Console.WriteLine($"Waste to tableau result: {moveSuccessful}");
                        break;

                    default:
                        Console.WriteLine($"Unknown move type: {request.MoveType}");
                        return Json(new { success = false, error = $"Unknown move type: {request.MoveType}" });
                }

                var model = CreateViewModel();
                return Json(new
                {
                    success = moveSuccessful,
                    gameState = model,
                    isGameWon = _game.IsGameWon()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR in MakeMove: {ex}");
                return Json(new { success = false, error = ex.Message });
            }
        }



        private bool MoveWasteToTableau(int toColumn)
        {
            if (_game.Waste.IsEmpty()) return false;

            var wasteCard = _game.Waste.Peek();
            var targetColumn = _game.Tableau[toColumn];

            if (targetColumn.IsEmpty())
            {
                // Can only place King on empty column
                if (wasteCard.Rank == 13)
                {
                    var card = _game.Waste.Pop();
                    targetColumn.PushBack(card);
                    return true;
                }
            }
            else
            {
                var targetTopCard = targetColumn.tail.Data;
                if (_game.IsOppositeColor(wasteCard, targetTopCard) &&
                    wasteCard.Rank == targetTopCard.Rank - 1)
                {
                    var card = _game.Waste.Pop();
                    targetColumn.PushBack(card);
                    return true;
                }
            }

            return false;
        }

        [HttpPost]
        public IActionResult EmergencyTest()
        {
            try
            {
                // Test stock
                int stockBefore = _game.Stock.Size();
                _game.DrawFromStock();
                int stockAfter = _game.Stock.Size();
                int wasteAfter = _game.Waste.Size();

                return Json(new
                {
                    success = true,
                    message = $"Stock: {stockBefore} -> {stockAfter}, Waste: {wasteAfter}",
                    worked = stockAfter < stockBefore
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult NewGame()
        {
            _game = new GameEngine();
            var model = CreateViewModel();
            return Json(new { success = true, gameState = model });
        }

        private GameViewModel CreateViewModel()
        {
            var model = new GameViewModel
            {
                Tableau = new List<List<Card>>(),
                Foundations = new Dictionary<string, List<Card>>(),
                Waste = new List<Card>(),
                StockCount = _game.Stock.Size(),
                IsGameWon = _game.IsGameWon()
            };

            // Convert tableau to list of lists for easier display
            foreach (var column in _game.Tableau)
            {
                var columnCards = new List<Card>();
                var node = column.head;
                while (node != null)
                {
                    columnCards.Add(node.Data);
                    node = node.Next;
                }
                model.Tableau.Add(columnCards);
            }

            // Convert foundations
            var suits = new[] { "hearts", "diamonds", "clubs", "spades" };
            foreach (var suit in suits)
            {
                model.Foundations[suit] = new List<Card>();
                var foundationStack = _game.Foundations[suit];

                // Convert stack to list (top card last for display)
                var tempStack = new Stack<Card>();
                while (!foundationStack.IsEmpty())
                {
                    tempStack.Push(foundationStack.Pop());
                }

                // Restore stack and build list
                foreach (var card in tempStack)
                {
                    model.Foundations[suit].Add(card);
                    foundationStack.Push(card);
                }
                model.Foundations[suit].Reverse(); // So top card is last
            }

            // Convert waste stack to list
            var wasteCards = new List<Card>();
            var tempWasteStack = new Stack<Card>();
            while (!_game.Waste.IsEmpty())
            {
                var card = _game.Waste.Pop();
                tempWasteStack.Push(card);
                wasteCards.Add(card);
            }
            // Restore waste stack
            while (tempWasteStack.Count > 0)
            {
                _game.Waste.Push(tempWasteStack.Pop());
            }
            model.Waste = wasteCards;

            return model;
        }

    }

    public class MoveRequest
    {
        public string MoveType { get; set; }
        public int? FromColumn { get; set; }
        public int? ToColumn { get; set; }
        public int? CardIndex { get; set; }
        public string FoundationSuit { get; set; }
    }
}