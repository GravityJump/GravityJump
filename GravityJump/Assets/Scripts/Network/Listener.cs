using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace Network
{
    public class Listener : TcpConfig
    {
        TcpListener TCPListener { get; set; }

        public Listener()
        {
            this.TCPListener = null;
        }

        public void Start()
        {
            Debug.Log($"Server initialization on port {this.Port}...");
            this.TCPListener = new TcpListener(IPAddress.Parse("0.0.0.0"), this.Port);
            this.TCPListener.Start();
        }

        public void Stop()
        {
            try
            {
                this.TCPListener.Stop();
                Debug.Log("Server stopped");
            }
            catch
            {
                Debug.Log("Could not stop the server");
            }
        }
    }
}
