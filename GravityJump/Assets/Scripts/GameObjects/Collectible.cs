using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : PhysicBody
{
    protected GameObject target;
    public float frequency = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        target = other.gameObject;
        OnCollect();
    }

    public abstract void OnCollect();
}
