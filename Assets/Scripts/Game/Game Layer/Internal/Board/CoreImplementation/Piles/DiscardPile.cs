using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class DiscardPile : IDiscardPile
{
    readonly Vector2 start;
    readonly Vector2 offset;

    readonly Stack<Card> cards = new Stack<Card>(GameRules.NUM_CARDS);
    readonly int[] cardCounts = new int[(int)Value.King + 1];

    const float scaleFactor = 0.5f;

    public DiscardPile(IDiscardPileLayout layout)
    {
        if (layout == null) throw new ArgumentNullException("layout");
        start = layout.GetPosition();
        offset = layout.GetOffset();
    }
    
    public void Add(Card card, Mover mover)
    {
        if (card == null)
            throw new System.ArgumentNullException();

        ShrinkCard(card);
        cards.Push(card);
        Vector3 position = FindPositionFor(card);
        card.MoveTo(position, mover);
        IncrementCount(card);
    }

    public Card Remove()
    {
        Card removed = cards.Pop();
        RestoreCardSize(removed);
        DecrementCount(removed);
        return removed;
    }

    public void Reset()
    {
        foreach (Card card in cards)
        {
            RestoreCardSize(card);
        }
        cards.Clear();
        System.Array.Clear(cardCounts, 0, cardCounts.Length);
    }

    public bool Contains(Card card)
    {
        return cards.Contains(card);
    }

    void ICardPile.Remove(Card card)
    {
        Card removed = Remove();
        Assert.AreEqual(removed, card);
    }

    void ShrinkCard(Card card)
    {
        card.gameObject.transform.localScale *= scaleFactor;
    }

    void RestoreCardSize(Card card)
    {
        card.gameObject.transform.localScale /= scaleFactor;
    }

    Vector3 FindPositionFor(Card card)
    {
        CardFace face = card.Face;
        return new Vector3(
            start.x + (int)face.Value * offset.x, 
            start.y - GetCount(card) * offset.y, 
            ComputeDepth(card));
    }

    float ComputeDepth(Card card)
    {
        CardFace face = card.Face;
        int suit = (int)face.Suit;
        int value = (int)face.Value;
        float result = 52f - ((value - 1) * 4 + GetCount(card));
        Assert.IsTrue(result > 0);
        return result;
    }

    void IncrementCount(Card card)
    {
        cardCounts[GetCountIndex(card)]++;
    }

    void DecrementCount(Card card)
    {
        cardCounts[GetCountIndex(card)]--;
    }

    int GetCount(Card card)
    {
        return cardCounts[(int)card.Face.Value];
    }

    int GetCountIndex(Card card)
    {
        return (int)card.Face.Value;
    }
}
