using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Network.Dhcp
{
    public enum OptionType : byte
    {
        Pad = 0,
        SubnetMask = 1,
        TimeOffset = 2,
        Router = 3,
        TimeServer = 4,
        NameServer = 5,
        DomainNameServer = 6,
        LogServer = 7,
        CookieServer = 8,
        LprServer = 9,
        ImpressServer = 10,
        ResourceLocationServer = 11,
        HostName = 12,
        BootFileSize = 13,
        MeritDump = 14,
        DomainName = 15,
        SwapServer = 16,
        RootPath = 17,
        ExtensionsPath = 18,
        IPForwardingEnabled = 19,
        NonLocalSourceRootingEnabled = 20,
        PolicyFilter = 21,
        MaximumDatagramReassemblySize = 22,
        DefaultIpTtl = 23,
        PathMtuAgingTimeout = 24,
        PathMtuPlateauTable = 25,
        //IPLayerParameter
        InterfaceMtu = 26,
        AreAllSubnetsLocal = 27,
        BroadcastAddress = 28,
        PerformMaskDiscovery = 29,
        MaskSuplierRequiered = 30,
        PerformRouterDiscoveryEnabled = 31,
        RouterSolicitationAddress = 32,
        StaticRouteOption = 33,
        //Link Layer Parameters per interface
        TrailerEncapsulation = 34,
        ArpCacheTimeout = 35,
        EthernetEncapsulation = 36,
        //TCP Parameters
        TcpDefaultTtl = 37,
        TcpKeepaliveInterval = 38,
        TcpKeepAliveGarbage = 39,
        //Application and Service Parameters
        NetworkInformationServiceDomain = 40,
        NetworkInformationServers = 41,
        NetworkTimeProtocolServers = 42,
        VendorSpecificInformation = 43,
        NetbiosOverTcpIpNameServer = 44,
        NetbiosOverTcpIpDatagramDistributionServer = 45,
        NetbiosOverTcpIpNodeType = 46,
        NetbiosOverTcpIpScope = 47,
        XWindowSystemFontServer = 48,
        XWindowSystemDisplayManager = 49,
        //DHCP Extensions
        RequestedIp = 50,
        IpAddressLeaseTime = 51,
        OptionOverload = 52,
        DhcpMessageType = 53,
        ServerIdentifier = 54,
        ParameterRequestList = 55,
        Message = 56,
        MaximumDhcpMessageSize = 57,
        RenewalTimeValue = 58,
        RebindingTimeValue = 59,
        ClassIdentifier = 60,
        ClientIdentifier = 61,
        Extensions = 62,
        End = 255,
    }

    public abstract class Option
    {
        public abstract OptionType Type { get; }

        public abstract void WriteTo(System.IO.Stream stream);

        public static Option Read(System.IO.Stream stream)
        {
            switch ((OptionType)stream.ReadByte())
            {
                case OptionType.Pad:
                    return new PadOption();
                case OptionType.SubnetMask:
                    return SubnetMaskOption.Read(stream);
                case OptionType.TimeOffset:
                    return TimeOffsetOption.Read(stream);
                case OptionType.Router:
                    return RouterOption.Read(stream); ;
                case OptionType.TimeServer:
                    return TimeServerOption.Read(stream);
                case OptionType.NameServer:
                    return NameServerOption.Read(stream);
                case OptionType.DomainNameServer:
                    return DomainNameServerOption.Read(stream);
                case OptionType.LogServer:
                    return LogServerOption.Read(stream);
                case OptionType.CookieServer:
                    return CookieServerOption.Read(stream);
                case OptionType.LprServer:
                    return LprServerOption.Read(stream);
                case OptionType.ImpressServer:
                    return ImpressServerOption.Read(stream);
                case OptionType.ResourceLocationServer:
                    return ResourceLocationServerOption.Read(stream);
                case OptionType.HostName:
                    return HostNameOption.Read(stream);
                case OptionType.BootFileSize:
                    return BootFileSizeOption.Read(stream);
                case OptionType.MeritDump:
                    return MeritDumpOption.Read(stream);
                case OptionType.DomainName:
                    return DomainNameOption.Read(stream);
                case OptionType.SwapServer:
                    return SwapServerOption.Read(stream);
                case OptionType.RootPath:
                    return RootPathOption.Read(stream);
                case OptionType.ExtensionsPath:
                    return ExtensionsPathOption.Read(stream);
                case OptionType.IPForwardingEnabled:
                    return IpForwardingEnabledOption.Read(stream);
                case OptionType.NonLocalSourceRootingEnabled:
                    return NonLocalSourceRootingEnabledOption.Read(stream);
                case OptionType.PolicyFilter:
                    return PolicyFilterOption.Read(stream);
                case OptionType.MaximumDatagramReassemblySize:
                    return MaximumDatagramReassemblySizeOption.Read(stream);
                case OptionType.DefaultIpTtl:
                    return DefaultIpTtlOption.Read(stream);
                case OptionType.PathMtuAgingTimeout:
                    return PathMtuAgingTimeoutOption.Read(stream);
                case OptionType.PathMtuPlateauTable:
                    return PathMtuPlateauTableOption.Read(stream);
                case OptionType.InterfaceMtu:
                    return InterfaceMtuOption.Read(stream);
                case OptionType.AreAllSubnetsLocal:
                    return AreAllSubnetsLocalOption.Read(stream);
                case OptionType.BroadcastAddress:
                    return BroadcastAddressOption.Read(stream);
                case OptionType.PerformMaskDiscovery:
                    return PerformMaskDiscoveryOption.Read(stream);
                case OptionType.MaskSuplierRequiered:
                    return MaskSuplierRequiredOption.Read(stream);
                case OptionType.PerformRouterDiscoveryEnabled:
                    return PerformRouterDiscoveryEnabledOption.Read(stream);
                case OptionType.RouterSolicitationAddress:
                    return RouterSolicitationAddressOption.Read(stream);
                case OptionType.StaticRouteOption:
                    return StaticRouteOption.Read(stream);
                case OptionType.TrailerEncapsulation:
                    return TrailerEncapsulationOption.Read(stream);
                case OptionType.ArpCacheTimeout:
                    return ArpCacheTimeoutOption.Read(stream);
                case OptionType.EthernetEncapsulation:
                    return EthernetEncapsulationOption.Read(stream);
                case OptionType.TcpDefaultTtl:
                    return TcpDefaultTtlOption.Read(stream);
                case OptionType.TcpKeepaliveInterval:
                    return TcpKeepAliveIntervalOption.Read(stream);
                case OptionType.TcpKeepAliveGarbage:
                    return TcpKeepAliveGarbageOption.Read(stream);
                case OptionType.NetworkInformationServiceDomain:
                    return NetworkInformationServiceDomainOption.Read(stream);
                case OptionType.NetworkInformationServers:
                    return NetworkInformationServersOption.Read(stream);
                case OptionType.NetworkTimeProtocolServers:
                    return NetworkTimeProtocolServersOption.Read(stream);
                case OptionType.VendorSpecificInformation:
                    return VendorSpecificationInformationOption.Read(stream);
                case OptionType.NetbiosOverTcpIpNameServer:
                    return NetbiosOverTcpIpNameServerOption.Read(stream);
                case OptionType.NetbiosOverTcpIpDatagramDistributionServer:
                    return NetbiosOverTcpIpDatagramDistributionServerOption.Read(stream);
                case OptionType.NetbiosOverTcpIpNodeType:
                    return NetbiosOverTcpIpNodeTypeOption.Read(stream);
                case OptionType.NetbiosOverTcpIpScope:
                    return NetbiosOverTcpIpScopeOption.Read(stream);
                case OptionType.XWindowSystemFontServer:
                    return XWindowSystemFontServerOption.Read(stream);
                case OptionType.XWindowSystemDisplayManager:
                    return XWindowSystemDisplayManagerOption.Read(stream);
                case OptionType.RequestedIp:
                    return RequestedIpOption.Read(stream);
                case OptionType.IpAddressLeaseTime:
                    return IpAddressLeaseTimeOption.Read(stream);
                case OptionType.OptionOverload:
                    return OptionOverloadOption.Read(stream);
                case OptionType.DhcpMessageType:
                    return DhcpMessageTypeOption.Read(stream);
                case OptionType.ServerIdentifier:
                    return ServerIdentifierOption.Read(stream);
                case OptionType.ParameterRequestList:
                    return ParameterRequestListOption.Read(stream);
                case OptionType.Message:
                    return MessageOption.Read(stream);
                case OptionType.MaximumDhcpMessageSize:
                    return MaximumDhcpMessageSizeOption.Read(stream);
                case OptionType.RenewalTimeValue:
                    return RenewalTimeValueOption.Read(stream);
                case OptionType.RebindingTimeValue:
                    return RebindingTimeValueOption.Read(stream);
                case OptionType.ClassIdentifier:
                    return ClassIdentifierOption.Read(stream);
                case OptionType.ClientIdentifier:
                    return ClientIdentifierOption.Read(stream);
                case OptionType.End:
                    return null;
                    break;
                default:
                    throw new NotSupportedException();
                    break;
            }
        }
    }
}
