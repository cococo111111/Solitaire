/* This class dictates how all the cards in the game should be positioned, using viewport space. (0,0) is the
 bottom left of the screen, and (1,1) is the top right. The third vector determines depth - given the use of an
 orthographic camera, depth only affects whether or not something is visible and how objects overlap, not any
 perception of depth.
 
  Changes to the layout of any cards often requires changes to other cards, so the layouts for all the different
 piles are implemented here. The interface has been segregated for each pile, as a given pile does not need access
 to the layout for other piles. 
 
  Most of the non-trivial logic in this class involves the active cards, since those have a more involved layout
 than the other piles.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Layout: IActiveCardsLayout, IDrawPileLayout, IHandLayout, IDiscardPileLayout
{
    const float center = 0.5f;

    readonly Vector2 drawPilePosition = new Vector2(0.38f, 0.25f);
    readonly Vector2 drawPileOffset = new Vector2(0.0015f, 0f);

    readonly Vector2 handPosition = new Vector2(0.6f, 0.25f);

    readonly Vector2 discardPosition = new Vector2(0.7f, 0.3f);
    readonly Vector2 discardOffset = new Vector2(0.02f, 0.04f);

    // Core active layout parameters
    const float xOffset = 0.09f;   // horizontal space allocated for each card
    const float yOffset = 0.11f;   // vertical space allocated for each card, with vertical overlap intended

    // Derivative active layout parameters
    const float margin = (1 - 9 * xOffset) / 2;     // horizontal margin of first layer
    const float marginOffset = xOffset / 2;         // extra horizontal margin per layer

    IEnumerable<Vector3> IActiveCardsLayout.GetPositions()
    {
        return GetFirstLayer().Concat(GetSecondLayer())
                              .Concat(GetThirdLayer())
                              .Concat(GetFourthLayer());
    }

    Vector3 IDrawPileLayout.GetPosition()
    {
        return drawPilePosition;
    }

    Vector3 IDrawPileLayout.GetOffset()
    {
        return drawPileOffset;
    }

    Vector3 IHandLayout.GetPosition()
    {
        return handPosition;
    }

    Vector3 IDiscardPileLayout.GetOffset()
    {
        return discardOffset;
    }

    Vector3 IDiscardPileLayout.GetPosition()
    {
        return discardPosition;
    }

    IEnumerable<Vector3> GetFirstLayer()
    {
        Vector3 position = GetStartPosition(layer: 0);
        for (int i = 0; i < 10; i++)
        {
            yield return position;
            position.x += xOffset;
        }
    }

    IEnumerable<Vector3> GetSecondLayer()
    {
        Vector3 position = GetStartPosition(layer: 1);
        for (int i = 0; i < 9; i++)
        {
            yield return position;
            position.x += xOffset;
        }
    }

    IEnumerable<Vector3> GetThirdLayer()
    {
        Vector3 position = GetStartPosition(layer: 2);
        for (int i = 0; i < 3; i++)
        {
            yield return position;
            position.x += xOffset;
            yield return position;
            position.x += 2 * xOffset;
        }
    }

    IEnumerable<Vector3> GetFourthLayer()
    {
        Vector3 position = GetStartPosition(layer: 3);
        for (int i = 0; i < 3; i++)
        {
            yield return position;
            position.x += 3 * xOffset;
        }
    }

    // Layernum should be 0, 1, 2 or 3 corresponding to 1st through 4th layers
    Vector3 GetStartPosition(int layer)
    {
        Assert.IsTrue(-1 < layer && layer < 4);
        return new Vector3(margin, center, 1f) + layer * new Vector3(marginOffset, yOffset, 1f);
    }
}
