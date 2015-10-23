using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ProtocolStackLib
{
    public class TransportLayer: Layer
    {
        private Socket socket;
        private int portNumber;
        private IPAddress ipAddress;
        ProtocolType currentProtocolType;
        private int numberOfConnections = 5;
     
        public TransportLayer() { }
        ~TransportLayer() 
        {
            socket.Close();
            Console.WriteLine("Socket closed");
        }
        public override object UdpReceive(IPEndPoint endPoint)
        {
            Console.WriteLine("Waiting for clients...");
           // IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remoteEndpoint = (EndPoint)(endPoint);

            byte[] data = new byte[1024];
            int receivedDataLength = socket.ReceiveFrom(data, ref remoteEndpoint);
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, receivedDataLength));

            return data;
        }
        public override void UdpSend(object data, IPEndPoint endPoint)
        {          
            socket.SendTo((byte[])data, endPoint);           
        }
        public override object TcpReceive()
        {
            byte[] data = null;
           /* if (currentProtocolType == ProtocolType.Udp)
            {

                Console.WriteLine("Waiting for clients...");
                IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remoteEndpoint = (EndPoint)(client);
                   
                data = new byte[1024];
                int receivedDataLength = socket.ReceiveFrom(data, ref remoteEndpoint);
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, receivedDataLength));                
                 // socket.SendTo(data, receivedDataLength, SocketFlags.None, remoteEndpoint);
                  
            }*/
            return data;
        }

        public override void TcpSend(object data)
        {
           /* if (currentProtocolType == ProtocolType.Udp)
            {
                Console.WriteLine("Sending...");
                socket.Send((byte[])data);
                Console.WriteLine("Sended!");
            }*/
        }

        public override void InitClient(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
            //socket.Bind(endPoint);
           // portNumber = ((IPEndPoint)socket.LocalEndPoint).Port;
           // ipAddress = ((IPEndPoint)socket.LocalEndPoint).Address;

            currentProtocolType = protocolType;

            if (protocolType == ProtocolType.Tcp)
            {
                socket.Connect(endPoint);
            }
            else if (protocolType == ProtocolType.Udp)
            {
                socket.EnableBroadcast = true;
            }
        }

        public override void InitServer(SocketType socketType, ProtocolType protocolType, IPEndPoint endPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
            socket.Bind(endPoint);
            portNumber = ((IPEndPoint)socket.LocalEndPoint).Port;
            ipAddress = ((IPEndPoint)socket.LocalEndPoint).Address;

            Console.WriteLine("Port = {0}, ip = {1}",portNumber,ipAddress);

            currentProtocolType = protocolType;

            if (protocolType == ProtocolType.Tcp)
            {               
                socket.Listen(numberOfConnections);
                Console.WriteLine("Ok listen");
            }
            
          
        }
    }
}
