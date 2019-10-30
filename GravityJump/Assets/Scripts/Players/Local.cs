using UnityEngine;

namespace Players
{
    public class Local : Physic.AttractableBody
    {
        void Update()
        {
            this.horizontalSpeed = Input.GetAxisRaw("Horizontal");

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
