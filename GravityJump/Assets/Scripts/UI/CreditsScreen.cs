using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    // CreditsScreen simply displays the game credits.
    public class CreditsScreen : BasicScreen
    {
        public Button Back { get; set; }

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/CreditsScreen");
            this.Back = GameObject.Find("Canvas/CreditsScreen/BackButton").GetComponent<Button>();
            this.Name = Names.Menu.Credits;
        }
    }
}
