using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace RDProtocolLib
{
    public enum CommandType { FindSever, LogIn, LogOut, SignIn, MoveMouse, PressKey, ShowScreen, ClientsInfo, ConnectToClient, ServerFound,
                                LogInOk, LogInError, MessageSize, MessageSizeAccepted, DisconnectFromClient, MouseDblClick, MouseLeftBtnCLick,
                                MouseRightBtnCLick, MouseWheel, MouseUp, MouseDown, iAlreadyConnected, SmbConnectedToMe,
                                ChangeLanguageLayout, ClientHasAlreadyBeenLogIn}
                                
    [Serializable]
    public class RDProtocol
    {       
        public CommandType commandType { get; set; }
        public byte[] data { get; set; }       
    }

    [Serializable]
    public class MouseCoordinates
    {
        public int MouseX { get; set; }
        public int MouseY { get; set; }
    }

    [Serializable]
    public class ClientKeys
    {
        public string ClientLogin { get; set; }
        public string ClientPassword { get; set; }
    }
    [Serializable]
    public class RDEndpoint
    {
        public IPAddress ipAddress { get; set; }
        public int portNumber { get; set; }
    }
    [Serializable]
    public class ClientsInfo
    {
        public List<RDEndpoint> clientsList { get; set; }
    }
    [Serializable]
    public class ClientLoginData
    {
        public ClientKeys clientKeys { get; set; }
        public RDEndpoint clientTCPEndpoint { get; set; }
        public RDEndpoint tcpCheckIsOnline { get; set; }
    }
}
