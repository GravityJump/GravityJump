using UnityEngine;
using System.Collections.Generic;
using System;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        private UI.HUD HUD { get; set; }
        private Network.Connection Connection { get; set; }
        private Players.LocalPlayerSpawner LocalPlayerSpawner { get; set; }
        private Players.RemotePlayerSpawner RemotePlayerSpawner { get; set; }
        private Planets.Spawner PlanetSpawner { get; set; }
        private Collectibles.Spawner CollectibleSpawner { get; set; }
        private Decors.Spawner DecorSpawner { get; set; }
        private List<ObjectManagement.Spawner> Spawners { get; set; }
        private Physic.Speed Speed { get; set; }
        private bool IsHost { get; set; }

        private void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();

            this.LocalPlayerSpawner = GameObject.Find("GameController/LocalPlayerSpawner").GetComponent<Players.LocalPlayerSpawner>();
            this.RemotePlayerSpawner = GameObject.Find("GameController/RemotePlayerSpawner").GetComponent<Players.RemotePlayerSpawner>();
            this.PlanetSpawner = GameObject.Find("GameController/PlanetSpawner").GetComponent<Planets.Spawner>();
            this.CollectibleSpawner = GameObject.Find("GameController/CollectibleSpawner").GetComponent<Collectibles.Spawner>();
            this.DecorSpawner = GameObject.Find("GameController/DecorSpawner").GetComponent<Decors.Spawner>();

            this.Spawners = new List<ObjectManagement.Spawner>();
            this.Spawners.Add(this.PlanetSpawner);
            this.Spawners.Add(this.CollectibleSpawner);
            this.Spawners.Add(this.DecorSpawner);

            this.Connection = Data.Storage.Connection; // Will be null in a solo game.
            this.IsHost = Data.Storage.IsHost;

            this.Speed = new Physic.Speed(1f);
        }

        private void Update()
        {
            if (this.LocalPlayerSpawner.PlayerObject == null && this.PlanetSpawner.PlayerSpawningPlanet != null)
            {
                this.LocalPlayerSpawner.InstantiatePlayer(this.PlanetSpawner.PlayerSpawningPlanet);
            }

            if (this.RemotePlayerSpawner.PlayerObject == null)
            {
                // this.RemotePlayerSpawner.InstantiatePlayer(this.PlanetSpawner.PlayerSpawningPlanet);
            }

            // If the user is the Host, or plays a solo game.
            if (this.Connection == null || this.IsHost)
            {
                foreach (ObjectManagement.Spawner spawner in this.Spawners)
                {
                    if (spawner.ShouldSpawn())
                    {
                        Network.SpawnerPayload assetPayload = spawner.GetNextAssetPayload();
                        spawner.Spawn(assetPayload);

                        // This is a multiplayer game.
                        if (this.Connection != null)
                        {
                            // Send assetPayload to Client.
                            this.Connection.Write(assetPayload);
                            Debug.Log(BitConverter.ToString(assetPayload.GetBytes()));
                        }

                        spawner.PrepareNextAsset();
                    }
                }
            }
            else
            {
                // Listen to payloads sent by the Host.
                Network.BasePayload payload = this.Connection.Read();
                if (payload != null)
                {
                    if (payload.Code == Network.OpCode.Spawn)
                    {
                        this.SpawnOnPayloadReception(payload as Network.SpawnerPayload);
                    }
                }
            }

            this.transform.Translate(this.Speed.Value * Time.deltaTime, 0, 0); // `transform` is a field of `MonoBehaviour`.
            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
            this.Speed.Increment(Time.deltaTime);
        }

        private void SpawnOnPayloadReception(Network.SpawnerPayload assetPayload)
        {
            switch (assetPayload.spawnerType)
            {
                case ObjectManagement.SpawnerType.Planet:
                    this.PlanetSpawner.Spawn(assetPayload);
                    break;
                case ObjectManagement.SpawnerType.Collectible:
                    this.CollectibleSpawner.Spawn(assetPayload);
                    break;
                case ObjectManagement.SpawnerType.Decor:
                    this.DecorSpawner.Spawn(assetPayload);
                    break;
                default:
                    break;
            }
        }
    }
}
