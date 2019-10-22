using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectibleSpawner : Spawner
{
    private float total_frequency;
    private GameObject ActiveCollectible;
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
            GetRandomCollectible(),
            new Vector3(transform.position.x, (Random.value - 0.5f) * 2, 0),
            Quaternion.Euler(0, 0, 0));
        x = transform.position.x + Random.value * 10 + 3;
    }

    protected override void InitiateSpawner()
    {
        this.AvailablePrefabs = new List<GameObject>();
        this.AvailablePrefabs.Add(Resources.Load("Prefabs/Collectibles/Boost") as GameObject);
        foreach (GameObject collectible in this.AvailablePrefabs)
        {
            total_frequency += collectible.GetComponent<Collectible>().frequency;
        }
        PrepareNextAsset();
    }
    private GameObject GetRandomCollectible()
    {
        // We select a random number between 0 and the sum of available collectibles frequencies
        // This number will define the selected collectible
        float v = Random.value * total_frequency;
        float f;
        foreach (GameObject collectible in this.AvailablePrefabs)
        {
            f = collectible.GetComponent<Collectible>().frequency;
            if (v <= f)
            {
                return collectible;
            }
            else
            {
                v -= f;
            }
        }
        // In case nothing got selected 
        // Should not happen as long as AvailablePrefabs and their frequencies remain unchanged
        Debug.Log("No random collectible could be selected");
        return this.AvailablePrefabs[0];
    }
}
