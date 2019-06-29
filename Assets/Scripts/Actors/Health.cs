// Health used by actors
using UnityEngine;

public class Health : MonoBehaviour
{
    // Requisite scripts
    public PopupManager popupManager;

    // Numeric fields
    private float health;
    public float _health { get => health; set => health = value; }
    public float HealthMax;
    private float healthPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        popupManager = GameObject.Find("Managers").GetComponent<PopupManager>();
        HealthMax = 100f;
        health = HealthMax;
    }

    void Update()
    {
        if (healthPerSecond != 0)
        {
            ModifyHealthOverTime();
        }
    }

    // Public health modifier function
    public void ModifyHealth(float healthMod)
    {
        health += healthMod;
        if (healthMod < 0)
        {
            popupManager.DamagePopup(healthMod, transform);
        }
        //health = Mathf.Clamp(health, 0, healthMax);
        CheckIfDead();
    }

    // Change health over time, call every frame
    private void ModifyHealthOverTime()
    {
        health += (healthPerSecond * Time.deltaTime);
        CheckIfDead();
    }

    // Change current rate of health change per second
    public void ModifyHealthPerSecond(float healthMod)
    {
        healthPerSecond += healthMod;
    }

    // Check if entity is dead. Call every time health is modified
    private void CheckIfDead()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    // Cause entity to die
    public void Die()
    {
        Destroy(transform.gameObject);
    }

}
