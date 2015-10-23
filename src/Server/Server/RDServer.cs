using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ClientServerTypeLib;
using RDProtocolLib;
using System.Threading;
using FileLoggerLib;

namespace Server
{
    public class RDServer
    {
        private int broadcastPortNumber;      

        private FileLogger fileLogger;

        private ServerUDP serverUDP;

        private ServerTCP serverTCP;

        private RDEndpoint serverEndpoit;

        private RDProtocol serverAnswer;

        private ClientTCP onlineClientsDetector;

        private List<ClientInformation> clients;     
      
        public RDServer(string logFileName, int portNumber, int tcpPort)
        {
            broadcastPortNumber = portNumber;           
            fileLogger = new FileLogger(logFileName, FileLoggerLib.LoggerType.SERVER);
            serverUDP = new ServerUDP();
            serverTCP = new ServerTCP();
            onlineClientsDetector = new ClientTCP();

            serverEndpoit = new RDEndpoint();
            serverEndpoit.portNumber = tcpPort;                    
      
            CreateBroadCastAnswer();
            FillClientsInformation();
        }
  
        public void Start()
        {
            fileLogger.WriteLogFile("Сервер запущен\n");      
      
            Thread broadCastThread = new Thread(new ThreadStart(BroadCastConnections));
            broadCastThread.IsBackground = true;
            broadCastThread.Start();

            Thread ckeckOnlineClientsThread = new Thread(new ThreadStart(CheckOnlineClients));
            ckeckOnlineClientsThread.IsBackground = true;
            ckeckOnlineClientsThread.Start();
            
            GetTCPConnections();
        }  
        private void CheckOnlineClients()
        {
            while (true)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].tcpForClients.ipAddress != null && clients[i].tcpForClients.portNumber != 0 &&
                        clients[i].tcpCheckOnline.portNumber != 0 && clients[i].tcpCheckOnline.ipAddress != null)
                    {
                        try
                        {
                            onlineClientsDetector.Init(SocketType.Stream, ProtocolType.Tcp);
                            IPEndPoint endPoint = new IPEndPoint(clients[i].tcpCheckOnline.ipAddress, clients[i].tcpCheckOnline.portNumber);
                            onlineClientsDetector.ConnectToEndPoint(endPoint);                         
                            onlineClientsDetector.CloseConnection();
                        }
                        catch (Exception exception)     // клиент аварийно завершился или забыл выйти из сисмтемы
                        { 
                            Console.WriteLine("Client " + clients[i].tcpForClients.ipAddress + ":" +
                                Convert.ToString(clients[i].tcpForClients.portNumber) + " stopped unexpectedly");

                            fileLogger.WriteLogFile("Клиент " + clients[i].tcpForClients.ipAddress + ":" +
                                Convert.ToString(clients[i].tcpForClients.portNumber) + " аварийно завершился\n");

                            clients[i].tcpForClients.portNumber = 0;
                            clients[i].tcpForClients.ipAddress = null;
                            clients[i].tcpCheckOnline.ipAddress = null;
                            clients[i].tcpCheckOnline.portNumber = 0;
                        }
                    }
                }
            }
        }
        private void CreateBroadCastAnswer()
        {
            try
            {
                serverAnswer = new RDProtocol();
                serverAnswer.commandType = CommandType.ServerFound;
                byte[] data = null;
                RDProtocolConvertor.RDEndpointToByteArray(serverEndpoit, out data);
                serverAnswer.data = data;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void FillClientsInformation()
        {
            clients = new List<ClientInformation>();
            ClientInformation firstClient = new ClientInformation();
            firstClient.Login = "Lera";
            firstClient.Password = "123";
            firstClient.tcpForClients = new RDEndpoint();
            firstClient.tcpCheckOnline = new RDEndpoint();

            clients.Add(firstClient);

            ClientInformation secondClient = new ClientInformation();
            secondClient.Login = "User1";
            secondClient.Password = "123";
            secondClient.tcpForClients = new RDEndpoint();
            secondClient.tcpCheckOnline = new RDEndpoint();

            clients.Add(secondClient);

            ClientInformation thirdClient = new ClientInformation();
            thirdClient.Login = "User2";
            thirdClient.Password = "123";
            thirdClient.tcpForClients = new RDEndpoint();
            thirdClient.tcpCheckOnline = new RDEndpoint();

            clients.Add(thirdClient);
        }
        private void BroadCastConnections()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, broadcastPortNumber);
                serverUDP.InitBroadCast(SocketType.Dgram, ProtocolType.Udp, endPoint);
                IPEndPoint clientEndpoint = null;

                Console.WriteLine("Waiting for clients broadcast...");
                fileLogger.WriteLogFile("Сервер ожидает широковещательный запрос\n");

                while (true)
                {
                    RDProtocol message = null;
                    serverUDP.Receive(out  message, out clientEndpoint);
                    if (message.commandType == CommandType.FindSever)
                    {
                        Console.WriteLine("{0}  {1} ", clientEndpoint.Address, clientEndpoint.Port);
                        Console.WriteLine("Received : {0}", message.commandType);
                        serverUDP.Send(serverAnswer, clientEndpoint);
                        fileLogger.WriteLogFile("UDP клиент: " + clientEndpoint.Address.ToString() + ":" + Convert.ToString(clientEndpoint.Port) + '\n');
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void TcpClientConnection(object client)
        {
            try
            {
                RDProtocol message = null;
                int messageSize = 0;
                serverTCP.ReceiveMsgSize(out messageSize, (Socket)client);
                RDProtocol serverAnswer = new RDProtocol();
                serverAnswer.commandType = CommandType.MessageSizeAccepted;
                serverTCP.Send(serverAnswer, (Socket)client);
                serverTCP.Receive(out message, (Socket)client, messageSize);
                if (message.commandType == CommandType.LogIn)
                {
                    ClientLogIn((Socket)client, message);
                }
                else if (message.commandType == CommandType.LogOut)
                {
                    ClientLogOut((Socket)client, message);
                }
                else if (message.commandType == CommandType.ClientsInfo)
                {
                    SendOnlineClientsInfo((Socket)client, message);
                }
                else if (message.commandType == CommandType.CheckCLientStatus)
                {
                    CheckClientStatus((Socket)client, message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }
        private void CheckClientStatus(Socket client, RDProtocol message)
        {
            try
            {
                RDEndpoint checkClientEndPoint = null;
                RDProtocolConvertor.ByteArrayToRDEndpoint(message.data, out checkClientEndPoint);
                bool clientOnline = false;
                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].tcpForClients.ipAddress.ToString() == checkClientEndPoint.ipAddress.ToString() && 
                        clients[i].tcpForClients.portNumber == checkClientEndPoint.portNumber)
                    {
                        clientOnline = true;
                        break;
                    }
                }
                RDProtocol serverAnswer = new RDProtocol();
                if (clientOnline)
                {
                    serverAnswer.commandType = CommandType.ClientOnline;
                }
                else
                {
                    serverAnswer.commandType = CommandType.ClientOffline;
                }
                serverTCP.SendMsgSize(serverAnswer, client);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void SendOnlineClientsInfo(Socket client, RDProtocol message)
        {
            try
            {
                ClientKeys clientKeys = null;
                RDProtocolConvertor.ByteArrayToClientKeys(message.data, out clientKeys);

                ClientsInfo info = new ClientsInfo();
                info.clientsList = GetClientsForSend(clientKeys.ClientLogin);

                RDProtocol serverAnswer = new RDProtocol();
                serverAnswer.commandType = CommandType.ClientsInfo;
                byte[] byteMessage = null;
                RDProtocolConvertor.ClientsInfoToByteArray(info, out byteMessage);
                serverAnswer.data = byteMessage;
                serverTCP.SendMsgSize(serverAnswer, client);

                RDProtocol clientAnswer = null;
                serverTCP.Receive(out clientAnswer, client);
                if (clientAnswer.commandType == CommandType.ClientsInfo)
                {
                    serverTCP.Send(serverAnswer, client);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private List<RDEndpoint> GetClientsForSend(string Login)
        {
            List<RDEndpoint> clientList = new List<RDEndpoint>();
            int clientsNumber = clients.Count;
            for (int i = 0; i < clientsNumber; i++)
            {
                if (clients[i].Login != Login  && clients[i].tcpForClients.ipAddress != null && clients[i].tcpForClients.portNumber != 0)
                {
                    RDEndpoint endPoint = new RDEndpoint();
                    endPoint.ipAddress = clients[i].tcpForClients.ipAddress;
                    endPoint.portNumber = clients[i].tcpForClients.portNumber;

                    clientList.Add(endPoint);
                }
            }
            return clientList;
        }
        private void ClientLogOut(Socket client, RDProtocol message)
        {
            try
            {
                int clientsNumber = clients.Count;
                IPAddress address = ((IPEndPoint)client.LocalEndPoint).Address;
                int port = ((IPEndPoint)client.LocalEndPoint).Port;

                ClientKeys clientKeys = null;
                RDProtocolConvertor.ByteArrayToClientKeys(message.data, out clientKeys);

                for (int i = 0; i < clientsNumber; i++)
                {
                    if (clients[i].Login == clientKeys.ClientLogin)
                    {
                        clients[i].tcpForClients.portNumber = 0;
                        clients[i].tcpForClients.ipAddress = null;
                        clients[i].tcpCheckOnline.ipAddress = null;
                        clients[i].tcpCheckOnline.portNumber = 0;
                        break;
                    }
                }
                fileLogger.WriteLogFile("Клиент " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + ":" +
                   Convert.ToString(((IPEndPoint)client.RemoteEndPoint).Port) + " вышел из системы\n");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }        
        }     
        private void ClientLogIn(Socket client, RDProtocol message)
        {
            try
            {
                ClientLoginData loginData = null;
                RDProtocolConvertor.ByteArrayToClientLoginData(message.data, out loginData);
                int clientsNumber = clients.Count;
                bool clientExists = false;
                bool clientAlreadyLogIn = false;
                for (int i = 0; i < clientsNumber; i++)
                {
                    if (clients[i].Login == loginData.clientKeys.ClientLogin && clients[i].Password == loginData.clientKeys.ClientPassword)
                    {
                        if (clients[i].tcpForClients.ipAddress != null && clients[i].tcpForClients.portNumber != 0)
                        {
                            clientAlreadyLogIn = true;
                            break;
                        }
                        clients[i].tcpForClients.ipAddress = ((IPEndPoint)client.RemoteEndPoint).Address;
                        clients[i].tcpForClients.portNumber = loginData.clientTCPEndpoint.portNumber;
                        clients[i].tcpCheckOnline.ipAddress = ((IPEndPoint)client.RemoteEndPoint).Address;
                        clients[i].tcpCheckOnline.portNumber = loginData.tcpCheckIsOnline.portNumber;
                        clientExists = true;
                        break;
                    }
                }
                if (clientAlreadyLogIn)
                {
                    ClientHasAlreadyBeenLogIn(client);
                }
                else if (clientExists)
                {
                    ClientLogInOk(loginData, client);
                }
                else
                {
                    ClientLogInError(client);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void ClientLogInOk(ClientLoginData loginData, Socket client)
        {
            try
            {
                RDProtocol serverAnswerOkLogin = new RDProtocol();
                serverAnswerOkLogin.commandType = CommandType.LogInOk;
                serverTCP.Send(serverAnswerOkLogin, client);

                RDProtocol serverAnswer = new RDProtocol();
                List<RDEndpoint> clientList = GetClientsForSend(loginData.clientKeys.ClientLogin);
                ClientsInfo clientsInfoToSend = new ClientsInfo();
                clientsInfoToSend.clientsList = clientList;
                byte[] clientListByte = null;
                RDProtocolConvertor.ClientsInfoToByteArray(clientsInfoToSend, out clientListByte);
                serverAnswer.commandType = CommandType.ClientsInfo;
                serverAnswer.data = clientListByte;

                serverTCP.SendMsgSize(serverAnswer, client);
                RDProtocol answerSize = null;
                serverTCP.Receive(out answerSize, client);

                if (answerSize.commandType == CommandType.MessageSizeAccepted)
                {
                    serverTCP.Send(serverAnswer, client);
                }
                fileLogger.WriteLogFile("Клиент " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + ":" +
                    Convert.ToString(((IPEndPoint)client.RemoteEndPoint).Port) + " успешно авторизован\n");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void ClientHasAlreadyBeenLogIn(Socket client)
        {
            try
            {
                RDProtocol serverAnswer = new RDProtocol();
                serverAnswer.commandType = CommandType.ClientHasAlreadyBeenLogIn;
                serverTCP.Send(serverAnswer, client);
                fileLogger.WriteLogFile("Клиент " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + ":" +
                    Convert.ToString(((IPEndPoint)client.RemoteEndPoint).Port) + " уже был зарегистрирован\n");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void ClientLogInError(Socket client)
        {
            try
            {
                RDProtocol serverAnswer = new RDProtocol();
                serverAnswer.commandType = CommandType.LogInError;
                serverTCP.Send(serverAnswer, client);
                fileLogger.WriteLogFile("Клиент " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + ":" +
                    Convert.ToString(((IPEndPoint)client.RemoteEndPoint).Port) + " провалил авторизацию\n");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void GetTCPConnections()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverEndpoit.portNumber);
                serverTCP.Init(SocketType.Stream, ProtocolType.Tcp, endPoint);
                fileLogger.WriteLogFile("Сервер ожидает TCP запросы от клиентов\n");

                while (true)
                {
                    Socket client = null;
                    serverTCP.AcceptClient(out client);
                    fileLogger.WriteLogFile("TCP клиент: " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + ":" +
                        Convert.ToString(((IPEndPoint)client.RemoteEndPoint).Port) + '\n');
                    Console.WriteLine("Client : {0}:{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.RemoteEndPoint).Port);
                    ParameterizedThreadStart th = new ParameterizedThreadStart(TcpClientConnection);

                    Thread clientThread = new Thread(th);
                    clientThread.Start(client);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }        
    }
}
