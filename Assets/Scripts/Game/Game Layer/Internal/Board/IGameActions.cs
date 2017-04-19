using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The core game events.
/// </summary>
public interface IGameActions
{
    /// <summary>
    /// Perform a run on this card, which should be a valid, face-up card in the active pile.
    /// </summary>
    /// <param name="card">Non-null, face-up card belonging to the active pile.</param>
    void ExecuteRun(Card card);

    /// <summary>
    /// Draw a card from the draw pile, placing it into the hand. Should not be called if the draw pile is empty.
    /// </summary>
    void DrawCard();

    /// <summary>
    /// Start a new game, using the provided cards to build the appropriate piles.
    /// </summary>
    /// <param name="cards">Full set of cards.</param>
    /// <returns>An iterator that allows the cards to be laid out asynchronously. </returns>
    IEnumerator StartNew(IEnumerable<Card> cards);
}
