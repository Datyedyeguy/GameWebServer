using Nancy;

using QueryMaster;

using GameWebServer.Models;

using System.Net;
using System.Collections.Generic;
using System;

namespace GameWebServer
{
    public class IndexModule : NancyModule
    {
        private Server _hiddenServer;
        private Server _insurgencyServer;
        private Server _gmodServer;

        public IndexModule()
        {
            _gmodServer = ServerQuery.GetServerInstance(EngineType.Source, "127.0.0.1", 27015);
            _hiddenServer = ServerQuery.GetServerInstance(Game.Half_Life_2_Deathmatch, "127.0.0.1", 27020);
            _insurgencyServer = ServerQuery.GetServerInstance(EngineType.Source, "127.0.0.1", 27025);

            var localIpAddress = Dns.GetHostAddresses("game.datyedyeguy.net")[0].ToString();

            Get["/"] = parameters =>
            {
                ICollection<ServerInfo> infos = new List<ServerInfo>();

                infos.Add(_gmodServer.GetInfo());
                infos.Add(_hiddenServer.GetInfo());
                infos.Add(_insurgencyServer.GetInfo());

                return View["index", new ServerInfoModel(infos, localIpAddress)];
            };
        }
    }
}