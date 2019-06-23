// Class storing Talent data

//
using System.Collections;
using UnityEngine;

public enum Opener
{
    None,
    ForwardsLeap,
    Teleport,
    Jump,
    BackwardsLeap,
    Charge
}

//
public enum CastingPattern
{
    None,
    OnCharacter,
    OrbitPoint,
    GroundPoint,
    FreePoint
}

//
public enum EmissionBehaviour
{
    Static,
    Swing,
    Thrust,
    FollowCharacter
}

public enum CastAnimation
{
    None = 0
}

public abstract class Talent : Module
{
    // Requisite scripts
    protected TargettingController targettingController;
    protected TalentController talentController;

    // Enum properties
    protected int castAnimation;
    public int _castAnimation { get => castAnimation; set => castAnimation = value; }
    protected CastingPattern castingPattern;
    public CastingPattern CastingPattern { get => castingPattern; set => castingPattern = value; }

    // Numeric properties
    protected float manaCost;
    public float ManaCost { get => manaCost; set => manaCost = value; }
    protected float telegraphTime;
    public float TelegraphTime { get => telegraphTime; set => telegraphTime = value; }
    protected float castingTime;
    public float CastingTime { get => castingTime; set => castingTime = value; }

    // Bool properties
    protected bool costsHealth;
    public bool CostsHealth { get => costsHealth; set => costsHealth = value; }

    // Status vars
    protected bool isActive;
    public bool IsActive { get => isActive; set => isActive = value; }
    protected bool isTelegraphing;
    public bool IsTelegraphing { get => isTelegraphing; set => isTelegraphing = value; }
    protected bool isCasting;
    public bool IsCasting { get => isCasting; set => isCasting = value; }

    public virtual void Initialize(TargettingController targettingController, TalentController talentController)
    {
        this.targettingController = targettingController;
        this.talentController = talentController;
    }

    public virtual IEnumerator Cast()
    {
        isActive = true;
        isTelegraphing = true;
        talentController.SetStatus(true, true, false);
        yield return new WaitForSeconds(telegraphTime);

        isTelegraphing = false;
        isCasting = true;
        talentController.SetStatus(true, false, true);
        CastEffect();
        yield return new WaitForSeconds(castingTime);

        isCasting = false;
        isActive = false;
        talentController.SetStatus(false, false, false);
    }

    protected virtual void CastEffect()
    {

    }

    protected virtual void Emit()
    {

    }
}
