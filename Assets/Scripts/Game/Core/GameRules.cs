/* Global configuration/rules for the game as a whole. Certain values (like the fact that the deck holds 52 cards)
 are used throughout the game. Global classes like this are normally (strongly) discouraged, especially in SOLID, 
 but is appropriate here as it is stateless, simple, does not have dependencies, and will not need to be stubbed in 
 testing. The alternative would be to inject this class everywhere it's needed, but given the above considerations
 and the fact that there will only be one set of game rules, that would increase complexity with no benefits.*/

using UnityEngine;

public static class GameRules
{
    /// <summary>
    /// Tag given to all cards.
    /// </summary>
    public const string CARD_TAG = "Card";

    /// <summary>
    /// Number of cards in a full deck.
    /// </summary>
    public const int NUM_CARDS = 52;

    /// <summary>
    /// The number of cards laid out initially into the active pile.
    /// </summary>
    public const int NUM_INITIAL_LAYOUT_CARDS = 28;

    /// <summary>
    /// How many times a player can draw a new set of cards before game over.
    /// </summary>
    public const int NUM_STARTING_DRAWS = 2;

    /// <summary>
    /// Do the given card faces correspond to a valid run? A valid run occurs when the difference in value between
    /// the two cards is equal to 1. Note that the Jack, Queen, King are valued 11, 12, 13 and the Ace can be either
    /// 1 or 14, so that it forms a run with both the 2 and the King. 
    /// </summary>
    public static bool IsValidRun(CardFace cardA, CardFace cardB)
    {
        int absDifference = Mathf.Abs(GetValue(cardA) - GetValue(cardB));
        return 1 == absDifference || 12 == absDifference;
    }	

    /// <summary>
    /// Determine's card's value in the context of forming a run.
    /// </summary>
    static int GetValue(CardFace face)
    {
        return (int)face.Value;
    }
}
