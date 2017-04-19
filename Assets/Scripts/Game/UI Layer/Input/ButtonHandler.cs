/* This class is responsible for mapping button clicks onto game events. */

using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ButtonHandler : MonoBehaviour
{
    [SerializeField] Button draw;
    [SerializeField] Button undo;
    [SerializeField] Button newGame;
    [SerializeField] Button undoBoard;

    public void Init(IGame game, IUndoHandler undoHandler)
    {
        if (game == null) throw new ArgumentNullException("game");
        if (undoHandler == null) throw new ArgumentNullException("undoHandler");

        newGame.onClick.AddListener(game.NewGame);

        draw.onClick.AddListener(game.Draw);

        undo.onClick.AddListener(undoHandler.Undo);

        undoBoard.onClick.AddListener(undoHandler.UndoAll);
    }
}
