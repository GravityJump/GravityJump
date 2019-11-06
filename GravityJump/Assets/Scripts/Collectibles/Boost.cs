using UnityEngine;

namespace Collectibles
{
    public class Boost : Collectible
    {
        override public void OnCollect()
        {
            Physic.AttractableBody attractableBody = target.gameObject.GetComponent<Physic.AttractableBody>();
            if (attractableBody != null)
            {
                attractableBody.Throw(new Vector2(1000, 0));
                Destroy(gameObject);
            }
        }
    }
}
