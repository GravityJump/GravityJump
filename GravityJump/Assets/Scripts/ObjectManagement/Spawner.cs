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
        protected List<GameObject> AvailablePrefabs { get; set; }
        protected int AssetId { get; set; }
        protected float TotalFrequency { get; set; }
        protected Vector3 Position { get; set; }
        protected float Rotation { get; set; }
        protected float ScaleRatio { get; set; }
        public ObjectManagement.SpawnerType SpawnerType { get; set; }
        protected void Awake()
        {
            this.AvailablePrefabs = new List<GameObject>();
            this.InitiateSpawner();
            this.SumFrequency();
            this.PrepareNextAsset();
        }
        protected void SumFrequency()
        {
            this.TotalFrequency = 0;
            for (int index = 0; index < this.AvailablePrefabs.Count; index++)
            {
                this.TotalFrequency += this.GetFrequency(index);
            }
        }
        public bool ShouldSpawn()
        {
            return this.transform.position.x >= this.Position.x;
        }
        public Network.SpawnerPayload GetNextAssetPayload()
        {
            return new Network.SpawnerPayload(this.SpawnerType, this.AssetId, this.Position, this.Rotation, this.ScaleRatio);
        }
        public void Spawn(Network.SpawnerPayload payload)
        {
            GameObject generatedObject = Instantiate(
                this.AvailablePrefabs[payload.assetId],
                payload.position,
                Quaternion.Euler(0, 0, payload.rotation));
            generatedObject.transform.localScale *= payload.scaleRatio;
        }
        protected void SetRandomAssetId()
        {
            // We select a random number between 0 and the sum of available prefabs frequencies
            // This number will define the selected prefab
            float v = Random.value * TotalFrequency;
            float f;
            this.AssetId = 0;
            for (int index = 0; index < this.AvailablePrefabs.Count; index++)
            {
                f = GetFrequency(index);
                if (f > v)
                {
                    this.AssetId = index;
                    return;
                }
                v -= f;
            }
            // In case nothing got selected
            // Should not happen as long as AvailablePrefabs and their frequencies remain unchanged
            Debug.Log($"No random {this.SpawnerType} could be selected.");
        }
        abstract public void PrepareNextAsset();
        abstract public void InitiateSpawner();
        abstract public float GetFrequency(int index);
    }
}
