﻿using UnityEngine;

namespace Players
{
    public class LocalPlayerSpawner : Spawner
    {
        void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/LocalPlayer") as GameObject;
            this.PlayerObject = null;
        }

        protected override void SetClosestAttractiveBody(Planets.SpawningPoint point)
        {
            this.Prefab.GetComponent<Local>().closestAttractiveBody = point.Planet.GetComponent<Physic.AttractiveBody>();
        }
    }
}