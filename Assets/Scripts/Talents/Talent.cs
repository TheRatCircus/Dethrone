// Class storing Talent data
using System.Collections;
using UnityEngine;

// This Talent's animation index, for Animator
public enum CastAnimation
{
    None = 0,
    Throw = 1,
    SabreSlash = 2
}

public abstract class Talent : Module
{
    // Requisite objects
    protected GameObject owner;
    protected LandMovementController movementController;
    protected SpriteRenderer spriteRenderer;

    protected int castAnimation;
    public int CastAnimation { get => castAnimation; set => castAnimation = value; }
    protected Vector2 castPosition;

    // Inherent vars
    protected float manaCost;
    public float ManaCost { get => manaCost; set => manaCost = value; }
    protected float telegraphTime;
    public float TelegraphTime { get => telegraphTime; set => telegraphTime = value; }
    protected float castingTime;
    public float CastingTime { get => castingTime; set => castingTime = value; }

    // Augment vars
    protected bool costsHealth;
    public bool CostsHealth { get => costsHealth; set => costsHealth = value; }

    // Status vars
    protected int talentStatus; // 0 inactive, 1 telegraphing, 2 casting
    public int TalentStatus { get => talentStatus; }

    public virtual void Initialize(GameObject owner)
    {
        this.owner = owner;
        movementController = owner.GetComponent<LandMovementController>();
        spriteRenderer = owner.GetComponent<SpriteRenderer>();
    }

    // Cast this talent
    public virtual IEnumerator Cast()
    {
        talentStatus = 1;
        movementController.CanMove = false;
        TelegraphEffect();
        yield return new WaitForSeconds(telegraphTime);
        talentStatus = 2;
        CastEffect();
        yield return new WaitForSeconds(castingTime);
        talentStatus = 0;
        movementController.CanMove = true;
    }

    // This talent's effect upon starting to telegraph
    protected virtual void TelegraphEffect() { }

    // This talent's effect upon being cast
    protected virtual void CastEffect() { }

    // This talent's emission behaviour
    protected virtual void Emit() { }
}
