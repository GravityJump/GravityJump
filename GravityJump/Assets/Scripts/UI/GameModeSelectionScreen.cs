using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameModeSelectionScreen : BasicScreen
    {
        Button SoloButton;
        public Button HostButton;
        public Button JoinButton;
        Button ExitButton;
        public Text Version;
        public Text Ip;

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/GameModeSelectionScreen");
            this.Version = GameObject.Find("Canvas/GameModeSelectionScreen/Version").GetComponent<Text>();
            this.Ip = GameObject.Find("Canvas/GameModeSelectionScreen/Ip").GetComponent<Text>();
            this.SoloButton = GameObject.Find("Canvas/GameModeSelectionScreen/SoloButton").GetComponent<Button>();
            this.HostButton = GameObject.Find("Canvas/GameModeSelectionScreen/HostButton").GetComponent<Button>();
            this.JoinButton = GameObject.Find("Canvas/GameModeSelectionScreen/JoinButton").GetComponent<Button>();
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
            this.Version.gameObject.SetActive(true);
            this.Ip.gameObject.SetActive(true);

            this.CheckNetworkAvailability();

            Debug.Log("test");
        }

        void CheckNetworkAvailability()
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

        void DisableMultiplayer(string reason)
        {
            this.HostButton.interactable = false;
            this.JoinButton.interactable = false;
            this.Ip.text = reason;
        }
    }
}
