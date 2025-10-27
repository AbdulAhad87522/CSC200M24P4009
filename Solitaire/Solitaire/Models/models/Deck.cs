namespace Solitaire.Models.models;
using System;
using System.Collections.Generic;
using Solitaire.Models.datastructures;

public class Deck
{
    private CustomQueue<Card> cards;

    public Deck()
    {
        cards = new CustomQueue<Card>();
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        string[] suits = { "hearts", "diamonds", "spades", "clubs" };
        int[] ranks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        foreach (string suit in suits)
        {
            foreach (int rank in ranks)
            {
                cards.Enqueue(new Card(suit, rank, false));
            }
        }
    }

    public void Shuffle()
    {
        List<Card> cardsArray = new List<Card>();

        // Empty the queue into a list
        while (!cards.IsEmpty())
        {
            cardsArray.Add(cards.Dequeue());
        }

        // Fisher-Yates shuffle
        Random rng = new Random();
        for (int i = cardsArray.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            // Swap cards
            Card temp = cardsArray[i];
            cardsArray[i] = cardsArray[j];
            cardsArray[j] = temp;
        }

        // Put cards back into queue
        foreach (Card card in cardsArray)
        {
            cards.Enqueue(card);
        }
    }

    public Card DrawCard()
    {
        return cards.IsEmpty() ? null : cards.Dequeue();
    }

    public int GetSize()
    {
        return cards.Size();
    }
}