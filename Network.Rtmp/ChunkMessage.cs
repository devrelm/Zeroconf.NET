using System;
using System.Collections.Generic;
using System.Text;

namespace Network.Rtmp
{
    public enum HeaderType : byte
    {
        Type0 = 0,
        Type1 = 1,
        Type2 = 2,
    }

    public enum MessageType : byte
    {
        Type0 = 0,
        Type1 = 1,
        Type2 = 2,
    }

    public class Chunk : IClientResponse<Chunk>
    {
        public HeaderType Type { get; set; }
        public int StreamId { get; set; }
        public ChunkMessage Message { get; set; }

        public Chunk GetResponse(System.IO.Stream stream)
        {
            Chunk message = new Chunk();
            byte b = (byte)stream.ReadByte();
            message.Type = (HeaderType)(b >> 6);
            b = (byte)(b % 64);
            switch (b)
            {
                //If 0 then the stream ID is a 2 bytes long
                case 0:
                    message.StreamId = 64 + stream.ReadByte();
                    break;
                //If 1 then the stream ID is a 3 bytes long
                case 1:
                    message.StreamId = 64 + stream.ReadByte() + stream.ReadByte() * 256;
                    break;
                //If 2 then the stream ID is a 2 bytes long
                case 2:
                    throw new NotSupportedException();
                default:
                    message.StreamId = b;
                    break;
            }
            message.Message = new ChunkMessage();

            switch (message.Type)
            {
                case HeaderType.Type0:
                    message.Message.Timestamp = BitConverter.ToInt16(BinaryHelper.Read(stream, 2), 0) * byte.MaxValue + stream.ReadByte();
                    message.Message.Content = BinaryHelper.Read(stream, BitConverter.ToInt16(BinaryHelper.Read(stream, 2), 0) * byte.MaxValue + stream.ReadByte());
                    message.Message.Type = (MessageType)stream.ReadByte();

                    break;
                case HeaderType.Type1:

                    message.Message.Timestamp = BitConverter.ToInt16(BinaryHelper.Read(stream, 2), 0) * byte.MaxValue + stream.ReadByte();
                    message.Message.Content = BinaryHelper.Read(stream, BitConverter.ToInt16(BinaryHelper.Read(stream, 2), 0) * byte.MaxValue + stream.ReadByte());
                    message.Message.Type = (MessageType)stream.ReadByte();

                    break;
                case HeaderType.Type2:
                    message.Message.Timestamp = BitConverter.ToInt16(BinaryHelper.Read(stream, 2), 0) * byte.MaxValue + stream.ReadByte();
                   
                    break;
                default:
                    break;
            }

            return message;
        }

        #region IClientResponse<Chunk> Members


        public Chunk GetResponse(byte[] requestBytes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ChunkMessage
    {
        public MessageType Type { get; set; }
        public int StreamId { get; set; }
        public int Timestamp { get; set; }
        public byte[] Content { get; set; }
    }
}
