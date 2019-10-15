using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Controllers
{
    public class Menu : MonoBehaviour
    {
        public readonly string Version = "0.0.1";

        UI.Stack Screens;

        UI.TitleScreen TitleScreen;
        UI.GameModeSelectionScreen GameModeSelectionScreen;
        UI.HostScreen HostScreen;
        UI.JoinScreen JoinScreen;

        Network.Connection Connection;

        void Awake()
        {
            this.TitleScreen = GameObject.Find("Canvas/TitleScreen").GetComponent<UI.TitleScreen>();
            this.GameModeSelectionScreen = GameObject.Find("Canvas/GameModeSelectionScreen").GetComponent<UI.GameModeSelectionScreen>();
            this.HostScreen = GameObject.Find("Canvas/HostScreen").GetComponent<UI.HostScreen>();
            this.JoinScreen = GameObject.Find("Canvas/JoinScreen").GetComponent<UI.JoinScreen>();
            if (this.Connection == null)
            {
                this.Connection = new Network.Connection();
            }
        }

        void Start()
        {
            this.Screens = new UI.Stack();

            this.GameModeSelectionScreen.Version.text = $"Version {this.Version}";
            this.SetButtonsCallbacks();
            this.ConfigureNetwork();

            this.TitleScreen.Clear();
            this.GameModeSelectionScreen.Clear();
            this.HostScreen.Clear();
            this.JoinScreen.Clear();

            this.Screens.Push(this.TitleScreen);
        }

        void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreen.SoloButton.onClick.AddListener(() => { SceneManager.LoadScene("GameScene"); });
            this.GameModeSelectionScreen.HostButton.onClick.AddListener(() => { this.SetHostScreen(); });
            this.GameModeSelectionScreen.JoinButton.onClick.AddListener(() => { this.SetJoinScreen(); });
            this.GameModeSelectionScreen.ExitButton.onClick.AddListener(() => { Application.Quit(); });
            this.HostScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
                this.Connection.Stop();
            });
            this.JoinScreen.Back.onClick.AddListener(() => { this.Screens.Pop(); });
            this.JoinScreen.Join.onClick.AddListener(() => { });
        }

        void ConfigureNetwork()
        {
            if (Network.Utils.IsInternetAvailable())
            {
                try
                {
                    this.GameModeSelectionScreen.Ip.text = $"IP {Network.Utils.GetHostIpAddress()}";
                }
                catch
                {
                    this.DisableMultiPlayer("No IP address");
                }
            }
            else
            {
                this.DisableMultiPlayer("No Internet connection");
            }
        }

        void DisableMultiPlayer(string reason)
        {
            this.GameModeSelectionScreen.HostButton.interactable = false;
            this.GameModeSelectionScreen.JoinButton.interactable = false;
            this.GameModeSelectionScreen.Ip.text = reason;
        }

        void SetHostScreen()
        {
            this.Screens.Push(this.HostScreen);
            this.Connection.Listen();
        }

        void SetJoinScreen()
        {
            this.Screens.Push(this.JoinScreen);
            Debug.Log(this.JoinScreen.Ip);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (this.Screens.Top() == this.TitleScreen)
                {
                    this.Screens.Push(this.GameModeSelectionScreen);
                }
            }
        }
    }
}
