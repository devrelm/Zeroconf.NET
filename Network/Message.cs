using System;
using System.Collections.Generic;
using System.Text;

namespace Network
{
    public abstract class Message<T> : IClientResponse<T>, IServerRequest<T>, IServerResponse, IClientRequest
    {
        #region IClientResponse<T> Members

        public T GetResponse(System.IO.Stream stream)
        {
            return GetMessage(stream);
        }

        public T GetResponse(byte[] requestBytes)
        {
            return GetMessage(requestBytes);
        }

        #endregion

        #region IServerRequest<T> Members

        public T GetRequest(System.IO.Stream stream)
        {
            return GetMessage(stream);
        }

        protected abstract T GetMessage(System.IO.Stream stream);
        protected virtual T GetMessage(byte[] requestBytes)
        {
            return BinaryHelper.FromBytes((IServerRequest<T>)this, requestBytes);
        }

        public T GetRequest(byte[] requestBytes)
        {
            return GetMessage(requestBytes);
        }

        #endregion

        #region IServerResponse Members

        public abstract void WriteTo(System.IO.Stream writer);

        public byte[] GetBytes()
        {
            return BinaryHelper.GetBytes((IServerResponse)this);
        }

        #endregion
    }
}
