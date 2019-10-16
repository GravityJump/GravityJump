using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Network
{
    public class Listener : TcpConfig
    {
        TcpListener TCPListener { get; set; }
        TcpClient TCPClient { get; set; }

        public Listener()
        {
            this.TCPListener = null;
            this.TCPClient = null;
        }

        public void Start()
        {
            Debug.Log($"Server initialization on port {this.Port}...");
            this.TCPListener = new TcpListener(IPAddress.Parse("0.0.0.0"), this.Port);
            this.TCPListener.Start();

            Thread th = new Thread(new ThreadStart(this.WaitForConnection));
            th.Start();
        }

        void WaitForConnection()
        {
            this.TCPClient = TCPListener.AcceptTcpClient();
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

        public Network.Connection GetConnection()
        {
            if (this.TCPClient != null)
            {
                return new Connection(this.TCPClient);
            }

            return null;
        }
    }
}
