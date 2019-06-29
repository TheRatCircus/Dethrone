// Generic actor class, holds common actor information and status variables
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected string actorName;

    protected bool canCharacterAction;
    public bool CanCharacterAction { get => canCharacterAction; set => canCharacterAction = value; }
}
