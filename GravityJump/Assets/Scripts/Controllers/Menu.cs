using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;

namespace Controllers
{
    // Menu wraps the menu state machine.
    public class Menu : BaseController
    {
        private UI.Stack Screens { get; set; }
        private UI.TitleScreen TitleScreen { get; set; }
        private UI.GameModeSelectionScreen GameModeSelectionScreen { get; set; }
        private UI.HostScreen HostScreen { get; set; }
        private UI.JoinScreen JoinScreen { get; set; }
        private UI.ChatScreen ChatScreen { get; set; }
        private UI.CreditsScreen CreditsScreen { get; set; }
        private UI.HelpScreen HelpScreen { get; set; }
        private bool Ready { get; set; }
        private bool OtherPlayerReady { get; set; }

        private new void Awake()
        {
            this.TitleScreen = GameObject.Find("Canvas/TitleScreen").GetComponent<UI.TitleScreen>();
            this.GameModeSelectionScreen = GameObject.Find("Canvas/GameModeSelectionScreen").GetComponent<UI.GameModeSelectionScreen>();
            this.HostScreen = GameObject.Find("Canvas/HostScreen").GetComponent<UI.HostScreen>();
            this.JoinScreen = GameObject.Find("Canvas/JoinScreen").GetComponent<UI.JoinScreen>();
            this.ChatScreen = GameObject.Find("Canvas/ChatScreen").GetComponent<UI.ChatScreen>();
            this.CreditsScreen = GameObject.Find("Canvas/CreditsScreen").GetComponent<UI.CreditsScreen>();
            this.HelpScreen = GameObject.Find("Canvas/HelpScreen").GetComponent<UI.HelpScreen>();

            this.Screens = new UI.Stack();
            this.Connection = null;
            this.Ready = false;
            this.OtherPlayerReady = false;

            base.Awake();
        }

        private void Start()
        {
            this.SetButtonsCallbacks();

            this.TitleScreen.Clear();
            this.GameModeSelectionScreen.Clear();
            this.HostScreen.Clear();
            this.JoinScreen.Clear();
            this.ChatScreen.Clear();
            this.CreditsScreen.Clear();
            this.HelpScreen.Clear();

            this.Screens.Push(this.TitleScreen);

            this.MusicPlayer.Play(Audio.MusicPlayer.MusicClip.Menu, true);
        }

        private void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreen.HostButton.onClick.AddListener(() =>
            {
                try
                {
                    this.Screens.Push(this.HostScreen);
                }
                catch
                {
                    Debug.Log("Could not start the listener");
                    this.Screens.Pop();
                }
            });
            this.GameModeSelectionScreen.JoinButton.onClick.AddListener(() =>
            {
                this.Screens.Push(this.JoinScreen);
            });
            this.GameModeSelectionScreen.CreditsButton.onClick.AddListener(() =>
            {
                this.Screens.Push(this.CreditsScreen);
            });
            this.GameModeSelectionScreen.HelpButton.onClick.AddListener(() =>
            {
                this.Screens.Push(this.HelpScreen);
            });
            this.HostScreen.Back.onClick.AddListener(() =>
            {
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
                    this.Connection = new Network.Connection(this.JoinScreen.Ip);
                    this.Screens.Push(this.ChatScreen);
                }
                catch
                {
                    Debug.Log($"Could not establish a connection with {this.JoinScreen.Ip}");
                }
            });
            this.ChatScreen.Send.onClick.AddListener(() =>
            {
                try
                {
                    this.Connection.Write(new Network.Message(this.ChatScreen.Input.text));
                    // TODO: add messages to existing history
                    this.ChatScreen.Conversation.text = $"[Me] {this.ChatScreen.Input.text}";
                    this.ChatScreen.Input.text = "";
                }
                catch
                {
                    Debug.Log("Connection lost");
                    this.Screens.Pop();
                }
            });
            this.ChatScreen.Quit.onClick.AddListener(() =>
            {
                this.Connection.Close();
                this.Connection = null;
                this.Screens.Pop();
            });
            this.ChatScreen.Start.onClick.AddListener(() =>
            {
                this.Ready = true;
                this.Connection.Write(new Network.Ready());
                this.ChatScreen.Start.interactable = false;
            });
            this.CreditsScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
            this.HelpScreen.Back.onClick.AddListener(() =>
            {
                this.Screens.Pop();
            });
        }

        private void Update()
        {
            // Following the active screen, it handles different inputs.
            switch (((UI.BasicScreen)this.Screens.Top()).Name)
            {
                case UI.Names.Menu.Title:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        this.Screens.Push(this.GameModeSelectionScreen);
                    }
                    break;
                case UI.Names.Menu.Host:
                    if (this.Connection == null && this.HostScreen.GetConnection() != null)
                    {
                        this.Connection = this.HostScreen.GetConnection();
                        this.Screens.Push(this.ChatScreen);
                    }
                    break;
                case UI.Names.Menu.Chat:
                    Network.BasePayload payload = this.Connection.Read();
                    if (payload != null)
                    {
                        this.HandleMessage(payload);
                    }

                    if (this.Ready && this.OtherPlayerReady)
                    {
                        Data.Storage.Connection = this.Connection;
                        SceneManager.LoadScene("GameScene");
                    }
                    break;
                default:
                    break;
            }
        }

        private void HandleMessage(Network.BasePayload payload)
        {
            switch (payload.Code)
            {
                case Network.OpCode.Message:
                    this.ChatScreen.Conversation.text = $"[The Stranger] {((Network.Message)payload).Text}\n";
                    break;
                case Network.OpCode.Ready:
                    this.OtherPlayerReady = true;
                    this.ChatScreen.OtherPlayerReadyText.SetActive(this.OtherPlayerReady);
                    break;
                default:
                    break;
            }
        }
    }
}
