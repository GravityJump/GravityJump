using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class MenuController : MonoBehaviour
{
    public GameObject TitleScreen;
    public GameObject GameModeScreen;
    public GameObject HostScreen;
    public GameObject JoinScreen;
    public Text Version;
    public readonly string versionNumber = "0.0.1";
    public readonly int port = 3000;
    public Network.Node Node { get; set; }
    public Thread registrationThread { get; set; }

    public Text HostIP;
    void Start()
    {
        this.Node = null;
        this.HostIP = null;
        this.registrationThread = null;
        this.Version.text = $"Version {this.versionNumber}";
        this.GameModeScreen.gameObject.SetActive(false);
        this.HostScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.TitleScreen.gameObject.SetActive(false);
            this.GameModeScreen.gameObject.SetActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Solo()
    {
        Debug.Log("Start a solo game");
    }

    public void Host()
    {
        this.GameModeScreen.gameObject.SetActive(false);
        this.HostScreen.gameObject.SetActive(true);
        this.Node = new Network.Host(Network.Utils.GetIPAddress(), this.port);

        this.registrationThread = new Thread(new ThreadStart(((Network.Host)this.Node).StartRegistration));
        this.registrationThread.Start();
    }

    public void Join()
    {
        this.GameModeScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(true);
        this.Node = new Network.Client(Network.Utils.GetIPAddress(), this.port);
    }

    public void SendJoinRequest()
    {
        ((Network.Client)this.Node).Neighbour = new Network.Node(this.HostIP.text, this.port);
        ((Network.Client)this.Node).Register();
    }

    public void GameMode()
    {
        this.GameModeScreen.gameObject.SetActive(true);
        this.HostScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(false);
        if (this.registrationThread != null)
        {
            this.registrationThread.Interrupt();
        }
    }
}
