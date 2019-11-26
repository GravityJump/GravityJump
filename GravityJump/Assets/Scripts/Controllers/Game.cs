using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

namespace Controllers
{
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
        private float PositionSendingFrequency { get; set; } // The number of position message sent per second
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

            this.IsHost = false;

            this.GameSpeed = new Physic.GameSpeed(1f);
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
            }

            // Spawn items.
            foreach (ObjectManagement.Spawner spawner in this.Spawners)
            {
                if (spawner.ShouldSpawn())
                {
                    spawner.Spawn();

                    spawner.PrepareNextAsset();
                }
            }

            this.transform.Translate(this.GameSpeed.ScrollingSpeed * Time.deltaTime, 0, 0); // `transform` is a field of `MonoBehaviour`.
            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
            this.GameSpeed.Increment(Time.deltaTime);

            // Check if the player is in the danger zone.
            if (this.LocalPlayerSpawner.PlayerObject.transform.position.x - this.transform.position.x < -11)
            {

                this.GameOver(false);
            }
            // Switching universe at defined audio time 59 109, taking into account the Spawner delay
            float delay = (this.PlanetSpawner.transform.position.x - 10f - this.transform.position.x) / this.GameSpeed.ScrollingSpeed;
            this.PlanetSpawner.SwitchUniverse(this.MusicPlayer.AudioSource.time < 59 - delay || this.MusicPlayer.AudioSource.time > 108 - delay);
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
            SceneManager.LoadScene("Menu");
        }
    }
}
