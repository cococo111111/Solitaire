using System.Collections.Generic;
using UnityEngine;

public interface IActiveCardsLayout
{
    /// <summary>
    /// Get a sequence of viewport positions that go from left to right, bottom to top. Layers have 10, 9, 6 and 3
    /// positions respectively.
    /// </summary>
    IEnumerable<Vector3> GetPositions();
}
