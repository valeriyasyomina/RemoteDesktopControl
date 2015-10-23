using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    public class ApplicationLayer : Layer
    {
        private PresentationLayer presentationLayer;

        public ApplicationLayer() 
        {
            presentationLayer = new PresentationLayer();
        }
       
        ~ApplicationLayer() { }
        public override object TcpReceive()
        {
            return presentationLayer.TcpReceive();
        }

        public override void TcpSend(object data)
        {
            presentationLayer.TcpSend(data);
        }
        public override object UdpReceive(IPEndPoint endPoint)
        {
            return presentationLayer.UdpReceive(endPoint);
        }
        public override void UdpSend(object data, IPEndPoint endPoint)
        {
            presentationLayer.UdpSend(data, endPoint);
        }

        public override void InitClient(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            presentationLayer.InitClient(socketType, protocolType, endPoint);           
        }

        public override void InitServer(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            presentationLayer.InitServer(socketType, protocolType, endPoint);           
        }
    }
}
