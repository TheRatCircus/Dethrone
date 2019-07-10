// Health used by actors
using UnityEngine;

public class Health : MonoBehaviour
{
    // Events
    public event System.Action<float, Transform> OnHealthChangeEvent;
    public event System.Action OnDeathEvent;

    // Numeric fields
    private float health;
    public float _health { get => health; set => health = value; }
    public float HealthMax;

    // Start is called before the first frame update
    void Start()
    {
        if (HealthMax <= 0)
        {
            HealthMax = 100f;
        }
        health = HealthMax;
    }

    // Public health modifier function
    public void ChangeHealth(float healthChange)
    {
        health += healthChange;
        OnHealthChangeEvent?.Invoke(healthChange, transform);
        health = Mathf.Clamp(health, 0, HealthMax);
        CheckIfDead();
    }

    // Check if entity is dead. Call every time health is modified
    private void CheckIfDead()
    {
        if (health <= 0)
        {
            OnDeathEvent?.Invoke();
        }
    }
}
