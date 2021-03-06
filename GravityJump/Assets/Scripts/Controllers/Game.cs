﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Controllers
{
    // Game wraps the game state machine.
    public class Game : BaseController
    {
        private UI.HUD HUD { get; set; }
        private Players.LocalPlayerSpawner LocalPlayerSpawner { get; set; }
        private Players.RemotePlayerSpawner RemotePlayerSpawner { get; set; }
        public Planets.Spawner PlanetSpawner { get; private set; }
        private Collectibles.Spawner CollectibleSpawner { get; set; }
        private Decors.Spawner DecorSpawner { get; set; }
        private Backgrounds.Manager BackgroundManager { get; set; }
        private List<ObjectManagement.Spawner> Spawners { get; set; }
        public Physic.GameSpeed GameSpeed { get; private set; }
        private bool IsHost { get; set; }
        private float PositionSendingFrequency { get; set; } // The number of position message sent per second.
        private float TimeSinceLastPositionSending { get; set; }
        private bool IsGameOver { get; set; }

        private new void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();

            this.LocalPlayerSpawner = GameObject.Find("GameController/LocalPlayerSpawner").GetComponent<Players.LocalPlayerSpawner>();
            this.RemotePlayerSpawner = GameObject.Find("GameController/RemotePlayerSpawner").GetComponent<Players.RemotePlayerSpawner>();
            this.PlanetSpawner = GameObject.Find("GameController/PlanetSpawner").GetComponent<Planets.Spawner>();
            this.CollectibleSpawner = GameObject.Find("GameController/CollectibleSpawner").GetComponent<Collectibles.Spawner>();
            this.DecorSpawner = GameObject.Find("GameController/DecorSpawner").GetComponent<Decors.Spawner>();
            this.BackgroundManager = GameObject.Find("GameController/BackgroundManager").GetComponent<Backgrounds.Manager>();

            this.Spawners = new List<ObjectManagement.Spawner>();
            this.Spawners.Add(this.PlanetSpawner);
            this.Spawners.Add(this.CollectibleSpawner);
            this.Spawners.Add(this.DecorSpawner);

            this.Connection = Data.Storage.Connection; // Will be null in a solo game.
            this.IsHost = Data.Storage.IsHost;
            this.PositionSendingFrequency = 10f;
            this.TimeSinceLastPositionSending = 0f;

            this.GameSpeed = new Physic.GameSpeed(1.3f);
            this.IsGameOver = false;

            base.Awake();
        }

        private void Start()
        {
            this.MusicPlayer.Play(Audio.MusicPlayer.MusicClip.Game, true);
        }

        private void Update()
        {
            this.TimeSinceLastPositionSending += Time.deltaTime;

            // Instantiate the local player if not already done.
            if (this.LocalPlayerSpawner.PlayerObject == null && this.PlanetSpawner.PlayerSpawningPlanet != null)
            {
                this.LocalPlayerSpawner.InstantiatePlayer(this);

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
                            this.GameOver(true);
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

            this.transform.Translate(this.GameSpeed.ScrollingSpeed * Time.deltaTime, 0, 0); // `transform` is a field of `MonoBehaviour`.
            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
            this.GameSpeed.Increment(Time.deltaTime);

            // Check if the player is in the danger zone.
            if (this.LocalPlayerSpawner.PlayerObject.transform.position.x - this.transform.position.x < -11)
            {
                // If multiplayer, warn the other that death occurred.
                if (this.Connection != null)
                {
                    this.Connection.Write(new Network.Death());
                }

                this.GameOver(false);
            }
            // Switching universe at defined audio time 59 109, taking into account the Spawner delay
            float delay = (this.PlanetSpawner.transform.position.x - 10f - this.transform.position.x) / this.GameSpeed.ScrollingSpeed;
            this.PlanetSpawner.SwitchUniverse(this.MusicPlayer.AudioSource.time < 59 - delay || this.MusicPlayer.AudioSource.time > 108 - delay);
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

        private void GameOver(bool didWin)
        {
            if (!IsGameOver)
            {
                IsGameOver = true;

                if (didWin)
                {
                    this.MusicPlayer.Play(Audio.MusicPlayer.MusicClip.Win, false);
                }
                else
                {
                    this.MusicPlayer.Play(Audio.MusicPlayer.MusicClip.Death, false);
                }

                this.HUD.GameOver(didWin);
                this.LocalPlayerSpawner.FreezeInput();
                StartCoroutine(this.BackToMenu());
            }
        }

        private System.Collections.IEnumerator BackToMenu()
        {
            yield return new WaitForSeconds(4.5f);

            if (this.Connection != null)
            {
                this.Connection.Close();
                Data.Storage.Connection = null;
                Data.Storage.IsHost = false;
            }

            SceneManager.LoadScene("Menu");
        }
    }
}
