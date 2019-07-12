// Controls the blink dodge
using System.Collections;
using UnityEngine;

public class DodgeController : MonoBehaviour
{
    // Requisite objects
    private Actor actor;
    private TargettingController targettingController;
    public LandMovementController movementController;
    private Collider2D thisCollider;
    private Rigidbody2D rb2d;
    private Mana mana;
    public ParticleSystem afterimage;

    private Vector2 dodgeDirection;
    private Vector2 dodgeDestination;
    private ContactFilter2D contactFilter;

    private float dodgeManaCost;
    // Status
    private bool isDodging;
    private bool isRecovering;
    private Vector2 currentVelocity;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        dodgeManaCost = 25f;
    }

    // Start is called before the first frame update
    private void Start()
    {
        targettingController = GetComponent<TargettingController>();
        thisCollider = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        mana = GetComponent<Mana>();

        if (transform.tag == "Player")
        {
            actor = GetComponent<PlayerCharacterController>();
        }
        contactFilter.useTriggers = false;
        
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Ignore Other Actors")));

        isDodging = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDodging)
        {
            rb2d.position += ((dodgeDestination - rb2d.position) * 3 * Time.deltaTime);
        }
    }

    // Set the direction in which the actor will dodge
    public void SetDodgeDirection((int dodgeX, int dodgeY) dodgeAxes)
    {
        if (!isDodging)
        {
            dodgeDirection.x = dodgeAxes.dodgeX;
            dodgeDirection.y = dodgeAxes.dodgeY;
        }

        SetDodgeDestination();
    }
    
    // Determine the destination to which the actor will dodge
    private void SetDodgeDestination()
    {
        if (!isDodging)
        {
            RaycastHit2D[] dodgeCollideBuffer = new RaycastHit2D[16];
            Ray2D ray = new Ray2D(transform.position, dodgeDirection);
            int collideBufferCount = thisCollider.Cast(dodgeDirection, contactFilter, dodgeCollideBuffer, 3.0f);

            if (collideBufferCount > 0)
            {
                dodgeDestination = ray.GetPoint(dodgeCollideBuffer[0].distance - 0.05f);
            }
            else
            {
                dodgeDestination = ray.GetPoint(dodgeDirection.y > 0 ? 4 : 3);
            }
        }
    }

    // Check if dodge is possible; called only by player controller
    public void TryDodge()
    {
        if (mana._mana >= dodgeManaCost)
        {
            mana.ModifyMana(-dodgeManaCost, false);
            StartCoroutine(Dodge());
        }
        else
        {
            Debug.Log("Insufficient mana to dodge.");
        }
    }

    // Finally carry out a dodge
    IEnumerator Dodge()
    {
        afterimage.Play();
        actor.CanCharacterAction = false;
        isDodging = true;
        actor.IsImmuneToHit = true;
        gameObject.layer = 14;
        movementController.ResetLayerMask();

        yield return new WaitForSeconds(0.5f);

        isDodging = false;
        actor.IsImmuneToHit = false;
        actor.CanCharacterAction = true;
        afterimage.Stop();
        gameObject.layer = 8;
        movementController.ResetLayerMask();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(dodgeDestination, .2f);
    }
}
