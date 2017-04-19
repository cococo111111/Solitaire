using System.Collections;
using System.Collections.Generic;

public interface IActiveCards : ICardPile
{
    bool IsEmpty();
    IEnumerator LayOutCards(IEnumerable<Card> cardsToPlace, Mover mover);
}