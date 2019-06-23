using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    private Health health;

    private float mana;
    public float _mana { get => mana; }
    private float manaMax;
    private float manaRegenRate; // per second

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        manaMax = 100;
        manaRegenRate = 25;
        mana = manaMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (mana < manaMax)
        {
            mana += manaRegenRate * Time.deltaTime;
            mana = Mathf.Clamp(mana, 0, manaMax);
        }
    }

    //
    public void ConsumeMana(float manaCost, bool costsHealth)
    {
        if (!costsHealth)
        {
            mana -= manaCost;
        } else
        {
            health.modHealth(-manaCost);
        }
        
    }

}
