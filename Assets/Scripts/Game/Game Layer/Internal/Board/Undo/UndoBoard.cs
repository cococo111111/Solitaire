using System;
using System.Collections;
using System.Collections.Generic;

public sealed class UndoBoard: IGameActions, IUndoActions
{
    readonly IBoardQuery boardQuery;
    readonly IBoardActions boardActions;

    readonly Stack<Action> undoEvents = new Stack<Action>(GameRules.NUM_CARDS);

    readonly Mover instant;
    readonly Mover simple;
    readonly Mover fancy;

    public UndoBoard(IBoardActions boardActions, IBoardQuery boardQuery, Mover fancy, Mover simple, Mover instant)
    {
        if (fancy == null) throw new ArgumentNullException("fancy");
        if (simple == null) throw new ArgumentNullException("simple");
        if (instant == null) throw new ArgumentNullException("instant");
        if (boardQuery == null) throw new ArgumentNullException("boardQuery");
        if (boardActions == null) throw new ArgumentNullException("boardActions");

        this.fancy = fancy;
        this.simple = simple;
        this.instant = instant;
        this.boardQuery = boardQuery;
        this.boardActions = boardActions;
    }

    public bool TryUndo()
    {
        bool canUndo = undoEvents.Count > 0;
        if (canUndo)
        {
            undoEvents.Pop().Invoke();
        }
        return canUndo;
    }

    public void DrawCard()
    {
        Card handBeforeDraw = boardQuery.GetCurrentHand();
        Card handAfterDraw = boardQuery.GetNextDrawCard();

        boardActions.TransferCard(handBeforeDraw, simple, CardPile.Hand, CardPile.Discard);
        boardActions.TransferCard(handAfterDraw, simple, CardPile.Draw, CardPile.Hand);

        undoEvents.Push(() =>
        {
            boardActions.TransferCard(handAfterDraw, instant, CardPile.Hand, CardPile.Draw);
            boardActions.TransferCard(handBeforeDraw, instant, CardPile.Discard, CardPile.Hand);
        });
    }

    public void ExecuteRun(Card card)
    {
        Card handBeforeRun = boardQuery.GetCurrentHand();
        Card handAfterRun = card;

        boardActions.TransferCard(handBeforeRun, simple, CardPile.Hand, CardPile.Discard);
        boardActions.TransferCard(handAfterRun, fancy, CardPile.Active, CardPile.Hand);

        undoEvents.Push(() =>
        {
            boardActions.TransferCard(handAfterRun, instant, CardPile.Hand, CardPile.Active);
            boardActions.TransferCard(handBeforeRun, instant, CardPile.Discard, CardPile.Hand);
        });
    }

    public IEnumerator StartNew(IEnumerable<Card> cards)
    {
        undoEvents.Clear();
        return boardActions.LayoutBoard(cards, simple);
    }
}
