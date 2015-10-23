using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    public class SessionLayer : Layer
    {
        private TransportLayer transportLayer;
        public SessionLayer() 
        {
            transportLayer = new TransportLayer();
        }
       
        ~SessionLayer() { }
        public override object TcpReceive()
        {
            return transportLayer.TcpReceive();
        }

        public override void TcpSend(object data)
        {
            transportLayer.TcpSend(data);
        }
        public override object UdpReceive(IPEndPoint endPoint)
        {
            return transportLayer.UdpReceive(endPoint);
        }
        public override void UdpSend(object data, IPEndPoint endPoint)
        {
            transportLayer.UdpSend(data, endPoint);
        }

        public override void InitClient(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            transportLayer.InitClient(socketType, protocolType, endPoint);          
        }

        public override void InitServer(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            transportLayer.InitServer(socketType, protocolType, endPoint);            
        }
        
    }
}
