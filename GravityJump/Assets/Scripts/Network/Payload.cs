using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    // OpCode is an enumeration of the possible payload types.
    public enum OpCode : byte
    {
        Raw,
        Message,
        Ready,
        PlayerCoordinates,
        Spawn,
        Death,
    }

    // Payload is an interface that can be serialized and deserialized into an array of bytes.
    public interface Payload
    {
        byte[] GetBytes();
        int Length();
    }

    // BasicPayload is a virtual class implementing Payload with no data.
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

    // Message implements Payload, to encode an UTF-8 string of arbitrary length. It is used to send messages on the game chat.
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

    // Ready implements Payload, to send a start signal to the other player.
    public class Ready : BasePayload
    {
        public Ready()
        {
            this.Code = OpCode.Ready;
        }

        public override byte[] GetBytes()
        {
            return new byte[] { (byte)this.Code };
        }

        public override int Length()
        {
            return 1;
        }
    }

    // PlayerCoordinates implements Payload, to send the local player position.
    public class PlayerCoordinates : BasePayload
    {
        public Physic.Coordinates2D coordinates2D;

        public PlayerCoordinates(float x, float y, float zAngle)
        {
            this.Code = OpCode.PlayerCoordinates;
            this.coordinates2D = new Physic.Coordinates2D(x, y, zAngle);
        }

        public PlayerCoordinates(Physic.Coordinates2D coordinates)
        {
            this.Code = OpCode.PlayerCoordinates;
            this.coordinates2D = coordinates;
        }

        public override byte[] GetBytes()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte)this.Code);
            payload.AddRange(BitConverter.GetBytes(this.coordinates2D.X));
            payload.AddRange(BitConverter.GetBytes(this.coordinates2D.Y));
            payload.AddRange(BitConverter.GetBytes(this.coordinates2D.ZAngle));
            return payload.ToArray();
        }

        public override int Length()
        {
            return 1 + 3 * 4;
        }
    }

    // SpawnerPayload implements Payload, to send randomly generated objects to the non-host player.
    public class SpawnerPayload : BasePayload
    {
        public Vector3 position;
        public float rotation;
        public float scaleRatio;
        public ObjectManagement.SpawnerType spawnerType;
        public int assetId;

        public SpawnerPayload(ObjectManagement.SpawnerType spawnerType, int assetId, Vector3 position, float rotation, float scaleRatio)
        {
            this.Code = OpCode.Spawn;
            this.spawnerType = spawnerType;
            this.assetId = assetId;
            this.position = position;
            this.rotation = rotation;
            this.scaleRatio = scaleRatio;
        }

        public override byte[] GetBytes()
        {
            List<byte> payload = new List<byte>();
            payload.Add((byte)this.Code);
            payload.Add((byte)this.spawnerType);
            payload.AddRange(BitConverter.GetBytes(this.assetId));
            payload.AddRange(BitConverter.GetBytes(this.position.x));
            payload.AddRange(BitConverter.GetBytes(this.position.y));
            payload.AddRange(BitConverter.GetBytes(this.position.z));
            payload.AddRange(BitConverter.GetBytes(this.rotation));
            payload.AddRange(BitConverter.GetBytes(this.scaleRatio));
            return payload.ToArray();
        }

        public override int Length()
        {
            return 2 + 6 * 4;
        }
    }

    // Death implements Payload, to send a game over signal to the other player.
    public class Death : BasePayload
    {
        public Death()
        {
            this.Code = OpCode.Death;
        }

        public override byte[] GetBytes()
        {
            return new byte[] { (byte)this.Code };
        }

        public override int Length()
        {
            return 1;
        }
    }
}
