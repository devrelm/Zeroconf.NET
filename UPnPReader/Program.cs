using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Network.Rest;
using Network.UPnP;
using System.Threading;
using Network.UPnP.DLNA.ContentDirectory;
using System.Xml;

namespace UPnPReader
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceResolver resolver = new ServiceResolver();
            resolver.ServiceFound += new Network.ZeroConf.ObjectEvent<Network.ZeroConf.IService>(resolver_ServiceFound);
            resolver.Resolve("urn:schemas-upnp-org:service:ContentDirectory:1");

            //resolver.Resolve("upnp:rootdevice");
            //resolver.Resolve("urn:schemas-upnp-org:service:RenderingControl:1");
            //IPEndPoint server = new IPEndPoint(IPAddress.Parse("192.168.1.13"), 2869);
            //Browse browse = new Browse("http://192.168.1.13:2869/upnphost/udhisapi.dll?control=uuid:a6da68b3-3d15-4655-861f-503e63673e7d+urn:upnp-org:serviceId:ContentDirectory", null);
            //XmlDocument didlDoc = new XmlDocument();
            //XmlDocument browseResponse = browse.GetResponse().Document;
            //didlDoc.LoadXml(browseResponse.DocumentElement.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Value);
            //XmlNamespaceManager xmlns = new XmlNamespaceManager(didlDoc.NameTable);
            //xmlns.AddNamespace("didl", "urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/");
            //xmlns.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            //xmlns.AddNamespace("upnp", "urn:schemas-upnp-org:metadata-1-0/upnp/");
            //foreach (XmlNode item in didlDoc.SelectNodes("//didl:container/dc:title/text()", xmlns))
            //{
            //    Console.WriteLine(item.Value);
            //}

            Console.WriteLine("Press enter to exit");
            Console.Read();
            resolver.ServiceFound -= resolver_ServiceFound;
            resolver.Dispose();
        }

        static void resolver_ServiceFound(Network.ZeroConf.IService item)
        {
            item.Resolve();
            Console.WriteLine(item);
        }
    }
}
