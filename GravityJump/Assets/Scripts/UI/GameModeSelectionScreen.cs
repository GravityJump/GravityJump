using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameModeSelectionScreen : BasicScreen
    {
        public Button SoloButton;
        public Button HostButton;
        public Button JoinButton;
        public Button ExitButton;
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

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            this.Version.gameObject.SetActive(true);
            this.Ip.gameObject.SetActive(true);
        }
    }
}
