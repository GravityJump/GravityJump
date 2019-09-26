using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TItleScreen : MonoBehaviour
{
    public Text press_start_text;
    void Start()
    {
        StartCoroutine(this.ToggleVisibility());
    }
    private IEnumerator ToggleVisibility()
    {
        while (true)
        {
            this.press_start_text.enabled = false;
            yield return new WaitForSeconds(0.7f);
            this.press_start_text.enabled = true;
            yield return new WaitForSeconds(0.7f);
        }
    }
    void Update()
    {

    }
}
