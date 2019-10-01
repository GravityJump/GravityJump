using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : AttractableBody
{
    protected float runSpeed = 7;
    protected bool isJumpEnabled = true;

    private string username;
}
