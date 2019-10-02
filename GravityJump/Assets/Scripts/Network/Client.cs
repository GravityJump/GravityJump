using UnityEngine.Networking;

namespace Network
{
    public class Client
    {
        ConnectionConfig Config;
        int ChannelId;
        HostTopology Topology;
        int HostId;
        int Port;
        int ConnectionId;

        public Client(int port)
        {
            this.Port = port;
            this.Config = new ConnectionConfig();
            this.ChannelId = this.Config.AddChannel(QosType.Reliable);
            this.Topology = new HostTopology(this.Config, 1);
            this.HostId = NetworkTransport.AddHost(this.Topology, this.Port);
        }

        public void Receive()
        {

        }
    }
}
