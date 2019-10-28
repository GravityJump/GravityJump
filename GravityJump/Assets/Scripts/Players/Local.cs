using UnityEngine;

namespace Players
{
    public class Local : Player
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
                TakeOff();
            }
        }
    }
}
