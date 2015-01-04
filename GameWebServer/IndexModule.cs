using Nancy;

using GameWebServer.Models;

using System.Net;
using System.Collections.Generic;
using System;

using SteamMasterServer.Lib;

namespace GameWebServer
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            var gmodServer = new SourceServerQuery("127.0.0.1", 27015);
            var hiddenServer = new SourceServerQuery("127.0.0.1", 27020);
            var insurgencyServer = new SourceServerQuery("127.0.0.1", 27025);
            var localIpAddress = Dns.GetHostAddresses("game.datyedyeguy.net")[0].ToString();

            Get["/"] = parameters =>
            {
                IList<ServerData> datas = new List<ServerData>();

                datas.Add(new ServerData(gmodServer.GetServerInformation(), localIpAddress, 27015));
                datas.Add(new ServerData(hiddenServer.GetServerInformation(), localIpAddress, 27020));
                datas.Add(new ServerData(insurgencyServer.GetServerInformation(), localIpAddress, 27025));

                return View["index", new ServerInfoModel(datas)];
            };
        }
    }
}