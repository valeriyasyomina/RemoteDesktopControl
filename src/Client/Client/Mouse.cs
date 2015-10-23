using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using User32Lib;

namespace Client
{
    public class Mouse
    {

        public static void SetCursorPosition(int MouseX, int MouseY)
        {
            Cursor.Position = new Point(MouseX, MouseY);
            //User32.mouse_event(User32.MOUSEEVENTF_MOVE, MouseX, MouseY, 0, 0);
            //MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_ABSOLUTE | MouseUser32.MOUSEEVENTF_MOVE, MouseX, MouseY, 0, 0);
        }
        public static void LeftButtonClick(int MouseX, int MouseY)
        {
            SetCursorPosition(MouseX, MouseY);
            MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_LEFTDOWN | MouseUser32.MOUSEEVENTF_LEFTUP, MouseX, MouseY, 0, 0);
            
        }
        public static void DblClick(int MouseX, int MouseY)
        {
            SetCursorPosition(MouseX, MouseY);
            MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_LEFTDOWN | MouseUser32.MOUSEEVENTF_LEFTUP |
                                    MouseUser32.MOUSEEVENTF_LEFTDOWN | MouseUser32.MOUSEEVENTF_LEFTUP, MouseX, MouseY, 0, 0);
          //  MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_LEFTDOWN | MouseUser32.MOUSEEVENTF_LEFTUP, MouseX, MouseY, 0, 0);
        }
        public static void RightButtonClick(int MouseX, int MouseY)
        {
            SetCursorPosition(MouseX, MouseY);
            MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_RIGHTDOWN | MouseUser32.MOUSEEVENTF_RIGHTUP, MouseX, MouseY, 0, 0);            
        }
        public static void Wheel(int MouseX, int MouseY)
        {
            SetCursorPosition(MouseX, MouseY);
            MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_WHEEL, MouseX, MouseY, 0, 0);
        }
        public static void Up(int MouseX, int MouseY)
        {
            SetCursorPosition(MouseX, MouseY);
            MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_LEFTUP, MouseX, MouseY, 0, 0);
        }
        public static void Down(int MouseX, int MouseY)
        {
            SetCursorPosition(MouseX, MouseY);
            MouseUser32.mouse_event(MouseUser32.MOUSEEVENTF_LEFTDOWN, MouseX, MouseY, 0, 0);
        }
    }
}
