using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class HostScreen : BasicScreen
    {
        public Button Back { get; set; }
        private Network.Listener Listener { get; set; }

        public override void Awake()
        {
            this.Panel = GameObject.Find("Canvas/HostScreen");
            this.Back = GameObject.Find("Canvas/HostScreen/BackButton").GetComponent<Button>();
            this.Listener = new Network.Listener();
            this.Name = Names.Menu.Host;
        }

        public override void OnStart()
        {
            this.Panel.SetActive(true);
            this.Listener.Start();
        }

        public override void OnStop()
        {
            this.Listener.Stop();
            this.Panel.SetActive(false);
        }

        public Network.Connection GetConnection()
        {
            return this.Listener.GetConnection();
        }
    }
}
