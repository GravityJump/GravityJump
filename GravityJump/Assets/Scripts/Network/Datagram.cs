using System.Net;
using System;

namespace Network
{
    interface Datagram
    {
        byte[] GetBytes();
    }

    enum Instruction : int
    {
        RegistrationRequest,
    };

    class RegistrationRequest : Datagram
    {
        public readonly static byte[] InstructionCode = BitConverter.GetBytes((int)Instruction.RegistrationRequest);
        public IPAddress Ip { get; private set; }
        public int Port { get; private set; }
        public RegistrationRequest(IPAddress ip, int port)
        {
            this.Ip = ip;
            this.Port = port;
        }
        public byte[] GetBytes()
        {
            byte[] ip = this.Ip.GetAddressBytes();
            byte[] port = BitConverter.GetBytes(this.Port);
            byte[] payload = new byte[4 + ip.Length + port.Length];

            Buffer.BlockCopy(InstructionCode, 0, payload, 0, InstructionCode.Length);
            Buffer.BlockCopy(ip, 0, payload, InstructionCode.Length, ip.Length);
            Buffer.BlockCopy(port, 0, payload, InstructionCode.Length + ip.Length, port.Length);

            return payload;
        }
    }

    class DatagramParser
    {
        public static Datagram Parse(byte[] bytes)
        {
            byte[] instructionBytes = new byte[4];
            Array.Copy(bytes, instructionBytes, 4);

            if (BitConverter.ToInt32(instructionBytes, 0) == (int)Instruction.RegistrationRequest)
            {
                byte[] ipBytes = new byte[4];
                Array.Copy(bytes, 4, ipBytes, 0, 4);
                byte[] portBytes = new byte[4];
                Array.Copy(bytes, 8, portBytes, 0, 4);
                return new RegistrationRequest(new IPAddress(ipBytes), BitConverter.ToInt32(portBytes, 0));
            }

            throw new Exception("no matching datagram");
        }
    }
}
