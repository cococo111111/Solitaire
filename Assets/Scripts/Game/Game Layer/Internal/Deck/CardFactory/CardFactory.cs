using System;
using UnityEngine;

public sealed class CardFactory : ICardFactory
{
    readonly Transform anchor;

    const float cardScale = 20f;

    /// <param name="anchor">GameObject to hold all created cards as their parent in the hierarchy.</param>
    public CardFactory(GameObject anchor)
    {
        if (anchor == null) throw new ArgumentNullException("anchor");

        this.anchor = anchor.transform;
    }

    public Card CreateCard(CardFace face, GameObject prefab)
    {
        Card card = Card.CreateNewCard<Card>(face, prefab);
        card.transform.parent = anchor;
        card.transform.localScale *= cardScale;
        return card;
    }
}
