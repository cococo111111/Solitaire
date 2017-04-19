/* Decorates the IGame and IUndoHandler interfaces with UI-related side effects.*/

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public sealed class UIUpdater : IGame, IUndoHandler
{
    readonly IGame game;
    readonly IScore score;
    readonly IUndoHandler undo;

    readonly Text drawText;
    readonly Text scoreText;

    int lastScore = 0;

    const string drawTextFormat = "Draws: {0}";
    const int drawTextDefaultValue = 2;

    public UIUpdater(IGame game, IUndoHandler undo, IScore score, TextElements textElements)
    {
        if (game == null) throw new ArgumentNullException("game");
        if (undo == null) throw new ArgumentNullException("undo");
        if (score == null) throw new ArgumentNullException("score");
        if (textElements == null) throw new ArgumentNullException("textElements");

        if (textElements.Score == null) throw new ArgumentException("Missing score text.");
        if (textElements.DrawsLeft == null) throw new ArgumentException("Missing draw text");

        this.game = game;
        this.undo = undo;
        this.score = score;

        this.scoreText = textElements.Score;
        this.drawText = textElements.DrawsLeft;

        ResetDrawText();
    }

    public void ClickCard(Card card)
    {
        game.ClickCard(card);
        UpdateScoreText();
    }

    public void Draw()
    {
        game.Draw();
        UpdateDrawText();
    }

    public void NewGame()
    {
        game.NewGame();
        ResetDrawText();
    }

    public void Undo()
    {
        undo.Undo();
        UpdateScoreText();
    }

    public void UndoAll()
    {
        undo.UndoAll();
        UpdateScoreText();
    }

    void ResetDrawText()
    {
        drawText.text = string.Format(drawTextFormat, drawTextDefaultValue);
    }

    void UpdateDrawText()
    {
        string text = drawText.text;
        string drawsLeftString = new string(text.Where(char.IsNumber).ToArray());
        Assert.IsTrue(drawsLeftString.Length > 0);

        int drawsLeft;
        bool parseSucceeded = int.TryParse(drawsLeftString, out drawsLeft);
        Assert.IsTrue(parseSucceeded, string.Format("Could not parse {0} in text {1}.", drawsLeftString, text));
        if (parseSucceeded)
        {
            drawsLeft = Mathf.Max(0, drawsLeft - 1);
            drawText.text = string.Format(drawTextFormat, drawsLeft);
        }
    }

    void UpdateScoreText()
    {
        // ToString will create garbage, so we only call it if the score has changed.
        int newScore = score.Score;
        if (newScore != lastScore)
        {
            lastScore = newScore;
            scoreText.text = newScore.ToString();
        }
    }
}
