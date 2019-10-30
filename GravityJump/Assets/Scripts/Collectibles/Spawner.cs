using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public class Spawner : ObjectManagement.Spawner
    {
        public override void PrepareNextAsset()
        {
            this.SetRandomAssetId();
            this.Position = new Vector3(this.transform.position.x + Random.value * 10 + 3, (Random.value - 0.5f), 0);
            this.Rotation = 0;
            this.ScaleRatio = 1;
        }

        public override void InitiateSpawner()
        {
            this.SpawnerType = ObjectManagement.SpawnerType.Collectible;

            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Boost") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Minimizer") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Maximizer") as GameObject);
        }
        public override float GetFrequency(int assetId)
        {
            return this.AvailablePrefabs[assetId].GetComponent<Collectible>().Frequency;
        }
    }
}
