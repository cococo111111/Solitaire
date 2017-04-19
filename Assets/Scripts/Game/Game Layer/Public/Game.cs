using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Game : MonoBehaviour, IGame
{
    IGameActions gameActions;
    IBoardQuery boardQuery;
    IDeck deck;

    int drawsLeft = GameRules.NUM_STARTING_DRAWS;

    public void Init(IGameActions gameActions, IBoardQuery boardQuery, IDeck deck)
    {
        if (gameActions == null) throw new ArgumentNullException("gameActions");
        if (boardQuery == null) throw new ArgumentNullException("boardQuery");
        if (deck == null) throw new ArgumentNullException("deck");

        this.gameActions = gameActions;
        this.boardQuery = boardQuery;
        this.deck = deck;
    }

    public void NewGame()
    {
        // Given that this event wipes progress, we can just reload the scene. 
        SceneManager.LoadScene("Game");
    }

    public void Draw()
    {
        if (drawsLeft > 0)
        {
            drawsLeft--;
            StartNewGame(shuffle: true);
        }
    }

    public void ClickCard(Card card)
    {
        if (card == null)
            throw new ArgumentNullException("card");

        HandleCardClick(card);

        if (IsVictoryConditionMet())
        {
            StartNewGame(shuffle: true);
        }
    }

    bool IsVictoryConditionMet()
    {
        return boardQuery.IsActivePileClear();
    }

    void HandleCardClick(Card card)
    {
        if (FormsValidRun(card))
        {
            gameActions.ExecuteRun(card);
        }
        else if (DrawPileClicked(card))
        {
            gameActions.DrawCard();
        }
        else
        {
            // No behaviour associated with this card.
        }
    }

    void Start()
    {
        StartNewGame(shuffle: true);
    }

    bool FormsValidRun(Card card)
    {
        Card hand = boardQuery.GetCurrentHand();

        return card.IsFaceUp
            && boardQuery.IsContainedIn(card, CardPile.Active)
            && GameRules.IsValidRun(card.Face, hand.Face);
    }

    bool DrawPileClicked(Card card)
    {
        return boardQuery.IsContainedIn(card, CardPile.Draw);
    }

    void StartNewGame(bool shuffle)
    {
        StopAllCoroutines();
        if (shuffle)
        {
            deck.Shuffle();
        }
        IEnumerable<Card> cards = deck.GetAllCards();
        StartCoroutine(gameActions.StartNew(cards));
    }
}
