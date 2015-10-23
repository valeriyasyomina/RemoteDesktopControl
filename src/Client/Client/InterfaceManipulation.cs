using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using User32Lib;


namespace Client
{
  
    public class InterfaceManipulation
    {
        private static string Russian = "Russian (Russia)";
        private static string English = "English (United States)";
        public static void SetMouseCursorPosition(int MouseX, int MouseY)
        {
            Mouse.SetCursorPosition(MouseX, MouseY);
        }
        public static void MouseLeftButtonClick(int MouseX, int MouseY)
        {
            Mouse.LeftButtonClick(MouseX, MouseY);
        }
        public static void MouseRightButtonClick(int MouseX, int MouseY)
        {
            Mouse.RightButtonClick(MouseX, MouseY);
        }
        public static void MouseDblClick(int MouseX, int MouseY)
        {
            Mouse.DblClick(MouseX, MouseY);
        }
        public static void MouseWheel(int MouseX, int MouseY)
        {
            Mouse.Wheel(MouseX, MouseY);
        }
        public static void MouseUp(int MouseX, int MouseY)
        {
            Mouse.Up(MouseX, MouseY);
        }
        public static void MouseDown(int MouseX, int MouseY)
        {
            Mouse.Down(MouseX, MouseY);
        }
        public static Rectangle GetCurrentScreenResolution()
        {
            return ScreenResizer.GetScreenResolution();
        }        
        public static void ChangeScreenResolution(int screenWidth, int screenHeight)
        {
            ScreenResizer.SetScreenResolution(screenWidth, screenHeight);
        }
        public static void InitScreenDisplayParameters()
        {
            DisplayScreener.InitParameters();
        }
        public static byte[] GetDisplayScreen()
        {
            return DisplayScreener.ScreenDisplay();
        }
        public static void SetScreenOnPictureBox(byte[] screen, ref PictureBox pictureBox)
        {
            DisplayScreener.SetScreenOnPictureBox(screen, ref pictureBox);
        }
        public static void PressKeyBoardKey(byte key)
        {
            KeyBoard.PressKey(key);
        }      
        public static void ChangeKeyBoardLayout(string language)
        {
            if (language == Russian)
            {
                KeyBoard.ChangeKeyBoardLayout(KeyboardUser32.Languages.Russian);
            }
            else if (language == English)
            {
                KeyBoard.ChangeKeyBoardLayout(KeyboardUser32.Languages.English);
            }
            
        }
    }
}
