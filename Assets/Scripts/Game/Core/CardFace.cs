/* CardFace identifies the card's face: its suit and value, e.g. 3 of hearts. It uses enums for both Suit and
 Value. enums are very convenient for this purpose, but do have some limitations, */

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum Suit : byte
{
    Hearts   = 0,
    Clubs    = 1,
    Spades   = 2,
    Diamonds = 3
}

public enum Value : byte
{
    Ace   = 1,
    Two   = 2,
    Three = 3,
    Four  = 4,
    Five  = 5,
    Six   = 6,
    Seven = 7,
    Eight = 8,
    Nine  = 9,
    Ten   = 10,
    Jack  = 11,
    Queen = 12,
    King  = 13
}

/// <summary>
/// Representation of a possible card's face: its suit and value.
/// </summary>
public struct CardFace : IEquatable<CardFace>
{
    public Value Value { get { return value; } }
    public Suit Suit { get { return suit; } }

    readonly Value value;
    readonly Suit suit;

    public CardFace(Suit suit, Value value)
    {
        Assert.IsTrue(0 <= (int)suit && (int)suit < 4);
        Assert.IsTrue(1 <= (int)value && (int)value < 14);
       
        this.value = value;
        this.suit = suit;
    }

    public static IEnumerable<CardFace> EnumerateCardFaces()
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Value value in Enum.GetValues(typeof(Value)))
            {
                yield return new CardFace(suit, value);
            }
        }
    }

    public override string ToString()
    {
        return string.Format("{0} of {1}", value, suit);
    }

    public override int GetHashCode()
    {
        return GetUniqueId();
    }

    public override bool Equals(object obj)
    {
        return (obj is CardFace) && Equals((CardFace)obj);
    }

    public bool Equals(CardFace other)
    {
        return GetUniqueId() == other.GetUniqueId();
    }

    public static bool operator ==(CardFace a, CardFace b)
    {
        return a.GetUniqueId() == b.GetUniqueId();
    }

    public static bool operator !=(CardFace a, CardFace b)
    {
        return a.GetUniqueId() != b.GetUniqueId();
    }

    /// <summary>
    /// A number uniquely identifying this card's combination of suit and value, between 0 (inclusive) and the 
    /// number of cards in the deck (exclusive).
    /// </summary>
    public int GetUniqueId()
    {
        return ((int)Value - 1) + (int)Suit * 13;
    }
}