using UnityEngine;

namespace Collectibles
{
    public class Boost : Collectible
    {
        override public void OnCollect()
        {
            if (this.target.tag != "RemotePlayer")
            {
                this.target.gameObject.GetComponent<Physic.AttractableBody>().Throw(new Vector2(1000, 0));
            }

        }
    }
}
