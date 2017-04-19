/* This class is responsible for importing the assets in the "Playing Cards" package. In particular, this involves
 associating each card face to a model from the package, so that each card is ready to be instantiated.*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class CardImporter: ICardImporter
{
    const string CARD_FOLDER = "PlayingCards/Cards/";

    readonly ICardParser parser;

    public CardImporter(ICardParser parser)
    {
        if (parser == null) throw new ArgumentNullException("parser");

        this.parser = parser;
    }

    public IDictionary<CardFace, GameObject> Import()
    {
        var prefabDeck = new Dictionary<CardFace, GameObject>(GameRules.NUM_CARDS);
        GameObject[] cardPrefabs = Resources.LoadAll<GameObject>(CARD_FOLDER);
        Assert.IsTrue(cardPrefabs.Length == GameRules.NUM_CARDS);
        foreach (var prefab in cardPrefabs)
        {
            CardFace card = parser.Parse(prefab.name);
            prefabDeck.Add(card, prefab);
        }
        Assert.AreEqual(prefabDeck.Count, GameRules.NUM_CARDS);
        return prefabDeck;
    }
}