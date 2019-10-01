using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float speed = 2f;
    public float spawnRate = 3f;
    public List<GameObject> planets;
    private float total_frequency;
    void Start()
    {
        foreach (GameObject planet in planets)
        {
            total_frequency += planet.GetComponent<AttractiveBody>().frequency;
        }
    }

    GameObject GetRandomPlanet()
    {
        float v = Random.value * total_frequency;
        float f;
        foreach (GameObject planet in planets)
        {
            f = planet.GetComponent<AttractiveBody>().frequency;
            if (v <= f)
            {
                return planet;
            }
            else
            {
                v -= f;
            }
        }
        // if nothing has been found
        Debug.Log("No random planet could be selected");
        return planets[0];
    }
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if (Random.value <= Time.deltaTime * spawnRate)
        {
            GeneratePlanet();
        }
    }

    void GeneratePlanet()
    {
        GameObject generatedObject = Instantiate(
            GetRandomPlanet(),
            this.transform.position + new Vector3((Random.value - 0.5f) * 10, (Random.value - 0.5f) * 15, 0),
            Quaternion.Euler(0, 0, Random.value * 360));
        generatedObject.transform.localScale *= (Random.value * 0.3f + 0.7f);
        generatedObject.SetActive(true);
    }

}
