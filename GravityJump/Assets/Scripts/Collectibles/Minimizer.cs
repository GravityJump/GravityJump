using UnityEngine;

namespace Collectibles
{
    public class Minimizer : Collectible
    {
        public float ratio;
        override public void OnCollect()
        {
            target.gameObject.transform.localScale *= ratio;
        }
    }
}
