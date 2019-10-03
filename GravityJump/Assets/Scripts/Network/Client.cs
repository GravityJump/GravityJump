using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using UnityEngine;

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

    public class UDPSocket
    {
        public Node Self { get; private set; }
        public Node Peer { get; set; }

        Socket Socket;
        State Buffer;
        EndPoint epFrom;
        AsyncCallback recv = null;

        public class State
        {
            public readonly int bufferSize = 1024;
            public byte[] buffer;

            public State()
            {
                this.buffer = new byte[this.bufferSize];
            }
        }

        public UDPSocket(string ip, int port)
        {
            this.Self = new Node(ip, port);
            this.Peer = null;

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.Buffer = new State();
            this.epFrom = new IPEndPoint(IPAddress.Any, 0);
        }

        public void Server(string address, int port)
        {
            this.Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            this.Socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        public void Client(string address, int port)
        {
            this.Socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            this.Socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = this.Socket.EndSend(ar);
                Debug.Log($"SEND: {bytes}, {text}");
            }, this.Buffer);
        }

        private void Receive()
        {
            this.Socket.BeginReceiveFrom(this.Buffer.buffer, 0, this.Buffer.bufferSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = this.Socket.EndReceiveFrom(ar, ref epFrom);
                this.Socket.BeginReceiveFrom(so.buffer, 0, this.Buffer.bufferSize, SocketFlags.None, ref epFrom, recv, so);
                Debug.Log($"RECV: {this.epFrom.ToString()}: {bytes}, {Encoding.ASCII.GetString(so.buffer, 0, bytes)}");
            }, this.Buffer);
        }
    }
}
