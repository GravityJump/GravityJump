using UnityEngine;

namespace Players
{
    public class Remote : Physic.Body
    {
        public Physic.Coordinates2D coordinates2D;

        private void Awake()
        {
            this.coordinates2D = new Physic.Coordinates2D(0, 0, 0);
        }
    }
}
