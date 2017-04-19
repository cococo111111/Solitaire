/// <summary>
/// Interface to the game layer responsible for handling undo behaviour.
/// </summary>
public interface IUndoHandler
{
    /// <summary>
    /// Undo the last action.
    /// </summary>
    void Undo();

    /// <summary>
    /// Undo the entire board thus far, acting as a reset for the current board.
    /// </summary>
    void UndoAll();
}
