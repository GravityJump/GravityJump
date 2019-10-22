using System.Text;
using System;
using System.Collections.Generic;

namespace Network
{
    public enum OpCode : byte
    {
        Raw,
        Message,
        Ready,
        PlayerCoordinates
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

    public class Ready : BasePayload
    {
        public Ready()
        {
            this.Code = OpCode.Ready;
        }

        public override byte[] GetBytes()
        {
            return new byte[] { (byte)OpCode.Ready };
        }

        public override int Length()
        {
            return 1;
        }
    }

    public class PlayerCoordinates : BasePayload
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float ZAngle { get; set; }

        public PlayerCoordinates(float x, float y, float zAngle)
        {
            this.Code = OpCode.PlayerCoordinates;
            this.X = x;
            this.Y = y;
            this.ZAngle = zAngle;
        }

        public override byte[] GetBytes()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte)this.Code);
            payload.AddRange(BitConverter.GetBytes(this.X));
            payload.AddRange(BitConverter.GetBytes(this.Y));
            payload.AddRange(BitConverter.GetBytes(this.ZAngle));
            return payload.ToArray();
        }

        public override int Length()
        {
            return 1 + 3 * 4;
        }
    }
}
