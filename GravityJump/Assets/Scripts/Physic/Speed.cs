using UnityEngine;

namespace Physic
{
    public class Speed
    {
        public float Value { get; private set; }

        public Speed(float initSpeed)
        {
            this.Value = initSpeed;
        }

        public void Increment(float timeDelta)
        {
            // TODO: add a more interesting acceleration profile from a difficulty management point of view
            this.Value += timeDelta * 0.01f;
        }
    }
}
