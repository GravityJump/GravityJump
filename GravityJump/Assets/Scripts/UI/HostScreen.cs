using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HostScreen : BasicScreen
    {
        public Button Back;

        public override void Awake()
        {
            this.Panel = GameObject.Find("UICanvas/HostScreen");
            this.Back = GameObject.Find("UICanvas/HostScreen/BackButton").GetComponent<Button>();
        }
    }
}
