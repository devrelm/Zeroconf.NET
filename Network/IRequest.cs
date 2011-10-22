using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Network
{
    public interface IRequest
    {
    }

    public interface IClientRequest : IRequest
    {
        void WriteTo(Stream stream);
        byte[] GetBytes();
    }

    public interface IClientRequestWriter : IClientRequest
    {
        void WriteTo(BinaryWriter writer);
    }

    public interface IServerRequest<RequestType> : IRequest
    {
        RequestType GetRequest(Stream stream);
        RequestType GetRequest(byte[] requestBytes);
    }

    public interface IServerRequestReader<TRequest> : IServerRequest<TRequest>
    {
        TRequest GetRequest(BinaryReader writer);
    }
}
