using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Physic.AttractableBody AttractableBody;
        private List<Sprite> AnimationSprites;

        private void Awake()
        {
            this.AnimationSprites = new List<Sprite>();

            // Load jump sprites
            for (int i = 1; i < 5; i++)
            {
                string path = $"Animation/Player/Jump/{i}";
                this.AnimationSprites.Add(Resources.Load<Sprite>(path));
            }

            AttractableBody = this.gameObject.GetComponent<Physic.AttractableBody>();
        }

        private void Update()
        {
            switch (AttractableBody.playerMovingState.movingState)
            {
                case Physic.PlayerMovingState.MovingState.Grounded:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = AnimationSprites[0];
                    break;
                case Physic.PlayerMovingState.MovingState.Jumping:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = AnimationSprites[1];
                    break;
                case Physic.PlayerMovingState.MovingState.InFlight:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = AnimationSprites[2];
                    break;
                case Physic.PlayerMovingState.MovingState.Falling:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = AnimationSprites[3];
                    break;
                case Physic.PlayerMovingState.MovingState.Landing:
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = AnimationSprites[1];
                    break;
            }
        }
    }
}
