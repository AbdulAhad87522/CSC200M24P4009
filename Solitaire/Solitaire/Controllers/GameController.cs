using Microsoft.AspNetCore.Mvc;
using Solitaire.Models.models;
using System.Text.Json;

namespace Solitaire.Controllers
{
    public class GameController : Controller
    {
        private static GameEngine _game;
        private static object _gameLock = new object();

        public IActionResult Index()
        {
            lock (_gameLock)
            {
                _game = new GameEngine();
                var model = CreateViewModel();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult MakeMove([FromBody] MoveRequest request)
        {
            lock (_gameLock)
            {
                try
                {
                    if (_game == null)
                    {
                        _game = new GameEngine();
                    }

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
                            int fromCol = request.FromColumn ?? 0;
                            int toCol = request.ToColumn ?? 0;
                            int cardIdx = request.CardIndex ?? 0;

                            if (fromCol >= 0 && fromCol < 7 && toCol >= 0 && toCol < 7)
                            {
                                moveSuccessful = _game.MoveTableauCards(fromCol, toCol, cardIdx);
                                Console.WriteLine($"Tableau move result: {moveSuccessful}");
                            }
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
                            int wasteTo = request.ToColumn ?? 0;

                            if (wasteTo >= 0 && wasteTo < 7)
                            {
                                moveSuccessful = _game.MoveWasteToTableau(wasteTo);
                                Console.WriteLine($"Waste to tableau result: {moveSuccessful}");
                            }
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
                        isGameWon = _game.IsGameWon(),
                        error = moveSuccessful ? null : "Invalid move"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR in MakeMove: {ex}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Json(new { success = false, error = ex.Message });
                }
            }
        }

        [HttpPost]
        public IActionResult NewGame()
        {
            lock (_gameLock)
            {
                _game = new GameEngine();
                var model = CreateViewModel();
                return Json(new { success = true, gameState = model });
            }
        }

        // ✅ FIXED: Don't destroy stacks when creating view model
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

            // Convert tableau to list of lists
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

            // ✅ FIXED: Convert foundations WITHOUT destroying them
            var suits = new[] { "hearts", "diamonds", "clubs", "spades" };
            foreach (var suit in suits)
            {
                model.Foundations[suit] = GetStackAsList(_game.Foundations[suit]);
            }

            // ✅ FIXED: Convert waste WITHOUT destroying it
            model.Waste = GetStackAsList(_game.Waste);

            return model;
        }

        // ✅ NEW: Helper method to read stack without destroying it
        private List<Card> GetStackAsList(CustomStack<Card> stack)
        {
            var result = new List<Card>();
            var tempList = new List<Card>();

            // Pop all cards into temp list
            while (!stack.IsEmpty())
            {
                tempList.Add(stack.Pop());
            }

            // Reverse to get correct order (bottom to top)
            tempList.Reverse();

            // Push back onto stack to restore it
            foreach (var card in tempList)
            {
                stack.Push(card);
            }

            // Return the list in correct order
            result = new List<Card>(tempList);
            result.Reverse(); // Show top card last in list

            return result;
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