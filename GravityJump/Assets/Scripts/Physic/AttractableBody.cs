using System;
using System.Collections;
using UnityEngine;

namespace Physic
{
    // This script is responsible for computing physics on the gameObject it is attached to.
    // It will provides action methods that can be used to apply physical effects on the body (example: add a force to throw it away)
    // It has a playerMovingState that can be used to get information on the current behavior of the body (example: jumping, moving, ...)
    public class AttractableBody : PhysicBody
    {
        private Transform groundedCheck;
        private LayerMask groundMask;
        private Collider2D attractableBodyCollider;

        public AttractiveBody closestAttractiveBody { get; set; }

        // Physics constants
        protected float runSpeed = 2.7f * Data.Storage.SpeedFactor;
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
            this.groundMask = LayerMask.GetMask("Planetoid");
            this.attractableBodyCollider = GetComponent<Collider2D>();
            this.spriteRenderer = GetComponent<SpriteRenderer>();
            this.playerMovingState = new PlayerMovingState();
        }

        protected void FixedUpdate()
        {
            this.UpdateClosestAttractiveBody();

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

        private void UpdateClosestAttractiveBody()
        {
            float closestDistance = Mathf.Infinity;
            GameObject[] attractiveBodyGameObjects = GameObject.FindGameObjectsWithTag("AttractiveBody");
            foreach (GameObject attractiveBodyGameObject in attractiveBodyGameObjects)
            {
                AttractiveBody attractiveBody = attractiveBodyGameObject.GetComponent<AttractiveBody>();
                float distance = attractiveBody.ground.Distance(this.attractableBodyCollider).distance;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    this.closestAttractiveBody = attractiveBody;
                }
            }
        }

        // Actions
        // These methods can be called to apply actions on the body (ex: to apply a force)

        public void Walk(float speed)
        {
            this.horizontalSpeed = speed;
            if (Math.Abs(speed) > 0)
            {
                this.playerMovingState.Walk();
            }
            else
            {
                this.playerMovingState.Stop();
            }
        }

        public void Throw(Vector2 force)
        {
            this.rb2D.AddForce(force);
            this.playerMovingState.Throw();
        }
    }
}
