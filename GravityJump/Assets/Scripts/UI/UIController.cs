using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Network;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public readonly string Version = "0.0.1";

        Stack Screens;

        TitleScreen TitleScreen;
        GameModeSelectionScreen GameModeSelectionScreen;
        HostScreen HostScreen;
        JoinScreen JoinScreen;

        void Awake()
        {
            this.TitleScreen = GameObject.Find("UICanvas/TitleScreen").GetComponent<TitleScreen>();
            this.GameModeSelectionScreen = GameObject.Find("UICanvas/GameModeSelectionScreen").GetComponent<GameModeSelectionScreen>();
            this.HostScreen = GameObject.Find("UICanvas/HostScreen").GetComponent<HostScreen>();
            this.JoinScreen = GameObject.Find("UICanvas/JoinScreen").GetComponent<JoinScreen>();
        }

        void Start()
        {
            this.Screens = new Stack();

            this.GameModeSelectionScreen.Version.text = $"Version {this.Version}";
            this.SetButtonsCallbacks();
            this.ConfigureNetwork();

            this.Screens.Push(this.TitleScreen);
        }

        void SetButtonsCallbacks()
        {
            this.GameModeSelectionScreen.SoloButton.onClick.AddListener(() => { SceneManager.LoadScene("GameScene"); });
            this.GameModeSelectionScreen.HostButton.onClick.AddListener(() => { this.Screens.Push(this.HostScreen); });
            this.GameModeSelectionScreen.JoinButton.onClick.AddListener(() => { this.Screens.Push(this.JoinScreen); });
            this.GameModeSelectionScreen.ExitButton.onClick.AddListener(() => { Application.Quit(); });
            this.HostScreen.Back.onClick.AddListener(() => { this.Screens.Pop(); });
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

        void Update()
        {
            if (this.Screens.Top() == this.TitleScreen)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    this.Screens.Push(this.GameModeSelectionScreen);
                }
            }
        }
    }
}
