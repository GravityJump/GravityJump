using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject TitleScreen;
    public GameObject GameModeScreen;
    public GameObject HostScreen;
    public GameObject JoinScreen;
    public Text Version;
    public readonly string versionNumber = "0.0.1";
    void Start()
    {
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
    }

    public void Join()
    {
        this.GameModeScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(true);
    }

    public void GameMode()
    {
        this.GameModeScreen.gameObject.SetActive(true);
        this.HostScreen.gameObject.SetActive(false);
        this.JoinScreen.gameObject.SetActive(false);
    }
}
