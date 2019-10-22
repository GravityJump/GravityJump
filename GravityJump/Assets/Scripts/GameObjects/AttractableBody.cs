using System;
using System.Collections;
using UnityEngine;

public abstract class AttractableBody : PhysicBody
{
    [SerializeField] private Transform groundedCheck;
    [SerializeField] private LayerMask groundMask;
    // This should be a collider slightly below the ground collider, to keep the normal upward.
    [SerializeField] private Collider2D attractableBodyCollider;
    [SerializeField] public AttractiveBody closestAttractiveBody;
    [SerializeField] public AttractiveBody currentAttractiveBody;
    [SerializeField] protected float runSpeed = 7f;
    [SerializeField] protected float jumpForce = 10f;

    // Physics constants
    protected float groundedRadius = 0.1f;
    protected float landingDelay = 0.2f;
    protected float inertiaForce = 0.8f;
    protected float gravityForce = 10f;
    protected float minGravitySpeedLimit = -10f;

    // State variables
    protected bool isGrounded;
    protected JumpState jump;
    protected float horizontalInertia;
    protected float horizontalSpeed;

    protected void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        int defaultLayerMask = LayerMask.GetMask("Planetoid", "Character");
        if (groundMask.value != defaultLayerMask)
        {
            Debug.LogWarning($"groundMask for attractableBody {this.name} is different from the default layer mask: {defaultLayerMask}");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (
            collision.gameObject.layer == LayerMask.NameToLayer("Orbit")
        )
        {
            AttractiveBody collisionAttractiveBody = collision.gameObject.transform.parent.gameObject.GetComponent<AttractiveBody>();
            if (
                (jump == JumpState.Jumping || jump == JumpState.InFlight)
                && collisionAttractiveBody.id != currentAttractiveBody.id
                && closestAttractiveBody.id == currentAttractiveBody.id
            )
            {
                closestAttractiveBody = collisionAttractiveBody;
                rb2D.velocity = new Vector2(0, 0);
            }
        }
    }

    protected void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundedCheck.position, groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    switch (colliders[i].gameObject.layer)
                    {
                        case 8:
                            // Planetoid
                            StartCoroutine("Land");
                            currentAttractiveBody = colliders[i].gameObject.transform.parent.gameObject.GetComponent<AttractiveBody>();
                            break;
                        case 9:
                            // Player
                            Bounce();
                            break;
                    }
                }
            }
        }
        if (!isGrounded)
        {
            TakeOff();
        }

        if (horizontalSpeed > 0.01f)
            spriteRenderer.flipX = false;
        else if (horizontalSpeed < -0.01f)
            spriteRenderer.flipX = true;

        Move(horizontalSpeed, jump, Time.fixedDeltaTime);
    }

    protected void Move(float move, JumpState jump, float time)
    {
        ColliderDistance2D attractableToAttractiveBodyNormalDistance = attractableBodyCollider.Distance(closestAttractiveBody.normalShape);
        Vector2 groundNormal = attractableToAttractiveBodyNormalDistance.normal.normalized;
        float groundToNormalDistance = -closestAttractiveBody.getDistanceBetweenNormalAndGround().distance;

        if (jump == JumpState.Jumping)
        {
            rb2D.velocity = new Vector2();
            // Cancel gravity speed modifier and impulse force to jump
            rb2D.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else
        {
            rb2D.AddRelativeForce(new Vector2(0, -rb2D.mass * gravityForce));
        }
        // Add gravity acceleration every time. Limit max speed to avoid extreme behaviors.
        // We keep gravity acceleration after landing to stick the attractable body to the ground.
        if (transform.InverseTransformVector(rb2D.velocity).y < 0.1)
        {
            Fall();
        }

        if (isGrounded)
        {
            transform.up = groundNormal;
        }
        else
        {
            transform.up = transform.up + ((Vector3)groundNormal - transform.up) * time * 10;
        }

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        float horizontalMove = move * runSpeed * time * groundToNormalDistance / (groundToNormalDistance + attractableToAttractiveBodyNormalDistance.distance);
        Vector2 horizontalPositionMove = ((1 - inertiaForce) * horizontalMove + inertiaForce * horizontalInertia) * moveAlongGround;

        rb2D.position += horizontalPositionMove;
        horizontalInertia = Math.Abs(inertiaForce * horizontalInertia + (1 - inertiaForce) * horizontalMove) > 0.01f ? inertiaForce * horizontalInertia + (1 - inertiaForce) * horizontalMove : 0;
    }

    public enum JumpState
    {
        Grounded,
        Jumping,
        InFlight,
        Falling,
        Landing
    }

    protected void Jump()
    {
        if (jump == JumpState.Grounded)
        {
            jump = JumpState.Jumping;
        }
    }

    protected void Bounce()
    {
        if (jump == JumpState.Falling)
        {
            jump = JumpState.Jumping;
        }
    }

    protected void TakeOff()
    {
        if (jump == JumpState.Jumping && !isGrounded)
        {
            jump = JumpState.InFlight;
        }
    }

    protected void Fall()
    {
        if (jump == JumpState.InFlight)
        {
            jump = JumpState.Falling;
        }
    }

    // Use this function as a Coroutine: StartCoroutine("Land");
    protected IEnumerator Land()
    {
        if (jump == JumpState.Falling)
        {
            jump = JumpState.Landing;
            yield return new WaitForSeconds(landingDelay);
            jump = JumpState.Grounded;
        }
    }
}
