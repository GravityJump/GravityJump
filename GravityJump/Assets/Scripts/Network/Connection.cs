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
        NetworkStream Stream { get; set; }

        public Connection(TcpClient client)
        {
            this.Client = client;
            this.Stream = this.Client.GetStream();
        }

        public Connection(IPAddress ip)
        {
            this.Client = new TcpClient(ip.ToString(), this.Port);
            this.Stream = this.Client.GetStream();
            Debug.Log($"Connection established with {ip.ToString()}");
        }

        public void Write(Payload payload)
        {
            this.Stream.Write(payload.GetBytes(), 0, payload.Length());
        }

        public void Read()
        {
            if (this.Stream.DataAvailable)
            {
                byte[] buffer = new byte[256];

                this.Stream.Read(buffer, 0, 1);
                if (buffer[0] == (byte)Network.OpCode.Message)
                {
                    this.Stream.Read(buffer, 0, 4);
                    int msgLength = BitConverter.ToInt32(buffer, 0);
                    this.Stream.Read(buffer, 0, msgLength);
                    Debug.Log($"{Encoding.UTF8.GetString(buffer)}");
                }
            }
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
