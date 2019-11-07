using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : Animator
    {
        private Physic.AttractableBody AttractableBody;

        protected override string AnimationDirectoryPath => "Animation/Player/";

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
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations["Jump"][0];
                    break;
                case Physic.PlayerMovingState.MovingState.Jumping:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations["Jump"][1];
                    break;
                case Physic.PlayerMovingState.MovingState.InFlight:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations["Jump"][2];
                    break;
                case Physic.PlayerMovingState.MovingState.Falling:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations["Jump"][3];
                    break;
                case Physic.PlayerMovingState.MovingState.Landing:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations["Jump"][1];
                    break;
            }
        }
    }
}
