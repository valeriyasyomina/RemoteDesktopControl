using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientServerTypeLib;
using System.Net;
using System.Net.Sockets;
using RDProtocolLib;
using System.Windows.Forms;
using System.Threading;

namespace Client
{
    public class RDClient
    {
        public delegate void MouseMove(int MouseX, int MouseY);
        public event MouseMove onMouseMove;

        public delegate void MouseRightButtonClick(int MouseX, int MouseY);
        public event MouseRightButtonClick onMouseRightButtonClick;

        public delegate void MouseLeftButtonClick(int MouseX, int MouseY);
        public event MouseLeftButtonClick onMouseLeftButtonClick;

        public delegate void MouseDblClick(int MouseX, int MouseY);
        public event MouseDblClick onMouseDblClick;

        public delegate void MouseUp(int MouseX, int MouseY);
        public event MouseUp onMouseUp;

        public delegate void MouseDown(int MouseX, int MouseY);
        public event MouseDown onMouseDown;

        public delegate void MouseWheel(int MouseX, int MouseY);
        public event MouseWheel onMouseWheel;

        public delegate void ConnectedToMe(IPAddress ipAddress);
        public event ConnectedToMe onConnectedToMe;

        public delegate void DisconnectedFromMe(IPAddress ipAddress);
        public event DisconnectedFromMe onDisconnectedFromMe;

        public delegate void ChangeScreenResolution(int screenWidth, int screenHeight);
        public event ChangeScreenResolution onChangeScreenResolution;

        public delegate void MakeScreen();
        public event MakeScreen onMakeScreen;

        public delegate void ScreenReceived(byte[] screen);
        public event ScreenReceived onScreenReceived;

        public delegate void ConnectionTerminatedUnexpectedly();
        public event ConnectionTerminatedUnexpectedly onConnectionTerminatedUnexpectedly;

        public delegate void KeyPressed(byte key);
        public event KeyPressed onKeyPressed;

        public delegate void ChangeKeyBorderLayout(string language);
        public event ChangeKeyBorderLayout onChangeKeyBorderLayout;

        private ServerTCP ckeckIsOnlineReceiver;

        private ClientUDP clientUDP;
        private ClientTCP clientTCP;
        private ServerTCP commandsReceiver;
        private ClientTCP commandSender;

        private ServerTCP mouseReceiver;
        private ClientTCP mouseSender;

        private ServerTCP keyBoardReceiver;
        private ClientTCP keyBoardSender;

        private ClientTCP screenSender;
        private ServerTCP screenReceiver;

        private RDEndpoint mouseEndpoint;
        private RDEndpoint keyBoardEndpoint;
        private RDEndpoint screenEndpoint;
        private RDEndpoint otherClientScreenEndpoint;

        private IPEndPoint otherClientMouseEndpoint;
        private IPEndPoint otherClientKeyBoardEndpoint;

        private RDEndpoint otherClientEndpoint;  // к кому подсоединен


        private int serverBroadcastPort;
        private bool serverWasFound;
        private IPEndPoint serverEndpoit;

        private RDEndpoint myEndpointForTCP;
        private RDEndpoint endPointCheckOnline;

        private List<RDEndpoint> otherClients;
        private ErrorType clientlogInStatus;
        private ErrorType clientlogOutStatus;
        private ErrorType reloadOnlineClientsStatus;
        private ErrorType connectClientStatus;

        private string myLogin;

        private bool iHaveConnected;
        private bool smbConnectedToMe;
        private int screenWidth;
        private int screenHeight;

        private byte[] clientScreen;

        private const int requiredScreenWidth = 800;
        private const int requiredScreenHeight = 600;
        public RDClient(int portNumber) 
        {
            clientUDP = new ClientUDP();
            clientTCP = new ClientTCP();
            commandsReceiver = new ServerTCP();
            commandSender = new ClientTCP();
            otherClients = new List<RDEndpoint>();

            ckeckIsOnlineReceiver = new ServerTCP();

            mouseReceiver = new ServerTCP();
            mouseSender = new ClientTCP();

            keyBoardReceiver = new ServerTCP();
            keyBoardSender = new ClientTCP();

            screenReceiver = new ServerTCP();
            screenSender = new ClientTCP();

            mouseEndpoint = new RDEndpoint();
            keyBoardEndpoint = new RDEndpoint();
            screenEndpoint = new RDEndpoint();
            otherClientScreenEndpoint = new RDEndpoint();

            otherClientMouseEndpoint = new IPEndPoint(IPAddress.Any,0);
            otherClientKeyBoardEndpoint = new IPEndPoint(IPAddress.Any, 0);

            myEndpointForTCP = new RDEndpoint();
            endPointCheckOnline = new RDEndpoint();

            otherClientEndpoint = new RDEndpoint();

            serverBroadcastPort = portNumber;
            serverWasFound = false;
            serverEndpoit = null;

            clientScreen = null;
  
            clientlogInStatus = ErrorType.Ok;
            clientlogOutStatus = ErrorType.Ok;
            reloadOnlineClientsStatus = ErrorType.Ok;
            connectClientStatus = ErrorType.Ok;

            iHaveConnected = false;
            smbConnectedToMe = false;
        }
        ~RDClient() { }

        public List<RDEndpoint> GetOnlineClientsList()
        {
            return otherClients;
        }

        public void SetScreen(byte[] screen) { clientScreen = screen; }
        public int GetScreenWidth() { return screenWidth; }
        public int GetScreenHeight() { return screenHeight; }
        public void SetScreenSize(int screenWidth, int screenHeight) { this.screenWidth = screenWidth; this.screenHeight = screenHeight; }
        public ErrorType GetLogInStatus() { return clientlogInStatus; }
        public void SetLoginStatus(ErrorType status) { clientlogInStatus = status; }
        public ErrorType GetLogOutStatus() { return clientlogOutStatus; }
        public void SetLogOutStatus(ErrorType status) { clientlogOutStatus = status; }
        public ErrorType GetReloadOnlineClientsStatus() { return reloadOnlineClientsStatus; }
        public void SetReloadOnlineClientsStatus(ErrorType status) { reloadOnlineClientsStatus = status; }
        public ErrorType GetClientConnectStatus() { return connectClientStatus; }
        public void SetClientConnectStatus(ErrorType status) { connectClientStatus = status; }
        public bool IsConnectedToClient() { return iHaveConnected; }
     
        public void CloseConnectionToOtherClient()
        {
            iHaveConnected = false;
            smbConnectedToMe = false;

            otherClientEndpoint.ipAddress = null;
            otherClientEndpoint.portNumber = 0;

            otherClientMouseEndpoint.Address = null;
            otherClientMouseEndpoint.Port = 0;

            otherClientScreenEndpoint.ipAddress = null;
            otherClientScreenEndpoint.portNumber = 0;
        }

        private RDProtocol CreateLogInMessage(string Login, string Password)
        {
            RDProtocol message = new RDProtocol();
            message.commandType = CommandType.LogIn;
            ClientLoginData loginData = new ClientLoginData();
            ClientKeys keys = new ClientKeys();
            keys.ClientLogin = Login;
            keys.ClientPassword = Password;

            loginData.clientKeys = keys;
            loginData.clientTCPEndpoint = myEndpointForTCP;
            loginData.tcpCheckIsOnline = endPointCheckOnline;

            byte[] data = null;
            RDProtocolConvertor.ClientLoginDataToByteArray(loginData, out data);
            message.data = data;

            return message;
        }

        public void FindServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, serverBroadcastPort);          
            clientUDP.InitBroadCast(SocketType.Dgram, ProtocolType.Udp);
            RDProtocol message = new RDProtocol();
            message.commandType = CommandType.FindSever;

            while (!serverWasFound)
            {
                RDProtocol serverAnswer = null;
                clientUDP.Send(message, endPoint);
                clientUDP.Receive(out serverAnswer, out serverEndpoit);
                if (serverAnswer.commandType == CommandType.ServerFound)
                {
                    serverWasFound = true;
                    RDEndpoint tcpServerEndpoint = null;
                    RDProtocolConvertor.ByteArrayToRDEndpoint(serverAnswer.data,out tcpServerEndpoint);
                  //  serverEndpoit.Address = tcpServerEndpoint.ipAddress;
                    serverEndpoit.Port = tcpServerEndpoint.portNumber;
                    MessageBox.Show(Convert.ToString(serverEndpoit.Address));
                    MessageBox.Show(Convert.ToString(serverEndpoit.Port));
                }
            }            
        }

        private void MouseCommandReceived(object socket)
        {
            try
            {
                Socket client = (Socket)socket;
                RDProtocol message = null;
                mouseReceiver.Receive(out message, client);
                MouseCoordinates mouseCoordinates = null;
                RDProtocolConvertor.ByteArrayToMouseCoordinates(message.data, out mouseCoordinates);

                if (message.commandType == CommandType.MoveMouse)
                {
                    onMouseMove(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
                else if (message.commandType == CommandType.MouseLeftBtnCLick)
                {
                    onMouseLeftButtonClick(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
                else if (message.commandType == CommandType.MouseRightBtnCLick)
                {
                    onMouseRightButtonClick(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
                else if (message.commandType == CommandType.MouseDblClick)
                {
                    onMouseDblClick(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
                else if (message.commandType == CommandType.MouseWheel)
                {
                    onMouseWheel(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
                else if (message.commandType == CommandType.MouseUp)
                {
                    onMouseUp(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
                else if (message.commandType == CommandType.MouseDown)
                {
                    onMouseDown(mouseCoordinates.MouseX, mouseCoordinates.MouseY);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Mouse " + exception.Message);
            } 
        }

        private void ScreenMaker()
        {
            try
            {
                while (true)
                {
                    onMakeScreen();
                    if (smbConnectedToMe)
                    {
                        RDProtocol screenMessage = new RDProtocol();
                        screenMessage.commandType = CommandType.ShowScreen;
                        screenMessage.data = clientScreen;
                        screenSender.SendMsgSize(screenMessage);

                        RDProtocol serverAnswer = null;
                        screenSender.Receive(out serverAnswer);
                        screenSender.Send(screenMessage);

                        RDProtocol answer = null;
                        screenSender.Receive(out answer); // новое
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("screen maker " + exception.Message);
                CloseConnectionToOtherClient();
            } 
        }

       /* private void CommandToMakeScreen()
        {
            
            try
            {*/

              /*  IPEndPoint endPoint = new IPEndPoint(otherClientScreenEndpoint.ipAddress, otherClientScreenEndpoint.portNumber);
                screenSender.Init(SocketType.Stream, ProtocolType.Tcp);
                screenSender.ConnectToEndPoint(endPoint);*/

              //  onMakeScreen();

          /*      RDProtocol screenMessage = new RDProtocol();
                screenMessage.commandType = CommandType.ShowScreen;
                screenMessage.data = clientScreen;
                screenSender.SendMsgSize(screenMessage);

                RDProtocol serverAnswer = null;
                screenSender.Receive(out serverAnswer);
                screenSender.Send(screenMessage);

              //  screenSender.CloseConnection();
            }
            catch (Exception exception)
            {
                MessageBox.Show("command to make Screen " + exception.Message);
            } 
        }*/

        private void WaitForMouseCommand()
        {
            try
            {
                // IPEndPoint endPoint = new IPEndPoint(myEndpointForTCP.ipAddress, 0); //Any
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                mouseReceiver.Init(SocketType.Stream, ProtocolType.Tcp, endPoint);
                //    mouseEndpoint.ipAddress = myEndpointForTCP.ipAddress;   // не нужно!
                mouseEndpoint.portNumber = mouseReceiver.GetPortNumber();

                while (true)
                {
                    Socket client = null;
                    mouseReceiver.AcceptClient(out client);

                    ParameterizedThreadStart mouseThreadStart = new ParameterizedThreadStart(MouseCommandReceived);
                    Thread mouseThread = new Thread(mouseThreadStart);
                    mouseThread.Start(client);

                   /* ThreadStart screenThreadStart = new ThreadStart(CommandToMakeScreen);
                    Thread screenThread = new Thread(screenThreadStart);
                    screenThread.Start(); */ 
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Mouse control + screen" + exception.Message);
            }           
        }

        private void KeyBoardCommandReceived(object socket)
        {
            try
            {
                Socket client = (Socket)socket;
                RDProtocol message = null;
                keyBoardReceiver.Receive(out message, client);
                
                if (message.commandType == CommandType.PressKey)
                {
                    byte pressedKey = message.data[0];
                    onKeyPressed(pressedKey);
                }               
                else if (message.commandType == CommandType.ChangeLanguageLayout)
                {
                    string language = Encoding.ASCII.GetString(message.data);
                    onChangeKeyBorderLayout(language);
                }
            }
            catch (Exception exception)
            {

            }
        }
        private void WaitForKeyBoardCommand()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                keyBoardReceiver.Init(SocketType.Stream, ProtocolType.Tcp, endPoint);
                keyBoardEndpoint.portNumber = keyBoardReceiver.GetPortNumber();

                while (true)
                {
                    Socket client = null;
                    keyBoardReceiver.AcceptClient(out client);

                    ParameterizedThreadStart keyBoardThreadStart = new ParameterizedThreadStart(KeyBoardCommandReceived);
                    Thread keyBoardThread = new Thread(keyBoardThreadStart);
                    keyBoardThread.Start(client);
                }
            }
            catch (Exception exception)
            {
                onConnectionTerminatedUnexpectedly();
            } 
        }

        private void SetDisplayScreenThread(object screen)
        {
            onScreenReceived((byte[]) screen);
        }
        private void ScreenWasReceived(object socket)
        {
            try
            {
                while (iHaveConnected)
                {
                    Socket client = (Socket)socket;

                    int messageSize = 0;
                    screenReceiver.ReceiveMsgSize(out messageSize, client);

                    RDProtocol answer = new RDProtocol();
                    answer.commandType = CommandType.MessageSizeAccepted;
                    screenReceiver.Send(answer, client);

                    RDProtocol screenMessage = null;
                    screenReceiver.Receive(out screenMessage, client, messageSize);


                    screenReceiver.Send(answer, client); /// новое

                    ParameterizedThreadStart threadStart = new ParameterizedThreadStart(SetDisplayScreenThread);
                    Thread thread = new Thread(threadStart);
                    thread.Start(screenMessage.data);   
                }

             /*   MessageBox.Show("Screen");*/

               // onScreenReceived(screenMessage.data);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Screen received" + exception.Message);

                CloseConnectionToOtherClient();
            } 
        }
        private void WaitForScreens()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                screenReceiver.Init(SocketType.Stream, ProtocolType.Tcp, endPoint);
                screenEndpoint.portNumber = screenReceiver.GetPortNumber();

                while (true)
                {
                    Socket client = null;
                    screenReceiver.AcceptClient(out client);

                    ParameterizedThreadStart threadStart = new ParameterizedThreadStart(ScreenWasReceived);
                    Thread thread = new Thread(threadStart);
                    thread.Start(client);                    
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Wait for screenScreen " + exception.Message);

               // CloseConnectionToOtherClient();
            } 
        }
       /* public void SendScreen(byte[] screen)
        {
            try
            {
                RDProtocol screenMessage = new RDProtocol();
                screenMessage.data = screen;
                screenSender.SendMsgSize(screenMessage);

                RDProtocol serverAnswer = null;
                screenSender.Receive(out serverAnswer);

                screenSender.Send(screenMessage);
            }
            catch (Exception exception)
            {
               // onConnectionTerminatedUnexpectedly();
            } 
        }*/
        /*private void ScreenSender()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(otherClientScreenEndpoint.ipAddress, otherClientScreenEndpoint.portNumber);
                screenSender.Init(SocketType.Stream, ProtocolType.Tcp);

                 while (smbConnectedToMe)
                 {
                    screenSender.ConnectToEndPoint(endPoint);
                    onMakeScreen();

                    RDProtocol screenMessage = new RDProtocol();
                    screenMessage.data = clientScreen;
                    screenSender.SendMsgSize(screenMessage);

                    RDProtocol serverAnswer = null;
                    screenSender.Receive(out serverAnswer);

                    screenSender.Send(screenMessage);
                 }
                // screenEndpoint.portNumber = screenSender.GetPortNumber();
            }
            catch (Exception exception)
            {
               // onConnectionTerminatedUnexpectedly();
            } 
        }*/

        private void CheckAmIOnline()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            ckeckIsOnlineReceiver.Init(SocketType.Stream, ProtocolType.Tcp, endPoint);
            endPointCheckOnline.portNumber = ckeckIsOnlineReceiver.GetPortNumber();

            while (true)
            {
                try
                {
                    Socket client = null;                   
                    ckeckIsOnlineReceiver.AcceptClient(out client);       // сервер проверяет, жив ли клиент          
                }
                catch (Exception exception)             
                {

                }
            }
        }
        
        private void ClientAsTCPServer()
        {
           // IPEndPoint endPoint = new IPEndPoint(myEndpointForTCP.ipAddress,0); !!!
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            commandsReceiver.Init(SocketType.Stream, ProtocolType.Tcp, endPoint);
            myEndpointForTCP.portNumber = commandsReceiver.GetPortNumber();  
          
            while(true)
            {
                Socket client = null;
                commandsReceiver.AcceptClient(out client);
                ParameterizedThreadStart threadStart = new ParameterizedThreadStart(OneTcpClient);
                Thread thread = new Thread(threadStart);

                thread.Start(client);
            }
        }

        private RDProtocol CreateMouseKeyBoardMsg()
        {
            RDProtocol message = new RDProtocol();
            message.commandType = CommandType.ConnectToClient;

            ClientsInfo info = new ClientsInfo();
            List<RDEndpoint> mouseKeyBorard = new List<RDEndpoint>();
            mouseKeyBorard.Add(mouseEndpoint);
            mouseKeyBorard.Add(keyBoardEndpoint); // тут клавиатура
            info.clientsList = mouseKeyBorard;

            byte[] byteMessage = null;
            RDProtocolConvertor.ClientsInfoToByteArray(info, out byteMessage);
            message.data = byteMessage;

            return message;
        }

        private void OneTcpClient(object socket) 
        {
            Socket client = (Socket)socket;

            RDProtocol otherClientMsg = null;
            commandsReceiver.Receive(out otherClientMsg, (Socket)client);
            if (otherClientMsg.commandType == CommandType.ConnectToClient)
            {
                if (iHaveConnected)
                {
                    RDProtocol clientAnswer = new RDProtocol();
                    clientAnswer.commandType = CommandType.iAlreadyConnected;
                    commandsReceiver.Send(clientAnswer, (Socket)socket);
                }
                else if (smbConnectedToMe)
                {
                    RDProtocol clientAnswer = new RDProtocol();
                    clientAnswer.commandType = CommandType.SmbConnectedToMe;
                    commandsReceiver.Send(clientAnswer, (Socket)socket);
                }
                else
                {                      
                    RDProtocolConvertor.ByteArrayToRDEndpoint(otherClientMsg.data, out otherClientScreenEndpoint);  // новое!
                    otherClientScreenEndpoint.ipAddress = ((IPEndPoint)client.RemoteEndPoint).Address;  // новое

                    // новое
                    IPEndPoint endPoint = new IPEndPoint(otherClientScreenEndpoint.ipAddress, otherClientScreenEndpoint.portNumber);
                    screenSender.Init(SocketType.Stream, ProtocolType.Tcp);
                    screenSender.ConnectToEndPoint(endPoint);
                    // новое

                    smbConnectedToMe = true;  // новое



                    RDProtocol clientAnswer = CreateMouseKeyBoardMsg();
                    commandsReceiver.Send(clientAnswer, (Socket)socket);

                   // smbConnectedToMe = true;                    

                    onConnectedToMe(((IPEndPoint)(client.RemoteEndPoint)).Address);
                    onChangeScreenResolution(requiredScreenWidth, requiredScreenHeight);



                    // запуск потока для отправки скринов
             /*       Thread screenThread = new Thread(new ThreadStart(ScreenSender));
                    screenThread.IsBackground = true;
                    screenThread.Start();*/
                    ///
                }
            }
            else if (otherClientMsg.commandType == CommandType.DisconnectFromClient)
            {
                smbConnectedToMe = false;
                onDisconnectedFromMe(((IPEndPoint)(client.RemoteEndPoint)).Address);
                onChangeScreenResolution(screenWidth, screenHeight);
            }
        }

        private void DisconnectClient()
        {
            commandSender.Init(SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint clientEndpoint = new IPEndPoint(otherClientEndpoint.ipAddress, otherClientEndpoint.portNumber);
            commandSender.ConnectToEndPoint(clientEndpoint);
            Socket otherClientSocket = commandSender.GetSocket();

            RDProtocol messageDisconnect = new RDProtocol();
            messageDisconnect.commandType = CommandType.DisconnectFromClient;

            commandsReceiver.Send(messageDisconnect, otherClientSocket);
            otherClientEndpoint.ipAddress = null;
            otherClientEndpoint.portNumber = 0;           

            iHaveConnected = false;
            connectClientStatus = ErrorType.DisconnectOk;
            commandSender.CloseConnection();
        }

        public void ConnectToOtherClient(int clientIndex)
        {
            if (smbConnectedToMe)
            {
                connectClientStatus = ErrorType.SmbConnectedToMe;
                return;
            }
            if (!iHaveConnected)
            {

                iHaveConnected = true;  //новое



                otherClientEndpoint.ipAddress = otherClients[clientIndex].ipAddress;
                otherClientEndpoint.portNumber = otherClients[clientIndex].portNumber;
                
                commandSender.Init(SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint clientEndpoint = new IPEndPoint(otherClientEndpoint.ipAddress, otherClientEndpoint.portNumber);
                commandSender.ConnectToEndPoint(clientEndpoint);
                Socket otherClientSocket = commandSender.GetSocket();

                RDProtocol messageConnect = new RDProtocol();
                messageConnect.commandType = CommandType.ConnectToClient;

                byte[] byteConnectMsg = null;
                RDProtocolConvertor.RDEndpointToByteArray(screenEndpoint, out byteConnectMsg);
                messageConnect.data = byteConnectMsg;

                commandsReceiver.Send(messageConnect, otherClientSocket);       

                RDProtocol clientAnswer = null;
                commandsReceiver.Receive(out clientAnswer, otherClientSocket);
                if (clientAnswer.commandType == CommandType.ConnectToClient)
                {
                    byte[] byteMessage = clientAnswer.data;
                    ClientsInfo info = null;
                    RDProtocolConvertor.ByteArrayToClientsInfo(byteMessage, out info);
                   // otherClientMouseEndpoint = new IPEndPoint(info.clientsList[0].ipAddress, info.clientsList[0].portNumber);

                    otherClientMouseEndpoint = new IPEndPoint(((IPEndPoint)otherClientSocket.RemoteEndPoint).Address, info.clientsList[0].portNumber);
                    otherClientKeyBoardEndpoint = new IPEndPoint(((IPEndPoint)otherClientSocket.RemoteEndPoint).Address, info.clientsList[1].portNumber);

                  //  iHaveConnected = true;

                    connectClientStatus = ErrorType.Ok;                    
                }
                else if (clientAnswer.commandType == CommandType.SmbConnectedToMe)
                {
                    connectClientStatus = ErrorType.SmbConnectedToOtherClient;
                }
                else if (clientAnswer.commandType == CommandType.iAlreadyConnected)
                {
                    connectClientStatus = ErrorType.OtherClientAlreadyConnected;
                }
                commandSender.CloseConnection();
            }
            else 
            {
                if (otherClientEndpoint.ipAddress.ToString() == otherClients[clientIndex].ipAddress.ToString() &&
                otherClientEndpoint.portNumber == otherClients[clientIndex].portNumber)
                {
                    DisconnectClient();
                }
                else
                {
                    connectClientStatus = ErrorType.NeedToDisconnect;
                }
            }            
        }

        
        public void Start()
        {
            Thread tcpServerThread = new Thread(new ThreadStart(ClientAsTCPServer));   // Для приема TCP коннекта от других клиентов
            tcpServerThread.IsBackground = true;
            tcpServerThread.Start();

            Thread mouseThread = new Thread(new ThreadStart(WaitForMouseCommand));   // Для приема TCP коннекта от других клиентов
            mouseThread.IsBackground = true;
            mouseThread.Start();

            Thread keyBoardThread = new Thread(new ThreadStart(WaitForKeyBoardCommand));
            keyBoardThread.IsBackground = true;
            keyBoardThread.Start();

            Thread screenThread = new Thread(new ThreadStart(WaitForScreens));
            screenThread.IsBackground = true;
            screenThread.Start();

            Thread checkOnlineThread = new Thread(new ThreadStart(CheckAmIOnline));
            checkOnlineThread.IsBackground = true;
            checkOnlineThread.Start();
        }

        public void LogIn(string Login, string Password)
        {
            if (Login.Length == 0 || Password.Length == 0)
            {
                clientlogInStatus = ErrorType.EmptyData;
                return;
            }
            clientlogInStatus = ErrorType.Ok;

            clientTCP.Init(SocketType.Stream, ProtocolType.Tcp);

            RDProtocol messageLogin = CreateLogInMessage(Login, Password);
            messageLogin.commandType = CommandType.LogIn;           
        
            clientTCP.ConnectToEndPoint(serverEndpoit);
         //   Socket serverSocket = clientTCP.GetSocket();
      
            clientTCP.SendMsgSize(messageLogin);
            RDProtocol serverAnswer = null;
            clientTCP.Receive(out serverAnswer);

            if (serverAnswer.commandType == CommandType.MessageSizeAccepted)
            {              
                RDProtocol otherClientsInfo = null;
                clientTCP.Send(messageLogin);

                RDProtocol logInStatus = null;
                clientTCP.Receive(out logInStatus);
                if (logInStatus.commandType == CommandType.LogInOk)
                {
                    myLogin = Login;
                    int clientsInfoSize = 0;
                    clientTCP.ReceiveMsgSize(out clientsInfoSize);

                    RDProtocol sizeAnswer = new RDProtocol();
                    sizeAnswer.commandType = CommandType.MessageSizeAccepted;
                    clientTCP.Send(sizeAnswer);
                    clientTCP.Receive(out otherClientsInfo, clientsInfoSize);
                    ClientsInfo info = null;
                    RDProtocolConvertor.ByteArrayToClientsInfo(otherClientsInfo.data, out info);
                    otherClients = info.clientsList;


                    // поток для создания скринов
                    Thread makeScreensThread = new Thread(new ThreadStart(ScreenMaker));
                    makeScreensThread.IsBackground = true;
                    makeScreensThread.Start();
                    
                }
                else if (logInStatus.commandType == CommandType.LogInError)
                {
                    clientlogInStatus = ErrorType.IncorrectData;
                }
                else if (logInStatus.commandType == CommandType.ClientHasAlreadyBeenLogIn)
                {
                    clientlogInStatus = ErrorType.ClientHasAlreadyBeenLogIn;
                }
            }
            else
            {
                clientlogInStatus = ErrorType.ServerError;
            }
            clientTCP.CloseConnection();           
        }

        public void LogOut()
        {
            clientTCP.Init(SocketType.Stream, ProtocolType.Tcp);

            RDProtocol messageLogout = new RDProtocol();
            messageLogout.commandType = CommandType.LogOut;

            ClientKeys clientKeys = new ClientKeys();
            clientKeys.ClientLogin = myLogin;
            clientKeys.ClientPassword = null;

            byte[] byteMessage = null;
            RDProtocolConvertor.ClientKeysToByteArray(clientKeys, out byteMessage);
            messageLogout.data = byteMessage;
         
            clientTCP.ConnectToEndPoint(serverEndpoit);
          //  Socket serverSocket = clientTCP.GetSocket();
         
            clientTCP.SendMsgSize(messageLogout);

            RDProtocol serverAnswer = null;
            clientTCP.Receive(out serverAnswer);

            if (serverAnswer.commandType == CommandType.MessageSizeAccepted)
            {
                clientTCP.Send(messageLogout);                
                myLogin = null;
                clientlogOutStatus = ErrorType.Ok;
                CloseConnectionToOtherClient();
            }
            else
            {
                clientlogOutStatus = ErrorType.ServerError;
            }
            clientTCP.CloseConnection();
        }
        public void ReloadOnlineClientsList()
        {
            clientTCP.Init(SocketType.Stream, ProtocolType.Tcp);

            RDProtocol messageClientsList = new RDProtocol();
            messageClientsList.commandType = CommandType.ClientsInfo;
            
            ClientKeys clientKeys = new ClientKeys();
            clientKeys.ClientLogin = myLogin;
            byte[] byteMessage = null;
            RDProtocolConvertor.ClientKeysToByteArray(clientKeys, out byteMessage);
            messageClientsList.data = byteMessage;

            clientTCP.ConnectToEndPoint(serverEndpoit);
           // Socket serverSocket = clientTCP.GetSocket();

            clientTCP.SendMsgSize(messageClientsList);

            RDProtocol serverAnswer = null;
            clientTCP.Receive(out serverAnswer);

            if (serverAnswer.commandType == CommandType.MessageSizeAccepted)
            {                
                clientTCP.Send(messageClientsList);
                int messageSize = 0;
                clientTCP.ReceiveMsgSize(out messageSize);
                clientTCP.Send(messageClientsList);

                RDProtocol onlineCLients = null;
                clientTCP.Receive(out onlineCLients, messageSize);
                ClientsInfo info = null;
                RDProtocolConvertor.ByteArrayToClientsInfo(onlineCLients.data, out info);
                otherClients = info.clientsList;                
                reloadOnlineClientsStatus = ErrorType.Ok;
                if (otherClients.Count == 0)
                {
                    CloseConnectionToOtherClient();
                }
            }
            else
            {
                reloadOnlineClientsStatus = ErrorType.ServerError;
            }
            clientTCP.CloseConnection();
        }

        public void SendMouseCommand(MouseCoordinates mouseCoordinates, CommandType command)
        {
            try
            {
                mouseSender.Init(SocketType.Stream, ProtocolType.Tcp);
                RDProtocol mouseCommandMsg = new RDProtocol();
                mouseCommandMsg.commandType = command;


                byte[] byteMessage = null;
                RDProtocolConvertor.MouseCoordinatesToByteArray(mouseCoordinates, out byteMessage);
                mouseCommandMsg.data = byteMessage;

                mouseSender.ConnectToEndPoint(otherClientMouseEndpoint);
                mouseSender.Send(mouseCommandMsg);
                mouseSender.CloseConnection();
            }
            catch (Exception exception)
            {
                onConnectionTerminatedUnexpectedly();
            }
        }

        public void SendKeyBoardCommand(CommandType command, byte key = 0, string language = null)
        {
            try
            {
                keyBoardSender.Init(SocketType.Stream, ProtocolType.Tcp);
                RDProtocol keyBoardMsg = new RDProtocol();
                keyBoardMsg.commandType = command;

                if (command == CommandType.PressKey)
                {
                    byte[] byteMessage = new byte[1];
                    byteMessage[0] = key;
                    keyBoardMsg.data = byteMessage;
                }
                else if (command == CommandType.ChangeLanguageLayout)
                {
                    keyBoardMsg.data = Encoding.ASCII.GetBytes(language);
                }
                keyBoardSender.ConnectToEndPoint(otherClientKeyBoardEndpoint);
                keyBoardSender.Send(keyBoardMsg);
                keyBoardSender.CloseConnection();
            }
            catch (Exception exception)
            {
                onConnectionTerminatedUnexpectedly();
            }
        }
     
    }
}
