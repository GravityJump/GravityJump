using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // HelpScreen displays the help panel.
    public class HelpScreen : BasicScreen
    {
        public Button Back { get; set; }

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/HelpScreen");
            this.Back = GameObject.Find("Canvas/HelpScreen/BackButton").GetComponent<Button>();
            this.Name = Names.Menu.Help;
        }
    }
}
