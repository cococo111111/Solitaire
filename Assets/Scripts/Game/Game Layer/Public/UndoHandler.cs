using System;
using UnityEngine;

public sealed class UndoHandler : MonoBehaviour, IUndo
{
    IUndoActions undo;

    public void Init(IUndoActions undo)
    {
        if (undo == null) throw new ArgumentNullException("undo");

        this.undo = undo;
    }

    public void Undo()
    {
        undo.TryUndo();
    }

    public void UndoAll()
    {
        bool undoSuccess;
        do
        {
            undoSuccess = undo.TryUndo();
        } while (undoSuccess);
    }
}
