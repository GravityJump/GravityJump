using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;

namespace Network
{
    public class Connection : TcpConfig
    {
        TcpClient Client { get; set; }
        NetworkStream Stream { get; set; }
        public IPAddress Ip { get; set; }

        public Connection(TcpClient client)
        {
            this.Client = client;
            this.Stream = this.Client.GetStream();
            this.Ip = ((IPEndPoint)this.Client.Client.RemoteEndPoint).Address;
        }

        public Connection(IPAddress ip)
        {
            this.Client = new TcpClient(ip.ToString(), this.Port);
            this.Stream = this.Client.GetStream();
            this.Ip = ip;
            Debug.Log($"Connection established with {this.Ip.ToString()}");
        }

        public void Write(Payload payload)
        {
            this.Stream.Write(payload.GetBytes(), 0, payload.Length());
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

        public BasePayload Read()
        {
            if (this.Stream.DataAvailable)
            {
                byte[] buffer = new byte[256];

                this.Stream.Read(buffer, 0, 1);
                switch (buffer[0])
                {
                    case (byte)OpCode.Message:
                        this.Stream.Read(buffer, 0, 4);
                        int msgLength = BitConverter.ToInt32(buffer, 0);
                        this.Stream.Read(buffer, 0, msgLength);
                        return new Message(Encoding.UTF8.GetString(buffer));
                    case (byte)OpCode.Ready:
                        return new Ready();
                    case (byte)Network.OpCode.PlayerCoordinates:
                        this.Stream.Read(buffer, 0, 12);
                        float x = BitConverter.ToSingle(buffer, 0);
                        float y = BitConverter.ToSingle(buffer, 4);
                        float zAngle = BitConverter.ToSingle(buffer, 8);
                        return new PlayerCoordinates(x, y, zAngle);

                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
