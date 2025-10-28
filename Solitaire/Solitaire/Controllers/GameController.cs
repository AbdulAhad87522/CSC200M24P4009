using Microsoft.AspNetCore.Mvc;
using Solitaire.Models.models;

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
                Console.WriteLine($"Received move request: {request.MoveType}");

                bool moveSuccessful = false;

                switch (request.MoveType?.ToLower())
                {
                    case "draw":
                        _game.DrawFromStock();
                        moveSuccessful = true;
                        break;

                    case "tableau_to_tableau":
                        moveSuccessful = _game.MoveTableauCards(
                            request.FromColumn,
                            request.ToColumn,
                            request.CardIndex
                        );
                        break;

                    case "waste_to_foundation":
                        moveSuccessful = _game.MoveToFoundation(request.FoundationSuit);
                        break;

                    case "tableau_to_foundation":
                        moveSuccessful = _game.MoveToFoundation(request.FoundationSuit);
                        break;

                    case "waste_to_tableau":
                        moveSuccessful = MoveWasteToTableau(request.ToColumn);
                        break;

                    default:
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
                Console.WriteLine($"Error in MakeMove: {ex}");
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

            // Convert waste stack to list
            var wasteCards = new List<Card>();
            var tempStack = new Stack<Card>();
            while (!_game.Waste.IsEmpty())
            {
                var card = _game.Waste.Pop();
                tempStack.Push(card);
                wasteCards.Add(card);
            }
            // Restore waste stack
            foreach (var card in wasteCards)
            {
                _game.Waste.Push(card);
            }
            model.Waste = wasteCards;

            return model;
        }
    }
}
    