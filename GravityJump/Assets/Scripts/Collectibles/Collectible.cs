using UnityEngine;

namespace Collectibles
{
    public abstract class Collectible : Physic.PhysicBody
    {
        protected GameObject target { get; set; }
        public float Frequency = 1;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Physic.AttractableBody>() != null || other.gameObject.tag == "RemotePlayer")
            {
                this.target = other.gameObject;
                OnCollect();
            }
            Destroy(gameObject);
        }

        public abstract void OnCollect();
    }
}
