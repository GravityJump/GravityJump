using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Transform groundedCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Collider2D closestPlanetoid;
    [SerializeField] private Collider2D characterCollider;
    [SerializeField] private float runSpeed = 7;

    const float groundedRadius = 0.2f;
    private float jumpForce = 10f;
    private float gravityForce = 10f;
    private float minGravitySpeedLimit = -10f;

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    protected float horizontalSpeed;
    private float verticalSpeed;
    protected bool jump;
    private bool isGrounded;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                    Debug.Log("Character has grounded");
            }
        }
        if (wasGrounded && !isGrounded)
        {
            Debug.Log("Character has taken off");
        }

        if (horizontalSpeed > 0.01f)
            spriteRenderer.flipX = false;
        else if (horizontalSpeed < -0.01f)
            spriteRenderer.flipX = true;

        Move(horizontalSpeed, jump, Time.fixedDeltaTime);
    }

    protected void Move(float move, bool jump, float time)
    {
        ColliderDistance2D characterPlanetoidDistance = characterCollider.Distance(closestPlanetoid);
        Vector2 groundNormal = characterPlanetoidDistance.normal.normalized;

        if (jump)
        {
            if (isGrounded)
            {
                verticalSpeed = 0;
                // Cancel gravity speed modifier and impulse force to jump
                rb2D.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
        else
        {
            // Reset velocity to avoid having remaining jump forces applied after jump has stopped
            rb2D.velocity = new Vector2();
        }
        // Add gravity acceleration every time. Limit max speed to avoid extreme behaviors.
        // We keep gravity acceleration after landed to stick the character to the ground.
        verticalSpeed = Mathf.Max(verticalSpeed - rb2D.mass * gravityForce * time, minGravitySpeedLimit);

        transform.up = groundNormal;

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 horizontalMove = move * runSpeed * moveAlongGround * time;
        Vector2 verticalMove = verticalSpeed * groundNormal * time;

        rb2D.position = rb2D.position + horizontalMove + verticalMove;
    }
}
