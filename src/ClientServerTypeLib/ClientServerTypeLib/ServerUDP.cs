using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RDProtocolLib;


namespace ClientServerTypeLib
{
    public class ServerUDP : EntityUDP
    {
        protected const int numberOfConnections = 10;
        protected int portNumber = 0;
        public ServerUDP() { }
        ~ServerUDP() { }

        public int GetPortNumber() { return portNumber; } 
        public void Init(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
            socket.Bind(endPoint);
            portNumber = ((IPEndPoint)socket.LocalEndPoint).Port;                
        }
        public void InitBroadCast(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            Init(socketType, protocolType, endPoint);
            socket.EnableBroadcast = true;
        }       

    }
}
