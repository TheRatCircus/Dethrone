// Central storage for Talent objects and handling of their casting
using UnityEngine;
using System.Collections;
using Dethrone.Talents;

public class PlayerTalentController : MonoBehaviour
{
    // Requisite scripts
    protected TargettingController targettingController;
    protected Animator animator;
    protected Mana mana;
    protected Health health;

    // Talent sets
    private Talent[] primaryTalents;
    private Talent[] secondaryTalents;

    // Status vars
    protected bool isTelegraphing;
    protected bool isActive;
    protected bool isCasting;

    // Start is called before the first frame update
    void Start()
    {
        targettingController = GetComponent<TargettingController>();
        animator = GetComponent<Animator>();
        mana = GetComponent<Mana>();
        health = GetComponent<Health>();

        // TEMPORARY
        primaryTalents = new Talent[] { ScriptableObject.CreateInstance<Slash>(), ScriptableObject.CreateInstance<Impel>() };
        primaryTalents[0].Initialize(targettingController, gameObject);
        primaryTalents[1].Initialize(targettingController, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isTelegraphing", isTelegraphing);
        animator.SetBool("isCasting", isCasting);
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
        if (talent.ManaCost <= (talent.CostsHealth ? health._health : mana._mana))
        {
            CastTalent(talent);
        }
        else
        {
            Debug.Log($"Insufficient {(talent.CostsHealth ? "health" : "mana")}.");
        }
    }

    // Finally go through the process of casting the talent
    public void CastTalent(Talent talent)
    {
        animator.SetInteger("castAnimation", talent._castAnimation);
        StartCoroutine(talent.Cast());
        StartCoroutine(OnCast(talent));
        mana.ModifyMana(-talent.ManaCost, talent.CostsHealth);
    }

    IEnumerator OnCast(Talent talent)
    {
        isActive = true;
        isTelegraphing = true;
        while(talent.IsTelegraphing)
        {
            yield return null;
        }
        isTelegraphing = false;
        isCasting = true;
        while(talent.IsCasting)
        {
            yield return null;
        }
        isCasting = false;
        isActive = false;
    }
}
