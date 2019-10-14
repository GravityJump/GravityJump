using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HostScreen : BasicScreen
    {
        public Button Back;

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/HostScreen");
            this.Back = GameObject.Find("Canvas/HostScreen/BackButton").GetComponent<Button>();
        }
    }
}
