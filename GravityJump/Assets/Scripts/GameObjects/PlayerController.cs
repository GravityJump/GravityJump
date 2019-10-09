using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        // TODO: move prefab in dir `Resources` in order to be able to user `Resouces.Load` instead of a magic binding
        public GameObject Prefab;
        public GameObject Player;

        void Awake()
        {
            this.Player = null;
        }

        public void InstantiatePlayer(SpawningPoint point)
        {
            this.Prefab.GetComponent<InputPlayer>().closestAttractiveBody = point.Planet.GetComponent<AttractiveBody>();
            this.Player = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
        }
    }
}
