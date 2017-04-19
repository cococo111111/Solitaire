/* This mover slides the card to its new location, but also rapidly rotates it.*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverRotateAsync : Mover
{
    const int numFrames = 20;
    const int rotationSpeed = 2;

    protected override IEnumerator MoveToGlobal(Transform transform, Vector3 position)
    {
        Vector3 startPosition = transform.position;
        Vector3 positionIncrement = (position - startPosition) / numFrames;
        float rotateIncrement = rotationSpeed * 360f / numFrames;
        for (int frame = 0; frame < numFrames; frame++)
        {
            transform.position += positionIncrement;
            transform.Rotate(0f, 0f, rotateIncrement);
            yield return null;
        }
    }
}
