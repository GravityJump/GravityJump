using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    // Update is called once per frame
    void Update()
    {
        horizontalSpeed = Input.GetAxisRaw("Horizontal");
        jump = (jump || Input.GetButtonDown("Jump")) && !Input.GetButtonUp("Jump");
    }
}
