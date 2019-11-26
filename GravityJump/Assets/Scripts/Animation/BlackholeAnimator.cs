using UnityEngine;

namespace Animation
{
    public class BlackholeAnimator : Animator
    {
        protected override string GameObjectAnimationsDirectoryName => "Blackhole";

        private void Update()
        {
            TimeSinceLastImage += Time.deltaTime;

            this.DisplayNextSprite(this.Animations["Rotation"]);
        }
    }
}
