using System.Collections.Generic;
using UnityEngine;

namespace Decors
{
    public class Spawner : ObjectManagement.Spawner
    {
        private float total_frequency;
        private GameObject ActiveDecor;
        public float x = 0;

        protected override bool ShouldSpawn()
        {
            return transform.position.x >= x;
        }
        protected override void PrepareNextAsset()
        {
        }
        protected override void Spawn()
        {
            Instantiate(
                GetRandomDecor(),
                new Vector3(transform.position.x, 0, 0),
                Quaternion.Euler(0, 0, 0));
            x = transform.position.x + Random.value * 10 + 3;
        }

        protected override void InitiateSpawner()
        {
            this.AvailablePrefabs = new List<GameObject>();
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Decor/Star") as GameObject);
            foreach (GameObject decor in this.AvailablePrefabs)
            {
                total_frequency += decor.GetComponent<Decor>().frequency;
            }
            PrepareNextAsset();
        }
        private GameObject GetRandomDecor()
        {
            // We select a random number between 0 and the sum of available decors frequencies
            // This number will define the selected decor
            float v = Random.value * total_frequency;
            float f;
            foreach (GameObject decor in this.AvailablePrefabs)
            {
                f = decor.GetComponent<Decor>().frequency;
                if (v <= f)
                {
                    return decor;
                }
                else
                {
                    v -= f;
                }
            }
            // In case nothing got selected
            // Should not happen as long as AvailablePrefabs and their frequencies remain unchanged
            Debug.Log("No random decor could be selected");
            return this.AvailablePrefabs[0];
        }
    }
}
