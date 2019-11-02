using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
        private float PositionSendingFrequency { get; set; } // The number of position message sent per second
        private float TimeSinceLastPositionSending { get; set; } 

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
            this.PositionSendingFrequency = 10f;
            this.TimeSinceLastPositionSending = 0f;

            this.Speed = new Physic.Speed(1f);
        }

        private void Update()
        {
            this.TimeSinceLastPositionSending += Time.deltaTime;

            // Instantiate the local player if not already done.
            if (this.LocalPlayerSpawner.PlayerObject == null && this.PlanetSpawner.PlayerSpawningPlanet != null)
            {
                this.LocalPlayerSpawner.InstantiatePlayer(this.PlanetSpawner.PlayerSpawningPlanet);

                // Send the player position to the client if in a multiplayer game.
                if (this.Connection != null)
                {
                    this.SendLocalPlayerPosition();
                }
            }

            // If the user is the Host, or plays a solo game.
            if (this.Connection == null || (this.Connection != null && this.IsHost))
            {
                // Spawn items.
                foreach (ObjectManagement.Spawner spawner in this.Spawners)
                {
                    if (spawner.ShouldSpawn())
                    {
                        Network.SpawnerPayload assetPayload = spawner.GetNextAssetPayload();
                        spawner.Spawn(assetPayload);

                        // In a multiplayer game...
                        if (this.Connection != null)
                        {
                            // ...send the item data to the Client.
                            this.Connection.Write(assetPayload);
                            Debug.Log(BitConverter.ToString(assetPayload.GetBytes()));
                        }

                        spawner.PrepareNextAsset();
                    }
                }
            }

            // In a multiplayer game...
            if (this.Connection != null)
            {
                // ...listen to payloads coming from the other player.
                Network.Payload payload = this.Connection.Read();
                if (payload != null)
                {
                    switch (((Network.BasePayload)payload).Code)
                    {
                        case Network.OpCode.PlayerCoordinates:
                            if (this.RemotePlayerSpawner.PlayerObject == null)
                            {
                                this.RemotePlayerSpawner.InstantiatePlayer(((Network.PlayerCoordinates)payload).coordinates2D);
                            }
                            else
                            {
                                Physic.Coordinates2D coordinates = ((Network.PlayerCoordinates)payload).coordinates2D;
                                this.RemotePlayerSpawner.coordinates2D = coordinates;

                            }
                            break;
                        case Network.OpCode.Spawn:
                            if (!this.IsHost)
                            {
                                this.SpawnOnPayloadReception(payload as Network.SpawnerPayload);
                            }
                            break;
                        case Network.OpCode.Death:
                            // Come back to menu if the other died.
                            Data.Storage.LocalScore = this.HUD.Distance;
                            SceneManager.LoadScene("Menu");
                            break;
                        default:
                            break;
                    }
                }

                // ...and send local player position to the other player
                if (this.TimeSinceLastPositionSending >= 1 / this.PositionSendingFrequency)
                {
                    this.SendLocalPlayerPosition();
                }

            }

            this.transform.Translate(this.Speed.Value * Time.deltaTime, 0, 0); // `transform` is a field of `MonoBehaviour`.
            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
            this.Speed.Increment(Time.deltaTime);

            // Check if the player is in the danger zone.
            if (this.LocalPlayerSpawner.PlayerObject.transform.position.x - this.transform.position.x < -13)
            {
                // If multiplayer, warn the other that death occured.
                if (this.Connection != null)
                {
                    this.Connection.Write(new Network.Death(this.HUD.Distance));
                }

                // @TODO: Game Over animation
                Data.Storage.LocalScore = this.HUD.Distance;
                SceneManager.LoadScene("Menu");
            }
        }

        private void SendLocalPlayerPosition()
        {
            if (this.Connection != null)
            {
                this.Connection.Write(
                    new Network.PlayerCoordinates(
                        new Physic.Coordinates2D(
                            this.LocalPlayerSpawner.PlayerObject.transform.position.x,
                            this.LocalPlayerSpawner.PlayerObject.transform.position.y,
                            this.LocalPlayerSpawner.PlayerObject.transform.rotation.eulerAngles.z
                        )
                    )
                );
                this.TimeSinceLastPositionSending = 0f;
            }
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
