# Tri Peaks Solitaire

![](http://i.imgur.com/rH4nRbg.jpg)

### Introduction

This is an implementation of Solitaire that goes by the name of Tri Peaks or Prospector Solitaire. The core game is complete, and a standalone build is included in the Builds folder. A major change to the normal game of Tri Peaks is the inclusion of a discard pile that lays out the previously seen cards. This allows for a more strategic approach to forming runs. 

Pressing the escape key (or whatever key is mapped to the "Cancel" input axis) in the game will bring up the instructions.

### Code overview

This purpose of this project was largely to experiment with the use of SOLID principles for a complete game. A familiarity with dependency injection in particular is needed to understand the code base. Given the relatively small scope of the game, poor man's dependency injection is used to wire up the classes. 

The project uses two layers, one for UI and one for the game logic. The UI layer talks to the game layer solely through the IGame, IUndo and IScoreQuery interfaces. The game layer is oblivious to the existence of the UI layer.

A small, core set of classes is shared throughout the project, which do not depend on anything else. These are classes for the cards and essential game rules. 

The rest of this section gives a quick bottom-up overview of the entire game layer.

The Layout class dictates the positions of all the cards on the screen. Its readonly interface is segregated for each card pile (i.e. draw pile, active cards, discard pile and hand). These are injected into the implementations of the various card piles, who are then injected into Board, which implements the essential API for the state and interactions of all the draw piles, provided by the segregated interfaces IBoardActions and IBoardQuery. 

UndoBoard then uses these board interfaces to translate the higher level game actions (e.g. draw a card) into the appropriate board actions (move card from hand to discard pile, move card from draw pile to hand), implementing the higher level interfaces IGameActions and IUndoActions used by the top-level handlers in this layer. The ScoreBoard decorates these interfaces with scoring functionality, implementing the public IScoreQuery interface for the UI layer to read the current score.

Unrelated to this, the Deck class uses a card factory (ICardFactory) and a lookup table of card models to implement the IDeck interface, which provides access to a collection of cards.

Finally, the Game class uses the IGameActions, IBoardQuery (this is where we needed the board action/query segregation - Game should not manipulate the board directly) and IDeck interfaces to implement the high level game logic, implementing the public IGame interface. Similarly, UndoHandler implements the high level undo logic, implementing the public IUndo interface.

### Art assets

The card art is from a free Unity asset pack called []"Free Playing Cards - Ultimate Sport Pack"](https://www.assetstore.unity3d.com/en/#!/content/51076) by 1Poly. 