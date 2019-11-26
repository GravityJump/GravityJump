using System.Collections.Generic;
using UnityEngine;

namespace Physic
{
    /**
     * AttractiveBody provides getters for planet physics.
    **/
    public class AttractiveBody : PhysicBody
    {
        public float default_size;
        public float Frequency;

        // All following colliders are attached to children of the current gameObject.
        // A rigidbody collider that represent the ground on which the player will walk.
        public Collider2D ground;
        // A trigger collider, slightly below the ground collider. It is used to compute the normal to the ground.
        public Collider2D normalShape;

        // Give a random size for the spawner
        public float GetRandomSize()
        {
            return default_size * (Random.value * 6f + 1.2f);
        }
        // For a given size, return Minimal distance an adjacent planet can be
        public float GetMinimalDistance()
        {
            return 2;
        }

        // For a given size, Return Maximal distance an adjacent planet can be
        public float GetMaximalDistance()
        {
            return 4;
        }

        // getDistanceBetweenNormalAndGround returns a ColliderDistance2D representing the distance between the normal collider and the ground collider
        public ColliderDistance2D getDistanceBetweenNormalAndGround()
        {
            return normalShape.Distance(ground);
        }
    }
}
