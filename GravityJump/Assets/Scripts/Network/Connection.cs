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
                    case (byte)OpCode.PlayerCoordinates:
                        this.Stream.Read(buffer, 0, 12);
                        float xPlayer = BitConverter.ToSingle(buffer, 0);
                        float yPlayer = BitConverter.ToSingle(buffer, 4);
                        float zAnglePlayer = BitConverter.ToSingle(buffer, 8);
                        return new PlayerCoordinates(xPlayer, yPlayer, zAnglePlayer);
                    case (byte)OpCode.Spawn:
                        this.Stream.Read(buffer, 0, 23);
                        ObjectManagement.SpawnerType spawnerType = (ObjectManagement.SpawnerType)buffer[0];
                        int assetId = (int)buffer[1];
                        float x = BitConverter.ToSingle(buffer, 2);
                        float y = BitConverter.ToSingle(buffer, 6);
                        float z = BitConverter.ToSingle(buffer, 10);
                        float rotation = BitConverter.ToSingle(buffer, 14);
                        float scaleRatio = BitConverter.ToSingle(buffer, 18);
                        return new SpawnerPayload(spawnerType, assetId, new Vector3(x, y, z), rotation, scaleRatio);
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
