using UnityEngine;

namespace Planets
{
    public class SpawningPoint
    {
        public GameObject Planet { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public SpawningPoint(GameObject planet, float x, float y)
        {
            this.Planet = planet;
            this.X = x;
            this.Y = y;
        }
    }
}
