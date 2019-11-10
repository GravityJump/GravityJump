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
        private Collider2D attractableBodyCollider;

        public AttractiveBody closestAttractiveBody { get; set; }

        // Physics constants
        protected float runSpeed = 2.7f * Data.Storage.SpeedFactor;
        protected float jumpForce = 10f;
        protected float groundedDistance = 0.1f;
        protected float landingDelay = 0.2f;
        protected float inertiaForce = 0.8f;
        protected float gravityForce = 10f;
        protected float minGravitySpeedLimit = -10f;

        // State variables
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

        protected void Move(float move, float time)
        {
            ColliderDistance2D attractableToAttractiveBodyNormalDistance = attractableBodyCollider.Distance(closestAttractiveBody.normalShape);
            Vector2 groundNormal = attractableToAttractiveBodyNormalDistance.normal.normalized;
            float groundToNormalDistance = -closestAttractiveBody.getDistanceBetweenNormalAndGround().distance;

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
                this.rb2D.AddRelativeForce(new Vector2(0, this.jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                this.rb2D.AddRelativeForce(new Vector2(0, -this.rb2D.mass * this.gravityForce));
            }

            if (this.transform.InverseTransformVector(this.rb2D.velocity).y < 0.1)
            {
                this.PlayerMovingState.Fall();
            }

            if (this.PlayerMovingState.CanMoveHorizontally())
            {
                var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
                float horizontalMove = move * this.runSpeed * time * groundToNormalDistance / (groundToNormalDistance + attractableToAttractiveBodyNormalDistance.distance);
                Vector2 horizontalPositionMove = ((1 - this.inertiaForce) * horizontalMove + this.inertiaForce * this.horizontalInertia) * moveAlongGround;

                this.rb2D.position += horizontalPositionMove;

                float newHorizontalInertia = this.inertiaForce * this.horizontalInertia + (1 - this.inertiaForce) * horizontalMove;
                this.horizontalInertia = Math.Abs(newHorizontalInertia) > 0.01f ? newHorizontalInertia : 0;
            }
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

        private void CheckGround()
        {
            ColliderDistance2D attractableBodyToAttractiveBodyGroundDistance = this.attractableBodyCollider.Distance(this.closestAttractiveBody.ground);
            if (attractableBodyToAttractiveBodyGroundDistance.distance < this.groundedDistance)
            {
                StartCoroutine(this.PlayerMovingState.Land());
            }
        }

        // Actions
        // These methods can be called to apply actions on the body (ex: to apply a force)

        public void Walk(float speed)
        {
            this.HorizontalSpeed = speed;
            if (Math.Abs(speed) > 0)
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
