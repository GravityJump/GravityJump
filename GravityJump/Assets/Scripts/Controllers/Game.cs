using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        UI.Stack Screens;

        UI.PauseScreen PauseScreen;

        public UI.HUD HUD { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public Spawner Spawner { get; private set; }

        void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();
            this.PlayerController = GameObject.Find("GameController/PlayerController").GetComponent<PlayerController>();
            this.Spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            this.PauseScreen = GameObject.Find("GameController/HUD/PauseScreen").GetComponent<UI.PauseScreen>();
        }

        void Start()
        {
            this.Screens = new UI.Stack();
            this.SetButtonsCallbacks();
            this.PauseScreen.Clear();
        }

        void SetButtonsCallbacks()
        {
            this.PauseScreen.Back.onClick.AddListener(() => { SceneManager.LoadScene("Menu"); });
            this.PauseScreen.Resume.onClick.AddListener(() => { this.Screens.Pop(); });
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

            if (this.PlayerController.Player == null && this.Spawner.PlayerSpawningPlanet != null)
            {
                this.PlayerController.InstantiatePlayer(this.Spawner.PlayerSpawningPlanet);
                this.Spawner.IsPlayerAlive = true;
            }

            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
        }
    }
}
