using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    // GameModeSelectionScreen displays the different options to start a game.
    public class GameModeSelectionScreen : BasicScreen
    {
        private Button SoloButton { get; set; }
        public Button HostButton { get; set; }
        public Button JoinButton { get; set; }
        public Button CreditsButton { get; set; }
        public Button HelpButton { get; set; }
        private Button ExitButton { get; set; }
        private Text Ip { get; set; }

        public override void Awake()
        {
            this.Name = Names.Menu.GameModeSelection;
            this.Panel = GameObject.Find("Canvas/GameModeSelectionScreen");
            this.Ip = GameObject.Find("Canvas/GameModeSelectionScreen/Ip").GetComponent<Text>();
            this.SoloButton = GameObject.Find("Canvas/GameModeSelectionScreen/SoloButton").GetComponent<Button>();
            this.HostButton = GameObject.Find("Canvas/GameModeSelectionScreen/HostButton").GetComponent<Button>();
            this.JoinButton = GameObject.Find("Canvas/GameModeSelectionScreen/JoinButton").GetComponent<Button>();
            this.CreditsButton = GameObject.Find("Canvas/GameModeSelectionScreen/CreditsButton").GetComponent<Button>();
            this.HelpButton = GameObject.Find("Canvas/GameModeSelectionScreen/HelpButton").GetComponent<Button>();
            this.ExitButton = GameObject.Find("Canvas/GameModeSelectionScreen/ExitButton").GetComponent<Button>();
        }

        public void Start()
        {
            this.SoloButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });

            this.ExitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            this.Ip.gameObject.SetActive(true);
            this.CheckNetworkAvailability();
        }

        private void CheckNetworkAvailability()
        {
            if (Network.Utils.IsInternetAvailable())
            {
                try
                {
                    this.Ip.text = $"IP {Network.Utils.GetHostIpAddress()}";
                }
                catch
                {
                    this.DisableMultiplayer("No IP address");
                }
            }
            else
            {
                this.DisableMultiplayer("No Internet connection");
            }
        }

        private void DisableMultiplayer(string reason)
        {
            this.HostButton.interactable = false;
            this.JoinButton.interactable = false;
            this.Ip.text = reason;
        }
    }
}
