using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public virtual CardFace Face { get { return face; } }
    CardFace face;

    public virtual bool IsFaceUp { get { return isFaceUp; } }
    bool isFaceUp = false;

    protected readonly Vector3 flipCardRotation = new Vector3(180f, 0f, 0f);

    /// <summary>
    /// Factory method for card creation. Card implementation is specified via generic parameter to permit extension.
    /// Resulting Card object will have both collider and a kinematic rigidbody.
    /// </summary> 
    /// <typeparam name="T">Which implementation of Card to use.</typeparam>
    /// <param name="face">The card's face, e.g. three of hearts.</param>
    /// <param name="cardModel">The model to use for this card. Will be copied/instantiated.</param>
    /// <returns>The created Card object.</returns>
    public static Card CreateNewCard<T>(CardFace face, GameObject cardModel) where T : Card
    {
        if (cardModel == null) throw new ArgumentNullException("cardPrefab");

        GameObject cardObject = Instantiate(cardModel);
        cardObject.tag = GameRules.CARD_TAG;
        Card card = cardObject.AddComponent<T>();
        card.face = face;
        card.AddCollider();
        card.AddRigidbody();
        return card;
    }

    public virtual void SetFaceUp()
    {
        if (!isFaceUp)
        {
            transform.Rotate(flipCardRotation);
        }
        isFaceUp = true;
    }

    public virtual void SetFaceDown()
    {
        if (IsFaceUp)
        {
            transform.Rotate(flipCardRotation);
        }
        isFaceUp = false;
    }

    /// <summary>
    /// Move the card to the viewport position, using the given mover. 
    /// </summary>
    /// <param name="viewportPosition">Where to move the card.</param>
    /// <param name="mover">Determines how the card is moved.</param>
    public virtual void MoveTo(Vector3 viewportPosition, Mover mover)
    {
        if (mover == null) throw new ArgumentNullException("mover");

        EndCurrentMovement();
        StartCoroutine(mover.MoveTo(transform, viewportPosition));
    }

    void EndCurrentMovement()
    {
        StopAllCoroutines();
        transform.rotation = isFaceUp ? Quaternion.Euler(flipCardRotation) : Quaternion.identity;
    }

    void AddRigidbody()
    {
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true; // Transform is manipulated directly (no physics needed)
    }

    void AddCollider()
    {
        gameObject.AddComponent<BoxCollider>(); // Useful for click detection
    }
}