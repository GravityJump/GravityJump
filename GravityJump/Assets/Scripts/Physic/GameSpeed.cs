using UnityEngine;

namespace Physic
{
    public class GameSpeed
    {
        // Global speed variable representing the game speed (ex: the scrolling speed, player speed,...)
        public float ScrollingSpeed { get; set; }
        // Speed factor defining how fast is the player compared to the game speed
        public float PlayerSpeedFactor { get; set; }
        // Readonly speed variable to sync player animation with player speed
        public float PlayerSpeed => ScrollingSpeed * PlayerSpeedFactor;

        public GameSpeed(float initSpeed)
        {
            this.ScrollingSpeed = initSpeed;
            this.PlayerSpeedFactor = 1f;
        }

        public void Increment(float timeDelta)
        {
            // TODO: add a more interesting acceleration profile from a difficulty management point of view
            this.ScrollingSpeed += timeDelta * 0.01f;
        }
    }
}