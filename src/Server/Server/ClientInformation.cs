using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using RDProtocolLib;

namespace Server
{
    public class ClientInformation
    {
        public string Login { get; set; }
        public string Password { get; set; }
       // public IPAddress ipAddress { get; set; }
     //   public int portNumber { get; set; }
        public RDEndpoint tcpForClients { get; set; }
        public RDEndpoint tcpCheckOnline { get; set; }
    }
}
