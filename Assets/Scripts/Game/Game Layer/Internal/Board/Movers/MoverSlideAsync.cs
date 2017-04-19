using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Slide the object to the position asynchronously.
/// </summary>
public sealed class MoverSlideAsync : Mover
{
    const int numFramesDuringMovement = 10;

    static readonly WaitForFixedUpdate fixedFrame = new WaitForFixedUpdate();

    protected override IEnumerator MoveToGlobal(Transform transform, Vector3 position)
    {
        Vector3 stepVector = (position - transform.position) / numFramesDuringMovement;
        for (int i = 0; i < numFramesDuringMovement; i++)
        {
            transform.position += stepVector;
            yield return fixedFrame;
        }
    }
}