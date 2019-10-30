using UnityEngine;
using System.Collections.Generic;
using System;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        public UI.HUD HUD { get; private set; }

        Network.Connection Connection;

        public Players.Spawner PlayerController { get; private set; }
        public Planets.Spawner planetSpawner { get; private set; }
        public Collectibles.Spawner collectibleSpawner { get; private set; }
        public Decors.Spawner decorSpawner { get; private set; }
        public List<ObjectManagement.Spawner> spawners { get; private set; }

        public Physic.Speed Speed;

        void Awake()
        {
            this.spawners = new List<ObjectManagement.Spawner>();
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();
            this.PlayerController = GameObject.Find("GameController/MainPlayerController").GetComponent<Players.Spawner>();
            this.planetSpawner = GameObject.Find("GameController/PlanetSpawner").GetComponent<Planets.Spawner>();
            this.spawners.Add(planetSpawner);
            this.collectibleSpawner = GameObject.Find("GameController/CollectibleSpawner").GetComponent<Collectibles.Spawner>();
            this.spawners.Add(collectibleSpawner);
            this.decorSpawner = GameObject.Find("GameController/DecorSpawner").GetComponent<Decors.Spawner>();
            this.spawners.Add(decorSpawner);
            this.Connection = null;
            this.Speed = new Physic.Speed(1f);
        }

        void Start()
        {
            if (Data.Storage.Connection != null)
            {
                this.Connection = Data.Storage.Connection;
            }
        }

        void Update()
        {
            if (this.PlayerController.PlayerObject == null && this.planetSpawner.PlayerSpawningPlanet != null)
            {
                this.PlayerController.InstantiatePlayer(this.planetSpawner.PlayerSpawningPlanet);
            }

            this.transform.Translate(this.Speed.Value * Time.deltaTime, 0, 0);
            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
            this.Speed.Increment(Time.deltaTime);

            // If the user is the Host, or plays a solo game
            if (Data.Storage.Connection == null || Data.Storage.IsHost)
            {
                foreach (ObjectManagement.Spawner spawner in spawners)
                {
                    if (spawner.ShouldSpawn())
                    {
                        Network.SpawnerPayload assetPayload = spawner.GetNextAssetPayload();
                        spawner.Spawn(assetPayload);
                        // if this is not a solo game
                        if (Data.Storage.Connection != null)
                        {
                            // Send assetPayload to Client
                            this.Connection.Write(assetPayload);
                            Debug.Log(BitConverter.ToString(assetPayload.GetBytes()));
                        }
                        spawner.PrepareNextAsset();
                    }
                }
            }
            else
            {
                // we listen to payloads sent by the Host
                Network.BasePayload payload = this.Connection.Read();
                if (payload != null)
                {
                    if (payload.Code == Network.OpCode.Spawn)
                    {
                        this.SpawnOnPayloadReception(payload as Network.SpawnerPayload);
                    }
                }
            }
        }

        private void SpawnOnPayloadReception(Network.SpawnerPayload assetPayload)
        {
            switch (assetPayload.spawnerType)
            {
                case ObjectManagement.SpawnerType.Planet:
                    this.planetSpawner.Spawn(assetPayload);
                    break;
                case ObjectManagement.SpawnerType.Collectible:
                    this.collectibleSpawner.Spawn(assetPayload);
                    break;
                case ObjectManagement.SpawnerType.Decor:
                    this.decorSpawner.Spawn(assetPayload);
                    break;
                default:
                    break;
            }
        }
    }
}
