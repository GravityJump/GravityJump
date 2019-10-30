using System.Collections.Generic;
using UnityEngine;

namespace Decors
{
    public class Spawner : ObjectManagement.Spawner
    {
        private float total_frequency;
        public override void PrepareNextAsset()
        {
            SetRandomAssetId();
            position = new Vector3(transform.position.x + Random.value * 10 + 3, 0, 0);
            rotation = 0;
            scaleRatio = 1;
        }

        public override void InitiateSpawner()
        {
            this.spawnerType = ObjectManagement.SpawnerType.Decor;
            this.AvailablePrefabs = new List<GameObject>();
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Decor/Star") as GameObject);
            foreach (GameObject decor in this.AvailablePrefabs)
            {
                total_frequency += decor.GetComponent<Decor>().frequency;
            }
            PrepareNextAsset();
        }
        private void SetRandomAssetId()
        {
            // We select a random number between 0 and the sum of available decors frequencies
            // This number will define the selected decor
            float v = Random.value * total_frequency;
            float f;
            assetId = 0;
            foreach (GameObject decor in this.AvailablePrefabs)
            {
                f = decor.GetComponent<Decor>().frequency;
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
            Debug.Log("No random decor could be selected");
            assetId = 0;
        }
    }
}
