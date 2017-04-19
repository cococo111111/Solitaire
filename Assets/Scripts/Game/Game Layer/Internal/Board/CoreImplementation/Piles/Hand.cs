using System;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Hand : IHand
{
    public Card Current { get; private set; }

    readonly Vector3 position;

    const float cameraDepth = 1f;

    public Hand(IHandLayout layout)
    {
        if (layout == null) throw new ArgumentNullException("layout");

        position = new Vector3(layout.GetPosition().x, layout.GetPosition().y, cameraDepth);
    }

    public Card Remove()
    {
        if (Current == null)
            throw new System.InvalidOperationException("Hand is empty.");

        Card card = Current;
        Current = null;
        return card;
    }

    public void Add(Card card, Mover mover)
    {
        Assert.IsNull(Current);
        Assert.IsNotNull(card);

        Current = card;
        card.SetFaceUp();
        card.MoveTo(position, mover);
    }

    public void Reset()
    {
        Current = null;
    }

    public bool Contains(Card card)
    {
        return Current == card;
    }

    void ICardPile.Remove(Card card)
    {
        Assert.AreEqual(Current, card);
        Remove();
    }
}
