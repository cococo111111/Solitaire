using UnityEngine;

public interface ICardFactory 
{
    Card CreateCard(CardFace face, GameObject prefab);
}
