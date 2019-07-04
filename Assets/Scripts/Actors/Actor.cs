// Generic actor class, holds common actor information and status variables
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Status vars
    protected bool canCharacterAction;
    public bool CanCharacterAction { get => canCharacterAction; set => canCharacterAction = value; }
    protected bool isImmuneToHit;
    public bool IsImmuneToHit { get => isImmuneToHit; set => isImmuneToHit = value; }

    // Called when this actor is hit by an AoE or projectile
    public virtual void OnHit()
    {

    }
}
