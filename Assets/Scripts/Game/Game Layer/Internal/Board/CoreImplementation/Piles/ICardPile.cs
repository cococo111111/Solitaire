/// <summary>
/// Corresponds to different parts of the board that can hold cards, which support adding and removing cards.
/// </summary>
public interface ICardPile
{
    void Add(Card card, Mover mover);
    void Remove(Card card);
    bool Contains(Card card);
    void Reset();
}
