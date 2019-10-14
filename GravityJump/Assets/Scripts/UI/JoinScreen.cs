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
            this.Panel = GameObject.Find("UICanvas/JoinScreen");
            this.Back = GameObject.Find("UICanvas/JoinScreen/BackButton").GetComponent<Button>();
            this.Join = GameObject.Find("UICanvas/JoinScreen/JoinButton").GetComponent<Button>();
            this.Input = GameObject.Find("UICanvas/JoinScreen/HostIpInput/HostIpInputText").GetComponent<Text>();
        }
    }
}
