using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Transform groundedCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Collider2D closestPlanetoid;
    [SerializeField] private Collider2D characterCollider;

    const float groundedRadius = 0.2f;
    private float gravityStrengh = 50.0f;
    private float jumpForce = 100f;

    private Rigidbody2D rb2D;
    protected float horizontalMove;
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

        Move(horizontalMove, jump, Time.fixedDeltaTime);
        jump = false;
    }

    protected void Move(float move, bool jump, float time)
    {
        ColliderDistance2D characterPlanetoidDistance = characterCollider.Distance(closestPlanetoid);
        if (!isGrounded)
        {
            rb2D.AddForce(-characterPlanetoidDistance.normal * gravityStrengh);
        }
        //transform.Rotate(new Vector3(0, 0, -Vector3.Angle(transform.up, characterPlanetoidDistance.normal)));
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, characterPlanetoidDistance.normal), 0.1f);
        transform.up = characterPlanetoidDistance.normal.normalized;

        if (isGrounded && jump)
        {
            rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * time * 10f, rb2D.velocity.y);
        // And then smoothing it out and applying it to the character
        rb2D.velocity = transform.TransformVector(Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref acceleration, smoothTime));
    }
}
