/* This class is responsible for mapping button clicks onto game events. */

using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ButtonHandler : MonoBehaviour
{
    [SerializeField] Button drawButton;
    [SerializeField] Button undoButton;
    [SerializeField] Button newGameButton;
    [SerializeField] Button undoBoardButton;
    [SerializeField] Button quitButton;

    public void Init(IGame game, IUndo undoHandler, IQuit quitHandler)
    {
        if (game == null) throw new ArgumentNullException("game");
        if (undoHandler == null) throw new ArgumentNullException("undoHandler");

        newGameButton.onClick.AddListener(game.NewGame);

        drawButton.onClick.AddListener(game.Draw);

        undoButton.onClick.AddListener(undoHandler.Undo);

        undoBoardButton.onClick.AddListener(undoHandler.UndoAll);

        quitButton.onClick.AddListener(quitHandler.Quit);
    }
}
