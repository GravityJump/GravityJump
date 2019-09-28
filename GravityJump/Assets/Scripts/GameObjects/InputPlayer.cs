using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : Player
{
    // Mapping keyboard input to MoveUp, ... methods
    private void Update()
    {
        if (Input.GetKey("up"))
        {
            this.moveUp();
        }
        if (Input.GetKey("down"))
        {
            this.moveDown();
        }
        if (Input.GetKey("left"))
        {
            this.moveLeft();
        }
        if (Input.GetKey("right"))
        {
            this.moveRight();
        }
    }

}