using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Physic.AttractableBody AttractableBody;
        // Store the different animation types and their sprites in a key value pair data structure
        private Dictionary<AnimationTypes, Sprite[]> Animations;

        private enum AnimationTypes
        {
            Jump,
        }

        private void Awake()
        {
            this.Animations = new Dictionary<AnimationTypes, Sprite[]>();

            // Load sprites
            foreach (AnimationTypes animationType in Enum.GetValues(typeof(AnimationTypes)))
            {
                string path = $"Animation/Player/{animationType.ToString("g")}";
                this.Animations.Add(animationType, Resources.LoadAll<Sprite>(path));
            }

            AttractableBody = this.gameObject.GetComponent<Physic.AttractableBody>();
        }

        private void Update()
        {
            switch (AttractableBody.playerMovingState.movingState)
            {
                case Physic.PlayerMovingState.MovingState.Grounded:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Animations[AnimationTypes.Jump][0];
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
