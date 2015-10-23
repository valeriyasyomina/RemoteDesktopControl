using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    public class ServerProtocolStack : ProtocolStack
    {
        public ServerProtocolStack() { }
        ~ServerProtocolStack() { }
        public override void Init(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            applicationLayer.InitServer(socketType, protocolType, endPoint);              
        }
    }
}
