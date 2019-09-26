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
    [SerializeField] private float jumpSpeed = 7;

    const float groundedRadius = 0.2f;
    private float gravityStrengh = 50.0f;
    private float jumpForce = 100f;

    private Rigidbody2D rb2D;
    protected float horizontalSpeed;
    protected bool jump;
    private bool isGrounded;
    // For velocity smooth damp
    private float smoothTime = .05f;
    private Vector3 acceleration = Vector3.zero;


    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
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

        Move(horizontalSpeed, jump, Time.fixedDeltaTime);
    }

    protected void Move(float move, bool jump, float time)
    {
        float verticalVelocity = -10f;
        ColliderDistance2D characterPlanetoidDistance = characterCollider.Distance(closestPlanetoid);
        Vector2 groundNormal = characterPlanetoidDistance.normal.normalized;

        if (jump)
        {
            verticalVelocity = 0;
            if (isGrounded)
            {
                rb2D.AddRelativeForce(new Vector2(0, 10), ForceMode2D.Impulse);
            }
        }
        else
        {
            // Reset velocity to avoid having remaining jump forces applied after jump has stopped
            rb2D.velocity = new Vector2();
        }

        transform.up = groundNormal;

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 horizontalMove = move * runSpeed * moveAlongGround * time;
        Vector2 verticalMove = verticalVelocity * groundNormal * time;

        rb2D.position = rb2D.position + horizontalMove + verticalMove;
    }
}
