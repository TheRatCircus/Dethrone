using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LandMovementController : PhysicsObject
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D thisCollider;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private bool canMove;
    public bool CanMove { get => canMove; set => canMove = value; }
    private Vector2 move;
    private float moveInput;
    private bool isJumping;
    private bool ignoringPlatforms;

    LayerMask defaultLayerMask;
    LayerMask platformLayerMask;
    int emissionLayerMaskExclusion;
    int platformLayerMaskExclusion;

    protected RaycastHit2D[] stepBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> stepBufferList = new List<RaycastHit2D>(16);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<BoxCollider2D>();

        contactFilter.useTriggers = false;
        emissionLayerMaskExclusion = ~LayerMask.GetMask("Ignore Player", "Ignore NPCs");
        platformLayerMaskExclusion = ~LayerMask.GetMask("Platform");
        ResetLayerMask();

        canMove = true;
    }

    // Set the current horizontal move. Call every frame
    public void SetMove(float moveX) => moveInput = moveX;

    // Set if actor is going down through platforms
    public void SetIgnoringPlatforms(bool ignoringPlatforms) => this.ignoringPlatforms = ignoringPlatforms;

    // Set whether or not character is jumping. Call every frame
    public void SetJumping(bool isJumping)
    {
        this.isJumping = isJumping;
    }

    // Make a fixed-power jump (used by enemies)
    public IEnumerator ControlledJump()
    {
        isJumping = true;
        yield return null;
        isJumping = false;
    }

    // Change this actor's LayerMask to match a new layer. Call every time this
    // actor's layer is changed
    public void ResetLayerMask()
    {
        defaultLayerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        // Prevent actors from moving on collision with Talent emissions
        // while still allowing hit detection in Unity
        defaultLayerMask = defaultLayerMask & emissionLayerMaskExclusion;
        platformLayerMask = defaultLayerMask & platformLayerMaskExclusion;
    }

    // Calculate this actor's current velocity
    protected override void ComputeVelocity()
    {
        // Start move at zero, then feed input from external controller
        Vector2 move = Vector2.zero;
        if (canMove)
        {
            move.x = moveInput;
        }

        if (isJumping && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (isJumping)
        {
            if (velocity.y > 0)
            {
                velocity.y *= .5f;
            }
        }

        if (move.x > .01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < -.01f)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            // Do nothing
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    // Handle this actor's movement
    protected override void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        // Only check for collisions if moving
        if (distance > minMoveDistance)
        {
            // Check if overlapping with a platform
            bool overlappingWithPlatform = false;

            ContactFilter2D platformOverlapFilter = new ContactFilter2D();
            platformOverlapFilter.useTriggers = false;
            platformOverlapFilter.SetLayerMask((1 << 17));

            Collider2D[] platformCheckColliders = new Collider2D[4];

            Rect colliderRect = new Rect();
            colliderRect.min = thisCollider.bounds.min;
            colliderRect.max = thisCollider.bounds.max;

            thisCollider.OverlapCollider(platformOverlapFilter, platformCheckColliders);

            foreach (BoxCollider2D collider in platformCheckColliders)
            {
                if (collider != null)
                {
                    Rect platformRect = new Rect();
                    platformRect.min = collider.bounds.min;
                    platformRect.max = collider.bounds.max;
                    if (colliderRect.Overlaps(platformRect))
                    {
                        overlappingWithPlatform = true;
                        break;
                    }
                }
            }

            if (velocity.y > 0 || overlappingWithPlatform || ignoringPlatforms)
            {
                contactFilter.SetLayerMask(platformLayerMask);
            }
            else
            {
                contactFilter.SetLayerMask(defaultLayerMask);
            }

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
