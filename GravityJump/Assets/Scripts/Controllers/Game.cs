using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        UI.Stack Screens;

        UI.PauseScreen PauseScreen;

        public UI.HUD HUD { get; private set; }
        public Player PlayerController { get; private set; }
        public PlanetSpawner planetSpawner { get; private set; }
        // public CollectibleSpawner collectibleSpawner { get; private set; }
        // public DecorSpawner decorSpawner { get; private set; }

        public Speed Speed;

        void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();
            this.PlayerController = GameObject.Find("GameController/PlayerController").GetComponent<Player>();
            this.planetSpawner = GameObject.Find("GameController/PlanetSpawner").GetComponent<PlanetSpawner>();
            this.PauseScreen = GameObject.Find("GameController/HUD/PauseScreen").GetComponent<UI.PauseScreen>();
        }

        void Start()
        {
            this.Speed = new Speed(1f);
            this.Screens = new UI.Stack();
            this.SetButtonsCallbacks();
            this.PauseScreen.Clear();
        }

        void SetButtonsCallbacks()
        {
            this.PauseScreen.Back.onClick.AddListener(() =>
            {
                this.PlayerController.Clear();
                SceneManager.LoadScene("Menu");
            });
            this.PauseScreen.Resume.onClick.AddListener(() => { this.Screens.Pop(); });
        }

        void Update()
        {
            this.transform.Translate(this.Speed.Value * Time.deltaTime, 0, 0);
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
