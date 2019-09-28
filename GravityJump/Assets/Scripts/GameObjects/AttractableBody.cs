using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttractableBody : Body
{
    private AttractiveBody ClosestAttractiveBody;
    private bool isGrounded { get; }
    private bool shouldRotate { get; }
    private float frictionRatio { get; }
    protected void UpdateClosestAttractiveBody()
    {
        ClosestAttractiveBody = AttractiveBody.ActiveAttractiveBodies[0];
    }
    protected void Rotate()
    {

    }
}
