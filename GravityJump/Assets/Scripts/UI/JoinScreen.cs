using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;

namespace UI
{
    public class JoinScreen : BasicScreen
    {
        public Button Back;
        public Button Join;
        Text Input;

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
        }
    }
}
