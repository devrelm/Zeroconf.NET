using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Network.Dhcp
{
    public enum Operation : byte
    {
        Request = 1,
        Reply = 2
    }

    public enum HardwareType : byte
    {
        Ethernet = 1,
        ExperimentalEthernet,
        AmateurRadioAXDot25,
        ProteonProNetTokenRing,
        Chaos,
        IEEE802,
        Arcnet,
        Huperchannel,
        Lanstar,
        AutonetShortAddress,
        LocalTalk,
        LocalNet,
        UltraLink,
        SMDS,
        FrameRelay,
        AsynchronousTransmissionMode1,
        HDLC,
        FiberChannel,
        AsynchronousTransmissionMode2,
        SerialLine,
        AsynchronousTransmissionMode3,
    }

    public class Message : IClientResponse<Message>, IServerRequest<Message>, IClientRequest, IServerResponse
    {
        public Operation Operation { get; set; }

        public HardwareType HardwareType { get; set; }

        public byte HardwareAddressLength { get; set; }

        public byte Hops { get; set; }

        public uint TransactionId { get; set; }

        public ushort SecondElapsed { get; set; }

        public bool Broadcast { get; set; }

        public IPAddress ClientIP { get; set; }

        public IPAddress Your { get; set; }

        public IPAddress NextIP { get; set; }

        public IPAddress RelayAgentIP { get; set; }

        public byte[] ClientHardwareAddress { get; set; }

        public string ServerHostName { get; set; }

        public string BootFileName { get; set; }

        public List<Option> Options { get; set; }

        #region IServerRequest<Message> Members

        public Message GetRequest(System.IO.Stream stream)
        {
            Message message = new Message();
            message.Operation = (Operation)stream.ReadByte();
            message.HardwareType = (HardwareType)stream.ReadByte();
            message.HardwareAddressLength = (byte)stream.ReadByte();
            message.Hops = (byte)stream.ReadByte();
            message.TransactionId = BinaryHelper.ReadUInt32(stream);
            message.SecondElapsed = BinaryHelper.ReadUInt16(stream);
            message.Broadcast = BinaryHelper.ReadUInt32(stream) == 0x8000;
            message.ClientIP = new IPAddress(BinaryHelper.Read(stream, 4));
            message.Your = new IPAddress(BinaryHelper.Read(stream, 4));
            message.NextIP = new IPAddress(BinaryHelper.Read(stream, 4));
            message.RelayAgentIP = new IPAddress(BinaryHelper.Read(stream, 4));
            message.ClientHardwareAddress = BinaryHelper.Read(stream, 16);
            message.ServerHostName = BinaryHelper.ReadString(stream, 64);
            message.BootFileName = BinaryHelper.ReadString(stream, 128);
            Option option;
            while ((option = Option.Read(stream)) != null)
                message.Add(option);
            return message;

        }

        private void Add(Option option)
        {
            Options.Add(option);
        }

        public Message GetRequest(byte[] requestBytes)
        {
            using (MemoryStream s = new MemoryStream(requestBytes))
            {
                return GetRequest(s);
            }
        }

        #endregion

        #region IClientResponse<Message> Members

        public Message GetResponse(System.IO.Stream stream)
        {
            return GetRequest(stream);
        }

        public Message GetResponse(byte[] requestBytes)
        {
            return GetRequest(requestBytes);
        }

        #endregion

        #region IServerResponse Members

        public void WriteTo(Stream writer)
        {
            writer.WriteByte((byte)Operation);
            writer.WriteByte((byte)HardwareType);
            writer.WriteByte((byte)HardwareAddressLength);
            writer.WriteByte((byte)Hops);
            BinaryHelper.Write(writer, TransactionId);
            BinaryHelper.Write(writer, SecondElapsed);
            if (Broadcast)
                BinaryHelper.Write(writer, 0x8000);
            else
                BinaryHelper.Write(writer, (uint)0);
            BinaryHelper.Write(writer, ClientIP.GetAddressBytes());
            BinaryHelper.Write(writer, Your.GetAddressBytes());
            BinaryHelper.Write(writer, NextIP.GetAddressBytes());
            BinaryHelper.Write(writer, RelayAgentIP.GetAddressBytes());
            BinaryHelper.Write(writer, ClientHardwareAddress);
            BinaryHelper.Write(writer, ServerHostName);
            BinaryHelper.Write(writer, BootFileName);
            if (IsDhcpMessage) //Magic cookie
                BinaryHelper.Write(writer, new byte[] { 63, 82, 53, 63 });
            if (Options != null && Options.Count > 0)
            {
                foreach (Option option in Options)
                {
                    writer.WriteByte((byte)option.Type);
                    option.WriteTo(writer);
                }
            }
        }

        public byte[] GetBytes()
        {
            return BinaryHelper.GetBytes((IClientRequest)this);
        }

        #endregion

        public bool IsDhcpMessage { get; set; }
    }
}
