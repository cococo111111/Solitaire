/* The active cards on the board are represented by a 2D array with sentinel values.
 At the beginning, when all cards are in play, the array will look like this (c is a card, - is a null):
 
 c - - c - - c - - - -
 c c - c c - c c - - -
 c c c c c c c c c - -
 c c c c c c c c c c -
 - - - - - - - - - - -

  We can update card visibility by iterating through the entire array, and for each card, check below and below to
 the right. If both those cards are null, then the card should be made visible. Gratuitous nulls make this logic 
 simple to implement. 
 
  Many deliberately inefficient routines are used in this script to favour readability and robustness over speed.
 This is because the array is small enough that even on mobile, these routines take microseconds at most.*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class ActiveCards : IActiveCards
{
    readonly IActiveCardsLayout layout;

    // Bottom row and right column are sentinel values
    readonly Card[,] activeCards = new Card[numCardSlotsPerLayer + 1, numLayers + 1];
    readonly Dictionary<Card, Vector3> cardPositions = new Dictionary<Card, Vector3>(GameRules.NUM_CARDS);

    // Tracks the indices of removed cards: most recent removal corresponds to top index.
    // This is needed to return a card to the appropriate spot in the activeCards array. 
    readonly Stack<CardIndex> removedCardIndices = new Stack<CardIndex>();

    readonly WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

    const int numLayers = 4;
    const int numCardSlotsPerLayer = 10;

    const int yFirstLayer = 1;
    const int ySecondLayer = 2;
    const int yThirdLayer = 3;
    const int yFourthLayer = 4;

    public ActiveCards(IActiveCardsLayout layout)
    {
        if (layout == null) throw new ArgumentNullException("layout");
        this.layout = layout;
    }

    public IEnumerator LayOutCards(IEnumerable<Card> cardsToPlace, Mover mover)
    {
        Reset();

        ExtractCards(cardsToPlace);
        ExtractPositionsFromLayout();
        yield return MoveCards(mover);
        UpdateCardOrientations();
    }

    public bool Contains(Card card)
    {
        Assert.IsNotNull(card);
        return EnumerateCards().Any(card.Equals);
    }

    public bool IsEmpty()
    {
        return EnumerateCards().Count() == 0;
    }

    public void Reset()
    {
        cardPositions.Clear();
        removedCardIndices.Clear();
        Array.Clear(activeCards, 0, activeCards.Length);
    }

    void ICardPile.Add(Card card, Mover mover)
    {
        Return(card);
        card.MoveTo(cardPositions[card], mover);
    }

    void ICardPile.Remove(Card card)
    {
        Remove(card);
    }

    void Return(Card card)
    {
        Assert.IsNotNull(card);
        if (!cardPositions.ContainsKey(card))
            throw new ArgumentException("Can only add card to active pile if it was removed from there.");

        if (removedCardIndices.Count == 0)
            throw new InvalidOperationException("No removals to undo.");

        CardIndex index = removedCardIndices.Pop();

        Assert.IsNull(activeCards[index.x, index.y]);

        SetCard(card, index.x, index.y);
    }

    void Remove(Card card)
    {
        Assert.IsNotNull(card);
        foreach (CardIndex index in GetCardIndices())
        {
            if (card == activeCards[index.x, index.y])
            {
                removedCardIndices.Push(index);
                SetCard(null, index.x, index.y);
                return;
            }
        }
        throw new InvalidOperationException("Card not found.");
    }

    void ExtractCards(IEnumerable<Card> cardsToPlace)
    {
        IEnumerator<Card> cards = cardsToPlace.GetEnumerator();
        foreach (CardIndex index in GetCardIndices())
        {
            activeCards[index.x, index.y] = TakeNext(cards);
        }
    }

    void ExtractPositionsFromLayout()
    {
        IEnumerator<Vector3> positions = layout.GetPositions().Reverse().GetEnumerator();
        foreach (CardIndex index in GetCardIndices())
        {
            Card card = activeCards[index.x, index.y];
            cardPositions[card] = TakeNext(positions);
        }
    }

    /// <summary>
    /// Moves all cards to the positions given by cardPositions.
    /// </summary>
    IEnumerator MoveCards(Mover mover)
    {
        foreach (Card card in EnumerateCards())
        {
            Vector3 position = cardPositions[card];
            card.MoveTo(position, mover);
            yield return fixedUpdate;
        }
    }

    /// <summary>
    /// Enumerates all the indices in the array corresponding to valid card positions. This includes positions
    /// that may be null during a game. Note that the order of the enumeration is right to left, top to bottom.
    /// This is the opposite order of the positions given by the layout, and ensures enumeration goes from deep
    /// to shallow.
    /// </summary>
    IEnumerable<CardIndex> GetCardIndices()
    {
        // The iteration logic in this method is ad hoc and rather dirty. See the diagram in the comment
        // at the top of this class to see the guiding idea behind where cards are supposed to end up.
        for (int x = 6; x >= 0; x -= 3)
        {
            yield return new CardIndex(x, yFourthLayer);
        }
        for (int x = 6; x >= 0; x -= 3)
        {
            yield return new CardIndex(x + 1, yThirdLayer);
            yield return new CardIndex(x, yThirdLayer);
        }
        for (int x = 8; x >= 0; x--)
        {
            yield return new CardIndex(x, ySecondLayer);
        }
        for (int x = 9; x >= 0; x--)
        {
            yield return new CardIndex(x, yFirstLayer);
        }
    }

    /// <summary>
    /// Get all the active (non-null) cards.
    /// </summary>
    IEnumerable<Card> EnumerateCards()
    {
        return GetCardIndices()
            .Select(index => activeCards[index.x, index.y])
            .Where(card => card != null);
    }

    /// <summary>
    /// Place the card in the corresponding spot in the array, and update orientations of cards appropriately.
    /// </summary>
    void SetCard(Card card, int x, int y)
    {
        activeCards[x, y] = card;
        UpdateCardOrientations();
    }

    void UpdateCardOrientations()
    {
        foreach (CardIndex index in GetCardIndices())
        {
            UpdateOrientation(index.x, index.y);
        }
    }

    void UpdateOrientation(int x, int y)
    {
        Card card = activeCards[x, y];

        if (card == null)
            return;
        
        if (activeCards[x, y - 1] == null && activeCards[x + 1, y - 1] == null)
        {
            card.SetFaceUp();
        }
        else
        {
            card.SetFaceDown();
        }
    }

    // In multiple places in this script, we need to explicitly use an enumerator to iterate. This method
    // wraps up the two method calls to do this, and also adds an assertion in case we've prematurely hit
    // the end of the enumerator. Otherwise this would create visual issues without throwing an exception.
    T TakeNext<T>(IEnumerator<T> enumerator)
    {
        bool nextExists = enumerator.MoveNext();
        Assert.IsTrue(nextExists);
        return enumerator.Current;
    }

    struct CardIndex
    {
        public readonly byte x, y;

        public CardIndex(int x, int y)
        {
            this.x = (byte)x;
            this.y = (byte)y;
        }
    }
}
