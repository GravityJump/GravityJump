using UnityEngine;
using System.Collections.Generic;
using System;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        UI.Stack Screens;

        UI.PauseScreen PauseScreen;
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
            this.PauseScreen = GameObject.Find("GameController/HUD/PauseScreen").GetComponent<UI.PauseScreen>();
            this.Connection = null;
            this.Screens = new UI.Stack();
            this.Speed = new Physic.Speed(1f);
        }

        void Start()
        {
            this.PauseScreen.Clear();
            this.SetButtonsCallbacks();

            if (Data.Storage.Connection != null)
            {
                this.Connection = Data.Storage.Connection;
            }
        }

        void SetButtonsCallbacks()
        {
            this.PauseScreen.Resume.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
        }

        void Update()
        {
            // Checking for Pause mode
            if (Input.GetKeyDown(KeyCode.Escape) || (this.Screens.Count() == 0 && this.PlayerController.PlayerObject != null && this.PlayerController.PlayerObject.transform.position.x < this.transform.position.x - 14))
            {
                if (this.Screens.Count() == 0)
                {
                    // Pause
                    this.Screens.Push(this.PauseScreen);
                }
                else
                {
                    // Unpause
                    this.Screens.Pop();
                }
            }
            // If game is unpaused
            if (this.Screens.Count() == 0)
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
                                Debug.Log(BitConverter.ToString(assetPayload.GetBytes()));
                            }
                            spawner.PrepareNextAsset();
                        }
                    }
                }
            }
        }
        private void SpawnOnPayloadReception(Network.SpawnerPayload assetPayload)
        {
            // @TODO Implement
            // Switch on payload type using ObjectManagement.SpawnerType
            // Call the relevant spawner
            // spawner.Spawn(assetPayload);
        }
    }
}
