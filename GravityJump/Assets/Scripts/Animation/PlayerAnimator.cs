using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : Animator
    {
        private Physic.AttractableBody AttractableBody;
        // This enum represent animation types. They must be named after the animation name in the dictionary (and the folder name in file system)
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
        private const float SecondPerImage = 1/12;
        private float TimeSinceLastImage;
        private int currentFrameIndex;

        protected override string GameObjectAnimationsDirectoryName => "Player";

        private new void Awake()
        {
            AttractableBody = this.gameObject.GetComponent<Physic.AttractableBody>();
            base.Awake();
        }

        private void Update()
        {
            TimeSinceLastImage += Time.deltaTime;
            switch (AttractableBody.playerMovingState.movingState)
            {
                case Physic.PlayerMovingState.MovingState.Grounded:
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
        }

        private void PlayAnimation(AnimationType type)
        {
            Sprite[] animationSprites = Animations[type.ToString("g")];
            if (currentAnimationPlayed == type)
            {
                if (TimeSinceLastImage >= SecondPerImage)
                {
                    currentFrameIndex = (currentFrameIndex + 1) % animationSprites.Length;
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = animationSprites[currentFrameIndex];
                    TimeSinceLastImage = 0;
                }

            } else
            {
                currentAnimationPlayed = type;
                currentFrameIndex = 0;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = animationSprites[currentFrameIndex];
                TimeSinceLastImage = 0;
            }
        }
    }
}
