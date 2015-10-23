using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RDProtocolLib;

namespace ClientServerTypeLib
{
    public class ClientTCP : EntityTCP
    {
        public ClientTCP() { }
        ~ClientTCP() { }

        public void Init(SocketType socketType, ProtocolType protocolType)
        {
            socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
               
        }
        public void ConnectToEndPoint(IPEndPoint endPoint)
        {
            if (socket != null)
            {
                socket.Connect(endPoint); 
                
            }          
        }
        public void Send(RDProtocol message)
        {
            base.Send(message, socket);
        }

        public void SendMsgSize(RDProtocol message)
        {
            base.SendMsgSize(message, socket);
        }

        public void ReceiveMsgSize(out int messageSize)
        {
            base.ReceiveMsgSize(out messageSize, socket);
        }

        public void Receive(out RDProtocol message)
        {
            base.Receive(out message, socket);
        }
        public void Receive(out RDProtocol message, int messageSize)
        {
            base.Receive(out message, socket, messageSize);
        }
    }
}
