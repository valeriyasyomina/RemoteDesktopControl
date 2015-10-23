using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    public class ClientProtocolStack : ProtocolStack
    {
        public ClientProtocolStack() { }
        ~ClientProtocolStack() { }

        public override void Init(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            applicationLayer.InitClient(socketType, protocolType, endPoint);           
        }
    }
}
