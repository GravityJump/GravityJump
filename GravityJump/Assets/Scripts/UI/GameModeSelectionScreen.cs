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
            this.Panel = GameObject.Find("UICanvas/GameModeSelectionScreen");
            this.Version = GameObject.Find("UICanvas/GameModeSelectionScreen/Version").GetComponent<Text>();
            this.Ip = GameObject.Find("UICanvas/GameModeSelectionScreen/Ip").GetComponent<Text>();
            this.SoloButton = GameObject.Find("UICanvas/GameModeSelectionScreen/SoloButton").GetComponent<Button>();
            this.HostButton = GameObject.Find("UICanvas/GameModeSelectionScreen/HostButton").GetComponent<Button>();
            this.JoinButton = GameObject.Find("UICanvas/GameModeSelectionScreen/JoinButton").GetComponent<Button>();
            this.ExitButton = GameObject.Find("UICanvas/GameModeSelectionScreen/ExitButton").GetComponent<Button>();
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            this.Version.gameObject.SetActive(true);
            this.Ip.gameObject.SetActive(true);
        }
    }
}
