using UnityEngine;

namespace Animation
{
    public class BlackholeAnimator : Animator
    {
        private void Update()
        {
            TimeSinceLastImage += Time.deltaTime;

            this.DisplayNextSprite(this.Animations["Rotation"]);
        }
    }
}
