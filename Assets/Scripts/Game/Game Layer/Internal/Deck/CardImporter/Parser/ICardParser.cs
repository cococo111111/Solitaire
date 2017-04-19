/// <summary>
/// Determines which card face the string represents.
/// </summary>
public interface ICardParser
{
    CardFace Parse(string cardString);
}