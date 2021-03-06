﻿using System.Collections;
using UnityEngine;

namespace Physic
{
    // This class is the model representing the current move state of a player. It can be walking, jumping, falling,...
    // States are described in the MovingState enum.
    // Use status methods to get pre-processed information on the current state (example: is it jumping ?)
    // Use action methods to update the moving state.
    public class PlayerMovingState
    {
        private float jumpingDelay = 0.1f;
        private float landingDelay = 0.2f;
        public MovingState movingState { get; private set; }

        public PlayerMovingState()
        {
            this.movingState = MovingState.Idle;
        }

        public enum MovingState
        {
            Idle,
            Walking,
            Jumping,
            InFlight,
            Falling,
            Landing
        }

        // Status methods

        public bool IsIdle()
        {
            return movingState == MovingState.Idle;
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
            return movingState == MovingState.Idle || movingState == MovingState.Walking || movingState == MovingState.Landing;
        }

        public bool CanMoveHorizontally()
        {
            return movingState != MovingState.Jumping && movingState != MovingState.Landing;
        }

        // Action methods

        public void Walk()
        {
            if (this.movingState == MovingState.Idle)
            {
                this.movingState = MovingState.Walking;
            }
        }

        public void Stop()
        {
            if (this.movingState == MovingState.Walking)
            {
                this.movingState = MovingState.Idle;
            }
        }

        public IEnumerator Jump()
        {
            if (this.movingState == MovingState.Idle || this.movingState == MovingState.Walking)
            {
                this.movingState = MovingState.Jumping;
                yield return new WaitForSeconds(jumpingDelay);
                this.TakeOff();
            }
        }

        public void Throw()
        {
            this.movingState = MovingState.InFlight;
        }

        public void TakeOff()
        {
            if (this.movingState == MovingState.Jumping)
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
            if (this.movingState == MovingState.InFlight || this.movingState == MovingState.Falling)
            {
                this.movingState = MovingState.Landing;
                yield return new WaitForSeconds(landingDelay);
                this.movingState = MovingState.Idle;
            }
        }
    }
}
