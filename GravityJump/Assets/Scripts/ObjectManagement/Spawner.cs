using System.Collections.Generic;
using UnityEngine;

namespace ObjectManagement
{
    public enum SpawnerType : byte
    {
        Planet,
        Collectible,
        Decor,
    }
    public abstract class Spawner : MonoBehaviour
    {
        protected List<GameObject> AvailablePrefabs;
        protected int assetId;
        protected Vector3 position;
        protected float rotation;
        protected float scaleRatio;
        protected ObjectManagement.SpawnerType spawnerType;
        void Awake()
        {
            InitiateSpawner();
        }
        public bool ShouldSpawn()
        {
            return transform.position.x >= position.x;
        }
        abstract public void PrepareNextAsset();
        public Network.SpawnerPayload GetNextAssetPayload()
        {
            return new Network.SpawnerPayload(spawnerType, assetId, position, rotation, scaleRatio);
        }
        public void Spawn(Network.SpawnerPayload payload)
        {
            GameObject generatedObject = Instantiate(
                AvailablePrefabs[payload.assetId],
                payload.position,
                Quaternion.Euler(0, 0, payload.rotation));
            generatedObject.transform.localScale *= payload.scaleRatio;
        }
        abstract public void InitiateSpawner();
    }
}
