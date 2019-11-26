using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Network
{
    public class Utils
    {
        // GetHostIpAddress returns the IP address of the local host.
        public static string GetHostIpAddress()
        {
            if (SystemInfo.operatingSystem.Contains("Mac"))
            {
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.Name == "en0")
                    {
                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation item in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return item.Address.ToString();
                            }
                        }
                    }
                }
            }

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

        // IsInternetAvailable performs a ping to the Cloudflare DNS server to check that Internet is accessible.
        public static bool IsInternetAvailable()
        {
            System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping();
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
