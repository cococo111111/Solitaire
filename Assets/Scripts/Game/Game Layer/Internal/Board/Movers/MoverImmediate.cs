using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Set the position synchronously.
/// </summary>
public sealed class MoverImmediate : Mover
{
    protected override IEnumerator MoveToGlobal(Transform transform, Vector3 position)
    {
        transform.position = position;
        yield return null;
    }
}