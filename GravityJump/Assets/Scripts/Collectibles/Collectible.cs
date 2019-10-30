using UnityEngine;

namespace Collectibles
{
    public abstract class Collectible : Physic.PhysicBody
    {
        protected GameObject target;
        public float Frequency = 1;

        void OnTriggerEnter2D(Collider2D other)
        {
            target = other.gameObject;
            OnCollect();
        }

        public abstract void OnCollect();
    }
}
