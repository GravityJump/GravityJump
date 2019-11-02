using UnityEngine;

namespace Players
{
    public class RemotePlayerSpawner : Spawner
    {
        public Physic.Coordinates2D coordinates2D { get; set; }

        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/RemotePlayer") as GameObject;
            this.PlayerObject = null;
        }

        public void InstantiatePlayer(Physic.Coordinates2D coordinates)
        {
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(coordinates.X, coordinates.Y, 0), Quaternion.Euler(0, 0, coordinates.ZAngle));
        }

        public void SmoothlyMovePlayer()
        {
            if (this.PlayerObject != null && this.coordinates2D != null)
            {
                this.PlayerObject.transform.position = Vector2.Lerp(this.PlayerObject.transform.position, coordinates2D.getVector2(), 0.1f);
                this.PlayerObject.transform.rotation = Quaternion.Slerp(this.PlayerObject.transform.rotation, Quaternion.AngleAxis(coordinates2D.ZAngle, Vector3.forward), 0.1f);
            }
        }

        public void FixedUpdate()
        {
            SmoothlyMovePlayer();
        }
    }
}
