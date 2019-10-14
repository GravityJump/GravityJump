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
            this.Panel = GameObject.Find("Canvas/TitleScreen");
            this.Caption = GameObject.Find("Canvas/TitleScreen/PressStart").GetComponent<Text>();
            this.blinker = this.BlinkCaption(0.7f);
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            StartCoroutine(this.blinker);
        }

        public override void OnStop()
        {
            this.Panel.SetActive(false);
            StopCoroutine(this.blinker);
        }

        public override void OnResume()
        {
            this.OnStart();
        }

        public override void OnPause()
        {
            this.OnStop();
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
