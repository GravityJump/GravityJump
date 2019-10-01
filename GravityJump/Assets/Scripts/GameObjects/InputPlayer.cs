using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : Player
{    
    void Update()
    {
        horizontalSpeed = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            StopJumping();
        }
    }
}