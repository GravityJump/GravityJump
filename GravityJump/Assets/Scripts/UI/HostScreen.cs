using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // HostScreen starts a TcpListener waiting for connections.
    public class HostScreen : BasicScreen
    {
        public Button Back { get; set; }

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/HostScreen");
            this.Back = GameObject.Find("Canvas/HostScreen/BackButton").GetComponent<Button>();
            this.Name = Names.Menu.Host;
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
        }

        public override void OnStop()
        {
            this.Panel.SetActive(false);
        }

        public override void OnPause()
        {
            this.Panel.SetActive(false);
        }
    }
}
