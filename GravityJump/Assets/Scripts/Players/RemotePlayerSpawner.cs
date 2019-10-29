using UnityEngine;

namespace Players
{
    public class RemotePlayerSpawner : Spawner
    {
        void Awake()
        {
            this.Prefab = Resources.Load("Prefabs/Characters/RemotePlayer") as GameObject;
        }
    }
}