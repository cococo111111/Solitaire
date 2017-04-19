using System;
using UnityEngine;

public sealed class UndoHandler : MonoBehaviour, IUndoHandler
{
    IUndo undo;

    public void Init(IUndo undo)
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
