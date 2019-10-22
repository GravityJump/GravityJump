using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : Player
{
    public GameObjects.Coordinates2D coordinates2D;

    private void Awake()
    {
        //this.coordinates2D = new GameObjects.Coordinates2D(0, 0, 0);
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector2.Lerp(this.transform.position, this.coordinates2D.getVector2(), 0.1f);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.AngleAxis(this.coordinates2D.ZAngle, Vector3.forward), 0.01f);
        //coordinates2D.X += 0.2f;
        //coordinates2D.Y += 0.2f;
        //coordinates2D.ZAngle += 2f;
    }
}
