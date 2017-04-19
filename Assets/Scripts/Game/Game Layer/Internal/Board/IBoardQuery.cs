/// <summary>
/// Queries regarding the state of the board, which do not affect the board's state.
/// </summary>
public interface IBoardQuery
{
    bool IsContainedIn(Card card, CardPile pile);
    bool IsActivePileClear();
    Card GetCurrentHand();
    Card GetNextDrawCard();
}
