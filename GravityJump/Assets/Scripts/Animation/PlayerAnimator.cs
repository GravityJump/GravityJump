﻿using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : Animator
    {
        private Physic.AttractableBody AttractableBody;

        // Sprites for each animation
        public Sprite[] Idle;
        public Sprite[] Walking;
        public Sprite[] Jumping;
        public Sprite[] InFlight;
        public Sprite[] Falling;
        public Sprite[] Landing;

        // This enum represent animation types. They must be named after the animation name in the dictionary (and the folder name in file system).
        private enum AnimationType
        {
            Idle,
            Walking,
            Jumping,
            InFlight,
            Falling,
            Landing,
        }
        private AnimationType currentAnimationPlayed;
        protected override float SecondPerImage => 1 / 12f * 1 / this.AttractableBody.GameSpeed.PlayerSpeed;

        private new void Awake()
        {
            AttractableBody = this.gameObject.GetComponent<Physic.AttractableBody>();
            // Load Sprites
            this.Animations = new Dictionary<string, Sprite[]>
            {
                { "Idle", Idle },
                { "Walking", Walking },
                { "Jumping", Jumping },
                { "InFlight", InFlight },
                { "Falling", Falling },
                { "Landing", Landing }
            };
        }

        private void Update()
        {
            TimeSinceLastImage += Time.deltaTime;

            switch (AttractableBody.PlayerMovingState.movingState)
            {
                case Physic.PlayerMovingState.MovingState.Idle:
                    this.PlayAnimation(AnimationType.Idle);
                    break;
                case Physic.PlayerMovingState.MovingState.Walking:
                    this.PlayAnimation(AnimationType.Walking);
                    break;
                case Physic.PlayerMovingState.MovingState.Jumping:
                    this.PlayAnimation(AnimationType.Jumping);
                    break;
                case Physic.PlayerMovingState.MovingState.InFlight:
                    this.PlayAnimation(AnimationType.InFlight);
                    break;
                case Physic.PlayerMovingState.MovingState.Falling:
                    this.PlayAnimation(AnimationType.Falling);
                    break;
                case Physic.PlayerMovingState.MovingState.Landing:
                    this.PlayAnimation(AnimationType.Landing);
                    break;
            }

            if (this.AttractableBody.HorizontalSpeed > 0.01f)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            else if (this.AttractableBody.HorizontalSpeed < -0.01f)
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        private void PlayAnimation(AnimationType type)
        {
            Sprite[] animationSprites = Animations[type.ToString("g")];
            if (currentAnimationPlayed == type)
            {
                this.DisplayNextSprite(animationSprites);
            }
            else
            {
                // Set the animation to new type and display it from the beginning
                currentAnimationPlayed = type;
                this.DisplayFirstSprite(animationSprites);
            }
        }
    }
}
