using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : Animator
    {
        private Physic.AttractableBody AttractableBody;
        // This enum represent animation types. They must be named after the animation name in the dictionary (and the folder name in file system)
        private enum AnimationTypes
        {
            Idle,
            Walk,
            Jump,
        }

        protected override string GameObjectAnimationsDirectoryName => "Player";

        private new void Awake()
        {
            AttractableBody = this.gameObject.GetComponent<Physic.AttractableBody>();
            base.Awake();
        }

        private void Update()
        {
            switch (AttractableBody.playerMovingState.movingState)
            {
                case Physic.PlayerMovingState.MovingState.Grounded:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Idle.ToString("g")][0];
                    break;
                case Physic.PlayerMovingState.MovingState.Jumping:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump.ToString("g")][1];
                    break;
                case Physic.PlayerMovingState.MovingState.InFlight:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump.ToString("g")][2];
                    break;
                case Physic.PlayerMovingState.MovingState.Falling:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump.ToString("g")][3];
                    break;
                case Physic.PlayerMovingState.MovingState.Landing:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump.ToString("g")][1];
                    break;
            }
        }
    }
}
