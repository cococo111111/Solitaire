/* This provides a centralized access point to text objects with text that may need to change during the course
 of a game. This way, the IoC container doesn't need to keep separate references to each one. */

using UnityEngine;
using UnityEngine.UI;

public sealed class TextElements : MonoBehaviour
{
    [SerializeField] Text score;
    [SerializeField] Text drawsLeft;

    public Text Score { get { return score; } }
    public Text DrawsLeft { get { return drawsLeft; } }
}
