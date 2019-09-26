using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject TitleScreen;
    public GameObject GameModeScreen;
    public Text Version;
    public readonly string versionNumber = "0.0.1";
    void Start()
    {
        this.Version.text = "Version " + this.versionNumber;
        this.GameModeScreen.gameObject.SetActive(false);
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
}
