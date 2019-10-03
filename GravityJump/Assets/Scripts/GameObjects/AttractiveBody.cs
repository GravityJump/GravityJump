using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractiveBody : Body
{
    public static List<AttractiveBody> ActiveAttractiveBodies;
    public float frequency;
    public Collider2D orbit;
    public Collider2D normalShape;

    public float default_size;

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
    public float GetRandomSize()
    {
        return default_size * (Random.value + 0.5f);
    }
    public float GetMinimalDistance(float size)
    {
        return size / 2;
    }
    public float GetMaximalDistance(float size)
    {
        return 2 * size;
    }
}
