using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Network.Rtmp
{
    public interface IHandshakeMessage : IClientRequest, IClientResponse<IHandshakeMessage>
    {
        byte Version { get; set; }
        int Time { get; set; }
        int Time2 { get; set; }
        byte[] Random { get; set; }
    }

    public class Handshake : IClientRequest, IClientResponse<Handshake>
    {
        public Handshake()
            : this(true)
        {

        }

        public byte Version { get; set; }
        public int Time { get; set; }
        public int Time2 { get; set; }
        public byte[] Random { get; set; }

        private Handshake(bool initialize)
        {
            if (initialize)
            {
                Version = 3;
                Time = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMinutes);
                Random r = new Random(Time);
                Random = new byte[1528];
                r.NextBytes(Random);
            }
        }

        #region IClientResponse<Handshake> Members

        public Handshake GetResponse(Stream stream)
        {
            Handshake hs = new Handshake(false);
            if (Version != byte.MaxValue)
                hs.Version = (byte)stream.ReadByte();
            hs.Time = BinaryHelper.ReadInt32(stream);
            hs.Time2 = BinaryHelper.ReadInt32(stream);
            hs.Random = BinaryHelper.Read(stream, 1528);
            return hs;
        }

        public Handshake GetResponse(byte[] requestBytes)
        {
            return GetResponse(new MemoryStream(requestBytes));
        }

        #endregion

        //#region IServerRequest<Handshake> Members

        //public Handshake GetRequest(System.IO.BinaryReader stream)
        //{
        //    return GetResponse(stream);
        //}

        //public Handshake GetRequest(byte[] requestBytes)
        //{
        //    return GetResponse(requestBytes);
        //}

        //#endregion

        #region IClientRequest Members

        public void WriteTo(Stream stream)
        {
            if (Version != byte.MaxValue)
                stream.WriteByte(Version);
            BinaryHelper.Write(stream, Time);
            BinaryHelper.Write(stream, Time2);
            BinaryHelper.Write(stream, Random);
        }

        public byte[] GetBytes()
        {
            return BinaryHelper.GetBytes(this);
        }

        #endregion
    }
}
