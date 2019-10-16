using System.Text;
using System;

namespace Network
{
    public enum OpCode : byte
    {
        Raw,
        Message,
    }

    public abstract class Payload
    {
        public OpCode Code { get; protected set; }

        public Payload()
        {
            this.Code = OpCode.Raw;
        }

        public virtual byte[] GetBytes()
        {
            return new byte[] { };
        }
    }

    public class Message : Payload
    {
        public string Text { get; set; }

        public Message(string text)
        {
            this.Code = OpCode.Message;
            this.Text = text;
        }

        public override byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.Text);
        }
    }

    public class Parser
    {
        public static Payload Parse(byte[] data)
        {
            if (data.Length < 1)
            {
                throw new Exception("empty data");
            }

            switch (data[0])
            {
                case (byte)OpCode.Message:
                    byte[] msg = new byte[data.Length + 3];
                    Array.Copy(data, 1, BitConverter.GetBytes(msg.Length), 0, 4);
                    Array.Copy(data, 5, msg, 0, msg.Length);
                    return new Message(Encoding.UTF8.GetString(msg));
                default:
                    throw new Exception("invalid opcode");
            }
        }
    }
}
