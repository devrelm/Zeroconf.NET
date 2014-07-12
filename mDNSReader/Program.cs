﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Network.Bonjour;
using System.Net.NetworkInformation;

namespace mDNSReader
{
    class Program
    {
        static void Main(string[] args)
        {
            BonjourServiceResolver bsr = new BonjourServiceResolver();
            bsr.ServiceFound += new Network.ZeroConf.ObjectEvent<Network.ZeroConf.IService>(bsr_ServiceFound);
            bsr.Resolve("_airport._tcp.local", NetworkInterface.GetAllNetworkInterfaces().SelectMany(nic => nic.GetIPProperties().UnicastAddresses.Where(ip => ip.IsDnsEligible).Select(ip => ip.Address)).Select(ip => new IPEndPoint(ip, 0)).ToArray());
            Console.ReadLine();
            bsr.Dispose();
            //Service s = new Service();
            //s.AddAddress(new Network.Dns.EndPoint() { DomainName = "AIL.local.", Port = 50508, Addresses = Network.ZeroConf.ResolverHelper.GetEndPoint().Addresses });
            //s.Protocol = "_touch-remote._tcp.local.";
            //s.Name = "MyName";
            //s.HostName = "ASPERGE.local.";
            //s["DvNm"] = "PC Remote";
            //s["RemV"] = "10000";
            //s["DvTy"] = "iPod";
            //s["RemN"] = "Remote";
            //s["txtvers"] = "1";
            //s["Pair"] = "0000000000000001";
            //s.Publish();
            //Thread.Sleep(3600000);
            //s.Stop();
        }

        static void bsr_ServiceFound(Network.ZeroConf.IService item)
        {
            Console.WriteLine(item);
        }
    }
}