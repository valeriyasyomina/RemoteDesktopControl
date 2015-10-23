using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RDProtocolLib;
using System.Threading;
using System.Windows.Input.Manipulations;
using System.Drawing;
using System.Net;
using System.IO;


namespace Client
{
    public partial class ClientForm : Form
    {

        public ClientForm()
        {            
           InitializeComponent();
           pbScreen.MouseMove += pbScreenOnMouseMove;
           pbScreen.MouseClick += pbScreenOnMouseClick;
           pbScreen.MouseDoubleClick += pbScreenOnMouseDblClick;
           pbScreen.MouseDown += pbScreenOnMouseDown;
           pbScreen.MouseUp += pbScreenOnMouseUp;
           pbScreen.MouseWheel += pbScreenMouseWheel;
           
           pbScreen.PreviewKeyDown += pbScreenOnPreviewKeyDown;

           Facade.onMouseMove += onMouseMoveEventHandler;
           Facade.onMouseLeftButtonClick += onMouseLeftBtnCLickEventHandler;
           Facade.onMouseRightButtonClick += onMouseRightBtnClickEventHandler;
           Facade.onMouseDblClick += onMouseDblCLickEventHandler;
           Facade.onMouseUp += onMouseUpEventHandler;
           Facade.onMouseDown += onMouseDownEventHandler;
           Facade.onMouseWheel += onMouseWheelEventHandler;

           Facade.onConnectedToMe += onConnectedToMeEventHandler;
           Facade.onDisconnectedFromMe += onDisconnectedFromMeEventHandler;
           Facade.onChangeScreenResolution += onChangeScreenResolutionEventHandler;

           Facade.onMakeScreen += onMakeScreenEventHandler;
           Facade.onScreenReceived += onScreenReceivedEventHandler;

           Facade.onConnectionTerminatedUnexpectedly += onConnTerminatedUnexpEventHandler;

           Facade.onKeyPressed += onKeyPressedEventHandler;        
           Facade.onChangeKeyBorderLayout += onChangeKeyBoardLayoutEventHandler;

           Rectangle screenResolution = InterfaceManipulation.GetCurrentScreenResolution();
           Facade.SetClientScreenSize(screenResolution.Width, screenResolution.Height);

           InterfaceManipulation.InitScreenDisplayParameters();
        }
       
        private static void onKeyPressedEventHandler(byte key)
        {
            InterfaceManipulation.PressKeyBoardKey(key);
        }
        private static void onChangeKeyBoardLayoutEventHandler(string language)
        {
            InterfaceManipulation.ChangeKeyBoardLayout(language);
        }
        private static void onConnTerminatedUnexpEventHandler()
        {
            Facade.CloseAllClientConnections();
            InterfaceManipulation.ChangeScreenResolution(Facade.GetClientScreenWidth(), Facade.GetClientScreenHeight());
            MessageBox.Show("Соединение было неожиданно разорвано!");           
        }

        private void onMakeScreenEventHandler()
        {
            byte[] screenDisplay = InterfaceManipulation.GetDisplayScreen();

            ParameterizedThreadStart screenThreadStart = new ParameterizedThreadStart(Facade.SetClientScreen); 
            Thread screenThread = new Thread(screenThreadStart);
            screenThread.Start(screenDisplay);
        }
        private void onScreenReceivedEventHandler(byte[] screen)
        {
            InterfaceManipulation.SetScreenOnPictureBox(screen, ref pbScreen);
           // MessageBox.Show("Screen");
        }       

        private void onChangeScreenResolutionEventHandler(int screenWidth, int screenHeight)
        {
            InterfaceManipulation.ChangeScreenResolution(screenWidth, screenHeight);
        }
        private void onMouseMoveEventHandler(int MouseX, int MouseY)
        {
            InterfaceManipulation.SetMouseCursorPosition(MouseX, MouseY);
        }
        private void onMouseLeftBtnCLickEventHandler(int MouseX, int MouseY)
        {
            InterfaceManipulation.MouseLeftButtonClick(MouseX, MouseY);
        }
        private void onMouseRightBtnClickEventHandler(int MouseX, int MouseY)
        {
             InterfaceManipulation.MouseRightButtonClick(MouseX, MouseY);
        }        
        private void onMouseDblCLickEventHandler(int MouseX, int MouseY)
        {
             InterfaceManipulation.MouseDblClick(MouseX, MouseY);
        }
        private void onMouseUpEventHandler(int MouseX, int MouseY)
        {
            InterfaceManipulation.MouseUp(MouseX, MouseY);
        }
        private void onMouseDownEventHandler(int MouseX, int MouseY)
        {
            InterfaceManipulation.MouseDown(MouseX, MouseY);
        }
        private void onMouseWheelEventHandler(int MouseX, int MouseY)
        {
            InterfaceManipulation.MouseWheel(MouseX, MouseY);
        }

        private void onConnectedToMeEventHandler(IPAddress ipAddress)
        {
            MessageBox.Show("Пользователь " + ipAddress.ToString()  +  
                            " произвел удаленное подключение к Вашему компьютеру!\nВнимание!\nСейчас произойдет смена разрешения экрана");
        }
        private void onDisconnectedFromMeEventHandler(IPAddress ipAddress)
        {
            MessageBox.Show("Пользователь " + ipAddress.ToString() +
                            " завершил сеанс удаленного подключения!");
        }

        private void pbScreenOnMouseDown(object sender, MouseEventArgs eventArgs)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MouseDown);
            }
        }
        private void pbScreenOnMouseUp(object sender, MouseEventArgs eventArgs)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MouseUp);
            }
        }

        private void pbScreenMouseWheel(object sender, MouseEventArgs eventArgs)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MouseWheel);
            }
        }
        private void pbScreenOnMouseMove(object sender, MouseEventArgs eventArgs)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MoveMouse);
            }
        }
        private void pbScreenOnMouseClick(object sender, MouseEventArgs eventArgs)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                if (eventArgs.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MouseLeftBtnCLick);
                }
                else if (eventArgs.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MouseRightBtnCLick);
                }
            }
        }
        private void pbScreenOnMouseDblClick(object sender, MouseEventArgs eventArgs)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                Facade.SendMouseCommandToClient(eventArgs.Location.X, eventArgs.Location.Y, RDProtocolLib.CommandType.MouseDblClick);
            }
        }

        private void pbScreenOnPreviewKeyDown(object sender, PreviewKeyDownEventArgs args)
        {
            bool isConnected = Facade.IsConnectedToOtherClient();
            if (isConnected)
            {
                if ( (Control.ModifierKeys & Keys.Shift) == Keys.Shift && (Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                {
                    InputLanguage currentLanguage = InputLanguage.CurrentInputLanguage;
                    Facade.SendKeyBoardCommandToClient(RDProtocolLib.CommandType.ChangeLanguageLayout,0,currentLanguage.Culture.EnglishName);                 
                }
                else
                {
                    Facade.SendKeyBoardCommandToClient(RDProtocolLib.CommandType.PressKey,((byte)args.KeyValue));
                }
            }
                
        }


        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                ClientKeys clientKeys = new ClientKeys();
                clientKeys.ClientLogin = edtLogin.Text;
                clientKeys.ClientPassword = edtPassword.Text;

                ParameterizedThreadStart logInThreadStart = new ParameterizedThreadStart(Facade.ClientLogIn);
                logInThreadStart += onLogInHandler;

                Thread logInThread = new Thread(logInThreadStart);
                logInThread.Start(clientKeys);
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void onLogInHandler(object data)
        {
            ErrorType logInStatus = Facade.GetClientLogInStatus();
            if (logInStatus == ErrorType.EmptyData)
            {
                MessageBox.Show("Введите логин и пароль!");
            }
            else if (logInStatus == ErrorType.IncorrectData)
            {
                MessageBox.Show("Ошибка авторизации!\nПроверьте введенные данные!");
            }
            else if (logInStatus == ErrorType.ServerError)
            {
                MessageBox.Show("Внутренняя ошибка сервера!");
            }
            else if (logInStatus == ErrorType.ImpossibleToConnect)
            {
                MessageBox.Show("Ошибка подключения к серверу!\nСервер не отвечает или отсутствует соединение по сети!");
            }
            else if (logInStatus == ErrorType.ClientHasAlreadyBeenLogIn)
            {
                MessageBox.Show("Клиент с введенным логин и паролем уже был авторизован!");
            }
            else if (logInStatus == ErrorType.Ok)
            {
                MessageBox.Show("Авторизация прошла успешно!");
                pbScreen.Invoke(new Action(() => pbScreen.Select()));
              
                mainMenu.Invoke(new Action(() => mainMenu.Visible = true));
                lblLogIn.Invoke(new Action(() => lblLogIn.Visible = false));
                edtLogin.Invoke(new Action(() => edtLogin.Visible = false));
                edtPassword.Invoke(new Action(() => edtPassword.Visible = false));
                btnLogIn.Invoke(new Action(() => btnLogIn.Visible = false));
                mainPanel.Invoke(new Action(() => mainPanel.Visible = true));
                pbScreen.Invoke(new Action(() => pbScreen.Visible = true));

                List<RDEndpoint> onlineClients = Facade.GetOnlineClientsList();
                FillOnlineClientsList(onlineClients);             
            }
        }
        private void FillOnlineClientsList(List<RDEndpoint> onlineClients)
        {
            lbxOnlineClients.Invoke(new Action(() => lbxOnlineClients.Items.Clear()));
            if (onlineClients.Count == 0)
            {
                MessageBox.Show("К сожалению, на данный момент нет ни одного клиента онлайн!");                                   
            }
            else
            {                
                for (int i = 0; i < onlineClients.Count; i++)
                {
                    string clientInfoString = onlineClients[i].ipAddress + ":" + Convert.ToString(onlineClients[i].portNumber);
                 
                    lbxOnlineClients.Invoke(new Action(() =>  lbxOnlineClients.Items.Add(clientInfoString)));                    
                }
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Название программного продукта: Подключение к удаленной рабочей станции\n" +
                        "Авторы: Щетинин Г. А., Сёмина В. А\nГруппа: ИУ7 - 71\n2015 год");
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ThreadStart logOutThreadStart = new ThreadStart(Facade.ClientLogOut);
                logOutThreadStart += onLogOutHandler;
                Thread logOutThread = new Thread(logOutThreadStart);
                logOutThread.Start();
                
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void onReloadOnlineClients()
        {
            ErrorType reloadStatus = Facade.GetReloadOnlineClientStatus();
            if (reloadStatus == ErrorType.ServerError)
            {
                MessageBox.Show("Внутренняя ошибка сервера!");
            }
            else if (reloadStatus == ErrorType.ImpossibleToConnect)
            {
                MessageBox.Show("Ошибка подключения к серверу!\nСервер не отвечает или отсутствует соединение по сети!");
            }
            else if (reloadStatus == ErrorType.Ok)
            {
                List<RDEndpoint> onlineClients = Facade.GetOnlineClientsList();
                FillOnlineClientsList(onlineClients);      
            }
        }

        private void onLogOutHandler()
        {
            ErrorType logOutStatus = Facade.GetClientLogOutStatus();
            if (logOutStatus == ErrorType.ServerError)
            {
                MessageBox.Show("Внутренняя ошибка сервера!");
            }
            else if (logOutStatus == ErrorType.ImpossibleToConnect)
            {
                MessageBox.Show("Ошибка подключения к серверу!\nСервер не отвечает или отсутствует соединение по сети!");
            }
            else if (logOutStatus == ErrorType.Ok)
            {
                mainMenu.Invoke(new Action(() => mainMenu.Visible = false));
                lblLogIn.Invoke(new Action(() => lblLogIn.Visible = true));
                edtLogin.Invoke(new Action(() => edtLogin.Visible = true));
                edtPassword.Invoke(new Action(() => edtPassword.Visible = true));
                btnLogIn.Invoke(new Action(() => btnLogIn.Visible = true));
                mainPanel.Invoke(new Action(() => mainPanel.Visible = false));
                pbScreen.Invoke(new Action(() => pbScreen.Visible = false));
            }           
        }
        private void onConnectToCLientHandler(object data)
        {
            ErrorType connectToCLient = Facade.GetCLientConnectStatus();

            if (connectToCLient == ErrorType.ImpossibleToConnect)
            {
                MessageBox.Show("Ошибка подключения к другому пользователю!\nПользователь не отвечает или отсутствует соединение по сети!");
            }
            else if (connectToCLient == ErrorType.Ok)
            {
                MessageBox.Show("Подключение было успешно установлено!");
                pbScreen.Invoke(new Action(() => pbScreen.Select()));
            }
            else if (connectToCLient == ErrorType.NeedToDisconnect)
            {
                MessageBox.Show("У Вас открыто другое удаленное подключение!\nЧтобы подключиться к другому пользователю, " +
                    "необходимо закрыть текущее подключение.");
            }
            else if (connectToCLient == ErrorType.DisconnectOk)
            {
                MessageBox.Show("Сеанс удаленное подключения был завершен!");
            }
            else if (connectToCLient == ErrorType.SmbConnectedToMe)
            {
                MessageBox.Show("Вы не можете осуществлять удаленное подключение, пока не завершен сеанс, связанный с Вами!");
            }
            else if (connectToCLient == ErrorType.SmbConnectedToOtherClient)
            {
                MessageBox.Show("У пользователя, к которому вы хотите подключиться, уже открыто подключение!");
            }
            else if (connectToCLient == ErrorType.OtherClientAlreadyConnected)
            {
                MessageBox.Show("Пользователь, к которому вы хотите подключиться, производит свой сеанс удаленного подключения!");
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            mainMenu.Visible = false;
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для подключения к удаленному рабочему столу дважды кликните\n" +
                "по выбранному клиенту в списке.\n" +
                "Перед подключением нажмите кнопку Обновить список клиентов, чтобы\n" +
                "предотвратить проблему подключения к недоступной рабочей станции.");
        }

        private void просмотрСпискаОнлайнУчастниковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ThreadStart reloadOnlineClientsThreadStart = new ThreadStart(Facade.ReloadOnlineClientsList);
                reloadOnlineClientsThreadStart += onReloadOnlineClients;
                Thread reloadOnlineThread = new Thread(reloadOnlineClientsThreadStart);
                reloadOnlineThread.Start();

            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void lbxOnlineClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = lbxOnlineClients.SelectedIndex;
                if (selectedIndex == -1)
                {
                     MessageBox.Show("Вы не выбрали пользователя для подключения!");
                }
                else
                {
                    ParameterizedThreadStart connectClientThreadStart = new ParameterizedThreadStart(Facade.ConnectClientToOtherClient);
                    connectClientThreadStart += onConnectToCLientHandler;
                    Thread connectClientThread = new Thread(connectClientThreadStart);
                    connectClientThread.Start(selectedIndex);
                }

            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }

        }

        private void помощьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для подключения к удаленному рабочему столу дважды кликните\n" +
                "по выбранному клиенту в списке.\n" +
                "Перед подключением нажмите кнопку Обновить список клиентов, чтобы\n" +
                "предотвратить проблему подключения к недоступной рабочей станции.");
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ThreadStart logOutThreadStart = new ThreadStart(Facade.ClientLogOut);
                logOutThreadStart += onLogOutHandler;
                Thread logOutThread = new Thread(logOutThreadStart);
                logOutThread.Start();

            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void помощьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Для подключения к удаленному рабочему столу дважды кликните\n" +
                "по выбранному клиенту в списке.\n" +
                "Перед подключением нажмите кнопку Обновить список клиентов, чтобы\n" +
                "предотвратить проблему подключения к недоступной рабочей станции.");
        }
        
    }
}
