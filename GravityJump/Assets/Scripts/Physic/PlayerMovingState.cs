using System.Collections;
using UnityEngine;

namespace Physic
{
    // This class is the model representing the current move state of a player. It can be walking, jumping, falling,...
    // States are described in the MovingState enum.
    // Use status methods to get pre-processed information on the current state (example: is it jumping ?)
    // Use action methods to update the moving state.
    public class PlayerMovingState
    {
        private float landingDelay = 0.2f;
        public MovingState movingState { get; private set; }
        public bool isGrounded { get; set; }

        public PlayerMovingState()
        {
            this.movingState = MovingState.Grounded;
        }

        public enum MovingState
        {
            Grounded,
            Walking,
            Jumping,
            InFlight,
            Falling,
            Landing
        }

        // Status methods

        public bool IsGrounded()
        {
            return movingState == MovingState.Grounded;
        }


        public bool IsWalking()
        {
            return movingState == MovingState.Walking;
        }

        public bool IsJumping()
        {
            return movingState == MovingState.Jumping;
        }

        public bool IsInFlight()
        {
            return movingState == MovingState.InFlight;
        }

        public bool IsFalling()
        {
            return movingState == MovingState.Falling;
        }

        public bool IsLanding()
        {
            return movingState == MovingState.Landing;
        }

        public bool IsOnGround()
        {
            return movingState == MovingState.Grounded || movingState == MovingState.Walking || movingState == MovingState.Landing;
        }

        // Action methods

        public void Walk()
        {
            if (this.movingState == MovingState.Grounded)
            {
                this.movingState = MovingState.Walking;
            }
        }

        public void Stop()
        {
            if(this.movingState == MovingState.Walking)
            {
                this.movingState = MovingState.Grounded;
            }
        }

        public void Jump()
        {
            if (this.movingState == MovingState.Grounded || this.movingState == MovingState.Walking)
            {
                this.movingState = MovingState.Jumping;
            }
        }

        public void Throw()
        {
            this.movingState = MovingState.InFlight;
        }

        public void TakeOff()
        {
            if (this.movingState == MovingState.Jumping && !isGrounded)
            {
                this.movingState = MovingState.InFlight;
            }
        }

        public void Fall()
        {
            if (this.movingState == MovingState.InFlight)
            {
                this.movingState = MovingState.Falling;
            }
        }

        // Use this function as a Coroutine: StartCoroutine("Land");
        public IEnumerator Land()
        {
            if (this.movingState == MovingState.Falling)
            {
                this.movingState = MovingState.Landing;
                yield return new WaitForSeconds(landingDelay);
                this.movingState = MovingState.Grounded;
            }
        }
    }
}
