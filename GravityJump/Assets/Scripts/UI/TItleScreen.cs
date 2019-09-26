using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TItleScreen : MonoBehaviour
{
    public Text PressStart;
    void Start()
    {
        StartCoroutine(this.ToggleVisibility());
    }
    private IEnumerator ToggleVisibility()
    {
        while (true)
        {
            this.PressStart.enabled = false;
            yield return new WaitForSeconds(0.7f);
            this.PressStart.enabled = true;
            yield return new WaitForSeconds(0.7f);
        }
    }
    void Update()
    {

    }
}
