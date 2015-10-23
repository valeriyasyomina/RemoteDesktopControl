using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RDProtocolLib;


namespace ClientServerTypeLib
{
    public class ServerTCP : EntityTCP
    {
        
        protected int portNumber;
        protected int numberOfConnections = 10;
        public ServerTCP()
        {

        }
        ~ServerTCP() { }
        public int GetPortNumber() { return portNumber; }

        public void Init(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
            socket.Bind(endPoint);
            portNumber = ((IPEndPoint)socket.LocalEndPoint).Port;

            socket.Listen(numberOfConnections);
        }

        public void AcceptClient(out Socket clientSocket)
        {
            if (socket != null)
            {
                clientSocket = socket.Accept();
            }
            else
            {
                clientSocket = null;
            }
        }

        public void Send(RDProtocol message, Socket socket)
        {
            base.Send(message, socket);
        }

        public void SendMsgSize(RDProtocol message, Socket socket)
        {
            base.SendMsgSize(message, socket);
        }

        public void ReceiveMsgSize(out int messageSize, Socket socket)
        {
            base.ReceiveMsgSize(out messageSize, socket);
        }

        public void Receive(out RDProtocol message, Socket socket)
        {
            base.Receive(out message, socket);
        }
        public void Receive(out RDProtocol message, Socket socket, int messageSize)
        {
            base.Receive(out message, socket, messageSize);
        }
        
    }
}
