/* This class is responsible for mapping inputs onto game events.*/

using System;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class InputHandler : MonoBehaviour
{
    IGame game;
    IUndoHandler undoHandler;

    const float raycastDistance = 100f; 

    public void Init(IGame game, IUndoHandler undoHandler)
    {
        if (game == null) throw new ArgumentNullException("game");
        if (undoHandler == null) throw new ArgumentNullException("undoHandler");
        
        this.game = game;
        this.undoHandler = undoHandler;
    }

    // This is simple raycast logic to detect if a card has been clicked on, and which one has been clicked.
    // Arguably the raycasting logic should belong to its own class, but is extremely unlikely to ever change,
    // so this is not a violation of the SRP. 
    bool HasClickedOnCard(out Card card)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(GetMousePosition(), Vector3.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, raycastDistance))
            {
                if (hitInfo.transform.tag == GameRules.CARD_TAG)
                {
                    card = hitInfo.transform.GetComponent<Card>();
                    Assert.IsNotNull(card, "Object tagged as Card without a Card component.");
                    return true;
                }
            }
        }
        card = null;
        return false;
    }

    void Start()
    {
        // Sidenote: this is actually true by default.
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1))
        {
            undoHandler.Undo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            undoHandler.UndoAll();
        }

        Card card;
        if (HasClickedOnCard(out card))
        {
            game.ClickCard(card);
        }
    }

    Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
