using System.Collections.Generic;

public interface IDeck
{
    void Shuffle();
    void Shuffle(int seed);
    IEnumerable<Card> GetAllCards();
}
