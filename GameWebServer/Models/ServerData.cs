using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SteamMasterServer.Lib;

namespace GameWebServer.Models
{
    class ServerData
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string Map { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }

        public ServerData(ServerInfoResponse serverInfoResponse, string address, int port)
        {
            Description = serverInfoResponse.game;
            Address = string.Format("{0}:{1}", address, port);
            Map = serverInfoResponse.map;
            Players = serverInfoResponse.players;
            MaxPlayers = serverInfoResponse.maxplayers;
        }
    }
}
