using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imports card assets, providing an easily consumed mapping of each card face to prefab.
/// </summary>
public interface ICardImporter
{
    IDictionary<CardFace, GameObject> Import();
}