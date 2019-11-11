using UnityEngine;

namespace Players
{
    public class LocalPlayerSpawner : Spawner
    {
        private Physic.AttractableBody AttractableBody;

        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/LocalPlayer") as GameObject;
            this.PlayerObject = null;
        }

        public void InstantiatePlayer(Planets.SpawningPoint point)
        {
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
            this.AttractableBody = this.PlayerObject.GetComponent<Physic.AttractableBody>();
            this.PlayerObject.AddComponent<Animation.PlayerAnimator>();
            this.SetClosestAttractiveBody(point);
        }

        private void SetClosestAttractiveBody(Planets.SpawningPoint point)
        {
            this.AttractableBody.ClosestAttractiveBody = point.Planet.GetComponent<Physic.AttractiveBody>();
        }

        void Update()
        {
            if (this.PlayerObject != null)
            {
                this.AttractableBody.Walk(Input.GetAxisRaw("Horizontal"));

                if (Input.GetButton("Jump"))
                {
                    this.AttractableBody.Jump();
                }

                if (Input.GetButtonDown("Sprint"))
                {
                    Data.Storage.PlayerSpeedFactor *= 2.0f;
                }
                else if (Input.GetButtonUp("Sprint"))
                {
                    Data.Storage.PlayerSpeedFactor /= 2.0f;
                }
            }
        }
    }
}
