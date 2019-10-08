using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Controller : MonoBehaviour
    {
        public bool IsMultiplayer { get; private set; }
        public HUD HUD { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public Spawner Spawner { get; private set; }

        void Awake()
        {
            this.IsMultiplayer = false;
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<HUD>();
            this.PlayerController = GameObject.Find("GameController/PlayerController").GetComponent<PlayerController>();
            this.Spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        }

        void Update()
        {
            if (this.PlayerController.Player == null && this.Spawner.PlayerSpawningPlanet != null)
            {
                this.PlayerController.InstantiatePlayer(this.Spawner.PlayerSpawningPlanet);
                this.Spawner.IsPlayerAlive = true;
            }

            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
        }
    }
}
