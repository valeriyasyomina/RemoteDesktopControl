using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    public class PresentationLayer : Layer
    {
        private SessionLayer sessionLayer;
        public PresentationLayer() 
        {
            sessionLayer = new SessionLayer();
        }
     
        ~PresentationLayer() { }
        public override object TcpReceive()
        {
            return sessionLayer.TcpReceive();
        }

        public override void TcpSend(object data)
        {
            sessionLayer.TcpSend(data);
        }

        public override object UdpReceive(IPEndPoint endPoint)
        {
            return sessionLayer.UdpReceive(endPoint);
        }
        public override void UdpSend(object data, IPEndPoint endPoint)
        {
            sessionLayer.UdpSend(data, endPoint);
        }

        public override void InitClient(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            sessionLayer.InitClient(socketType, protocolType, endPoint);           
        }

        public override void InitServer(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            sessionLayer.InitServer(socketType, protocolType, endPoint);           
        }
    }
}
