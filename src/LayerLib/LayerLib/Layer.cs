using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;


namespace ProtocolStackLib
{
    abstract public class Layer
    {
        abstract public void TcpSend(object data);
        abstract public object TcpReceive();
        abstract public void UdpSend(object data, IPEndPoint endPoint);
        abstract public object UdpReceive(IPEndPoint endPoint);
        abstract public void InitServer(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint);
        abstract public void InitClient(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint);
    }
}
