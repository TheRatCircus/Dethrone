// Central storage and casting of Talents in the player
using UnityEngine;
using System.Collections;
using Dethrone.Talents;

public class PlayerTalentController : MonoBehaviour
{
    // Requisite objects
    protected Animator animator;
    protected Mana mana;
    protected Health health;

    // Talent sets
    private Talent[] primaryTalents;
    private Talent[] secondaryTalents;

    // Status vars
    protected int talentStatus;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mana = GetComponent<Mana>();
        health = GetComponent<Health>();

        // TEMPORARY
        primaryTalents = new Talent[] { ScriptableObject.CreateInstance<Slash>(), ScriptableObject.CreateInstance<Impel>() };
        primaryTalents[0].Initialize(gameObject);
        primaryTalents[1].Initialize(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("talentStatus", talentStatus);
    }

    // Catch input attempting to cast a Talent
    public void TryCast(int talent, bool secondary)
    {
        if (talentStatus == 0)
        {
            TryCastTalent(!secondary ? primaryTalents[talent] : secondaryTalents[talent]);
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
        animator.SetInteger("castAnimation", talent.CastAnimation);
        StartCoroutine(talent.Cast());
        StartCoroutine(OnCast(talent));
        mana.ModifyMana(-talent.ManaCost, talent.CostsHealth);
    }

    // Read Talent status for use in own status
    IEnumerator OnCast(Talent talent)
    {
        talentStatus = talent.TalentStatus;
        while(talent.TalentStatus == 1)
        {
            yield return null;
        }
        talentStatus = talent.TalentStatus;
        while(talent.TalentStatus == 2 )
        {
            yield return null;
        }
        talentStatus = talent.TalentStatus;
    }
}
