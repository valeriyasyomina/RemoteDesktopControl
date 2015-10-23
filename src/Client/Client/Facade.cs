using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using RDProtocolLib;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Client
{
    public enum ErrorType  {Ok, EmptyData, IncorrectData, ServerError, ImpossibleToConnect, DisconnectOk, NeedToDisconnect,
                            SmbConnectedToMe, OtherClientAlreadyConnected, SmbConnectedToOtherClient,
                            ClientHasAlreadyBeenLogIn};
    public class Facade
    {
        public delegate void MouseMove(int MouseX, int MouseY);
        public static event MouseMove onMouseMove;

        public delegate void MouseRightButtonClick(int MouseX, int MouseY);
        public static event MouseRightButtonClick onMouseRightButtonClick;

        public delegate void MouseLeftButtonClick(int MouseX, int MouseY);
        public static event MouseLeftButtonClick onMouseLeftButtonClick;

        public delegate void MouseDblClick(int MouseX, int MouseY);
        public static event MouseDblClick onMouseDblClick;

        public delegate void MouseUp(int MouseX, int MouseY);
        public static event MouseUp onMouseUp;

        public delegate void MouseDown(int MouseX, int MouseY);
        public static event MouseDown onMouseDown;

        public delegate void MouseWheel(int MouseX, int MouseY);
        public static event MouseWheel onMouseWheel;

        public delegate void ConnectedToMe(IPAddress ipAddress);
        public static event ConnectedToMe onConnectedToMe;

        public delegate void DisconnectedFromMe(IPAddress ipAddress);
        public static event DisconnectedFromMe onDisconnectedFromMe;

        public delegate void ChangeScreenResolution(int screenWidth, int screenHeight);
        public static event ChangeScreenResolution onChangeScreenResolution;

        public delegate void MakeScreen();
        public static event MakeScreen onMakeScreen;

        public delegate void ScreenReceived(byte[] screen);
        public static event ScreenReceived onScreenReceived;

        public delegate void ConnectionTerminatedUnexpectedly();
        public static event ConnectionTerminatedUnexpectedly onConnectionTerminatedUnexpectedly;

        public delegate void KeyPressed(byte key);
        public static event KeyPressed onKeyPressed;
     
        public delegate void ChangeKeyBorderLayout(string language);
        public static event ChangeKeyBorderLayout onChangeKeyBorderLayout;


        private static RDClient client;
        public Facade() { }
        ~Facade() { }
        public static ErrorType GetClientLogInStatus() { return client.GetLogInStatus(); }
        public static ErrorType GetClientLogOutStatus() { return client.GetLogOutStatus(); }
        public static ErrorType GetReloadOnlineClientStatus() { return client.GetReloadOnlineClientsStatus(); }
        public static ErrorType GetCLientConnectStatus() { return client.GetClientConnectStatus(); }
        public static List<RDEndpoint> GetOnlineClientsList() { return client.GetOnlineClientsList(); }
        public static bool IsConnectedToOtherClient() { return client.IsConnectedToClient(); }

        public static void SetClientScreenSize(int screenWidth, int screenHeight) { client.SetScreenSize(screenWidth, screenHeight); }

        private static void onMouseMoveEventHandler(int MouseX, int MouseY)
        {
            onMouseMove(MouseX, MouseY);
        }
        private static void onMouseLeftBtnCLickEventHandler(int MouseX, int MouseY)
        {
            onMouseLeftButtonClick(MouseX, MouseY);
        }
        private static void onMouseRightBtnClickEventHandler(int MouseX, int MouseY)
        {
            onMouseRightButtonClick(MouseX, MouseY);
        }
        private static void onMouseDblCLickEventHandler(int MouseX, int MouseY)
        {
            onMouseDblClick(MouseX, MouseY);
        }
        private static void onMouseUpEventHandler(int MouseX, int MouseY)
        {
            onMouseUp(MouseX, MouseY);
        }
        private static void onMouseDownEventHandler(int MouseX, int MouseY)
        {
            onMouseDown(MouseX, MouseY);
        }
        private static void onMouseWheelEventHandler(int MouseX, int MouseY)
        {
            onMouseWheel(MouseX, MouseY);
        }
        private static void onConnectedToMeEventHandler(IPAddress ipAddress)
        {
            onConnectedToMe(ipAddress);
        }
        private static void onDisconnectedFromMeEventHandler(IPAddress ipAddress)
        {
            onDisconnectedFromMe(ipAddress);
        }
        private static void onChangeScreenResolutionEventHandler(int screenWidth, int screenHeight)
        {
            onChangeScreenResolution(screenWidth, screenHeight);
        }
        private static void onMakeScreenEventHandler()
        {
            onMakeScreen();
        }
        private static void onScreenReceivedEventHandler(byte[] screen)
        {
            onScreenReceived(screen);
        }
        private static void onConnTerminatedUnexpEventHandler()
        {
            onConnectionTerminatedUnexpectedly();
        }
        private static void onKeyPressedEventHandler(byte key)
        {
            onKeyPressed(key);
        }       
        private static void onChangeKeyBoardLayoutEventHandler(string language)
        {
            onChangeKeyBorderLayout(language);
        }
        public static void Init()
        {
            string logFileName = ConfigurationManager.AppSettings["LogFileName"];
            int broadcastPort = Convert.ToInt32(ConfigurationManager.AppSettings["UdpBroadcastPort"]);
            client = new RDClient(broadcastPort);

            client.onMouseMove += onMouseMoveEventHandler;
            client.onMouseLeftButtonClick += onMouseLeftBtnCLickEventHandler;
            client.onMouseRightButtonClick += onMouseRightBtnClickEventHandler;
            client.onMouseDblClick += onMouseDblCLickEventHandler;
            client.onMouseUp += onMouseUpEventHandler;
            client.onMouseDown += onMouseDownEventHandler;
            client.onMouseWheel += onMouseWheelEventHandler;

            client.onConnectedToMe += onConnectedToMeEventHandler;
            client.onDisconnectedFromMe += onDisconnectedFromMeEventHandler;
            client.onChangeScreenResolution += onChangeScreenResolutionEventHandler;

            client.onMakeScreen += onMakeScreenEventHandler;
            client.onScreenReceived += onScreenReceivedEventHandler;

            client.onKeyPressed += onKeyPressedEventHandler;           
            client.onChangeKeyBorderLayout += onChangeKeyBoardLayoutEventHandler;

            client.onConnectionTerminatedUnexpectedly += onConnTerminatedUnexpEventHandler;

            client.FindServer();
            client.Start();
        }
    
        public static int GetClientScreenWidth()
        {
            return client.GetScreenWidth();
        }
        public static int GetClientScreenHeight()
        {
            return client.GetScreenHeight();
        }
        public static void CloseAllClientConnections()
        {
            try
            {
                client.CloseConnectionToOtherClient();
            }
            catch (Exception exception)
            {
            }
        }
        public static void SetClientScreen(object screen)
        {
            client.SetScreen((byte[]) screen);       
        }
        public static void SendKeyBoardCommandToClient(CommandType command, byte key = 0, string language = null)
        {
            client.SendKeyBoardCommand(command, key, language);
        }

        public static void SendMouseCommandToClient(int MouseX, int MouseY, CommandType command)
        {
            MouseCoordinates mouseCoordinates = new MouseCoordinates();
            mouseCoordinates.MouseX = MouseX;
            mouseCoordinates.MouseY = MouseY;
            client.SendMouseCommand(mouseCoordinates, command);
        }

        public static void ConnectClientToOtherClient(object clientIndex)
        {
           try
           {
               client.ConnectToOtherClient((int) clientIndex);
           }
           catch (SocketException exception)
           {
               client.SetClientConnectStatus(ErrorType.ImpossibleToConnect);
           }
        }
        public static void ClientLogIn(object clientKeys)
        {
            try
            {
                client.LogIn(((ClientKeys)clientKeys).ClientLogin, ((ClientKeys)clientKeys).ClientPassword);
            }
            catch (SocketException exeption)
            {
                client.SetLoginStatus(ErrorType.ImpossibleToConnect);
            }
        }
        public static void ClientLogOut()
        {
            try
            {
                client.LogOut();
            }
            catch (SocketException exeption)
            {
                client.SetLogOutStatus(ErrorType.ImpossibleToConnect);
            }
        }
        public static void ReloadOnlineClientsList()
        {
            try
            {
                client.ReloadOnlineClientsList();
            }
            catch (SocketException exeption)
            {
                client.SetReloadOnlineClientsStatus(ErrorType.ImpossibleToConnect);
            }
        }       

    }
      
}
