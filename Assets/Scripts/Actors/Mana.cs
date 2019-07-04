using System.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Requisite scripts
    private Health health;

    // Numeric vars
    private float mana;
    public float _mana { get => mana; }
    private float manaMax;
    private float manaRegenRate; // per second
    private float manaRegenDelay; // in seconds

    // Status vars
    private bool manaIsRegenerating;

    private void Awake()
    {
        manaMax = 100;
        manaRegenRate = 25;
        mana = manaMax;
        manaRegenDelay = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manaIsRegenerating)
        {
            ModifyMana(manaRegenRate * Time.deltaTime, false);
        }
    }

    // Change the current mana amount, or take mana cost from health
    public void ModifyMana(float manaChange, bool costsHealth)
    {
        if (!costsHealth)
        {
            mana += manaChange;
        } else
        {
            health.ChangeHealth(-manaChange);
        }

        if (mana >= manaMax)
        {
            StopManaRegen();
            mana = Mathf.Clamp(mana, 0, manaMax);
        } else if (mana < manaMax && manaChange < 0)
        {
            StartCoroutine(StartManaRegen());
        }
    }

    // Stop regeneration, then start mana regeneration after a delay
    private IEnumerator StartManaRegen()
    {
        manaIsRegenerating = false;
        yield return new WaitForSeconds(manaRegenDelay);
        manaIsRegenerating = true;
    }

    // Stop regenerating mana
    public void StopManaRegen()
    {
        manaIsRegenerating = false;
    }
}
