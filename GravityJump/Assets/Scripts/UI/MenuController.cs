using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MenuController : MonoBehaviour
{
    public GameObject TitleScreen;
    public GameObject GameModeScreen;
    public GameObject HostScreen;
    public GameObject JoinScreen;
    public Text Version;
    public readonly string versionNumber = "0.0.1";
    public readonly int port = 3000;
    public Text HostIP;
    public HostTopology topology;
    public int HostId;
    public int connectionId;
    public int channelId;
    void Start()
    {
        this.Version.text = $"Version {this.versionNumber}";
        this.GameModeScreen.gameObject.SetActive(false);
        this.HostScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(false);

        GlobalConfig gConfig = new GlobalConfig();
        // gConfig.MaxPacketSize = 500;
        NetworkTransport.Init(gConfig);

        ConnectionConfig config = new ConnectionConfig();
        int myReliableChannelId = config.AddChannel(QosType.Reliable);
        this.topology = new HostTopology(config, 1);
        this.HostId = NetworkTransport.AddHost(this.topology, 8888);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.TitleScreen.gameObject.SetActive(false);
            this.GameModeScreen.gameObject.SetActive(true);
        }

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing: break;
            case NetworkEventType.ConnectEvent:
                Debug.Log($"connection {connectionId} initiated");
                break;
            case NetworkEventType.DataEvent: break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log($"connection {connectionId} closed");
                break;
            case NetworkEventType.BroadcastEvent: break;
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
    }

    public void Join()
    {
        this.GameModeScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(true);
    }

    public void SendJoinRequest()
    {
        byte error;
        this.connectionId = NetworkTransport.Connect(this.HostId, this.HostIP.text, 8888, 0, out error);
    }

    public void GameMode()
    {
        this.GameModeScreen.gameObject.SetActive(true);
        this.HostScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(false);
    }
}
