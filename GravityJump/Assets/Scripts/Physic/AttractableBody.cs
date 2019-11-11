using System;
using UnityEngine;

namespace Physic
{
    // This script is responsible for computing physics on the gameObject it is attached to.
    // It will provides action methods that can be used to apply physical effects on the body (example: add a force to throw it away)
    // It has a PlayerMovingState that can be used to get information on the current behavior of the body (example: jumping, moving, ...)
    public class AttractableBody : PhysicBody
    {
        private Collider2D attractableBodyCollider;

        // Physics values and constants
        private readonly float walkSpeed = 2.7f * Data.Storage.PlayerSpeed;
        private const float jumpForce = 10f;
        private const float groundedDistance = 0.1f;
        private const float inertiaForce = 0.8f;
        private const float gravityForce = 10f;

        // State variables
        public AttractiveBody ClosestAttractiveBody { get; set; }
        public PlayerMovingState PlayerMovingState { get; private set; }
        public float HorizontalSpeed { get; private set; }
        protected float horizontalInertia;

        protected void Awake()
        {
            this.rb2D = GetComponent<Rigidbody2D>();
            this.attractableBodyCollider = GetComponent<Collider2D>();
            this.spriteRenderer = GetComponent<SpriteRenderer>();
            this.PlayerMovingState = new PlayerMovingState();
        }

        protected void FixedUpdate()
        {
            this.UpdateClosestAttractiveBody();

            this.CheckGround();

            this.Move(HorizontalSpeed, Time.fixedDeltaTime);
        }

        // Function responsible for moving the body vertically (jump force and gravity) and horizontally (walk)
        protected void Move(float horizontalMoveValue, float time)
        {
            ColliderDistance2D attractableToAttractiveBodyNormalDistance = attractableBodyCollider.Distance(ClosestAttractiveBody.normalShape);
            Vector2 groundNormal = attractableToAttractiveBodyNormalDistance.normal.normalized;
            float groundToNormalDistance = -ClosestAttractiveBody.getDistanceBetweenNormalAndGround().distance;

            if (this.PlayerMovingState.IsOnGround())
            {
                this.transform.up = groundNormal;
            }
            else
            {
                this.transform.up = this.transform.up + ((Vector3)groundNormal - this.transform.up) * time * 10;
            }

            if (this.PlayerMovingState.IsJumping())
            {
                this.rb2D.velocity = new Vector2();
                // Cancel gravity speed modifier and impulse force to jump
                this.rb2D.AddRelativeForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                this.rb2D.AddRelativeForce(new Vector2(0, -this.rb2D.mass * gravityForce));
            }

            if (this.transform.InverseTransformVector(this.rb2D.velocity).y < 0.1)
            {
                this.PlayerMovingState.Fall();
            }

            if (this.PlayerMovingState.CanMoveHorizontally())
            {
                var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
                float horizontalMove = horizontalMoveValue * time * groundToNormalDistance / (groundToNormalDistance + attractableToAttractiveBodyNormalDistance.distance);
                Vector2 horizontalPositionMove = ((1 - inertiaForce) * horizontalMove + inertiaForce * this.horizontalInertia) * moveAlongGround;

                this.rb2D.position += horizontalPositionMove;

                float newHorizontalInertia = inertiaForce * this.horizontalInertia + (1 - inertiaForce) * horizontalMove;
                this.horizontalInertia = Math.Abs(newHorizontalInertia) > 0.01f ? newHorizontalInertia : 0;
            }
        }

        // This method is responsible for finding and updating the closest AttractiveBody.
        // It uses the distance between this gameObject collider and the AttractiveBody ground colliders.
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
                    this.ClosestAttractiveBody = attractiveBody;
                }
            }
        }

        // This method is responsible for checking if the player has reached the ground, and calling the Land action.
        private void CheckGround()
        {
            ColliderDistance2D attractableBodyToAttractiveBodyGroundDistance = this.attractableBodyCollider.Distance(this.ClosestAttractiveBody.ground);
            if (attractableBodyToAttractiveBodyGroundDistance.distance < groundedDistance)
            {
                StartCoroutine(this.PlayerMovingState.Land());
            }
        }

        // Actions
        // These methods can be called to apply actions on the body (ex: to apply a force)

        // walkingDirection must be a value im {-1, 0 ,1} indicating to which direction the player is moving (0 if idle)
        public void Walk(float walkingDirection)
        {
            this.HorizontalSpeed = walkingDirection * this.walkSpeed;
            if (Math.Abs(walkingDirection) > 0)
            {
                this.PlayerMovingState.Walk();
            }
            else
            {
                this.PlayerMovingState.Stop();
            }
        }

        public void Jump()
        {
            StartCoroutine(this.PlayerMovingState.Jump());
        }

        public void Throw(Vector2 force)
        {
            this.rb2D.AddForce(force);
            this.PlayerMovingState.Throw();
        }
    }
}
