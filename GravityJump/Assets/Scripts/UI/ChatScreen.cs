using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;

namespace UI
{
    public class ChatScreen : BasicScreen
    {
        public Button Send;
        public Button Quit;
        public Button Start;
        public Text Input;

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/ChatScreen");
            this.Send = GameObject.Find("Canvas/ChatScreen/SendButton").GetComponent<Button>();
            this.Input = GameObject.Find("Canvas/ChatScreen/MessageInput/MessageInputText").GetComponent<Text>();
            this.Quit = GameObject.Find("Canvas/ChatScreen/QuitButton").GetComponent<Button>();
            this.Start = GameObject.Find("Canvas/ChatScreen/StartButton").GetComponent<Button>();
        }
    }
}
