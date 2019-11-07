using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : Animator
    {
        private Physic.AttractableBody AttractableBody;
        // This struct links animation types to their name in Animations dictionary (and folder name in file system)
        private struct AnimationTypes
        {
            public const string Idle = "Idle";
            public const string Jump = "Jump";
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
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Idle][0];
                    break;
                case Physic.PlayerMovingState.MovingState.Jumping:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump][1];
                    break;
                case Physic.PlayerMovingState.MovingState.InFlight:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump][2];
                    break;
                case Physic.PlayerMovingState.MovingState.Falling:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump][3];
                    break;
                case Physic.PlayerMovingState.MovingState.Landing:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump][1];
                    break;
            }
        }
    }
}
