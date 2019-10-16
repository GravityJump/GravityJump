using System.Text;
using System;
using System.Collections.Generic;

namespace Network
{
    public enum OpCode : byte
    {
        Raw,
        Message,
        Payload,
    }

    public interface Payload
    {
        byte[] GetBytes();
        int Length();
    }

    public abstract class BasePayload : Payload
    {
        public OpCode Code { get; protected set; }

        public BasePayload()
        {
            this.Code = OpCode.Raw;
        }

        public virtual byte[] GetBytes()
        {
            return new byte[] { };
        }

        public virtual int Length()
        {
            return 0;
        }
    }

    public class Message : BasePayload
    {
        public string Text { get; set; }

        public Message(string text)
        {
            this.Code = OpCode.Message;
            this.Text = text;
        }

        public override byte[] GetBytes()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte)OpCode.Message);
            payload.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(this.Text)));
            payload.AddRange(Encoding.UTF8.GetBytes(this.Text));
            return payload.ToArray();
        }

        public override int Length()
        {
            return Encoding.UTF8.GetByteCount(this.Text) + 5;
        }
    }
}
