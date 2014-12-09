using System;
using System.ServiceProcess;

using Nancy.Hosting.Self;

namespace GameWebServer
{
    class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] { new Service() };

            ServiceBase.Run(servicesToRun);
        }
    }
}
