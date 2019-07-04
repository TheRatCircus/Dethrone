using System.Collections.Generic;
using UnityEngine;

public class LandMovementController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private Vector2 move;
    private float moveInput;
    private bool isJumping;

    private TargettingController targettingController;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D thisCollider;

    protected RaycastHit2D[] stepBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> stepBufferList = new List<RaycastHit2D>(16);

    void Start()
    {
        contactFilter.useTriggers = false;
        LayerMask defaultLayerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        // Prevent actors from moving on collision with Talent emissions
        defaultLayerMask = defaultLayerMask & ~((1 << 15) | (1 << 9));
        contactFilter.SetLayerMask(defaultLayerMask);

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider2D>();
        targettingController = GetComponent<TargettingController>();
    }

    // Set the current horizontal move. Call every frame
    public void SetMove(float moveX) => this.moveInput = moveX;

    // Set whether or not character is jumping. Call every frame
    public void SetJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }

    //
    protected override void ComputeVelocity()
    {
        // Start move at zero, then feed input from external controller
        Vector2 move = Vector2.zero;
        move.x = moveInput;

        if (isJumping && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (isJumping)
        {
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
            }
        }

        bool flipSprite = 
        (spriteRenderer.flipX ?
        (targettingController.GlobalPointer.x > transform.position.x) :
        (targettingController.GlobalPointer.x < transform.position.x));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    //
    protected override void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        // Only check for collisions if moving
        if (distance > minMoveDistance)
        {
            int hitBufferCount = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            int StepBufferCount = rb2d.Cast(
                move.x != 0 && move.x > 0 ? Vector2.right : Vector2.left,
                contactFilter, stepBuffer, distance + shellRadius);

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
        
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}


