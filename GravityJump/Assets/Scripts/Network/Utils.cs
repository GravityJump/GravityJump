using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Network
{
    public class Utils
    {
        public static string GetHostIpAddress()
        {
            System.Net.IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system");
        }

        public static bool IsInternetAvailable()
        {
            Ping pinger = new Ping();
            try
            {
                return pinger.Send("1.1.1.1").Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}
