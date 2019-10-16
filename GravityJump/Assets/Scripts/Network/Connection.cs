using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

namespace Network
{
    public class Connection : TcpConfig
    {
        TcpClient Client { get; set; }

        public Connection(TcpClient client)
        {
            this.Client = client;
        }

        public Connection(IPAddress ip)
        {
            this.Client = new TcpClient(ip.ToString(), this.Port);
            Debug.Log($"Connection established with {ip.ToString()}");
        }

        public void Write(Payload payload)
        {
            NetworkStream stream = this.Client.GetStream();
            stream.Write(payload.GetBytes(), 0, payload.Length());
        }

        public void Close()
        {
            try
            {
                Debug.Log("Connection closed");
                this.Client.Close();
            }
            catch
            {
                Debug.Log("Could not close the connection");
            }
        }
    }
}
