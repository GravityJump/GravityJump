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

        public void Write(string msg)
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte)OpCode.Message);
            payload.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(msg)));
            payload.AddRange(Encoding.UTF8.GetBytes(msg));
            NetworkStream stream = this.Client.GetStream();
            stream.Write(payload.ToArray(), 0, payload.Count);
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
