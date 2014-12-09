using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

using Nancy.Hosting.Self;

namespace GameWebServer
{
    class Service : ServiceBase
    {
        private readonly NancyHost _nancyHost;

        public Service() 
        {
            var uri = new Uri("http://localhost:80");
            _nancyHost = new NancyHost(uri);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _nancyHost.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _nancyHost.Stop();
        }
    }
}
