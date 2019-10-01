using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    // Update is called once per frame
    void Update()
    {
        horizontalSpeed = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Jump")) {
            Jump();
        } else if (Input.GetButtonUp("Jump"))
        {
            StopJumping();
        }
    }
}
