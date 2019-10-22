using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawningPoint2
{
    public GameObject Planet { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }

    public SpawningPoint2(GameObject planet, float x, float y)
    {
        this.Planet = planet;
        this.X = x;
        this.Y = y;
    }
}

public class Spawner2 : MonoBehaviour
{
    public Speed Speed;
    public float x = 3f;
    public float y = 1f;
    private float r = 1f;
    private List<GameObject> AvailablePlanetsPrefabs;
    private GameObject ActivePlanet;
    private float total_frequency;
    public SpawningPoint2 PlayerSpawningPlanet;
    public bool IsPlayerAlive;

    void Awake()
    {
        this.PlayerSpawningPlanet = null;
        this.IsPlayerAlive = false;

        this.AvailablePlanetsPrefabs = new List<GameObject>();
        this.AvailablePlanetsPrefabs.Add(Resources.Load("Prefabs/Planetoids/Planet") as GameObject);
        this.AvailablePlanetsPrefabs.Add(Resources.Load("Prefabs/Planetoids/Cucumboid") as GameObject);
    }

    void Start()
    {
        // Defining spawner speed
        this.Speed = new Speed(1f);
        // Computing the sum of available planets' frequency
        foreach (GameObject planet in this.AvailablePlanetsPrefabs)
        {
            total_frequency += planet.GetComponent<AttractiveBody>().frequency;
        }
        // First positions x and y are already defined, preparing the next planet and it size
        ActivePlanet = GetRandomPlanet();
        r = ActivePlanet.GetComponent<AttractiveBody>().GetRandomSize();
    }

    // Randomly select a Planet among the available ones.
    GameObject GetRandomPlanet()
    {
        // We select a random number between 0 and the sum of available planets frequencies
        // This number will define the selected planet
        float v = Random.value * total_frequency;
        float f;
        foreach (GameObject planet in this.AvailablePlanetsPrefabs)
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
        // In case nothing got selected 
        // Should not happen as long as AvailablePlanetsPrefabs and their frequencies remain unchanged
        Debug.Log("No random planet could be selected");
        return this.AvailablePlanetsPrefabs[0];
    }

    void Update()
    {
        // Spawner2 is moving
        transform.Translate(this.Speed.Value * Time.deltaTime, 0, 0);
        // Checking if next planet position has been reached
        if (transform.position.x >= x)
        {
            // Instanciate the Planet
            GeneratePlanet();
            // Prepare next Planet
            PrepareNextSpawn();
        }
        // Incrementing speed
        // this.Speed.Increment(Time.deltaTime);
    }

    // Instantiate the next planet with previously defined position and size
    void GeneratePlanet()
    {
        // Rotation is randomly decided on instantiation
        // x, y and r have been previously defined
        GameObject generatedObject = Instantiate(
            ActivePlanet,
            new Vector3(x, y, 0),
            Quaternion.Euler(0, 0, Random.value * 360));
        generatedObject.transform.localScale *= r;
        generatedObject.SetActive(true);
        // @TODO : To comment
        if (!this.IsPlayerAlive)
        {
            this.PlayerSpawningPlanet = new SpawningPoint2(generatedObject, x, y);
        }
    }

    // Defined the next planet's position and size. Camera's y position is assumed to be 0.
    void PrepareNextSpawn()
    {
        // Pulling previous planet's data
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
        // Defining constraints for next planet's position
        float y_limit = 5;
        float angle_margin = Mathf.PI / 8;
        float angle_min = Mathf.PI / 8;
        float angle_max = 7 * Mathf.PI / 8;
        // Planet can sometime appear slightly out of camera's range
        if (Mathf.Abs(y_last) < 5)
        {
            y_limit += r / 3;

        }
        // Finding an angle satisfying all constraints
        float teta = angle_min;
        int attempts = 0;
        while (attempts < 20 && (teta <= angle_min || teta >= angle_max || Mathf.Abs(y) > y_limit || Mathf.Abs(teta - Mathf.PI / 2) < angle_margin))
        {
            attempts += 1;
            teta = angle_min + (angle_max - angle_min) * Random.value;
            // Computing the y position of next planet
            y = y_last + Mathf.Cos(teta) * (r_last + d + r);
        }
        // If no compromise could be made, bringing next planet closer to center of camera
        if (attempts == 20)
        {
            Debug.Log("No random position could be selected");
            if (y_last < 0)
            {
                teta = Mathf.PI / 4;
            }
            else
            {
                teta = 3 * Mathf.PI / 4;
            }
            // Computing the y position of next planet
            y = y_last + Mathf.Cos(teta) * (r_last + d + r);
        }
        // Computing the x position of next planet
        x = x_last + Mathf.Sin(teta) * (r_last + d + r);
        ActivePlanet = NextPlanet;
    }
}
