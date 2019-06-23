using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMovementController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private Vector2 move;
    private bool isJumping;

    public GameObject crosshair;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D thisCollider;
    private PlayerInputHandler inputHandler;

    protected RaycastHit2D[] stepBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> stepBufferList = new List<RaycastHit2D>(16);

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider2D>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    public void SetMove(Vector2 move) => this.move = move;

    public void SetMove(float moveX, float moveY)
    {
        move.x = moveX;
        move.y = moveY;
    }

    public void SetJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }

    protected override void ComputeVelocity()
    {
        move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        // TODO: Double jumping
        //else if (Input.GetButtonDown("Jump") && !grounded && )
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = 
        (spriteRenderer.flipX ? (crosshair.transform.position.x > transform.position.x) : (crosshair.transform.position.x < transform.position.x));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    protected override void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        // Only check for collisions if moving
        if (distance > minMoveDistance)
        {
            int hitBufferCount = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            int StepBufferCount = rb2d.Cast(move.x != 0 && move.x > 0 ? Vector2.right : Vector2.left, contactFilter, stepBuffer, distance + shellRadius);

            hitBufferList.Clear();
            stepBufferList.Clear();

            for (int i = 0; i < hitBufferCount; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < StepBufferCount; i++)
            {
                stepBufferList.Add(stepBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        // If moving horizontally, handle stepping
        if (move.x != 0)
        {
            for (int i = 0; i < stepBufferList.Count; i++)
            {
                if (stepBufferList[i].collider.ClosestPoint(stepBufferList[i].centroid).y < stepBufferList[i].centroid.y
                    && stepBufferList[i].normal.y < minGroundNormalY)
                {
                    float centroidToClosest = stepBufferList[i].centroid.y - stepBufferList[i].collider.ClosestPoint(stepBufferList[i].centroid).y;
                    float stepDistance = thisCollider.bounds.size.y - centroidToClosest - thisCollider.bounds.extents.y;
                    transform.Translate(move.x != 0 && move.x > 0 ? 0.01f : -0.01f, stepDistance, 0, Space.Self);
                }
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}


