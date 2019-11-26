using UnityEngine;

namespace UI
{
    public class TitleScreen : BasicScreen
    {
        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/TitleScreen");
            this.Name = Names.Menu.Title;
        }
    }
}
