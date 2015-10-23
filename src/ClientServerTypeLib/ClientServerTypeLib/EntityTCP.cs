using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RDProtocolLib;

namespace ClientServerTypeLib
{
    abstract public class EntityTCP
    {
        protected Socket socket;

        public Socket GetSocket() { return socket; }
        
        protected void ReceiveMsgSizeFromSocket(out int messageSize, Socket socket)
        {
            byte[] data = new byte[5000];
            int receivedDataLength = socket.Receive(data);
            RDProtocol message = null;
            RDProtocolConvertor.ByteArrayToRDMessage(data, out message);
            messageSize = BitConverter.ToInt32(message.data, 0);
        }
        protected void ReceiveFromSocket(out byte[] data, Socket socket)
        {          
            data = new byte[5000];
            int receivedDataLength = socket.Receive(data);            
        }
        protected void ReceiveFromSocket(out byte[] data, Socket socket, int messageSize)
        {
            int position = 0;
            data = new byte[messageSize];
            byte[] tmpBuffer = new byte[8192];
            while (position < messageSize)
            {
                int receivedBytes = socket.Receive(tmpBuffer);
                for (int counter = position, counterCur = 0; counterCur < receivedBytes; counter++, counterCur++)
                {
                    data[counter] = tmpBuffer[counterCur];
                }
                position += receivedBytes;
            }
        }
        protected void SendToSocket(byte[] data, Socket socket)
        {
            socket.Send(data);
        }
       
        protected void ReceiveMsgSize(out int messageSize, Socket socket)
        {           
            ReceiveMsgSizeFromSocket(out messageSize, socket);
        }

        protected void Receive(out RDProtocol message, Socket socket)
        {
            byte[] receivedMsg = null;
            ReceiveFromSocket(out receivedMsg, socket);
            RDProtocolConvertor.ByteArrayToRDMessage(receivedMsg, out message);
        }
        protected void Receive(out RDProtocol message, Socket socket, int messageSize)
        {
            byte[] receivedMsg = null;
            ReceiveFromSocket(out receivedMsg, socket,messageSize);
            RDProtocolConvertor.ByteArrayToRDMessage(receivedMsg, out message);
        }
        protected void SendMsgSize(RDProtocol message, Socket socket)
        {
            RDProtocol messageSize = new RDProtocol();
            messageSize.commandType = CommandType.MessageSize;

            byte[] messageByte = null;
            RDProtocolConvertor.RDMessageToByteArray(message, out messageByte);

            byte[] messageLength = BitConverter.GetBytes(messageByte.Length);
            messageSize.data = messageLength;

            Send(messageSize, socket);
        }

        protected void Send(RDProtocol message, Socket socket)
        {
            byte[] byteMessage = null;
            RDProtocolConvertor.RDMessageToByteArray(message, out byteMessage);
            SendToSocket(byteMessage, socket);
        }       
        public void CloseConnection()
        {
            if (socket != null)
            {               
                socket.Close();
            }
        }
        public void Disconnect()
        {
            if (socket != null)
            {
                socket.Disconnect(true);
            }
        }

    }
}
