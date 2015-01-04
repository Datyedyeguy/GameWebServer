using Nancy;

using GameWebServer.Models;

using SteamMasterServer.Lib;

using System;
using System.Net;
using System.Collections.Generic;
using System.Timers;

namespace GameWebServer
{
    public class IndexModule : NancyModule
    {
        private static Object _lockServerDataObject = new Object();
        private static IList<ServerData> _serverDatas;

        private static string _localIpAddress;
        private static SourceServerQuery _gmodServer;
        private static SourceServerQuery _hiddenServer;
        private static SourceServerQuery _insurgencyServer;
        private static IList<Timer> _timers;

        static IndexModule()
        {
            _gmodServer = new SourceServerQuery("127.0.0.1", 27015);
            _hiddenServer = new SourceServerQuery("127.0.0.1", 27020);
            _insurgencyServer = new SourceServerQuery("127.0.0.1", 27025);
            _localIpAddress = Dns.GetHostAddresses("game.datyedyeguy.net")[0].ToString();

            lock(_lockServerDataObject)
            {
                _serverDatas = new List<ServerData>(3);
                _timers = new List<Timer>();

                var timer = new Timer();
                timer.Interval = 5000;
                timer.Elapsed += delegate { UpdateServerData(new { SourceServerQuery = _gmodServer, Port = 27015, DataIndex = 0 }); };
                _timers.Add(timer);
                _serverDatas.Add(new ServerData(new ServerInfoResponse(), null, 0));

                timer = new Timer();
                timer.Interval = 5000;
                timer.Elapsed += delegate { UpdateServerData(new { SourceServerQuery = _hiddenServer, Port = 27020, DataIndex = 1 }); };
                _timers.Add(timer);
                _serverDatas.Add(new ServerData(new ServerInfoResponse(), null, 0));

                timer = new Timer();
                timer.Interval = 5000;
                timer.Elapsed += delegate { UpdateServerData(new { SourceServerQuery = _insurgencyServer, Port = 27025, DataIndex = 2 }); };
                _timers.Add(timer);
                _serverDatas.Add(new ServerData(new ServerInfoResponse(), null, 0));
            }

            foreach (var timer in _timers)
            {
                timer.Start();
            }
        }

        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                lock (_lockServerDataObject)
                {
                    return View["index", new ServerInfoModel(_serverDatas)];
                }
            };
        }

        private static void UpdateServerData(dynamic info)
        {
            SourceServerQuery sourceServerQuery = info.SourceServerQuery;
            int port = info.Port;
            int dataIndex = info.DataIndex;
            var serverInfo = sourceServerQuery.GetServerInformation();

            if (serverInfo.name.Contains("N/A") == false)
            {
                lock (_lockServerDataObject)
                {
                    _serverDatas[dataIndex] = new ServerData(serverInfo, _localIpAddress, port);
                }
            }
        }
    }
}