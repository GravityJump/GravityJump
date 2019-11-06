﻿using System.Collections;
using UnityEngine;

namespace Physic
{
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
            Jumping,
            InFlight,
            Falling,
            Landing
        }

        public void Jump()
        {
            if (this.movingState == MovingState.Grounded)
            {
                this.movingState = MovingState.Jumping;
            }
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
