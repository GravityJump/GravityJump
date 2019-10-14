using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UI
{
    public class TitleScreen : BasicScreen
    {
        Text Caption;
        IEnumerator blinker;

        public override void Awake()
        {
            this.Panel = GameObject.Find("UICanvas/TitleScreen");
            this.Caption = GameObject.Find("UICanvas/TitleScreen/PressStart").GetComponent<Text>();
            this.blinker = this.BlinkCaption(0.7f);
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            StartCoroutine(this.blinker);
        }

        public override void OnStop()
        {
            StopCoroutine(this.blinker);
            this.Panel.SetActive(false);
        }

        IEnumerator BlinkCaption(float frequency)
        {
            while (true)
            {
                this.Caption.gameObject.SetActive(true);
                yield return new WaitForSeconds(frequency);
                this.Caption.gameObject.SetActive(false);
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}
