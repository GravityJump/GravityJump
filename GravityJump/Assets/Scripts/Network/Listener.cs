using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.Collections.Generic;

namespace Network
{
    public class Listener : TcpConfig
    {
        TcpListener Listener { get; set; }
        Thread RegistrationThread { get; set; }

        public Listener()
        {
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



            this.Listener.Stop();
        }
    }
}
