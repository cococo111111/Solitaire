/* This class handles board logic. Namely, moving cards between the various piles and queries about the state of the
 board. Its interface has been segragated into queries and actions, as there are classes which need to examine
 the state of the board but not alter its state, so they should not have access to the full set of operations.*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

public sealed class Board : IBoardActions, IBoardQuery
{
    readonly IHand hand;
    readonly IDrawPile drawPile;
    readonly IDiscardPile discardPile;
    readonly IActiveCards activeCards;

    public Board(IActiveCards active, IDrawPile draw, IDiscardPile discard, IHand hand)
    {
        if (hand == null) throw new ArgumentNullException("hand");
        if (draw == null) throw new ArgumentNullException("draw");
        if (active == null) throw new ArgumentNullException("active");
        if (discard == null) throw new ArgumentNullException("discard");

        this.hand = hand;
        this.drawPile = draw;
        this.activeCards = active;
        this.discardPile = discard;
    }

    public bool IsActivePileClear()
    {
        return activeCards.IsEmpty();
    }

    /// <summary>
    /// Returns the card in the hand. Does not remove from hand.
    /// </summary>
    public Card GetCurrentHand()
    {
        Assert.IsNotNull(hand.Current);
        return hand.Current;
    }

    /// <summary>
    /// Returns the card on top of the draw pile. Does not remove from draw pile.
    /// </summary>
    public Card GetNextDrawCard()
    {
        if (drawPile.IsEmpty())
            throw new InvalidOperationException("No cards in draw pile.");

        return drawPile.Top;
    }

    public bool IsContainedIn(Card card, CardPile pile)
    {
        Assert.IsNotNull(card);
        return GetPile(pile).Contains(card);
    }

    /// <summary>
    /// Lay out the board, using the provided cards.
    /// </summary>
    /// <param name="cards">A full set of cards.</param>
    /// <returns>An iterator permitting the asynchronous layout of cards.</returns>
    public IEnumerator LayoutBoard(IEnumerable<Card> cards, Mover mover)
    {
        if (cards == null)
            throw new ArgumentNullException("cards");

        if (mover == null)
            throw new ArgumentNullException("mover");

        Assert.AreEqual(cards.Count(), GameRules.NUM_CARDS);

        Reset();
        drawPile.Populate(cards, mover);
        TransferCard(drawPile.Top, mover, CardPile.Draw, CardPile.Hand);
        IEnumerable<Card> cardsToLayOut = drawPile.Take(GameRules.NUM_INITIAL_LAYOUT_CARDS);
        yield return activeCards.LayOutCards(cardsToLayOut, mover);
    }

    /// <summary>
    /// Transfer the given card from source pile to target. Cannot transfer from a pile to itself. Card must
    /// belong to source, and not to target.
    /// </summary>
    /// <param name="mover">Specificies the movement logic used for this transfer.</param>
    /// <param name="source">Pile from which the card is being taken.</param>
    /// <param name="target">Pile the card is entering.</param>
    public void TransferCard(Card card, Mover mover, CardPile source, CardPile target)
    {
        if (card == null)
            throw new ArgumentNullException("card");

        if (mover == null)
            throw new ArgumentNullException("mover");

        ICardPile src = GetPile(source);
        ICardPile tar = GetPile(target);

        if (!src.Contains(card))
            throw new ArgumentException("This card does not belong to the source pile.", "card");

        if (tar.Contains(card))
            throw new ArgumentException("This card already belongs to the target pile.", "card");

        src.Remove(card);
        tar.Add(card, mover);
    }

    ICardPile GetPile(CardPile pile)
    {
        switch (pile)
        {
            case CardPile.Active:
                return activeCards;
            case CardPile.Draw:
                return drawPile;
            case CardPile.Hand:
                return hand;
            case CardPile.Discard:
                return discardPile;
            default:
                throw new ArgumentException("Unidentified CardPile");
        }
    }

    void Reset()
    {
        hand.Reset();
        drawPile.Reset();
        activeCards.Reset();
        discardPile.Reset();
    }
}
