using UnityEngine.Networking;

namespace Network
{
    public class Node
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
        public Node Peer { get; set; }
        ConnectionConfig Config { get; set; }
        int ChannelId { get; set; }
        HostTopology Topology { get; set; }
        int HostId { get; set; }

        public Client(string ip, int port)
        {
            this.Self = new Node(ip, port);
            this.Peer = null;

            NetworkTransport.Init();

            this.Config = new ConnectionConfig();
            this.ChannelId = this.Config.AddChannel(QosType.Reliable);
            this.Topology = new HostTopology(this.Config, 1);
            this.HostId = NetworkTransport.AddHost(this.Topology, this.Self.Port);
        }
    }
}
