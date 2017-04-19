using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class DrawPile : IDrawPile
{
    readonly Stack<Card> pile = new Stack<Card>(GameRules.NUM_CARDS);
    readonly IDrawPileLayout layout;

    // Need to ensure the top of the pile has depth of at least 1.
    const int maxDepth = GameRules.NUM_CARDS + 1;

    public Card Top
    {
        get
        {
            if (pile.Count == 0) throw new InvalidOperationException();
            return pile.Peek();
        }
    }
    
    public DrawPile(IDrawPileLayout layout)
    {
        if (layout == null) throw new ArgumentNullException("layout");
        this.layout = layout;
    }

    public void Populate(IEnumerable<Card> cards, Mover mover)
    {
        foreach (Card card in cards)
        {
            card.SetFaceDown();
            card.MoveTo(GetNextPosition(), mover);
            pile.Push(card);
        }
    }

    public IEnumerable<Card> Take(int numCards)
    {
        if (pile.Count < numCards)
            throw new ArgumentException(string.Format("Draw pile has {1} cards, tried to take {0}.", numCards, pile.Count));

        for (int i = 0; i < numCards; i++)
        {
            yield return pile.Pop();
        }
    }

    public bool Contains(Card card)
    {
        return pile.Contains(card);
    }

    public bool IsEmpty()
    {
        return pile.Count == 0;
    }

    public void Add(Card card, Mover mover)
    {
        Assert.IsNotNull(card);
        card.SetFaceDown();
        card.MoveTo(GetNextPosition(), mover);
        pile.Push(card);
    }

    public Card DrawCard()
    {
        if (pile.Count == 0)
            throw new InvalidOperationException("Deck is empty!");

        return pile.Pop();
    }

    public void Reset()
    {
        pile.Clear();
    }

    void ICardPile.Remove(Card card)
    {
        Assert.AreEqual(Top, card);
        DrawCard();
    }

    Vector3 GetNextPosition()
    {
        Vector2 rootPosition = layout.GetPosition() + pile.Count * layout.GetOffset();
        return new Vector3(rootPosition.x, rootPosition.y, GetDepth());
    }

    int GetDepth()
    {
        Assert.IsTrue(pile.Count < maxDepth);
        return maxDepth - pile.Count;
    }
}
