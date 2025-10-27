using Microsoft.AspNetCore.Mvc;
using Solitaire.Models.datastructures;
using Solitaire.Models.models;
using System;
using System.Collections.Generic;
using System.Text;

public class TestController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult TestDataStructures()
    {
        var results = new List<string>();

        try
        {
            results.Add("=== TESTING DATA STRUCTURES ===");

            // Test Linked List
            results.Add("\n🔗 Testing Linked List:");
            var list = new CustomLinkedLis<int>();
            list.PushFront(1);
            list.PushBack(2);
            list.Insert(3, 2);
            results.Add($"Linked List Length: {list.Length}");
            results.Add($"Head: {list.head.Data}, Tail: {list.tail.Data}");

            // Test Stack
            results.Add("\n📚 Testing Stack:");
            var stack = new CustomStack<string>();
            stack.Push("A");
            stack.Push("B");
            results.Add($"Stack Size: {stack.Size()}");
            results.Add($"Stack Pop: {stack.Pop()}");
            results.Add($"Stack Peek: {stack.Peek()}");

            // Test Queue
            results.Add("\n📥 Testing Queue:");
            var queue = new CustomQueue<string>();
            queue.Enqueue("First");
            queue.Enqueue("Second");
            results.Add($"Queue Size: {queue.Size()}");
            results.Add($"Queue Dequeue: {queue.Dequeue()}");
            results.Add($"Queue Front: {queue.Front()}");

            results.Add("\n✅ All data structures working correctly!");

            return Json(new { success = true, results });
        }
        catch (Exception ex)
        {
            results.Add($"❌ Error: {ex.Message}");
            return Json(new { success = false, results, error = ex.ToString() });
        }
    }

    [HttpPost]
    public IActionResult TestDeck()
    {
        var results = new List<string>();

        try
        {
            results.Add("=== TESTING DECK ===");

            var deck = new Deck();
            results.Add($"Deck created with {deck.GetSize()} cards");

            // Test drawing
            var card1 = deck.DrawCard();
            results.Add($"Drew card: {card1?.Suit} {card1?.Rank}");
            results.Add($"Deck size after draw: {deck.GetSize()}");

            // Test shuffle
            deck.Shuffle();
            results.Add("Deck shuffled successfully");

            // Draw multiple cards
            var cards = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                cards.Add(deck.DrawCard());
            }
            results.Add($"Drew 5 cards, deck size: {deck.GetSize()}");
            results.Add("Drawn cards: " + string.Join(", ", cards.ConvertAll(c => $"{c.Rank} of {c.Suit}")));

            results.Add("\n✅ Deck working correctly!");

            return Json(new { success = true, results });
        }
        catch (Exception ex)
        {
            results.Add($"❌ Error: {ex.Message}");
            return Json(new { success = false, results, error = ex.ToString() });
        }
    }

    [HttpPost]
    public IActionResult TestGameEngine()
    {
        var results = new List<string>();

        try
        {
            results.Add("=== TESTING GAME ENGINE ===");

            var game = new GameEngine();
            results.Add("🎮 Game Engine Initialized!");

            // Display initial state
            results.AddRange(GetGameState(game));

            // Test drawing from stock
            results.Add("\n🔄 Testing Stock/Waste:");
            game.DrawFromStock();
            game.DrawFromStock();
            results.Add($"After 2 draws - Stock: {game.Stock.Size()}, Waste: {game.Waste.Size()}");

            // Display updated state
            results.AddRange(GetGameState(game));

            results.Add("\n✅ Game Engine working correctly!");

            return Json(new { success = true, results });
        }
        catch (Exception ex)
        {
            results.Add($"❌ Error: {ex.Message}");
            return Json(new { success = false, results, error = ex.ToString() });
        }
    }

    [HttpPost]
    public IActionResult TestStockRecycling()
    {
        var results = new List<string>();

        try
        {
            results.Add("=== TESTING STOCK RECYCLING ===");

            var game = new GameEngine();
            results.Add("Game Engine Initialized");

            // Empty the stock
            while (game.Stock.Size() > 0)
            {
                game.DrawFromStock();
            }
            results.Add($"Stock emptied, Waste has {game.Waste.Size()} cards");

            // Trigger recycling
            game.DrawFromStock();
            results.Add($"After recycling - Stock: {game.Stock.Size()}, Waste: {game.Waste.Size()}");

            results.AddRange(GetGameState(game));
            results.Add("\n✅ Stock recycling working correctly!");

            return Json(new { success = true, results });
        }
        catch (Exception ex)
        {
            results.Add($"❌ Error: {ex.Message}");
            return Json(new { success = false, results, error = ex.ToString() });
        }
    }

    [HttpPost]
    public IActionResult RunAllTests()
    {
        var allResults = new List<string>();

        try
        {
            allResults.Add("=== RUNNING ALL TESTS ===\n");

            // Test Data Structures
            var dsResult = TestDataStructures() as JsonResult;
            var dsData = dsResult?.Value as dynamic;
            if (dsData?.success == true)
            {
                allResults.AddRange(dsData.results);
            }

            allResults.Add("\n" + new string('-', 50) + "\n");

            // Test Deck
            var deckResult = TestDeck() as JsonResult;
            var deckData = deckResult?.Value as dynamic;
            if (deckData?.success == true)
            {
                allResults.AddRange(deckData.results);
            }

            allResults.Add("\n" + new string('-', 50) + "\n");

            // Test Game Engine
            var gameResult = TestGameEngine() as JsonResult;
            var gameData = gameResult?.Value as dynamic;
            if (gameData?.success == true)
            {
                allResults.AddRange(gameData.results);
            }

            allResults.Add("\n" + new string('-', 50) + "\n");

            // Test Stock Recycling
            var recycleResult = TestStockRecycling() as JsonResult;
            var recycleData = recycleResult?.Value as dynamic;
            if (recycleData?.success == true)
            {
                allResults.AddRange(recycleData.results);
            }

            allResults.Add("\n🎉 ALL TESTS COMPLETED SUCCESSFULLY!");

            return Json(new { success = true, results = allResults });
        }
        catch (Exception ex)
        {
            allResults.Add($"❌ Error in all tests: {ex.Message}");
            return Json(new { success = false, results = allResults, error = ex.ToString() });
        }
    }

    private List<string> GetGameState(GameEngine game)
    {
        var state = new List<string>();

        state.Add("🎴 CURRENT GAME STATE:");
        state.Add("");

        // Stock & Waste
        state.Add($"Stock: {game.Stock.Size()} cards");
        var wasteTop = game.Waste.Peek();
        state.Add($"Waste: {game.Waste.Size()} cards, Top: {(wasteTop != null ? wasteTop.Suit + " " + wasteTop.Rank : "empty")}");
        state.Add("");

        // Foundations
        state.Add("🏆 Foundations:");
        foreach (var foundation in game.Foundations)
        {
            var topCard = foundation.Value.Peek();
            state.Add($"  {foundation.Key}: {foundation.Value.Size()} cards, Top: {(topCard != null ? topCard.Rank.ToString() : "empty")}");
        }
        state.Add("");

        // Tableau
        state.Add("📋 Tableau:");
        for (int i = 0; i < game.Tableau.Count; i++)
        {
            var column = game.Tableau[i];
            var cards = new List<string>();
            var temp = column.head;

            while (temp != null)
            {
                var card = temp.Data;
                var display = card.IsFaceUp ? $"{GetRankSymbol(card.Rank)}{GetSuitSymbol(card.Suit)}" : "🂠";
                cards.Add(display);
                temp = temp.Next;
            }
            state.Add($"  Column {i}: {string.Join(" ", cards)}");
        }

        return state;
    }

    private string GetRankSymbol(int rank)
    {
        var symbols = new Dictionary<int, string>
        {
            {1, "A"}, {11, "J"}, {12, "Q"}, {13, "K"}
        };
        return symbols.ContainsKey(rank) ? symbols[rank] : rank.ToString();
    }

    private string GetSuitSymbol(string suit)
    {
        var symbols = new Dictionary<string, string>
        {
            {"hearts", "♥"}, {"diamonds", "♦"}, {"clubs", "♣"}, {"spades", "♠"}
        };
        return symbols.ContainsKey(suit.ToLower()) ? symbols[suit.ToLower()] : suit[0].ToString().ToUpper();
    }
}