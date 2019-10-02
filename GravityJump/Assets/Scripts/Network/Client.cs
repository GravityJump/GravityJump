using UnityEngine.Networking;

namespace Network
{
    public struct Node
    {
        public int Port { get; set; }
        public string Ip { get; set; }

        public Node(string ip, int port)
        {
            this.Ip = ip;
            this.Port = port;
        }
    }

    public class Client
    {
        public Node Self { get; set; }
        // ConnectionConfig Config;
        // int ChannelId;
        // HostTopology Topology;
        // int HostId;
        // int ConnectionId;

        public Client(string ip, int port)
        {
            this.Self = new Node(ip, port);

            // this.Config = new ConnectionConfig();
            // this.ChannelId = this.Config.AddChannel(QosType.Reliable);
            // this.Topology = new HostTopology(this.Config, 1);
            // this.HostId = NetworkTransport.AddHost(this.Topology, this.Port);
        }
    }
}
