// Class storing Talent data
using System.Collections;
using UnityEngine;

// This Talent's animation index, for Animator
public enum CastAnimation
{
    None = 0,
    Throw = 1,
    SlashSabre = 2,
    SlashSabreAlternate = 3
}

// Talent status is passed to Animator as an int; enum is for ease
public enum TalentStatus
{
    Inactive = 0,
    Telegraphing = 1,
    Casting = 2,
    Combo = 3
}

public abstract class Talent : Module
{
    // Requisite objects
    protected GameObject owner;
    protected LandMovementController movementController;
    protected SpriteRenderer spriteRenderer;

    protected (int normal, int alt) castAnimation;
    public (int normal, int alt) CastAnimation { get => castAnimation; }
    public int CastAnimationNormal { set => castAnimation.normal = value; }
    public int CastAnimationAlt { set => castAnimation.alt = value; }

    protected Vector2 castPosition;

    // Inherent vars
    protected float manaCost;
    public float ManaCost { get => manaCost; set => manaCost = value; }

    // Timing
    protected (float longTelegraphTime, float shortTelegraphTime, float castingTime, float comboTime) timing;
    public (float longTelegraphTime, float shortTelegraphTime, float castingTime, float comboTime) Timing { get => timing; }

    public float LongTelegraphTime { set => timing.longTelegraphTime = value; }
    public float ShortTelegraphTime { set => timing.shortTelegraphTime = value; }
    public float CastingTime { set => timing.castingTime = value; }
    public float ComboTime { set => timing.comboTime = value; }

    // Augment vars
    protected bool costsHealth;
    public bool CostsHealth { get => costsHealth; set => costsHealth = value; }

    // Status vars
    protected int talentStatus; // 0 inactive, 1 telegraphing, 2 casting, 3 combo
    public int TalentStatus { get => talentStatus; }

    public virtual void Initialize(GameObject owner)
    {
        this.owner = owner;
        movementController = owner.GetComponent<LandMovementController>();
        spriteRenderer = owner.GetComponent<SpriteRenderer>();
    }

    // Cast this talent
    public virtual IEnumerator Cast(bool combo, bool repeat)
    {
        if (talentStatus == (int)global::TalentStatus.Inactive
            || talentStatus == (int)global::TalentStatus.Combo)
        {
            talentStatus = 1;
            movementController.CanMove = false;
            TelegraphEffect(repeat);
            yield return new WaitForSeconds(combo ? timing.shortTelegraphTime : timing.longTelegraphTime);
            talentStatus = 2;
            CastEffect(repeat);
            yield return new WaitForSeconds(timing.castingTime);
            talentStatus = 3;
            CloseEffect();
            movementController.CanMove = true;
            yield return new WaitForSeconds(timing.comboTime);
            talentStatus = 0;
        }
        else
        {
            Debug.Log("A talent attempted an illegal cast.");
        }
    }

    // This talent's effect upon starting to telegraph
    protected virtual void TelegraphEffect(bool repeat) { }

    // This talent's effect upon being cast
    protected virtual void CastEffect(bool repeat) { }

    // This talent's effect after casting ends. Usually to destroy emissions
    protected virtual void CloseEffect() { }

    // This talent's emission behaviour
    protected virtual void Emit(bool repeat) { }

    // Stop this Talent immediately and destroy all its emissions
    public virtual void KillTalent()
    {
        talentStatus = (int)global::TalentStatus.Inactive;
        CoroutineSingleton.instance.StopCoroutine(Cast(false, false));
    }
}
