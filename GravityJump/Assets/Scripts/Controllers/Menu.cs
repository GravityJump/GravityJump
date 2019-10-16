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
        UI.ChatScreen ChatScreen;

        Network.Connection Connection;

        void Awake()
        {
            this.TitleScreen = GameObject.Find("Canvas/TitleScreen").GetComponent<UI.TitleScreen>();
            this.GameModeSelectionScreen = GameObject.Find("Canvas/GameModeSelectionScreen").GetComponent<UI.GameModeSelectionScreen>();
            this.HostScreen = GameObject.Find("Canvas/HostScreen").GetComponent<UI.HostScreen>();
            this.JoinScreen = GameObject.Find("Canvas/JoinScreen").GetComponent<UI.JoinScreen>();
            this.ChatScreen = GameObject.Find("Canvas/ChatScreen").GetComponent<UI.ChatScreen>();
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
            this.ChatScreen.Clear();

            this.Screens.Push(this.TitleScreen);
        }

        void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreen.SoloButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });
            this.GameModeSelectionScreen.HostButton.onClick.AddListener(() =>
            {
                this.SetHostScreen();
            });
            this.GameModeSelectionScreen.JoinButton.onClick.AddListener(() =>
            {
                this.SetJoinScreen();
            });
            this.GameModeSelectionScreen.ExitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            this.HostScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
                this.Connection.Stop();
            });
            this.JoinScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
            this.JoinScreen.Join.onClick.AddListener(() =>
            {
                this.Connection.To(this.JoinScreen.Ip);
                this.Screens.Push(this.ChatScreen);
            });
            this.ChatScreen.Send.onClick.AddListener(() =>
            {
                this.Connection.SendMessage(this.ChatScreen.Input.text);
                this.ChatScreen.Input.text = "";
            });
            this.ChatScreen.Quit.onClick.AddListener(() =>
            {
                this.Connection.Stop();
                this.Screens.Pop();
            });
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
        }

        void Update()
        {
            // a bit hacky...
            if (this.Connection.Status == Network.Status.Established && this.Screens.Top() != this.ChatScreen)
            {
                this.Screens.Push(this.ChatScreen);
            }

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
