namespace Solitaire.Models.models;
using System;
using System.Collections.Generic;
using Solitaire.Models.datastructures;

public class GameEngine
{
    public Deck Deck { get; private set; }
    public Dictionary<string, CustomStack<Card>> Foundations { get; private set; }
    public List<CustomLinkedList<Card>> Tableau { get; private set; }
    public CustomQueue<Card> Stock { get; private set; }
    public CustomStack<Card> Waste { get; private set; }

    public GameEngine()
    {
        Deck = new Deck();
        Foundations = new Dictionary<string, CustomStack<Card>>();
        Tableau = new List<CustomLinkedList<Card>>();
        Stock = new CustomQueue<Card>();
        Waste = new CustomStack<Card>();

        InitializeGame();
    }

    public void InitializeGame()
    {
        Deck.Shuffle();

        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        foreach (string suit in suits)
        {
            Foundations[suit] = new CustomStack<Card>();
        }

        Tableau = new List<CustomLinkedList<Card>>();
        for (int i = 0; i < 7; i++)
        {
            Tableau.Add(new CustomLinkedList<Card>());
        }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Card card = Deck.DrawCard();
                if (card != null)
                {
                    if (j == i)
                    {
                        card.IsFaceUp = true;
                    }
                    Tableau[i].PushBack(card);
                }
            }
        }

        while (Deck.GetSize() > 0)
        {
            Card card = Deck.DrawCard();
            if (card != null)
            {
                Stock.Enqueue(card);
            }
        }
    }

    // ADDED: Move waste card to foundation
    public bool MoveWasteToFoundation(string suit)
    {
        if (Waste.IsEmpty()) return false;

        var wasteCard = Waste.Peek();
        var foundation = Foundations[suit];

        if (IsValidFoundationMove(wasteCard, foundation))
        {
            var card = Waste.Pop();
            foundation.Push(card);
            return true;
        }

        return false;
    }

    // ADDED: Move waste card to tableau
    public bool MoveWasteToTableau(int toColumn)
    {
        if (Waste.IsEmpty()) return false;
        if (toColumn < 0 || toColumn >= Tableau.Count) return false;

        var wasteCard = Waste.Peek();
        var targetColumn = Tableau[toColumn];

        // Check if move is valid
        if (targetColumn.IsEmpty())
        {
            // Empty column - only Kings can be placed
            if (wasteCard.Rank == 13)
            {
                var card = Waste.Pop();
                targetColumn.PushBack(card);
                return true;
            }
        }
        else
        {
            var targetTopCard = targetColumn.tail.Data;
            // Must be opposite color and descending rank
            if (IsOppositeColor(wasteCard, targetTopCard) &&
                wasteCard.Rank == targetTopCard.Rank - 1)
            {
                var card = Waste.Pop();
                targetColumn.PushBack(card);
                return true;
            }
        }

        return false;
    }

    // UPDATED: Move tableau card to foundation with proper index handling
    public bool MoveTableauToFoundation(int fromColumn, int cardIndex, string suit)
    {
        if (fromColumn < 0 || fromColumn >= Tableau.Count) return false;

        var column = Tableau[fromColumn];
        if (column.IsEmpty()) return false;

        // Check if cardIndex is the last card in the column
        if (cardIndex != column.Length - 1)
        {
            // Can only move the top card to foundation
            return false;
        }

        var card = column.tail.Data;
        if (!card.IsFaceUp) return false;

        var foundation = Foundations[suit];

        if (IsValidFoundationMove(card, foundation))
        {
            var removedCard = column.PopBack();
            foundation.Push(removedCard);

            // Flip the next top card if exists
            FlipTopTableauCard(column);
            return true;
        }

        return false;
    }

    // ADDED: Helper method to check valid foundation move
    private bool IsValidFoundationMove(Card card, CustomStack<Card> foundation)
    {
        if (foundation.IsEmpty())
        {
            return card.Rank == 1; // Only Ace can start foundation
        }
        else
        {
            var topFoundationCard = foundation.Peek();
            return card.Suit == topFoundationCard.Suit &&
                   card.Rank == topFoundationCard.Rank + 1;
        }
    }

    // ADDED: Helper method to flip top card of tableau column
    private void FlipTopTableauCard(CustomLinkedList<Card> column)
    {
        if (!column.IsEmpty() && column.tail != null)
        {
            column.tail.Data.IsFaceUp = true;
        }
    }

    // EXISTING: Move cards within tableau (drag multiple cards)
    public bool MoveWithinTableau(int fromColumn, int toColumn, int cardIndex)
    {
        Console.WriteLine($"🎯 MoveWithinTableau called: from={fromColumn}, to={toColumn}, index={cardIndex}");

        if (fromColumn < 0 || fromColumn >= Tableau.Count ||
            toColumn < 0 || toColumn >= Tableau.Count)
        {
            Console.WriteLine($"❌ Invalid column indices");
            return false;
        }

        if (fromColumn == toColumn)
        {
            Console.WriteLine($"❌ Cannot move to same column");
            return false;
        }

        var sourceColumn = Tableau[fromColumn];
        var targetColumn = Tableau[toColumn];

        Console.WriteLine($"   Source column length: {sourceColumn.Length}");
        Console.WriteLine($"   Target column length: {targetColumn.Length}");

        var node = sourceColumn.head;
        for (int i = 0; i < cardIndex && node != null; i++)
        {
            node = node.Next;
        }

        if (node == null)
        {
            Console.WriteLine($"❌ Card not found at index {cardIndex}");
            return false;
        }

        if (!node.Data.IsFaceUp)
        {
            Console.WriteLine($"❌ Card is face down");
            return false;
        }

        var cardToMove = node.Data;
        Console.WriteLine($"   Moving card: {cardToMove.Rank} of {cardToMove.Suit} ({cardToMove.Color})");

        if (targetColumn.IsEmpty())
        {
            Console.WriteLine($"   Target column is empty");
            if (cardToMove.Rank == 13)
            {
                Console.WriteLine($"✅ King can move to empty column");
                MoveCards(sourceColumn, targetColumn, cardIndex);
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Only Kings can move to empty columns");
                return false;
            }
        }
        else
        {
            var targetTopCard = targetColumn.tail.Data;
            Console.WriteLine($"   Target top card: {targetTopCard.Rank} of {targetTopCard.Suit} ({targetTopCard.Color})");

            bool oppositeColor = IsOppositeColor(cardToMove, targetTopCard);
            bool descendingRank = cardToMove.Rank == targetTopCard.Rank - 1;

            Console.WriteLine($"   Opposite colors? {oppositeColor}");
            Console.WriteLine($"   Descending rank? {descendingRank} ({cardToMove.Rank} == {targetTopCard.Rank} - 1)");

            if (oppositeColor && descendingRank)
            {
                Console.WriteLine($"✅ Valid move!");
                MoveCards(sourceColumn, targetColumn, cardIndex);
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Invalid move - colors or ranks don't match");
                return false;
            }
        }
    }

    // UPDATED: Move cards helper with null checks
    private void MoveCards(CustomLinkedList<Card> from, CustomLinkedList<Card> to, int startIndex)
    {
        var cardsList = new List<Card>();
        var node = from.head;

        while (node != null)
        {
            cardsList.Add(node.Data);
            node = node.Next;
        }

        if (startIndex >= cardsList.Count || startIndex < 0)
            return;

        from.head = null;
        from.tail = null;
        from.length = 0;

        for (int i = 0; i < startIndex; i++)
        {
            from.PushBack(cardsList[i]);
        }

        for (int i = startIndex; i < cardsList.Count; i++)
        {
            to.PushBack(cardsList[i]);
        }

        if (!from.IsEmpty() && from.tail != null)
        {
            from.tail.Data.IsFaceUp = true;
        }
    }

    public bool CanMoveToFoundation(Card card, string suit)
    {
        var foundation = Foundations[suit];

        if (foundation.IsEmpty())
        {
            return card.Rank == 1;
        }
        else
        {
            var topCard = foundation.Peek();
            return card.Suit == suit && card.Rank == topCard.Rank + 1;
        }
    }

    public bool CanMoveTableauCard(int fromColumn, int cardIndex)
    {
        if (fromColumn < 0 || fromColumn >= Tableau.Count)
            return false;

        var column = Tableau[fromColumn];
        if (column.IsEmpty() || cardIndex < 0 || cardIndex >= column.Length)
            return false;

        var node = column.head;
        for (int i = 0; i < cardIndex && node != null; i++)
        {
            node = node.Next;
        }

        return node != null && node.Data.IsFaceUp;
    }

    public bool CanMoveTableauToTableau(int fromColumn, int toColumn, int cardIndex)
    {
        if (fromColumn == toColumn) return false;
        if (!CanMoveTableauCard(fromColumn, cardIndex)) return false;

        var source = Tableau[fromColumn];
        var target = Tableau[toColumn];

        var node = source.head;
        for (int i = 0; i < cardIndex && node != null; i++)
        {
            node = node.Next;
        }
        if (node == null) return false;

        var movingCard = node.Data;

        if (target.IsEmpty())
        {
            return movingCard.Rank == 13; // Only King on empty
        }
        else
        {
            var targetTopCard = target.tail.Data;
            return IsOppositeColor(movingCard, targetTopCard) &&
                   movingCard.Rank == targetTopCard.Rank - 1;
        }
    }

    // UPDATED: Using Card.Color property
    public bool IsOppositeColor(Card card1, Card card2)
    {
        return card1.Color != card2.Color;
    }

    public Card GetTableauCard(int column, int index)
    {
        if (column < 0 || column >= Tableau.Count) return null;

        var col = Tableau[column];
        var node = col.head;
        for (int i = 0; i < index && node != null; i++)
        {
            node = node.Next;
        }
        return node?.Data;
    }

    public int GetTableauCardCount(int column)
    {
        if (column < 0 || column >= Tableau.Count) return 0; // ADDED: Null check
        return Tableau[column].Length;
    }

    public bool IsGameWon()
    {
        foreach (var foundation in Foundations.Values)
        {
            if (foundation.IsEmpty() || foundation.Peek().Rank != 13)
                return false;
        }
        return true;
    }

    public Card GetTopTableauCard(int column)
    {
        if (column < 0 || column >= Tableau.Count || Tableau[column].tail == null)
            return null;
        return Tableau[column].tail.Data;
    }

    // UPDATED: DrawFromStock with proper null checks
    public void DrawFromStock()
    {
        if (Stock.IsEmpty())
        {
            // Recycle waste back to stock
            while (!Waste.IsEmpty())
            {
                var wasteCard = Waste.Pop();
                if (wasteCard != null)
                {
                    wasteCard.IsFaceUp = false;
                    Stock.Enqueue(wasteCard);
                }
            }
            return;
        }

        var stockCard = Stock.Dequeue();
        if (stockCard != null)
        {
            stockCard.IsFaceUp = true;
            Waste.Push(stockCard);
        }
    }

    // ADDED: Alias method for controller compatibility
    public bool MoveTableauCards(int fromColumn, int toColumn, int cardIndex)
    {
        return MoveWithinTableau(fromColumn, toColumn, cardIndex);
    }
}