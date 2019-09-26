using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Network;

public class Menu : MonoBehaviour
{
    public Text user_IP_text;
    public Text input_username_text;
    public Text input_ip_text;
    public Button join_button;
    public Button host_button;

    // Start is called before the first frame update
    void Start()
    {
        user_IP_text.text = "Your IP : " + Network.Utils.GetIPAddress();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void doExitGame()
    {
        Application.Quit();
        // @TODO : To test after build, as it has no effects in editor
        Debug.Log("Game has exited");
    }

    public void LaunchGame()
    {
        // @TODO : To implement
        Debug.Log("Game launched");
    }
    public void HostGame()
    {
        // @TODO : To implement : Block Join and Solo button, Input fields,  Transform Host button to Cancel, and Display that we are looking for a user
        Debug.Log("Listening for play request on address : " + Network.Utils.GetIPAddress() + " as " + input_username_text.text);
    }
    public void JoinGame()
    {
        // @TODO : To implement : Block Host and Solo buttons, Input fields, Transform Join button to Cancel, and Display that we are looking for a server
        Debug.Log("Joining game on address : " + input_ip_text.text + " as " + input_username_text.text);
    }
}
