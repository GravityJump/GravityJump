using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class JoinScreen : BasicScreen
    {
        public Button Back;
        public Button Join;
        public Text Input;

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/JoinScreen");
            this.Back = GameObject.Find("Canvas/JoinScreen/BackButton").GetComponent<Button>();
            this.Join = GameObject.Find("Canvas/JoinScreen/JoinButton").GetComponent<Button>();
            this.Input = GameObject.Find("Canvas/JoinScreen/HostIpInput/HostIpInputText").GetComponent<Text>();
        }
    }
}
