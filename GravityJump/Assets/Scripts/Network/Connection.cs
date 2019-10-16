using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.Collections.Generic;

namespace Network
{
    public enum Status
    {
        None,
        Established,
        Broken,
    }

    public class Connection
    {
        public readonly int Port = 8000;
        public Status Status { get; private set; }
        public IPAddress RemoteIp { get; private set; }
        NetworkStream Stream { get; set; }
        TcpListener Listener { get; set; }
        TcpClient Client { get; set; }
        Thread RegistrationThread { get; set; }

        public Connection()
        {
            this.Status = Status.None;
            this.RemoteIp = null;
            this.Stream = null;
            this.Listener = null;
            this.Client = null;
            this.RegistrationThread = null;
        }

        public void Listen()
        {
            Debug.Log($"Server initialization on port {this.Port}...");
            this.Listener = new TcpListener(IPAddress.Parse("0.0.0.0"), this.Port);
            this.Listener.Start();

            this.RegistrationThread = new Thread(new ThreadStart(this.WaitForValidConnection));
            this.RegistrationThread.Start();
        }

        void WaitForValidConnection()
        {
            while (this.Status != Status.Established)
            {
                Debug.Log($"Server waiting for connection");
                this.Client = this.Listener.AcceptTcpClient();
                this.RemoteIp = ((IPEndPoint)this.Client.Client.RemoteEndPoint).Address;
                Debug.Log($"Connection established with {this.RemoteIp.ToString()}");
                this.Stream = this.Client.GetStream();
                this.Status = Status.Established;
            }
        }

        public void To(IPAddress ip)
        {
            this.RemoteIp = ip;
            this.Client = new TcpClient(this.RemoteIp.ToString(), this.Port);
            Debug.Log($"Connection established with {this.RemoteIp.ToString()}");
            this.Stream = this.Client.GetStream();
            this.Status = Status.Established;
        }

        public void Stop()
        {
            if (this.RegistrationThread != null)
            {
                this.RegistrationThread.Interrupt();
            }
            if (this.Listener != null)
            {
                this.Listener.Stop();
            }
            if (this.Client != null)
            {
                this.Client.Close();
            }
            this.Status = Status.None;
            this.RemoteIp = null;
            this.Stream = null;
            this.Listener = null;
            this.Client = null;
            this.RegistrationThread = null;
        }

        public void SendMessage(string msg)
        {
            List<byte> payload = new List<Byte>();
            payload.Add((byte)Network.OpCode.Message);
            payload.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(msg)));
            payload.AddRange(Encoding.UTF8.GetBytes(msg));

            this.Stream.Write(payload.ToArray(), 0, payload.Count);
        }
    }
}
