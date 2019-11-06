using System;
using System.Collections;
using UnityEngine;

namespace Physic
{
    public class AttractableBody : PhysicBody
    {
        private Transform groundedCheck;
        private LayerMask groundMask;
        // This should be a collider slightly below the ground collider, to keep the normal upward.
        private Collider2D attractableBodyCollider;

        public AttractiveBody closestAttractiveBody { get; set; }
        public AttractiveBody currentAttractiveBody { get; set; }

        // Physics constants
        protected float runSpeed = 7f;
        protected float jumpForce = 10f;
        protected float groundedRadius = 0.1f;
        protected float landingDelay = 0.2f;
        protected float inertiaForce = 0.8f;
        protected float gravityForce = 10f;
        protected float minGravitySpeedLimit = -10f;

        // State variables
        public PlayerMovingState playerMovingState { get; private set; }
        protected float horizontalInertia;
        public float horizontalSpeed { get; set; }

        protected void Awake()
        {
            this.rb2D = GetComponent<Rigidbody2D>();
            this.groundedCheck = this.gameObject.transform.Find("GroundedCheck");
            this.groundMask = LayerMask.GetMask("Planetoid", "Character");
            this.attractableBodyCollider = GetComponent<Collider2D>();
            this.spriteRenderer = GetComponent<SpriteRenderer>();
            this.playerMovingState = new PlayerMovingState();
        }
        
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (
                collision.gameObject.layer == LayerMask.NameToLayer("Orbit")
            )
            {
                AttractiveBody collisionAttractiveBody = collision.gameObject.transform.parent.gameObject.GetComponent<AttractiveBody>();
                if (
                    !playerMovingState.IsOnGround()
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
            bool wasGrounded = playerMovingState.isGrounded;
            playerMovingState.isGrounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundedCheck.position, groundedRadius, groundMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    playerMovingState.isGrounded = true;
                    if (!wasGrounded)
                    {
                        switch (colliders[i].gameObject.layer)
                        {
                            case 8:
                                // Planetoid
                                StartCoroutine(playerMovingState.Land());
                                currentAttractiveBody = colliders[i].gameObject.transform.parent.gameObject.GetComponent<AttractiveBody>();
                                break;
                        }
                    }
                }
            }
            if (!playerMovingState.isGrounded)
            {
                playerMovingState.TakeOff();
            }

            if (horizontalSpeed > 0.01f)
                spriteRenderer.flipX = false;
            else if (horizontalSpeed < -0.01f)
                spriteRenderer.flipX = true;

            Move(horizontalSpeed, Time.fixedDeltaTime);
        }

        protected void Move(float move, float time)
        {
            ColliderDistance2D attractableToAttractiveBodyNormalDistance = attractableBodyCollider.Distance(closestAttractiveBody.normalShape);
            Vector2 groundNormal = attractableToAttractiveBodyNormalDistance.normal.normalized;
            float groundToNormalDistance = -closestAttractiveBody.getDistanceBetweenNormalAndGround().distance;

            if (playerMovingState.IsJumping())
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
                playerMovingState.Fall();
            }

            if (playerMovingState.isGrounded)
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

        // Actions

        public void Throw(Vector2 force)
        {
            this.rb2D.AddForce(force);
            this.playerMovingState.Throw();
        }
    }
}
