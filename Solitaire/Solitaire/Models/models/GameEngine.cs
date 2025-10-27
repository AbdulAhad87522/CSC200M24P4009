namespace Solitaire.Models.models;
using System;
using System.Collections.Generic;
using Solitaire.Models.datastructures;

public class GameEngine
{
    public Deck Deck { get; private set; }
    public Dictionary<string, CustomStack<Card>> Foundations { get; private set; }
    public List<CustomLinkedLis<Card>> Tableau { get; private set; }
    public CustomQueue<Card> Stock { get; private set; }
    public CustomStack<Card> Waste { get; private set; }

    public GameEngine()
    {
        Deck = new Deck();
        Foundations = new Dictionary<string, CustomStack<Card>>();
        Tableau = new List<CustomLinkedLis<Card>>();
        Stock = new CustomQueue<Card>();
        Waste = new CustomStack<Card>();

        InitializeGame();
    }

    public void InitializeGame()
    {
        // Shuffle the deck
        Deck.Shuffle();

        // Initialize foundations (4 piles - one for each suit)
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        foreach (string suit in suits)
        {
            Foundations[suit] = new CustomStack<Card>();
        }

        // Initialize tableau (7 piles)
        Tableau = new List<CustomLinkedLis<Card>>();
        for (int i = 0; i < 7; i++)
        {
            Tableau.Add(new CustomLinkedLis<Card>());
        }

        // Deal cards to tableau
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Card card = Deck.DrawCard();
                if (card != null)
                {
                    if (j == i) // Last card in each pile is face up
                    {
                        card.IsFaceUp = true;
                    }
                    Tableau[i].PushBack(card);
                }
            }
        }

        // Remaining cards go to stock
        while (Deck.GetSize() > 0)
        {
            Card card = Deck.DrawCard();
            if (card != null)
            {
                Stock.Enqueue(card);
            }
        }
    }

    public bool MoveCardToFoundation(Card card, string suit)
    {
        var foundation = Foundations[suit];

        if (foundation.IsEmpty())
        {
            // First card must be Ace
            if (card.Rank == 1)
            {
                foundation.Push(card);
                return true;
            }
            return false;
        }
        else
        {
            var topCard = foundation.Peek();
            // Card must be same suit and one rank higher
            if (card.Suit == suit && card.Rank == topCard.Rank + 1)
            {
                foundation.Push(card);
                return true;
            }
            return false;
        }
    }

    public bool MoveWithinTableau(int fromColumn, int toColumn, int cardIndex)
    {
        if (fromColumn < 0 || fromColumn >= Tableau.Count ||
            toColumn < 0 || toColumn >= Tableau.Count)
            return false;

        var sourceColumn = Tableau[fromColumn];
        var targetColumn = Tableau[toColumn];

        // Find the card to move
        var node = sourceColumn.head;
        for (int i = 0; i < cardIndex && node != null; i++)
        {
            node = node.Next;
        }

        if (node == null || !node.Data.IsFaceUp)
            return false;

        var cardToMove = node.Data;

        // Check if move is valid
        if (targetColumn.IsEmpty())
        {
            // Can only place King on empty column
            if (cardToMove.Rank == 13)
            {
                MoveCards(sourceColumn, targetColumn, cardIndex);
                return true;
            }
        }
        else
        {
            var targetTopCard = targetColumn.tail.Data;
            // Cards must be opposite colors and descending rank
            if (IsOppositeColor(cardToMove, targetTopCard) &&
                cardToMove.Rank == targetTopCard.Rank - 1)
            {
                MoveCards(sourceColumn, targetColumn, cardIndex);
                return true;
            }
        }

        return false;
    }

    private void MoveCards(CustomLinkedLis<Card> from, CustomLinkedLis<Card> to, int startIndex)
    {
        // Convert to list, manipulate, then convert back
        var cardsList = new List<Card>();
        var node = from.head;

        // Build list of all cards
        while (node != null)
        {
            cardsList.Add(node.Data);
            node = node.Next;
        }

        if (startIndex >= cardsList.Count)
            return;

        // Clear the from list
        from.head = null;
        from.tail = null;
        from.length = 0;

        // Add cards before startIndex back to from list
        for (int i = 0; i < startIndex; i++)
        {
            from.PushBack(cardsList[i]);
        }

        // Add cards from startIndex to to list
        for (int i = startIndex; i < cardsList.Count; i++)
        {
            to.PushBack(cardsList[i]);
        }

        // Reveal the new top card of source column if exists
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
            return card.Rank == 1; // Only Ace can start foundation
        }
        else
        {
            var topCard = foundation.Peek();
            return card.Suit == suit && card.Rank == topCard.Rank + 1;
        }
    }

    public bool MoveToFoundation(string foundationSuit)
    {
        // Try to move from waste first
        if (!Waste.IsEmpty())
        {
            var wasteCard = Waste.Peek();
            if (CanMoveToFoundation(wasteCard, foundationSuit))
            {
                var card = Waste.Pop();
                Foundations[foundationSuit].Push(card);
                return true;
            }
        }

        // Try to move from tableau
        for (int i = 0; i < Tableau.Count; i++)
        {
            var column = Tableau[i];
            if (!column.IsEmpty() && column.tail != null)
            {
                var topCard = column.tail.Data;
                if (CanMoveToFoundation(topCard, foundationSuit))
                {
                    var card = column.PopBack();
                    Foundations[foundationSuit].Push(card);

                    // Reveal next card if exists
                    if (!column.IsEmpty() && column.tail != null)
                    {
                        column.tail.Data.IsFaceUp = true;
                    }
                    return true;
                }
            }
        }

        return false;
    }

    public bool CanMoveTableauCard(int fromColumn, int cardIndex)
    {
        if (fromColumn < 0 || fromColumn >= Tableau.Count)
            return false;

        var column = Tableau[fromColumn];
        if (column.IsEmpty() || cardIndex < 0 || cardIndex >= column.Length)
            return false;

        // Find the card at the specified index
        var node = column.head;
        for (int i = 0; i < cardIndex && node != null; i++)
        {
            node = node.Next;
        }

        return node != null && node.Data.IsFaceUp;
    }

    public bool MoveTableauCards(int fromColumn, int toColumn, int cardIndex)
    {
        if (!CanMoveTableauCard(fromColumn, cardIndex))
            return false;

        var source = Tableau[fromColumn];
        var target = Tableau[toColumn];

        // Convert to list for manipulation
        var cards = new List<Card>();
        var node = source.head;

        // Build list and find start node
        while (node != null)
        {
            cards.Add(node.Data);
            node = node.Next;
        }

        // Clear source column
        source.head = null;
        source.tail = null;
        source.length = 0;

        // Add cards before the moved index back to source
        for (int i = 0; i < cardIndex; i++)
        {
            source.PushBack(cards[i]);
        }

        // Add moved cards to target
        for (int i = cardIndex; i < cards.Count; i++)
        {
            target.PushBack(cards[i]);
        }

        // Reveal new top card of source if exists
        if (!source.IsEmpty() && source.tail != null)
        {
            source.tail.Data.IsFaceUp = true;
        }

        return true;
    }

    public bool CanMoveTableauToTableau(int fromColumn, int toColumn, int cardIndex)
    {
        if (fromColumn == toColumn) return false;
        if (!CanMoveTableauCard(fromColumn, cardIndex)) return false;

        var source = Tableau[fromColumn];
        var target = Tableau[toColumn];

        // Get the card we want to move
        var node = source.head;
        for (int i = 0; i < cardIndex && node != null; i++)
        {
            node = node.Next;
        }
        if (node == null) return false;

        var movingCard = node.Data;

        // Check if move is valid
        if (target.IsEmpty())
        {
            // Can only place King on empty column
            return movingCard.Rank == 13;
        }
        else
        {
            var targetTopCard = target.tail.Data;
            // Cards must be opposite colors and descending rank
            return IsOppositeColor(movingCard, targetTopCard) &&
                   movingCard.Rank == targetTopCard.Rank - 1;
        }
    }

    //private bool IsOppositeColor(Card card1, Card card2)
    //{
    //    var redSuits = new[] { "hearts", "diamonds" };
    //    bool card1Red = redSuits.Contains(card1.Suit);
    //    bool card2Red = redSuits.Contains(card2.Suit);
    //    return card1Red != card2Red;
    //}

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
        return Tableau[column].Length;
    }

    public bool IsOppositeColor(Card card1, Card card2)
    {
        var redSuits = new[] { "hearts", "diamonds" };
        var blackSuits = new[] { "clubs", "spades" };

        bool card1IsRed = redSuits.Contains(card1.Suit);
        bool card2IsRed = redSuits.Contains(card2.Suit);

        return card1IsRed != card2IsRed;
    }

    public bool IsGameWon()
    {
        // Game is won when all foundations have King on top
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

    public void DrawFromStock()
    {
        if (Stock.IsEmpty())
        {
            // Recycle waste back to stock
            while (!Waste.IsEmpty())
            {
                Card card = Waste.Pop();
                card.IsFaceUp = false;
                Stock.Enqueue(card);
            }
            return;
        }

        Card drawnCard = Stock.Dequeue();
        if (drawnCard != null)
        {
            drawnCard.IsFaceUp = true;
            Waste.Push(drawnCard);
        }
    }
}