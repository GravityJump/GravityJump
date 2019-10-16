using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Controllers
{
    public class Menu : MonoBehaviour
    {
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

            this.Screens = new UI.Stack();
            this.SetButtonsCallbacks();
            if (this.Connection == null)
            {
                this.Connection = new Network.Connection();
            }
        }

        void Start()
        {
            this.TitleScreen.Clear();
            this.GameModeSelectionScreen.Clear();
            this.HostScreen.Clear();
            this.JoinScreen.Clear();
            this.ChatScreen.Clear();

            this.Screens.Push(this.TitleScreen);
        }

        void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreen.HostButton.onClick.AddListener(() =>
            {
                this.SetHostScreen();
            });
            this.GameModeSelectionScreen.JoinButton.onClick.AddListener(() =>
            {
                this.Screens.Push(this.JoinScreen);
            });
            this.HostScreen.Back.onClick.AddListener(() =>
            {
                this.Connection.Stop();
                this.Screens.Pop();
            });
            this.JoinScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
            this.JoinScreen.Join.onClick.AddListener(() =>
            {
                try
                {
                    this.Connection.To(this.JoinScreen.Ip);
                    this.Screens.Push(this.ChatScreen);
                }
                catch
                {
                }
            });
            this.ChatScreen.Send.onClick.AddListener(() =>
            {
                try
                {
                    this.Connection.SendMessage(this.ChatScreen.Input.text);
                    this.ChatScreen.Input.text = " ";
                }
                catch
                {
                    this.Screens.Pop();
                }
            });
            this.ChatScreen.Quit.onClick.AddListener(() =>
            {
                this.Connection.Stop();
                this.Screens.Pop();
            });
        }

        void SetHostScreen()
        {
            this.Screens.Push(this.HostScreen);
            try
            {
                this.Connection.Listen();
            }
            catch
            {
                this.Screens.Pop();
            }
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
