using System.Collections.Generic;
using UnityEngine;

namespace Decors
{
    public class Spawner : ObjectManagement.Spawner
    {
        public override void PrepareNextAsset()
        {
            this.SetRandomAssetId();
            this.Position = new Vector3(this.transform.position.x + Random.value * 10 + 3, 0, 0);
            this.Rotation = 0;
            this.ScaleRatio = 1;
        }

        public override void InitiateSpawner()
        {
            this.SpawnerType = ObjectManagement.SpawnerType.Decor;
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Decor/Star") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Decor/Corpse") as GameObject);
        }
        public override float GetFrequency(int index)
        {
            return this.AvailablePrefabs[index].GetComponent<Decor>().Frequency;
        }
    }
}
