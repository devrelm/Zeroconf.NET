using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Rest;
using System.IO;
using System.Xml.Linq;
using System.Net;
using Network;
using System.Xml;

namespace Network.UPnP.DLNA.ContentDirectory
{
    public class Browse : Command
    {
        string objectId; 
        public Browse(string connectionString, string objectId)
            : base(connectionString)
        {
            Method = "POST";
        }

        public override Command<HttpServerEventArgs, HttpRequest, HttpResponse> Initialize(HttpRequest request, IServiceProvider provider)
        {
            throw new NotImplementedException();
        }

        protected override HttpRequest GetRequest()
        {
            HttpRequest request = BuildRequest();
            request.AcceptEncoding = new string[0];
            request.Headers["SOAPAction"] = @"""urn:schemas-upnp-org:service:ContentDirectory:1#Browse""";
            XmlWriter writer = XmlWriter.Create(request.Body, new XmlWriterSettings() { Encoding = Encoding.UTF8 });
            StreamWriter sw = new StreamWriter(request.Body, Encoding.UTF8);
            XNamespace soapEnv = XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");
            XNamespace contentDirectory = XNamespace.Get("urn:schemas-upnp-org:service:ContentDirectory:1");
            XNamespace dt = XNamespace.Get("urn:schemas-microsoft-com:datatypes");

            writer.WriteStartElement("s", "Envelope", soapEnv.NamespaceName);
            writer.WriteAttributeString("encodingStyle", soapEnv.NamespaceName, "http://schemas.xmlsoap.org/soap/encoding/");
            writer.WriteStartElement("Body", soapEnv.NamespaceName);
            writer.WriteStartElement("u", "Browse", contentDirectory.NamespaceName);
            writer.WriteElementString("ObjectID", "0");
            writer.WriteElementString("BrowseFlag", "BrowseDirectChildren");
            writer.WriteElementString("Filter", "dc:title");
            writer.WriteElementString("StartingIndex", "0");
            writer.WriteElementString("RequestedCount", "200");
            writer.WriteElementString("SortCriteria", "");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();

            //var root = new XDocument(new XElement(soapEnv + "Envelope",
            //    new XAttribute(soapEnv + "encodingStyle", "http://schemas.xmlsoap.org/soap/encoding/"),
            //    new XElement(soapEnv + "Body",
            //        new XElement(contentDirectory + "Browse",
            //            new XElement(contentDirectory + "ObjectID", new XAttribute(dt + "dt", "string"), "0"),
            //            new XElement(contentDirectory + "BrowserFlag", new XAttribute(dt + "dt", "string"), "BrowseDirectChildren"),
            //            new XElement(contentDirectory + "Filter", new XAttribute(dt + "dt", "string"), "dc:title"),
            //            new XElement(contentDirectory + "StartingIndex", new XAttribute(dt + "dt", "ui4"), "0"),
            //            new XElement(contentDirectory + "RequestedCount", new XAttribute(dt + "dt", "ui4"), "200"),
            //            new XElement(contentDirectory + "SortCriteria", new XAttribute(dt + "dt", "string"), "")))));

            //sw.WriteLine(root);

            //sw.Flush();
            //request.Headers["Content-Length"] = (request.ContentType.Length).ToString();
            return request;
        }

        public override void Execute(HttpServerEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
