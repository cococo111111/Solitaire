/* This decorates both the IUndo and IGameActions interfaces with scoring functionality. Game events result
 in changes to the score, which are then sent to an IScore object to be represented (e.g. on the screen). 
 By implementing scoring using this decoration logic, clients of the undo and game action interfaces are 
 oblivious to the existence of a scoring system.*/

using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ScoreBoard: IUndoActions, IGameActions, IScoreQuery
{
    public int Score { get { return score; } }

    readonly IUndoActions undoBoard;
    readonly IGameActions boardActions;

    readonly Stack<int> runHistory = new Stack<int>(GameRules.NUM_CARDS);

    int score = 0;
    int currentRun = 0;

    const int scorePerRun = 100;

    public ScoreBoard(IGameActions actions, IUndoActions undo)
    {
        if (undo == null) throw new ArgumentNullException("undo");
        if (actions == null) throw new ArgumentNullException("actions");

        this.boardActions = actions;
        this.undoBoard = undo;
    }

    public void DrawCard()
    {
        ResetRun();
        boardActions.DrawCard();
    }

    public void ExecuteRun(Card card)
    {
        IncreaseRun();
        boardActions.ExecuteRun(card);
    }

    public IEnumerator StartNew(IEnumerable<Card> cards)
    {
        Reset();
        return boardActions.StartNew(cards);
    }

    public bool TryUndo()
    {
        bool undoSucceeded = undoBoard.TryUndo();
        if (undoSucceeded)
        {
            UndoLastScoreEvent();
        }
        return undoSucceeded;
    }

    void UndoLastScoreEvent()
    {
        score -= ComputeScore();
        currentRun = runHistory.Pop();
    }

    void Reset()
    {
        // Clear data related to runs and previous scores, but not the current score.
        currentRun = 0;
        runHistory.Clear();
    }

    void IncreaseRun()
    {
        runHistory.Push(currentRun);
        currentRun++;
        score += ComputeScore();
    }

    void ResetRun()
    {
        runHistory.Push(currentRun);
        currentRun = 0;
    }

    int ComputeScore()
    {
        return currentRun * scorePerRun;
    }
}
