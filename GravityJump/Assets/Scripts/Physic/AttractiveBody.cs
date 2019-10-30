using System.Collections.Generic;
using UnityEngine;

namespace Physic
{
    public class AttractiveBody : PhysicBody
    {
        public static List<AttractiveBody> ActiveAttractiveBodies;
        public float Frequency;
        // All following colliders are attached to children of the current gameObject.
        // A trigger collider that represent the orbit of the planet.
        public Collider2D orbit;
        // A rigidbody collider that represent the ground on which the player will walk.
        public Collider2D ground;
        // A trigger collider, slightly below the ground collider. It is used to compute the normal to the ground.
        public Collider2D normalShape;
        public int id { get; private set; }
        private static int idGenerator;

        public float default_size;

        AttractiveBody()
        {
            id = idGenerator;
            idGenerator++;
        }

        static AttractiveBody()
        {
            ActiveAttractiveBodies = new List<AttractiveBody>();
        }

        public AttractiveBody(bool isActive)
        {
            if (isActive)
            {
                AttractiveBody.ActiveAttractiveBodies.Add(this);
            }
        }

        public Vector2 getAttractiveForce(AttractableBody ab)
        {
            return new Vector2(0, 0);
        }

        // Give a random size for the spawner
        public float GetRandomSize()
        {
            return default_size * (Random.value + 3f);
        }
        // For a given size, return Minimal distance an adjacent planet can be
        public float GetMinimalDistance(float size)
        {
            return 2; //size;
        }

        // For a given size, Return Maximal distance an adjacent planet can be
        public float GetMaximalDistance(float size)
        {
            return 4; //2 * size;
        }

        public ColliderDistance2D getDistanceBetweenNormalAndGround()
        {
            return normalShape.Distance(ground);
        }
    }
}
