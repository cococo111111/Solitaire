using System.Collections.Generic;

public interface IDrawPile : ICardPile
{
    Card Top { get; }

    Card DrawCard();
    bool IsEmpty();
    void Populate(IEnumerable<Card> cards, Mover mover);
    IEnumerable<Card> Take(int numCards);
}