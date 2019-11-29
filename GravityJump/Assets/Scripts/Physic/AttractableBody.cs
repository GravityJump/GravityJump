using System.Collections;
using UnityEngine;

namespace Physic
{
    /**
     * AttractableBody is responsible for computing physics on the local player gameObject it is attached to.
     * Physics depend on PlayerMovingState, game environment and triggered actions (by user inputs).
     * It manages the following:
     * - Local gravity
     * - Orbit switching (when jumping from one planet to another)
     * - Walking and sprinting
     * - Jumping
     * 
     * AttractableBody provides action methods that can be used to apply physical effects on the body (example: add a force to throw it away).
     * It also triggers updates on PlayerMovingState.
    **/
    public class AttractableBody : PhysicBody
    {
        private Collider2D attractableBodyCollider;
        private TrailRenderer trailRenderer;

        // Physics values and constants
        public GameSpeed GameSpeed { get; set; }
        private float WalkSpeed => 2.7f * GameSpeed.PlayerSpeed;
        private const float jumpForce = 10f;
        private const float groundedDistance = 0.1f;
        private const float inertiaForce = 0.6f;
        private const float gravityForce = 10f;

        // State variables
        public AttractiveBody ClosestAttractiveBody { get; set; }
        public PlayerMovingState PlayerMovingState { get; private set; }
        public float HorizontalSpeed { get; private set; }
        protected float horizontalInertia;
        private bool isSprintAvailable;

        protected void Awake()
        {
            this.rb2D = GetComponent<Rigidbody2D>();
            this.attractableBodyCollider = GetComponent<Collider2D>();
            this.trailRenderer = GetComponent<TrailRenderer>();
            this.PlayerMovingState = new PlayerMovingState();
            this.isSprintAvailable = true;
        }

        protected void FixedUpdate()
        {
            this.UpdateClosestAttractiveBody();

            this.CheckGround();

            this.Move(HorizontalSpeed, Time.fixedDeltaTime);
        }

        // Move is responsible for moving the body vertically (jump force and gravity) and horizontally (walk and jump)
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
                float horizontalMove = horizontalMoveValue * time * groundToNormalDistance / (groundToNormalDistance + Mathf.Sqrt(attractableToAttractiveBodyNormalDistance.distance));
                Vector2 horizontalPositionMove = ((1 - inertiaForce) * horizontalMove + inertiaForce * this.horizontalInertia) * moveAlongGround;

                this.rb2D.position += horizontalPositionMove;

                float newHorizontalInertia = inertiaForce * this.horizontalInertia + (1 - inertiaForce) * horizontalMove;
                this.horizontalInertia = Mathf.Abs(newHorizontalInertia) > 0.01f ? newHorizontalInertia : 0;
            }
        }

        // UpdateClosestAttractiveBody is responsible for finding and updating the closestAttractiveBody.
        // It uses the distance between the player gameObject collider and the AttractiveBody ground colliders.
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

        // CheckGround is responsible for checking if the player has reached the ground, and calling the Land action.
        private void CheckGround()
        {
            ColliderDistance2D attractableBodyToAttractiveBodyGroundDistance = this.attractableBodyCollider.Distance(this.ClosestAttractiveBody.ground);
            if (attractableBodyToAttractiveBodyGroundDistance.distance < groundedDistance)
            {
                StartCoroutine(this.PlayerMovingState.Land());
            }
        }

        /**
         * Actions
        **/

        // These methods can be called to apply actions on the body (ex: to apply a force)

        // Walk is an action that can be called with user keyboard inputs to move the player horizontally.
        // walkingDirection must be a value im {-1, 0 ,1} indicating to which direction the player is moving (0 if idle).
        public void Walk(float walkingDirection)
        {
            this.HorizontalSpeed = walkingDirection * this.WalkSpeed;
            if (Mathf.Abs(walkingDirection) > 0)
            {
                this.PlayerMovingState.Walk();
            }
            else
            {
                this.PlayerMovingState.Stop();
            }
        }

        // Jump is an action that can be called to make the player jump.
        public void Jump()
        {
            StartCoroutine(this.PlayerMovingState.Jump());
        }

        // Throw is an action that can be called to push the player forward.
        public void Throw(Vector2 force)
        {
            this.rb2D.AddForce(force);
            this.PlayerMovingState.Throw();
        }

        // Throw is an action that can be called as a Coroutine to make the player sprint for a while.
        // When time's up, the sprint stops and the player has to wait a bit before being able to sprint again.
        public IEnumerator Sprint()
        {
            if (this.isSprintAvailable)
            {
                // Start sprint
                this.isSprintAvailable = false;
                this.GameSpeed.PlayerSpeedFactor *= 2;
                this.trailRenderer.emitting = true;
                yield return new WaitForSeconds(3);
                // End sprint
                this.GameSpeed.PlayerSpeedFactor /= 2;
                this.trailRenderer.emitting = false;
                yield return new WaitForSeconds(5);
                // Restore sprint
                this.gameObject.GetComponent<ParticleSystem>().Play();
                this.isSprintAvailable = true;
            }
        }
    }
}
