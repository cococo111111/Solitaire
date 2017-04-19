/* The obvious approach to handling card movement is to place the movement logic in the card class. But movement
 logic is very likely to change in the future, so the details are implemented in a separate Mover class, which is
 provided to the card class via method injection when it's told to move. Among other things, this ensures that a class
 which has not been injected with a Mover cannot use the Card's move method, avoiding the need to segregate the
 Card class's interface for movement logic.*/

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Mover governs the logic of how an object is moved from one position to another, supporting 
/// asynchronous movement.
/// </summary>
public abstract class Mover
{
    public IEnumerator MoveTo(Transform transform, Vector3 viewportPosition)
    {
        if (transform == null) throw new ArgumentNullException("transform");

        return MoveToGlobal(transform, ViewportToWorld(viewportPosition));
    }

    protected abstract IEnumerator MoveToGlobal(Transform transform, Vector3 position);

    protected Vector3 ViewportToWorld(Vector3 viewportPos)
    {
        return Camera.main.ViewportToWorldPoint(viewportPos);
    }
}