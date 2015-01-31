using Nancy;

using GameWebServer.Models;

using SteamMasterServer.Lib;

using System;
using System.Net;
using System.Collections.Generic;
using System.Timers;
using System.Linq;

namespace GameWebServer
{
    public class IndexModule : NancyModule
    {
        private static Object _lockServerDataObject = new Object();
        private static IList<ServerData> _serverDatas;

        private static string _localIpAddress;
        private static IDictionary<int, SourceServerQuery> _servers = new Dictionary<int, SourceServerQuery>();
        private static IList<Timer> _timers;

        static IndexModule()
        {
            _localIpAddress = Dns.GetHostAddresses("game.datyedyeguy.net")[0].ToString();

            lock(_lockServerDataObject)
            {
                _serverDatas = new List<ServerData>();
                _timers = new List<Timer>();

                for (int i = 27015; i <= 27030; i++)
                {
                    int port = i;
                    int dataIndex = _serverDatas.Count;
                    var timer = new Timer();
                    timer.Interval = 30000;
                    timer.Elapsed += delegate { UpdateServerData(new { Port = port, DataIndex = dataIndex }); };
                    _timers.Add(timer);
                    _serverDatas.Add(null);
                }
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
                IList<ServerData> validInfo = null;

                lock (_lockServerDataObject)
                {
                    validInfo = _serverDatas.Where(x => x != null).ToList();
                }

                return View["index", new ServerInfoModel(validInfo)];
            };
        }

        private static void UpdateServerData(dynamic info)
        {
            int port = info.Port;
            int dataIndex = info.DataIndex;

            if (_servers.ContainsKey(port) == false)
            {
                lock (_lockServerDataObject)
                {
                    _servers[port] = new SourceServerQuery("10.0.0.2", port);
                }
            }

            SourceServerQuery sourceServerQuery = _servers[port];
            var serverInfo = sourceServerQuery.GetServerInformation();

            lock (_lockServerDataObject)
            {
                if (serverInfo.name.Contains("N/A") == false)
                {
                    _serverDatas[dataIndex] = new ServerData(serverInfo, _localIpAddress, port);
                }
                else
                {
                    _serverDatas[dataIndex] = null;
                    _servers.Remove(port);
                }
            }
        }
    }
}