using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QueryMaster;

namespace GameWebServer.Models
{
    class ServerInfoModel
    {
        public IList<ServerData> ServerDatas { get; set; }

        public ServerInfoModel(IEnumerable<ServerInfo> serverInfos, string localIPAddress)
        {
            ServerDatas = new List<ServerData>();

            foreach (var serverInfo in serverInfos)
            {
                ServerDatas.Add(new ServerData
                {
                    Description = serverInfo.Description,
                    Address = string.Format("{0}{1}", localIPAddress, serverInfo.Address.Substring(serverInfo.Address.IndexOf(":"))),
                    Map = serverInfo.Map,
                    Players = serverInfo.Players,
                    MaxPlayers = serverInfo.MaxPlayers
                });
            }
        }
    }
}
