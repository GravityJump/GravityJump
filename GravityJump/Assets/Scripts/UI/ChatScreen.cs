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
        public InputField Input;
        public Text Conversation;
        public GameObject OtherPlayerReadyText;

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/ChatScreen");
            this.Send = GameObject.Find("Canvas/ChatScreen/SendButton").GetComponent<Button>();
            this.Input = GameObject.Find("Canvas/ChatScreen/MessageInput").GetComponent<InputField>();
            this.Quit = GameObject.Find("Canvas/ChatScreen/QuitButton").GetComponent<Button>();
            this.Start = GameObject.Find("Canvas/ChatScreen/StartButton").GetComponent<Button>();
            this.Conversation = GameObject.Find("Canvas/ChatScreen/Conversation").GetComponent<Text>();
            this.OtherPlayerReadyText = GameObject.Find("Canvas/ChatScreen/OtherPlayerReadyText");
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            this.OtherPlayerReadyText.SetActive(false);
        }
    }
}
