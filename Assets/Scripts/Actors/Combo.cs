// A series of Talents which can be cast quickly
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo : ScriptableObject
{
    private List<Talent> comboTalents;
    public List<Talent> ComboTalents { get => comboTalents; }

    private int current;
    public int Current { get => current; }
    public Talent activeTalent;

    public void Awake()
    {
        comboTalents = new List<Talent>(4);
    }

    // Start or continue this combo
    public Talent ComboCast()
    {
        activeTalent = comboTalents[current];

        if (CanCast())
        {
            if (comboTalents[current].TalentStatus == (int)TalentStatus.Inactive)
            {
                CoroutineSingleton.instance.StartCoroutine(comboTalents[current].Cast(false, IsRepeating()));
            }
            else
            {
                CoroutineSingleton.instance.StartCoroutine(comboTalents[current].Cast(true, IsRepeating()));
            }
            
            // Reset combo if at end
            if (++current == comboTalents.Count)
            {
                current = 0;
            }
        }
        return activeTalent;
    }

    // Check if any Talents are ongoing
    bool CanCast()
    {
        bool canCast = true;
        foreach (Talent t in comboTalents)
        {
            if (t.TalentStatus != (int)TalentStatus.Inactive
                && t.TalentStatus != (int)TalentStatus.Combo)
            {
                canCast = false;
            }
        }
        return canCast;
    }

    // Check if a Talent is repeating itself
    public bool IsRepeating()
    {
        if (current < comboTalents.Count && current > 0)
        {
            var a = comboTalents[current].GetType();
            var b = comboTalents[current - 1].GetType();
            return a.Equals(b);
        }
        else
        {
            return false;
        }
    }
}
