/* An implementation of poor man's dependancy injection, suitable given the small scope of this game.*/

using UnityEngine;
using UnityEngine.Assertions;

public sealed class DependencyInjector : MonoBehaviour
{
    // These are assigned manually in the inspector.
    public Game game;
    public GameObject cardAnchor;
    public TextElements text;

    public UndoHandler undoHandler;
    public InputHandler inputHandler;
    public ButtonHandler buttonHandler;

    void Awake()
    {
        Assert.IsNotNull(cardAnchor);
        Assert.IsNotNull(game);
        Assert.IsNotNull(text);
        Assert.IsNotNull(undoHandler);
        Assert.IsNotNull(inputHandler);
        Assert.IsNotNull(buttonHandler);

        Deck deck = InstallDeck();
        Board board = InstallBoard();
        ScoreBoard scoreBoard = InstallScoreBoard(board, board);

        game.Init(scoreBoard, board, deck);
        undoHandler.Init(scoreBoard);
        var uiUpdater = new UIUpdater(game, undoHandler, scoreBoard, text);
        var quitHandler = new BasicQuit();
        inputHandler.Init(uiUpdater, uiUpdater);
        buttonHandler.Init(uiUpdater, uiUpdater, quitHandler);
    }

    Deck InstallDeck()
    {
        var cardParser = new BasicCardParser();
        var cardFactory = new CardFactory(cardAnchor);
        var cardImporter = new CardImporter(cardParser);
        return new Deck(cardImporter.Import(), cardFactory);
    }

    ScoreBoard InstallScoreBoard(IBoardActions boardActions, IBoardQuery boardQuery)
    {
        var undoBoard = new UndoBoard(boardActions, boardQuery, 
            new MoverRotateAsync(), new MoverSlideAsync(), new MoverImmediate());

        var scoreBoard = new ScoreBoard(undoBoard, undoBoard);
        return scoreBoard;
    }

    Board InstallBoard()
    {
        var layout = new Layout();

        var hand = new Hand(layout);
        var drawPile = new DrawPile(layout);
        var discard = new DiscardPile(layout);
        var activeCards = new ActiveCards(layout);

        var board = new Board(activeCards, drawPile, discard, hand);
        return board;
    }
}
