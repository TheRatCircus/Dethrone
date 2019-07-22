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
    private Combo[] primaryTalents;
    private Combo[] secondaryTalents;

    // A singleton is used to call coroutines in ScriptableObjecets owned by
    // this class
    public static PlayerTalentController instance;

    // Status vars
    protected int playerTalentStatus; // 0 inactive, 1 telegraphing, 2 casting
    protected bool bufferedInput;
    protected Combo activeCombo;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mana = GetComponent<Mana>();
        health = GetComponent<Health>();

        // TEMPORARY
        primaryTalents = new Combo[] { ScriptableObject.CreateInstance<Combo>() };
        primaryTalents[0].ComboTalents.Add(ScriptableObject.CreateInstance<Slash>());
        primaryTalents[0].ComboTalents.Add(ScriptableObject.CreateInstance<Slash>());
        primaryTalents[0].ComboTalents.Add(ScriptableObject.CreateInstance<Impel>());
        foreach (Talent talent in primaryTalents[0].ComboTalents)
        {
            talent.Initialize(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeCombo != null && activeCombo.activeTalent != null)
        {
            playerTalentStatus = activeCombo.activeTalent.TalentStatus;
        }
        animator.SetInteger("talentStatus", playerTalentStatus);
        if (bufferedInput)
        {
            if (playerTalentStatus == (int)TalentStatus.Inactive ||
            playerTalentStatus == (int)TalentStatus.Combo)
            {
                TryCastTalent(activeCombo);
                bufferedInput = false;
            }
        }
    }

    // Buffer incoming input to cast a Talent
    public void BufferInput(int talent, bool secondary)
    {
        if (playerTalentStatus != (int)TalentStatus.Telegraphing)
        {
            bufferedInput = true;
            activeCombo = (!secondary ? primaryTalents[talent] : secondaryTalents[talent]);
        }
    }

    // Check if all prerequisites for casting this Talent are met
    bool CanCastTalent(Combo combo)
    {
        return combo.ComboTalents[combo.Current].ManaCost <=
            (combo.ComboTalents[combo.Current].CostsHealth ? health._health : mana._mana);
    }

    // Attempt to cast a specific Talent
    public void TryCastTalent(Combo combo)
    {
        if (CanCastTalent(combo))
        {
            Talent t = combo.ComboCast();
            animator.SetInteger("castAnimation", !combo.IsRepeating() ? t.CastAnimation.alt : t.CastAnimation.normal);
            mana.ModifyMana(-t.ManaCost, t.CostsHealth);
        }
    }
}
