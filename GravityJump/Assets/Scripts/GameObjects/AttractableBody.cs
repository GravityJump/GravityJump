using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttractableBody : Body
{
    [SerializeField] protected Transform groundedCheck;
    [SerializeField] protected LayerMask groundMask;
    // This should be a collider slightly below the ground collider, to keep the normal upward.
    [SerializeField] protected GameObject closestAttractingPlanetoid;
    [SerializeField] protected GameObject currentPlanetoid;

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
