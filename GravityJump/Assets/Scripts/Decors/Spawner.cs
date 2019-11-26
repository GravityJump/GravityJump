using UnityEngine;

namespace Decors
{
    public class Spawner : ObjectManagement.Spawner
    {
        private float xLastSpawn;
        public override void PrepareNextAsset()
        {
            this.SetRandomAssetId();
            this.xLastSpawn = this.xLastSpawn + Random.value * 10 + 3;
            this.Position = new Vector3(this.xLastSpawn, 0, 0);
            this.Rotation = 0;
            this.ScaleRatio = 1;
        }

        public override void InitiateSpawner()
        {
            this.xLastSpawn = 10;
            this.SpawnerType = ObjectManagement.SpawnerType.Decor;
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Decor/ShootingStar") as GameObject);
            this.AvailablePrefabs.Add(Resources.Load("Prefabs/Decor/Corpse") as GameObject);
        }
        public override float GetFrequency(int index)
        {
            return this.AvailablePrefabs[index].GetComponent<Decor>().Frequency;
        }
    }
}
