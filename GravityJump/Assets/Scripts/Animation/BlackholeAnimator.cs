using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class BlackholeAnimator : Animator
    {
        // Sprites for each animation
        public Sprite[] Rotation;

        private new void Awake()
        {
            // Load Sprites
            this.Animations = new Dictionary<string, Sprite[]>
            {
                { "Rotation", Rotation },
            };
        }

        private void Update()
        {
            TimeSinceLastImage += Time.deltaTime;

            this.DisplayNextSprite(this.Animations["Rotation"]);
        }
    }
}
