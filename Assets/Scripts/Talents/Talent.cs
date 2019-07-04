// Class storing Talent data
using System.Collections;
using UnityEngine;

// This Talent's animation index, for Animator
public enum CastAnimation
{
    None = 0,
    Throw = 1
}

public abstract class Talent : Module
{
    // Requisite objects
    protected TargettingController targettingController;
    protected GameObject owner;

    protected int castAnimation;
    public int CastAnimation { get => castAnimation; set => castAnimation = value; }

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
    protected bool isActive;
    public bool IsActive { get => isActive; set => isActive = value; }
    protected bool isTelegraphing;
    public bool IsTelegraphing { get => isTelegraphing; set => isTelegraphing = value; }
    protected bool isCasting;
    public bool IsCasting { get => isCasting; set => isCasting = value; }

    public virtual void Initialize(TargettingController targettingController, GameObject owner)
    {
        this.targettingController = targettingController;
        this.owner = owner;
    }

    // Cast this talent
    public virtual IEnumerator Cast()
    {
        isActive = true;
        isTelegraphing = true;
        yield return new WaitForSeconds(telegraphTime);

        isTelegraphing = false;
        isCasting = true;
        CastEffect();
        yield return new WaitForSeconds(castingTime);

        isCasting = false;
        isActive = false;
    }

    // This talent's effect upon being cast
    protected virtual void CastEffect()
    {

    }

    // This talent's emission behaviour
    protected virtual void Emit()
    {

    }
}
