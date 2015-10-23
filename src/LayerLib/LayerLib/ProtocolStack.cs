using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    abstract public class ProtocolStack
    {
        protected ApplicationLayer applicationLayer;
        public ProtocolStack() 
        {
            applicationLayer = new ApplicationLayer();
        }
     
        ~ProtocolStack() { }
        public void TcpSend(object data)
        {
            applicationLayer.TcpSend(data);
        }
        public object TcpReceive()
        {
            return applicationLayer.TcpReceive();
        }

        public void UdpSend(object data, IPEndPoint endPoint)
        {
            applicationLayer.UdpSend(data, endPoint);
        }
        public object UdpReceive(IPEndPoint endPoint)
        {
            return applicationLayer.UdpReceive(endPoint);
        }
        abstract public void Init(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint);
    }
}
