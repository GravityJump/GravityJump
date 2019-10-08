using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Speed Speed;
    public float x = 3f;
    public float y = 1f;
    private float r = 1f;
    public List<GameObject> planets;
    private GameObject ActivePlanet;
    private float total_frequency;
    public GameObject Player;

    void Start()
    {
        this.Speed = new Speed(2f);

        foreach (GameObject planet in planets)
        {
            total_frequency += planet.GetComponent<AttractiveBody>().frequency;
        }

        ActivePlanet = GetRandomPlanet();
        r = ActivePlanet.GetComponent<AttractiveBody>().GetRandomSize();
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
        transform.Translate(this.Speed.GetValue() * Time.deltaTime, 0, 0);

        if (transform.position.x >= x)
        {
            GeneratePlanet();
            PrepareNextSpawn();
        }
    }

    void GeneratePlanet()
    {
        // Rotation can be randomly decided on instantiation
        GameObject generatedObject = Instantiate(
            ActivePlanet,
            new Vector3(x, y, 0),
            Quaternion.Euler(0, 0, Random.value * 360));
        generatedObject.transform.localScale *= r;
        generatedObject.SetActive(true);
    }

    void PrepareNextSpawn()
    {
        // Pulling previous usefull data on
        float r_last = r;
        float x_last = x;
        float y_last = y;
        // Deciding next planet and it size
        GameObject NextPlanet = GetRandomPlanet();
        r = NextPlanet.GetComponent<AttractiveBody>().GetRandomSize();
        // Deducing minimal and maximal distance to the next planet
        float d_min = Mathf.Max(NextPlanet.GetComponent<AttractiveBody>().GetMinimalDistance(r), ActivePlanet.GetComponent<AttractiveBody>().GetMinimalDistance(r_last));
        float d_max = Mathf.Min(NextPlanet.GetComponent<AttractiveBody>().GetMaximalDistance(r), ActivePlanet.GetComponent<AttractiveBody>().GetMaximalDistance(r_last));
        // Deciding next planet distance to previous one
        float d = d_min + (d_max - d_min) * Random.value;
        // Deducing minimal and maximal angle to the next planet (to avoid new planet out of limits)
        float y_upper_limit = 5 + r / 2;
        float y_lower_limit = -5 - r / 2;
        float teta_upper_limit = 0;
        float teta_lower_limit = Mathf.PI;

        if (y_upper_limit < y_last + r_last + d + r)
        {
            teta_upper_limit += Mathf.Tan(r / (y_last + r_last + d + r - y_upper_limit));
        }
        if (y_lower_limit > y_last - r_last - d - r)
        {
            teta_lower_limit += Mathf.Tan(r / (y_last - r_last - d - r + y_lower_limit));
        }
        // Deciding next planet angle to previous one
        float teta = teta_upper_limit + (teta_lower_limit - teta_upper_limit) * Random.value;
        // Deducing the x and y position of next planet
        x = x_last + Mathf.Sin(teta) * (r_last + d + r);
        y = y_last + Mathf.Cos(teta) * (r_last + d + r);

        ActivePlanet = NextPlanet;
    }
}
