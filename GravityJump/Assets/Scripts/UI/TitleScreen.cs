using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace UI
{
    public class TitleScreen : BasicScreen
    {
        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/TitleScreen");
        }
    }
}
