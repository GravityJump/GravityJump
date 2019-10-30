using System.Collections.Generic;
using UnityEngine;

namespace Collectibles
{
    public class Spawner : ObjectManagement.Spawner
    {
        private float total_frequency;
        public override void PrepareNextAsset()
        {
            SetRandomAssetId();
            position = new Vector3(transform.position.x + Random.value * 10 + 3, (Random.value - 0.5f), 0);
            rotation = 0;
            scaleRatio = 1;
        }

        public override void InitiateSpawner()
        {
            this.spawnerType = ObjectManagement.SpawnerType.Collectible;
            this.AvailablePrefabs = new List<GameObject>();
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Boost") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Minimizer") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Maximizer") as GameObject);
            foreach (GameObject collectible in this.AvailablePrefabs)
            {
                total_frequency += collectible.GetComponent<Collectible>().frequency;
            }
            PrepareNextAsset();
        }

        private void SetRandomAssetId()
        {
            // We select a random number between 0 and the sum of available collectibles frequencies
            // This number will define the selected collectible
            float v = Random.value * total_frequency;
            float f;
            assetId = 0;
            foreach (GameObject collectible in this.AvailablePrefabs)
            {
                f = collectible.GetComponent<Collectible>().frequency;
                if (v <= f)
                {
                    return;
                }
                else
                {
                    v -= f;
                    assetId += 1;
                }
            }
            // In case nothing got selected
            // Should not happen as long as AvailablePrefabs and their frequencies remain unchanged
            Debug.Log("No random collectible could be selected");
            assetId = 0;
        }
    }
}
