using UnityEngine;

namespace Controllers
{
    public class OpponentPlayer : Player
    {
        private void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/OpponentPlayer") as GameObject;
        }

        public override void InstantiatePlayer(SpawningPoint point)
        {
            this.PlayerObject = Instantiate(this.Prefab, new Vector3(point.X, point.Y, 0), Quaternion.Euler(0, 0, Random.value * 360));
        }
    }
}