using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Update is called once per frame
    private float speed = 2;
    public GameObject planet;
    private float spawnRate = 0.25f;
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if (Random.value * spawnRate <= Time.deltaTime)
        {
            GeneratePlanet();
        }
    }

    void GeneratePlanet()
    {
        GameObject generatedObject = Instantiate(
            planet,
            this.transform.position + new Vector3((Random.value - 0.5f) * 10, (Random.value - 0.5f) * 10, 0),
            Quaternion.Euler(0, 0, Random.value * 360));
        generatedObject.transform.localScale *= Random.value;
    }

}
