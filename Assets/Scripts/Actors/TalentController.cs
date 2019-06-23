// Central storage for Talent objects and handling of their casting
using UnityEngine;
using Dethrone.Talents;

public class TalentController : MonoBehaviour
{
    // Requisite scripts
    protected TargettingController targettingController;
    protected Mana actorMana;
    protected Health actorHealth;

    // Talent sets
    private Talent[] primaryTalents;
    private Talent[] secondaryTalents;

    // Status vars
    private int castAnimation;
    private bool isTelegraphing;
    private bool isActive;
    private bool isCasting;

    // Start is called before the first frame update
    void Start()
    {
        targettingController = GetComponent<TargettingController>();
        actorMana = GetComponent<Mana>();
        actorHealth = GetComponent<Health>();

        // TEMPORARY
        primaryTalents = new Talent[] { ScriptableObject.CreateInstance<Slash>() };
        primaryTalents[0].Initialize(targettingController, this);
    }

    // Catch input attempting to cast a Talent
    public void TryCast(int talent, bool secondary)
    {
        if (!isActive)
        {
            if (!secondary)
            {
                TryCastTalent(primaryTalents[talent]);
            }
            else
            {
                TryCastTalent(secondaryTalents[talent]);
            }
        }
        else
        {
            Debug.Log("Cast failed: another talent is already casting.");
        }
    }

    // Attempt to cast a specific Talent, checking all prerequisites on way
    public void TryCastTalent(Talent talent)
    {
        if (talent.ManaCost <= (talent.CostsHealth ? actorHealth._health : actorMana._mana))
        {
            CastTalent(talent);
        }
        else
        {
            Debug.Log("Insufficient mana/health.");
        }
    }

    // Finally go through the process of casting the talent
    public void CastTalent(Talent talent)
    {
        castAnimation = talent._castAnimation;
        StartCoroutine(talent.Cast());
        actorMana.ConsumeMana(talent.ManaCost, talent.CostsHealth);
    }

    public void SetStatus(bool isActive, bool isTelegraphing, bool isCasting)
    {
        this.isActive = isActive;
        this.isTelegraphing = isTelegraphing;
        this.isCasting = isCasting;
    }

}
