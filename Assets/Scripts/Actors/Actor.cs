// Generic actor class, holds common actor information and status variables
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Requisite objects
    protected ParticleSystem blood;

    // Status vars
    protected bool canCharacterAction;
    public bool CanCharacterAction { get => canCharacterAction; set => canCharacterAction = value; }
    protected bool isImmuneToHit;
    public bool IsImmuneToHit { get => isImmuneToHit; set => isImmuneToHit = value; }

    protected virtual void Start()
    {
        blood = transform.Find("Blood").GetComponent<ParticleSystem>();
    }

    // Called when this actor is hit by an AoE or projectile
    public virtual void OnHit(Vector2 hitSrcPos)
    {
        BloodSpray(hitSrcPos);
    }

    // Generate a spray of blood in the opposite direction to the incoming hit
    void BloodSpray(Vector2 hitSrcPos)
    {
        // Blood particle system needs an angle of -180 < x < 180
        Vector3 bloodSplatDir = new Vector3(
            Mathf.Atan2(hitSrcPos.y + transform.position.y,
            hitSrcPos.x + transform.position.x) * Mathf.Rad2Deg,
            90, -90);
        ParticleSystem.ShapeModule bloodShape = blood.shape;
        bloodShape.rotation = bloodSplatDir;
        blood.Play();
    }
}
