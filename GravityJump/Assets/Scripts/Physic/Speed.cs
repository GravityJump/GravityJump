using UnityEngine;

namespace Physic
{
    public class Speed
    {

        public Speed(float initSpeed)
        {
            Data.Storage.GameSpeed = initSpeed;
        }

        public void Increment(float timeDelta)
        {
            // TODO: add a more interesting acceleration profile from a difficulty management point of view
            Data.Storage.GameSpeed += timeDelta * 0.01f;
        }
    }
}
