using UnityEngine;
using UnityEngine.UI;
using System.Net;

namespace UI
{
    public class JoinScreen : BasicScreen
    {
        public Button Back { get; set; }
        public Button Join { get; set; }
        public Text Input { get; set; }

        public IPAddress Ip
        {
            get
            {
                return IPAddress.Parse(this.Input.text);
            }
            private set { }
        }

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/JoinScreen");
            this.Back = GameObject.Find("Canvas/JoinScreen/BackButton").GetComponent<Button>();
            this.Join = GameObject.Find("Canvas/JoinScreen/JoinButton").GetComponent<Button>();
            this.Input = GameObject.Find("Canvas/JoinScreen/HostIpInput/HostIpInputText").GetComponent<Text>();
            this.Name = Names.Menu.Join;
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
        }
    }
}
