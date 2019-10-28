using UnityEngine;
using UnityEngine.SceneManagement;

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
        // public CollectibleSpawner collectibleSpawner { get; private set; }
        // public DecorSpawner decorSpawner { get; private set; }

        public Physic.Speed Speed;

        void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();
            this.PlayerController = GameObject.Find("GameController/PlayerController").GetComponent<Players.Spawner>();
            this.planetSpawner = GameObject.Find("GameController/PlanetSpawner").GetComponent<Planets.Spawner>();
            this.PauseScreen = GameObject.Find("GameController/HUD/PauseScreen").GetComponent<UI.PauseScreen>();

            this.Connection = null;
            this.Screens = new UI.Stack();
        }

        void Start()
        {
            this.Speed = new Physic.Speed(1f);
            this.Screens = new UI.Stack();
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
            this.transform.Translate(this.Speed.Value * Time.deltaTime, 0, 0);
            if (Input.GetKeyDown(KeyCode.Escape) || (this.Screens.Count() == 0 && this.PlayerController.PlayerObject != null && this.PlayerController.PlayerObject.transform.position.x < this.transform.position.x - 10))
            {
                if (this.Screens.Count() == 0)
                {
                    this.Screens.Push(this.PauseScreen);
                }
                else
                {
                    this.Screens.Pop();
                }
            }

            if (this.PlayerController.PlayerObject == null && this.planetSpawner.PlayerSpawningPlanet != null)
            {
                this.PlayerController.InstantiatePlayer(this.planetSpawner.PlayerSpawningPlanet);
                this.planetSpawner.IsPlayerAlive = true;
            }

            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
            this.Speed.Increment(Time.deltaTime);
        }
    }
}
