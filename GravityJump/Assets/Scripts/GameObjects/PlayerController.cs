using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private GameObject Prefab;
    public GameObject Player;

    void Awake()
    {
        this.Prefab = Resources.Load("Prefabs/Characters/Player") as GameObject;
        this.Player = null;
    }

    public void InstantiatePlayer(SpawningPoint point)
    {
        this.Prefab.GetComponent<InputPlayer>().closestAttractiveBody = point.Planet.GetComponent<AttractiveBody>();
        this.Player = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
    }
}
