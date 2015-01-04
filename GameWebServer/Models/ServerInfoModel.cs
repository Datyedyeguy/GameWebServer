using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameWebServer.Models
{
    class ServerInfoModel
    {
        public IList<ServerData> ServerDatas { get; set; }

        public ServerInfoModel(IList<ServerData> serverDatas)
        {
            ServerDatas = serverDatas;
        }
    }
}
