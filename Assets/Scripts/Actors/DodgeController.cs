using System.Collections;
using UnityEngine;

public class DodgeController : MonoBehaviour
{
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
    //private int dodgeCharges;
    //private int dodgeChargesMax;
    //public int DodgeCharges { get => dodgeCharges; }
    //public int DodgeChargesMax { get => dodgeChargesMax; set => dodgeChargesMax = value; }

    private bool isDodging;
    private bool isRecovering;
    private Vector2 currentVelocity;

    private void Awake()
    {
        dodgeManaCost = 34f;
        //dodgeChargesMax = 3;
        //dodgeCharges = dodgeChargesMax;
    }

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
        else
        {
            actor = GetComponent<NPCController>();
        }

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(14));

        isDodging = false;
    }

    private void Update()
    {
        if (isDodging)
        {
            //transform.Translate(dodgeDirection * 4.0f * Time.deltaTime, Space.Self);
            rb2d.position = Vector2.SmoothDamp(transform.position, dodgeDestination, ref currentVelocity, 0.07f);
        }
    }

    public void SetDodgeDirection((int dodgeX, int dodgeY) dodgeAxes)
    {
        if (!isDodging)
        {
            dodgeDirection.x = dodgeAxes.dodgeX;
            dodgeDirection.y = dodgeAxes.dodgeY;
        }

        SetDodgeDestination();
    }

    private void SetDodgeDestination()
    {
        if (!isDodging)
        {
            RaycastHit2D[] dodgeCollideBuffer = new RaycastHit2D[16];
            Ray2D ray = new Ray2D(transform.position, dodgeDirection);
            int collideBufferCount = thisCollider.Cast(dodgeDirection, contactFilter, dodgeCollideBuffer, 3.0f);

            //StartCoroutine(Dodge());

            if (collideBufferCount > 0)
            {
                dodgeDestination = ray.GetPoint(dodgeCollideBuffer[0].distance - 0.05f);
            }
            else
            {
                dodgeDestination = ray.GetPoint(3.0f);
            }
        }
    }

    public void TryDodge()
    {
        //if (!collideHandler.GetIsColliding() && dodgeCharges > 0)
        //{
        //    GetComponent<Rigidbody2D>().position = dodgeCrosshair.transform.position;
        //    dodgeCharges--;
        //    if (!isRecovering)
        //    {
        //        StartCoroutine("RecoverCharges");
        //    }
        //}

        //RaycastHit2D[] dodgeCollideBuffer = new RaycastHit2D[16];
        //Ray2D ray = new Ray2D(transform.position, dodgeDirection);
        //int collideBufferCount = thisCollider.Cast(dodgeDirection, contactFilter, dodgeCollideBuffer, 3.0f);

        if (mana._mana >= dodgeManaCost)
        {
            mana.ModifyMana(-dodgeManaCost, false);
            StartCoroutine(Dodge());
        }
        else
        {
            Debug.Log("Insufficient mana to dodge.");
        }

        //if (collideBufferCount > 0)
        //{
        //    rb2d.position = ray.GetPoint(dodgeCollideBuffer[0].distance);
        //}
        //else
        //{
        //    rb2d.position = ray.GetPoint(3.0f);
        //}
    }

    IEnumerator Dodge()
    {
        afterimage.Play();
        actor.CanCharacterAction = false;
        isDodging = true;
        actor.IsImmuneToHit = true;
        gameObject.layer = 14;

        yield return new WaitForSeconds(0.5f);

        isDodging = false;
        actor.IsImmuneToHit = false;
        actor.CanCharacterAction = true;
        afterimage.Stop();
        gameObject.layer = 8;
    }

    //IEnumerator RecoverCharges()
    //{
    //    isRecovering = true;
    //    while (dodgeCharges < dodgeChargesMax)
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        dodgeCharges++;
    //    }
    //    isRecovering = false;
    //}
}
