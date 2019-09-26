using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject TitleScreen;
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.TitleScreen.gameObject.SetActive(false);
        }
    }
}
