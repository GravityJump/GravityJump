using UnityEngine;

namespace UI
{
    // TitleScreen just displays the game title.
    public class TitleScreen : BasicScreen
    {
        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/TitleScreen");
            this.Name = Names.Menu.Title;
        }
    }
}
