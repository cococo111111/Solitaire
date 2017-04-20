using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public sealed class Instructions : MonoBehaviour
{
    [SerializeField] RectTransform instructionPanel;

    GameObject instructionGameObject;

    readonly WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();
    readonly Vector3 revealStartSize = 0.1f * Vector3.one;
    readonly Vector3 sizeIncrement = 0.1f * Vector3.one;

    const int numSizeIterations = 9;

    void Start()
    {
        instructionGameObject = instructionPanel.gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            if (instructionGameObject.activeInHierarchy)
            {
                StartCoroutine(Hide());
            }
            else
            {
                StartCoroutine(Reveal());
            }
        }
    }

    IEnumerator Reveal()
    {
        EnableInstructionPanel();
        instructionPanel.localScale = revealStartSize;
        for (int i = 0; i < numSizeIterations; i++)
        {
            instructionPanel.localScale += sizeIncrement;
            yield return waitForFrame;
        }
    }

    IEnumerator Hide()
    {
        instructionPanel.localScale = Vector3.one;
        for (int i = 0; i < numSizeIterations; i++)
        {
            instructionPanel.localScale -= sizeIncrement;
            yield return waitForFrame;
        }
        DisableInstructionPanel();
    }

    void EnableInstructionPanel()
    {
        instructionGameObject.SetActive(true);
    }

    void DisableInstructionPanel()
    {
        instructionGameObject.SetActive(false);
    }
}