// Generic actor class, holds common actor information and status variables
using UnityEngine;

public class Actor : MonoBehaviour
{
    public GameObject bloodSpray;
    public GameObject bloodSplat;
    public GameObject gibs;

    public AudioClip onHitSound;

    // Status vars
    protected bool canCharacterAction;
    public bool CanCharacterAction { get => canCharacterAction; set => canCharacterAction = value; }
    protected bool isImmuneToHit;
    public bool IsImmuneToHit { get => isImmuneToHit; set => isImmuneToHit = value; }

    Vector2 lastHitSrcPos;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GetComponent<Health>().OnDeathEvent += Die;
    }

    // Called when this actor is hit by an AoE or projectile
    public virtual void OnHit(Vector2 hitSrcPos)
    {
        lastHitSrcPos = hitSrcPos;
        AudioManager.instance.PlaySound(onHitSound, hitSrcPos);

        BloodSpray(hitSrcPos);
        BloodSplat(hitSrcPos);
    }

    // Generate a spray of blood in the opposite direction to the incoming hit
    void BloodSpray(Vector2 hitSrcPos)
    {
        Vector2 hitDirection = ((Vector2)transform.position - hitSrcPos).normalized;
        Destroy(Instantiate(bloodSpray, hitSrcPos, Quaternion.FromToRotation(Vector2.right, hitDirection)) as GameObject, 5);
    }

    // Generate a splatter of blood on the background wall if one exists
    void BloodSplat(Vector2 hitSrcPos)
    {
        if (GameObject.Find("Background Walls").GetComponent<Collider2D>().OverlapPoint(transform.position))
        {
            Destroy(Instantiate(bloodSplat, hitSrcPos, Quaternion.FromToRotation(hitSrcPos, transform.position)) as GameObject, 5);
        }
    }

    // Kill this actor
    void Die()
    {
        Destroy(Instantiate(gibs, lastHitSrcPos, Quaternion.FromToRotation(Vector2.right, lastHitSrcPos)) as GameObject, 5);
        Destroy(gameObject);
    }
}
