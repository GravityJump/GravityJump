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

        public MainPlayer MainPlayerController { get; private set; }
        public OpponentPlayer OpponentPlayerController { get; private set; }
        public Spawner Spawner { get; private set; }

        void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();
            this.MainPlayerController = GameObject.Find("GameController/MainPlayerController").GetComponent<MainPlayer>();
            this.OpponentPlayerController = GameObject.Find("GameController/OpponentPlayerController").GetComponent<OpponentPlayer>();

            this.Spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            this.PauseScreen = GameObject.Find("GameController/HUD/PauseScreen").GetComponent<UI.PauseScreen>();

            this.Connection = null;
            this.Screens = new UI.Stack();
        }

        void Start()
        {
            this.SetButtonsCallbacks();
            this.PauseScreen.Clear();
            if (Data.Storage.Connection == null)
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
            if (Input.GetKeyDown(KeyCode.Escape))
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

            if (this.MainPlayerController.PlayerObject == null && this.Spawner.PlayerSpawningPlanet != null)
            {
                this.MainPlayerController.InstantiatePlayer(this.Spawner.PlayerSpawningPlanet);
                this.OpponentPlayerController.InstantiatePlayer(this.Spawner.PlayerSpawningPlanet);
                this.Spawner.IsPlayerAlive = true;
            }

            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
        }
    }
}
