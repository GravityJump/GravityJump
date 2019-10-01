using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttractableBody : Body
{
    [SerializeField] private Transform groundedCheck;
    [SerializeField] private LayerMask groundMask;
    // This should be a collider slightly below the ground collider, to keep the normal upward.
    [SerializeField] private AttractiveBody closestAttractiveBody;
    [SerializeField] private AttractiveBody currentAttractiveBody;
    [SerializeField] private Collider2D attractableBodyCollider;
    [SerializeField] protected float runSpeed = 7f;
    [SerializeField] protected float jumpForce = 10f;

    protected bool isGrounded;
    protected float groundedRadius = 0.1f;
    protected float minGravitySpeedLimit = -10f;

    protected float landedToGroundedDelay = 0.2f;
    protected float gravityForce = 10f;

    protected JumpState jump;
    protected float horizontalSpeed;
    private float verticalSpeed;

    protected void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (
            collision.gameObject.layer == 10
            && jump == JumpState.Jumping
            && !ReferenceEquals(collision.gameObject, currentAttractiveBody.orbit)
            && ReferenceEquals(currentAttractiveBody, closestAttractiveBody)
        )
        {
            closestAttractiveBody = (AttractiveBody)collision.gameObject.transform.parent.gameObject.GetComponent("AttractiveBody");
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
                    StartCoroutine("Land");
                    currentAttractiveBody = (AttractiveBody)colliders[i].gameObject.transform.parent.gameObject.GetComponent("AttractiveBody");
                }
            }
        }

        if (horizontalSpeed > 0.01f)
            spriteRenderer.flipX = false;
        else if (horizontalSpeed < -0.01f)
            spriteRenderer.flipX = true;

        Move(horizontalSpeed, jump, Time.fixedDeltaTime);
    }

    protected void Move(float move, JumpState jump, float time)
    {
        ColliderDistance2D attractableToAttractiveBodyDistance = attractableBodyCollider.Distance(closestAttractiveBody.normalShape);
        Vector2 groundNormal = attractableToAttractiveBodyDistance.normal.normalized;

        if (jump == JumpState.Jumping)
        {
            if (isGrounded)
            {
                verticalSpeed = 0;
                // Cancel gravity speed modifier and impulse force to jump
                rb2D.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
        else if (jump == JumpState.Landed)
        {
            // Reset velocity to avoid having remaining jump forces applied after jump has stopped
            rb2D.velocity = new Vector2();
        }
        // Add gravity acceleration every time. Limit max speed to avoid extreme behaviors.
        // We keep gravity acceleration after landed to stick the attractable body to the ground.
        verticalSpeed = Mathf.Abs(move) > 0.1 || !isGrounded ? Mathf.Max(verticalSpeed - rb2D.mass * gravityForce * time, minGravitySpeedLimit) : 0;
        if (verticalSpeed < 0.1)
        {
            StopJumping();
        }

        transform.up = groundNormal;

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 horizontalMove = move * runSpeed * moveAlongGround * time;
        Vector2 verticalMove = verticalSpeed * groundNormal * time;

        rb2D.position = rb2D.position + horizontalMove + verticalMove;
    }

    public enum JumpState
    {
        Grounded,
        Jumping,
        InFlight,
        Landed,
        Disabled
    }

    protected void Jump()
    {
        if (jump == JumpState.Grounded)
        {
            jump = JumpState.Jumping;
        }
    }

    protected void StopJumping()
    {
        if (jump == JumpState.Jumping && !isGrounded)
        {
            jump = JumpState.InFlight;
        }
    }

    // Use this function as a Coroutine: StartCoroutine("Land");
    protected IEnumerator Land()
    {
        if (jump == JumpState.InFlight)
        {
            jump = JumpState.Landed;
        }
        yield return new WaitForSeconds(landedToGroundedDelay);
        jump = JumpState.Grounded;
    }
}
