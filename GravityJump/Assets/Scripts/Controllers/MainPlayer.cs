
using UnityEngine;

namespace Controllers
{
    public class MainPlayer : Player
    {
        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/MainPlayer") as GameObject;
        }

        public override void InstantiatePlayer(SpawningPoint point)
        {

            this.Prefab.GetComponent<InputPlayer>().closestAttractiveBody = point.Planet.GetComponent<AttractiveBody>();
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
        }
    }
}
