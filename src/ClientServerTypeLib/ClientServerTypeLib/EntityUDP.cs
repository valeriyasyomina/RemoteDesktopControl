using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RDProtocolLib;


namespace ClientServerTypeLib
{
    abstract public class EntityUDP
    {
        protected Socket socket;                    
        protected void ReceiveFromSocket(out byte[] data, out IPEndPoint endPoint)
        {
            endPoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remoteEndpoint = (EndPoint)(endPoint);
            data = new byte[5000];
            int receivedDataLength = socket.ReceiveFrom(data, ref remoteEndpoint);
            endPoint = (IPEndPoint)remoteEndpoint;
        }
        protected void SendToSocket(byte[] data, IPEndPoint endPoint)
        {
            if (socket != null)
            {
                socket.SendTo(data, endPoint);
            }
        }

        public void Receive(out RDProtocol message, out IPEndPoint endPoint)
        {
            byte[] receivedMsg = null;
            ReceiveFromSocket(out receivedMsg, out endPoint);
            RDProtocolConvertor.ByteArrayToRDMessage(receivedMsg, out message);
        }

        public void Send(RDProtocol message, IPEndPoint endPoint)
        {
            byte[] byteMessage = null;
            RDProtocolConvertor.RDMessageToByteArray(message, out byteMessage);
            SendToSocket(byteMessage, endPoint);
        }
        public void CloseConnection()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }

    }
}
