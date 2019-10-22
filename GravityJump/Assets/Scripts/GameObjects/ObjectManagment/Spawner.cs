using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    protected List<GameObject> AvailablePrefabs;
    void Awake()
    {
        InitiateSpawner();
    }
    void Update()
    {
        if (ShouldSpawn())
        {
            Spawn();
            PrepareNextAsset();
        }
    }
    abstract protected bool ShouldSpawn();
    abstract protected void PrepareNextAsset();
    abstract protected void Spawn();
    abstract protected void InitiateSpawner();
}
