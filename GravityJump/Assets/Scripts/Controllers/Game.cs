using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        UI.Stack Screens;

        UI.PauseScreen PauseScreen;
        public UI.HUD HUD { get; private set; }

        Network.Connection Connection;

        public Player PlayerController { get; private set; }
        public Spawner Spawner { get; private set; }

        void Awake()
        {
            this.HUD = GameObject.Find("GameController/HUD").GetComponent<UI.HUD>();
            this.PlayerController = GameObject.Find("GameController/PlayerController").GetComponent<Player>();
            this.Spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            this.PauseScreen = GameObject.Find("GameController/HUD/PauseScreen").GetComponent<UI.PauseScreen>();

            this.Connection = null;
            this.Screens = new UI.Stack();
        }

        void Start()
        {
            this.SetButtonsCallbacks();

            this.PauseScreen.Clear();

            if (Data.Storage.isMultiplayer)
            {
                this.InitNetwork();
            }
        }

        void InitNetwork()
        {
            if (Data.Storage.isHost)
            {
                Network.Listener listener = new Network.Listener();
                listener.Start();

                int maxTry = 10;
                int i = 0;
                while (i < maxTry)
                {
                    this.Connection = listener.GetConnection();
                    if (this.Connection == null)
                    {
                        Thread.Sleep(1000);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (i == maxTry)
                {
                    Debug.Log("could not establish a connection");
                    SceneManager.LoadScene("Menu");
                }
            }
            else
            {
                this.Connection = new Network.Connection(Data.Storage.otherIp);
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

            if (this.PlayerController.PlayerObject == null && this.Spawner.PlayerSpawningPlanet != null)
            {
                this.PlayerController.InstantiatePlayer(this.Spawner.PlayerSpawningPlanet);
                this.Spawner.IsPlayerAlive = true;
            }

            this.HUD.UpdateDistance(0.1f, Time.deltaTime);
        }
    }
}
