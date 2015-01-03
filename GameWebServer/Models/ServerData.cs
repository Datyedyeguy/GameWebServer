using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameWebServer.Models
{
    class ServerData
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string Map { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }
    }
}
