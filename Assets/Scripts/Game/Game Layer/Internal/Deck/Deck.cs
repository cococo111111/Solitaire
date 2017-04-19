/* The deck builds the actual cards using the provided factory and models. It then provides an interface to 
 Shuffle and extract the cards from the deck. It does not implement any game-specific features - instead, 
 it provides the bare minimum needed for any card game to have access to a full set of cards.*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Deck : IDeck
{
    readonly Dictionary<CardFace, Card> cards = new Dictionary<CardFace, Card>(GameRules.NUM_CARDS);
    readonly Card[] deck = new Card[GameRules.NUM_CARDS];

    int version = 0; // Used to throw an exception when enumerating through a deck that has been altered mid-enumeration.

    public Deck(IDictionary<CardFace, GameObject> cardModels, ICardFactory factory)
    {
        if (cardModels == null) throw new ArgumentNullException("cardModels");
        if (factory == null) throw new ArgumentNullException("factory");

        foreach (var pair in cardModels)
        {
            CardFace face = pair.Key;
            GameObject prefab = pair.Value;
            cards[pair.Key] = CreateCard(face, prefab, factory);
        }

        Assert.IsTrue(CardFace.EnumerateCardFaces().All(cards.ContainsKey));

        ResetDeck();
    }

    public void Shuffle(int seed)
    {
        UnityEngine.Random.InitState(seed);
        Shuffle();
    }

    public void Shuffle()
    {
        ResetDeck();
        for (int i = 0; i < deck.Length; i++)
        {
            SwapCards(i, UnityEngine.Random.Range(i, deck.Length));
        }
        version++;
    }

    /// <summary>
    /// Get all the cards in the deck, in order.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public IEnumerable<Card> GetAllCards()
    {
        int versionAtStart = version;
        for (int i = 0; i < deck.Length; i++)
        {
            if (versionAtStart != version)
                throw new InvalidOperationException("Deck has changed mid-enumeration, most likely from shuffling.");

            yield return deck[i];
        }
    }

    /// <summary>
    /// Set the cards in the deck to default order.
    /// </summary>
    void ResetDeck()
    {
        foreach (CardFace face in CardFace.EnumerateCardFaces())
        {
            deck[face.GetUniqueId()] = cards[face];
        }
        version++;
    }

    void SwapCards(int indexA, int indexB)
    {
        Card a = deck[indexA];
        deck[indexA] = deck[indexB];
        deck[indexB] = a;
    }

    Card CreateCard(CardFace face, GameObject prefab, ICardFactory factory)
    {
        Card card = factory.CreateCard(face, prefab);
        return card;
    }
}
