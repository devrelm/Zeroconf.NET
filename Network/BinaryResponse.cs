using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Network
{
    public class BinaryResponse : IClientResponse<BinaryResponse>, IServerResponse
    {
        private MemoryStream stream;

        public BinaryResponse()
        {
            stream = new MemoryStream();
        }



        #region IResponse Members

        public void WriteTo(Stream target)
        {
            this.stream.Position = 0;
            this.stream.WriteTo(target);
        }

        public byte[] GetBytes()
        {
            return stream.ToArray();
        }

        #endregion

        #region IResponse<BinaryResponse> Members

        public BinaryResponse GetResponse(Stream stream)
        {
            byte[] buffer = new byte[1024];
            int lengthRead = 0;
            do
            {
                lengthRead = stream.Read(buffer, 0, buffer.Length);
                this.stream.Write(buffer, 0, lengthRead);
            }
            while (lengthRead == 1024);
            return this;
        }

        public BinaryResponse GetResponse(byte[] requestBytes)
        {
            stream.Write(requestBytes, 0, requestBytes.Length);
            return this;
        }

        #endregion
    }
}
