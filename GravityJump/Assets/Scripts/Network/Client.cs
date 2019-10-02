using UnityEngine.Networking;

namespace Network
{
    public class Client
    {
        public int Port { get; private set; }
        public string Ip { get; private set; }
        // ConnectionConfig Config;
        // int ChannelId;
        // HostTopology Topology;
        // int HostId;
        // int ConnectionId;

        public Client(int port)
        {
            this.Port = port;
            this.Ip = "";
            // this.Config = new ConnectionConfig();
            // this.ChannelId = this.Config.AddChannel(QosType.Reliable);
            // this.Topology = new HostTopology(this.Config, 1);
            // this.HostId = NetworkTransport.AddHost(this.Topology, this.Port);
        }
    }
}
