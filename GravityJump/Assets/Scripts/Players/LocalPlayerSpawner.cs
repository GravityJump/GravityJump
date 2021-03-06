﻿using UnityEngine;

namespace Players
{
    public class LocalPlayerSpawner : Spawner
    {
        private Physic.AttractableBody AttractableBody { get; set; }
        private bool IsDead { get; set; }

        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/LocalPlayer") as GameObject;
            this.PlayerObject = null;
            this.IsDead = false;
        }

        public void InstantiatePlayer(Controllers.Game gameController)
        {
            Planets.SpawningPoint point = gameController.PlanetSpawner.PlayerSpawningPlanet;
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
            this.AttractableBody = this.PlayerObject.GetComponent<Physic.AttractableBody>();
            this.AttractableBody.GameSpeed = gameController.GameSpeed;
            this.SetClosestAttractiveBody(point);
        }

        private void SetClosestAttractiveBody(Planets.SpawningPoint point)
        {
            this.AttractableBody.ClosestAttractiveBody = point.Planet.GetComponent<Physic.AttractiveBody>();
        }

        public void FreezeInput()
        {
            this.IsDead = true;
        }

        public void Update()
        {
            if (this.PlayerObject != null && !this.IsDead)
            {
                this.AttractableBody.Walk(Input.GetAxisRaw("Horizontal"));

                if (Input.GetButton("Jump"))
                {
                    this.AttractableBody.Jump();
                }

                if (Input.GetButtonDown("Sprint"))
                {
                    StartCoroutine(this.AttractableBody.Sprint());
                }
            }
        }
    }
}
