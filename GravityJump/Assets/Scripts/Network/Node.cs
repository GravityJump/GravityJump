using System.Net;
using System;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;

namespace Network
{
    public class Node
    {
        static int _nodeCounter = 0;
        public IPAddress Ip { get; protected set; }
        public int Port { get; protected set; }
        public Node Neighbour { get; set; }

        public Node(string ip, int port)
        {
            if (_nodeCounter > 2)
            {
                throw new Exception("only two instances of Node should exist, one host and one client");
            }

            this.Ip = IPAddress.Parse(ip);
            this.Port = port;
            this.Neighbour = null;

            _nodeCounter++;
        }

        public void Send(Datagram datagram)
        {
            UdpClient client = new UdpClient();
            client.Connect(this.Neighbour.Ip, this.Neighbour.Port);
            byte[] payload = datagram.GetBytes();
            client.Send(payload, payload.Length);
            client.Close();
        }

        public Datagram ReceiveFromAll()
        {
            UdpClient client = new UdpClient(this.Port);
            IPEndPoint originIP = new IPEndPoint(IPAddress.Any, 0);
            return DatagramParser.Parse(client.Receive(ref originIP));
        }

        public Datagram Receive()
        {
            UdpClient client = new UdpClient(this.Port);
            IPEndPoint originIP = new IPEndPoint(this.Neighbour.Ip, 0);
            return DatagramParser.Parse(client.Receive(ref originIP));
        }
    }

    class Host : Node
    {
        public Host(string ip, int port) : base(ip, port) { }
        public void StartRegistration()
        {
            Debug.Log($"waiting for registration on port {this.Port.ToString()}");

            RegistrationRequest request = (RegistrationRequest)this.ReceiveFromAll();

            this.Neighbour = new Node(request.Ip.ToString(), request.Port);
            Debug.Log($"received request for new registration from IP {this.Neighbour.Ip.ToString()}, listening on port: {this.Neighbour.Port.ToString()}, assigning listening port: {this.Port.ToString()}");
        }
    }

    class Client : Node
    {

        public Client(string ip, int port) : base(ip, port)
        {
            this.Neighbour = null;
        }
        public Client(string ip, int port, string hostIP, int hostPort) : base(ip, port)
        {
            this.Neighbour = new Node(hostIP, hostPort);
        }

        public void Register()
        {
            this.Send(new RegistrationRequest(this.Ip, this.Port));
        }
    }
}
