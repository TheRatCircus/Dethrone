using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public PopupManager popupManager;

    private float health;
    public float _health { get => health; set => health = value; }
    public float healthMax;

    // Public health modifier function. Used to simplify calls.
    public void modHealth(float healthMod)
    {
        health += healthMod;
        if (healthMod < 0)
        {
            popupManager.DamagePopup(healthMod, transform);
        }
        health = Mathf.Clamp(health, 0, healthMax);
        if (health <= 0)
        {
            Die();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        popupManager = GameObject.Find("Managers").GetComponent<PopupManager>();
        health = healthMax;
    }

    private void Update()
    {

    }

    // Cause entity to die upon losing all health.
    public void Die()
    {
        Destroy(transform.gameObject);
    }

}
