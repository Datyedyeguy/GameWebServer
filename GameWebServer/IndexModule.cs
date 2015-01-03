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
            _gmodServer = ServerQuery.GetServerInstance(EngineType.Source, GetIPEndPointFromHostName("game.datyedyeguy.net", 27015, true));
            _hiddenServer = ServerQuery.GetServerInstance(Game.Half_Life_2_Deathmatch, GetIPEndPointFromHostName("game.datyedyeguy.net", 27020, true));
            _insurgencyServer = ServerQuery.GetServerInstance(EngineType.Source, GetIPEndPointFromHostName("game.datyedyeguy.net", 27025, true));

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

        private IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP)
        {
            var addresses = Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else if (throwIfMoreThanOneIP && addresses.Length > 1)
            {
                throw new ArgumentException(
                    "There is more that one IP address to the specified host.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], port); // Port gets validated here.
        }
    }
}