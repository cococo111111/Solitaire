/* This class is responsible for reading the name of a card and determining its Suit and Value for the "Playing Cards"
 card assets, to ensure the right model is associated with the right CardFace. This could be done manually, 
 but automating it makes it less error prone (and less tedious).*/

using UnityEngine.Assertions;
using System;

// Examples: "PlayingCards_10Heart", "PlayingCards_QSpades"
public sealed class BasicCardParser : ICardParser
{
    const string SPADES   = "Spades"; // Note the naming inconsistency: spades is the only suit to be pluralized
    const string HEARTS   = "Heart";
    const string CLUBS    = "Club";
    const string DIAMONDS = "Diamond";

    public CardFace Parse(string cardString)
    {
        if (cardString == null) throw new ArgumentNullException("cardString");

        // PlayingCards_3Heart -> [PlayingCards, 3Heart] -> 3Heart -> Value = 3, Suit = Heart
        string[] splitCardString = cardString.Split('_');
        Assert.AreEqual(2, splitCardString.Length);
        string suffix = splitCardString[1];
        Value value = GetValue(suffix);
        Suit suit = GetSuit(suffix);
        return new CardFace(suit, value);
    }

    Value GetValue(string suffix)
    {
        Assert.IsTrue(suffix.Length > 0);
        char firstChar = suffix[0];
        if (firstChar == 'Q')
        {
            return Value.Queen;
        }
        else if (firstChar == 'K')
        {
            return Value.King;
        }
        else if (firstChar == 'A')
        {
            return Value.Ace;
        }
        else if (firstChar == 'J')
        {
            return Value.Jack;
        }
        else
        {
            int numericValue;
            if (!int.TryParse(firstChar.ToString(), out numericValue))
            {
                throw new ArgumentException("Cannot determine value of card string.");
            }
            if (numericValue == 1) // All the numeric cases are single-digit except 10
            {
                numericValue = 10;
            }
            return (Value)numericValue;
        }
    }

    Suit GetSuit(string suffix)
    {
        if (suffix.Contains(CLUBS))
        {
            return Suit.Clubs;
        }
        else if (suffix.Contains(HEARTS))
        {
            return Suit.Hearts;
        }
        else if (suffix.Contains(SPADES))
        {
            return Suit.Spades;
        }
        else if (suffix.Contains(DIAMONDS))
        {
            return Suit.Diamonds;
        }
        else
        {
            throw new ArgumentException("Cannot identify suit for this card string");
        }
    }
}
