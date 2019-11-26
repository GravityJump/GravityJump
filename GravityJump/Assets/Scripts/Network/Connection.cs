using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.Text;

namespace Network
{
    public class Connection : TcpConfig
    {
        private readonly int buffer_size = 4096;

        private TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
        private byte[] Buffer { get; set; }
        public IPAddress Ip { get; set; }

        public Connection(TcpClient client)
        {
            this.Client = client;
            this.Stream = this.Client.GetStream();
            this.Ip = ((IPEndPoint)this.Client.Client.RemoteEndPoint).Address;
            this.Buffer = new byte[this.buffer_size];
        }

        public Connection(IPAddress ip)
        {
            this.Client = new TcpClient(ip.ToString(), this.Port);
            this.Stream = this.Client.GetStream();
            this.Ip = ip;
            this.Buffer = new byte[this.buffer_size];
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
                this.Stream.Read(this.Buffer, 0, 1);
                switch (this.Buffer[0])
                {
                    case (byte)OpCode.Message:
                        this.Stream.Read(this.Buffer, 0, 4);
                        int msgLength = BitConverter.ToInt32(this.Buffer, 0);
                        this.Stream.Read(this.Buffer, 0, msgLength);
                        return new Message(Encoding.UTF8.GetString(this.Buffer));
                    case (byte)OpCode.Ready:
                        return new Ready();
                    case (byte)OpCode.PlayerCoordinates:
                        this.Stream.Read(this.Buffer, 0, 12);
                        float xPlayer = BitConverter.ToSingle(this.Buffer, 0);
                        float yPlayer = BitConverter.ToSingle(this.Buffer, 4);
                        float zAnglePlayer = BitConverter.ToSingle(this.Buffer, 8);
                        return new PlayerCoordinates(xPlayer, yPlayer, zAnglePlayer);
                    case (byte)OpCode.Spawn:
                        this.Stream.Read(this.Buffer, 0, 25);
                        ObjectManagement.SpawnerType spawnerType = (ObjectManagement.SpawnerType)this.Buffer[0];
                        int assetId = BitConverter.ToInt32(this.Buffer, 1);
                        float x = BitConverter.ToSingle(this.Buffer, 5);
                        float y = BitConverter.ToSingle(this.Buffer, 9);
                        float z = BitConverter.ToSingle(this.Buffer, 13);
                        float rotation = BitConverter.ToSingle(this.Buffer, 17);
                        float scaleRatio = BitConverter.ToSingle(this.Buffer, 21);
                        return new SpawnerPayload(spawnerType, assetId, new Vector3(x, y, z), rotation, scaleRatio);
                    case (byte)OpCode.Death:
                        return new Death();
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
