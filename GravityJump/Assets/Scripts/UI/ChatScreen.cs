using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // ChatScreen displays the lobby, and handles the chat system between the players before a game is started.
    public class ChatScreen : BasicScreen
    {
        public Button Send { get; set; }
        public Button Quit { get; set; }
        public Button Start { get; set; }
        public InputField Input { get; set; }
        public Text Conversation { get; set; }
        public GameObject OtherPlayerReadyText { get; set; }

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/ChatScreen");
            this.Send = GameObject.Find("Canvas/ChatScreen/SendButton").GetComponent<Button>();
            this.Input = GameObject.Find("Canvas/ChatScreen/MessageInput").GetComponent<InputField>();
            this.Quit = GameObject.Find("Canvas/ChatScreen/QuitButton").GetComponent<Button>();
            this.Start = GameObject.Find("Canvas/ChatScreen/StartButton").GetComponent<Button>();
            this.Conversation = GameObject.Find("Canvas/ChatScreen/Conversation").GetComponent<Text>();
            this.OtherPlayerReadyText = GameObject.Find("Canvas/ChatScreen/OtherPlayerReadyText");
            this.Name = Names.Menu.Chat;
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            this.OtherPlayerReadyText.SetActive(false);
        }

        public override void OnStop()
        {
            this.Conversation.text = "";
            this.Panel.SetActive(false);
        }
    }
}
