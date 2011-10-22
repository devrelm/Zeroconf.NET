using System;
using System.Collections.Generic;
using System.Text;
using Network.ZeroConf;
using Network.Dns;

namespace Network.Bonjour.DACP
{
    public class Client : Client<DacpRequest, DacpResponse>
    {
        public static void Main()
        {
            Client client = new Client(new EndPoint() { Port = 3689, Addresses = { System.Net.IPAddress.Parse("192.168.1.15") } });
            client.Login();
            Console.WriteLine(client.SessionId);
            DaapMessage speakers = client.GetSpeakers();
            byte[] speakerId = null;
            foreach (DaapMessage speaker in speakers.Messages["mdcl"])
            {
                if (speaker["caia"] != null)
                    Console.Write("[X] ");
                else
                {
                    Console.Write("[ ] ");
                    speakerId = speaker["msma"][0].Value;
                }
                Console.Write(speaker["minm"][0].ToString() + " ");
                Console.WriteLine("(" + speaker["msma"][0].ToInt64() + ")");
            }
            client.SetSpeakers(new byte[1] { 0 }, speakerId);
            speakers = client.GetSpeakers();
            foreach (DaapMessage speaker in speakers.Messages["mdcl"])
            {
                if (speaker["caia"] != null)
                    Console.Write("[X] ");
                else
                {
                    Console.Write("[ ] ");
                }
                Console.Write(speaker["minm"][0].ToString() + " ");
                Console.WriteLine("(" + speaker["msma"][0].ToInt64() + ")");
            }

            Console.ReadLine();
        }

        public Client(IService service)
            : this(service.Addresses[0])
        {
        }

        public Client(EndPoint ep)
            : base(true)
        {
            this.Host = new System.Net.IPEndPoint(ep.Addresses[0], ep.Port);
            StartTcp();
        }

        protected override ClientEventArgs<DacpRequest, DacpResponse> GetEventArgs(DacpResponse response)
        {
            return new DacpEventArgs(response);
        }

        private int? sessionId;

        public int SessionId
        {
            get
            {
                if (!sessionId.HasValue)
                    Login();
                return sessionId.Value;
            }
            set { sessionId = value; }
        }


        public void Login()
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/login?hasFP=1&hsgid=00000000-066d-31e9-ed58-2b1c969b49c1";
            DacpResponse response = Send(request);
            if (response.Content.Name == "mlog")
            {
                SessionId = response.Content["mlid"][0].ToInt32();
                request = new DacpRequest();
                request.Uri = "/ctrl-int";
                response = Send(request);

            }
            else
                throw new NotSupportedException();
        }

        public DaapMessage GetStatus()
        {
            return GetStatusForRevision(1);
        }

        int notificationRevisionNumber = 1;

        public DaapMessage GetNewStatus()
        {
            DaapMessage result = GetStatusForRevision(notificationRevisionNumber);
            if (result.Name == "cmst")
                notificationRevisionNumber = result["cmsr"][0].ToInt32();
            return result;
        }

        private DaapMessage GetStatusForRevision(int revisionNumber)
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/playstatusupdate?revision-number=" + revisionNumber + "&session-id=" + SessionId;
            DacpResponse response = Send(request);
            return response.Content;
        }

        public void PlayPause()
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/playpause?session-id=" + SessionId;
            Send(request);
        }
        public void Next()
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/nextitem?session-id=" + SessionId;
            Send(request);
        }
        public void Previous()
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/previtem?session-id=" + SessionId;
            Send(request);
        }

        public int GetVolume()
        {
            return GetProperty("dmcp.volume")["cmvo"][0].ToInt32();
        }

        public DaapMessage GetProperty(string property)
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/getproperty?properties=" + property + "&session-id=" + SessionId;
            var response = Send(request);
            if (response.Content["mstt"][0].ToInt32() == 200)
                return response.Content;
            return null;
        }

        public DaapMessage SetProperty(string property, string value)
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/setproperty?" + property + "=" + value + "&session-id=" + SessionId;
            return Send(request).Content;
        }

        public DaapMessage GetDatabases()
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/databases?revision-id=1&session-id=" + SessionId;
            return Send(request).Content;
        }

        public DaapMessage GetPlaylists(int dbId)
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/databases/" + dbId + "containers?meta=dmap.itemid,dmap.itemname,dma.persistentid,com.apple.itunes.smart-playlist&revision-id=1&session-id=" + SessionId;
            return Send(request).Content;
        }

        public DaapMessage GetPlaylist(int dbId, int plid)
        {
            DacpRequest request = new DacpRequest();
            request.Uri = "/databases/" + dbId + "/containers/" + plid + "/items?type=music&meta=dmap.itemkind,dmap.itemid,dmap.containeritemid&revision-id=1&session-id=" + SessionId;
            return Send(request).Content;
        }

        public DaapMessage GetSpeakers()
        {
            if (SessionId == 0)
                Login();
            DacpRequest request = new DacpRequest();
            request.Uri = "/ctrl-int/1/getspeakers?session-id=" + SessionId + "&hsgid=00000000-066d-31e9-ed58-2b1c969b49c1";
            DacpResponse response = Send(request);
            if (response.Content.Name == "casp")
            {
                return response.Content;
            }
            else
                throw new NotSupportedException();
        }

        public void SetSpeakers(params byte[][] ids)
        {
            DacpRequest request = new DacpRequest();
            StringBuilder uriBuilder = new StringBuilder("/ctrl-int/1/setspeakers?speaker-id=");
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].Length == 1 && ids[i][0] == 0)
                    uriBuilder.Append("0");
                else
                    uriBuilder.AppendFormat("0x{0}", DaapMessage.ToHexString(ids[i]));
                if (i < ids.Length - 1)
                    uriBuilder.Append(",");
            }
            uriBuilder.Append("&session-id=");
            uriBuilder.Append(SessionId);
            uriBuilder.Append("&hsgid=00000000-066d-31e9-ed58-2b1c969b49c1");
            request.Uri = uriBuilder.ToString();
            Send(request);
        }
    }
}
