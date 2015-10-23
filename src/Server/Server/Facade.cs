using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;

namespace Server
{
    public class Facade 
    {
        private static RDServer server;

        public Facade() { }        
       
        static public void Init()
        {
            string logFileName = ConfigurationManager.AppSettings["LogFileName"];
            int broadcastPort = Convert.ToInt32(ConfigurationManager.AppSettings["UdpBroadcastPort"]);
            int tcpPort = Convert.ToInt32(ConfigurationManager.AppSettings["TCPPort"]);
            
            server = new RDServer(logFileName,broadcastPort,tcpPort);
            server.Start();
        }
    }
}
