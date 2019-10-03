using UnityEngine.Networking;
using UnityEngine;
using System;

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
        public Node Self { get; private set; }
        public Node Peer { get; set; }
        ConnectionConfig Config { get; set; }
        int ChannelId { get; set; }
        HostTopology Topology { get; set; }
        int SocketId { get; set; }
        int ConnectionId { get; set; }

        public Client(string ip, int port)
        {
            this.Self = new Node(ip, port);
            this.Peer = null;

            NetworkTransport.Init();

            this.Config = new ConnectionConfig();
            this.ChannelId = this.Config.AddChannel(QosType.Reliable);
            this.Topology = new HostTopology(this.Config, 1);
            this.SocketId = NetworkTransport.AddHost(this.Topology, this.Self.Port);
            this.ConnectionId = -1;
        }

        public void Connect()
        {
            if (this.Peer != null)
            {
                byte error;
                this.ConnectionId = NetworkTransport.Connect(this.SocketId, this.Peer.Ip, this.Peer.Port, 0, out error);
            }
        }

        public void Send(string message)
        {
            if (this.ConnectionId != -1)
            {
                byte error;
                byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(message);
                NetworkTransport.Send(this.SocketId, this.ConnectionId, this.ChannelId, buffer, buffer.Length, out error);
            }
        }

        public void Disconnect()
        {
            if (this.ConnectionId != -1)
            {
                byte error;
                NetworkTransport.Disconnect(this.SocketId, this.ConnectionId, out error);
            }
        }

        public byte[] Receive()
        {
            int socketId;
            int connectionId;
            int channelId;
            byte[] buffer = new byte[1024];
            int dataSize;
            byte error;
            NetworkEventType networkEvent = NetworkTransport.Receive(out socketId, out connectionId, out channelId, buffer, buffer.Length, out dataSize, out error);

            switch (networkEvent)
            {
                case NetworkEventType.Nothing:
                case NetworkEventType.BroadcastEvent:
                    break;
                case NetworkEventType.ConnectEvent:
                    if (connectionId == this.ConnectionId)
                    {
                        Debug.Log("Connection request accepted by peer");
                    }
                    else
                    {
                        Debug.Log("New connection request from a peer");
                    }
                    break;
                case NetworkEventType.DataEvent:
                    return buffer;
                case NetworkEventType.DisconnectEvent:
                    if (connectionId == this.ConnectionId)
                    {
                        Debug.Log("Connection refused");
                    }
                    else
                    {
                        Debug.Log("Peer disconnected");
                    }
                    break;
            }

            return null;
        }
    }
}
