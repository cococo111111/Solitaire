/// <summary>
/// Events triggered by the UI layer which must be handled by a game manager.
/// </summary>
public interface IGame
{
    /// <summary>
    /// Start a completely new game, resetting all progress.
    /// </summary>
    void NewGame();

    /// <summary>
    /// Draw a new board, retaining progress (e.g. score).
    /// </summary>
    void Draw();

    /// <summary>
    /// Handles the clicking of the given card.
    /// </summary>
    /// <param name="card">Must not be null. </param>
    void ClickCard(Card card);
}
