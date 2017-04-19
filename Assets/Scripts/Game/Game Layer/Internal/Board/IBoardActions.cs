using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Actions that move cards around on the board.
/// </summary>
public interface IBoardActions
{
    void TransferCard(Card card, Mover mover, CardPile source, CardPile target);

    /// <summary>
    /// Lay out a board for a new game using the provided cards.
    /// </summary>
    /// <returns> An iterator that yields each time a card is drawn from the deck.</returns>
    IEnumerator LayoutBoard(IEnumerable<Card> cards, Mover mover);
}
