using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractiveBody : Body
{
    public static List<AttractiveBody> ActiveAttractiveBodies;
    public float frequency;

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
}
