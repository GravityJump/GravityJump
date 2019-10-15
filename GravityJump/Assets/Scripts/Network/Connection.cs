using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
                TcpClient client = this.Listener.AcceptTcpClient();
                this.RemoteIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                Debug.Log($"Connection established with {this.RemoteIp.ToString()}");
                this.Stream = client.GetStream();

                this.Status = Status.Established;
            }
        }

        public void Stop()
        {
            this.RegistrationThread.Interrupt();
            this.Listener.Stop();
            this.Status = Status.None;
            this.RemoteIp = null;
            this.Stream = null;
            this.Listener = null;
            this.Client = null;
            this.RegistrationThread = null;
        }
    }
}
