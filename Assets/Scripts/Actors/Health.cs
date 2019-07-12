// Health used by actors
using UnityEngine;

public class Health : MonoBehaviour
{
    // Events
    public event System.Action OnDeathEvent;

    // Numeric fields
    private float health;
    public float _health { get => health; set => health = value; }
    public float HealthMax;

    private GameObject healthBar;
    private GameObject healthBarContainer;
    RectTransform healthBarTransform;

    void Awake()
    {
        if (HealthMax <= 0)
        {
            HealthMax = 100f;
        }
        health = HealthMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag != "Player")
        {
            healthBarContainer = gameObject.transform.Find("HealthBarContainer").gameObject;
            healthBar = gameObject.transform.Find("HealthBarContainer/HealthBar").gameObject;
            healthBarTransform = healthBar.GetComponent<RectTransform>();
        }
    }

    // Public health modifier function
    public void ChangeHealth(float healthChange)
    {
        health += healthChange;
        PopupManager.instance.HealthChangePopup(healthChange, transform);
        health = Mathf.Clamp(health, 0, HealthMax);

        if (healthBar != null)
        {
            healthBarContainer.SetActive(true);
            healthBarTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (health / HealthMax));
        }

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
