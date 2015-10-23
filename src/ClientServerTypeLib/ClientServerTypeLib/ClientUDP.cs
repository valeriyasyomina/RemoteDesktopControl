using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RDProtocolLib;

namespace ClientServerTypeLib
{
    public class ClientUDP : EntityUDP
    {
        public ClientUDP() { }
        ~ClientUDP() { }

        public void Init(SocketType socketType, ProtocolType protocolType)
        {
            socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);           
        }
        public void InitBroadCast(SocketType socketType, ProtocolType protocolType)
        {
            Init(socketType, protocolType);
            socket.EnableBroadcast = true;
        }

    }
}
