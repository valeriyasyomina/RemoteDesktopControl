using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using User32Lib;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Client
{
    public class ScreenResizer
    {
        public static Rectangle GetScreenResolution()
        {
            return Screen.PrimaryScreen.Bounds;
        }
        public static void SetScreenResolution(int screenWidth, int screenHeight)
        {
            ScreenResizeUser32.DEVMODE dm = new ScreenResizeUser32.DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);

            ScreenResizeUser32.EnumDisplaySettings(null, ScreenResizeUser32.ENUM_CURRENT_SETTINGS, ref dm);

            dm.dmPelsWidth = screenWidth;
            dm.dmPelsHeight = screenHeight;

            int iRet = ScreenResizeUser32.ChangeDisplaySettings(ref dm, ScreenResizeUser32.CDS_UPDATEREGISTRY);            
        }
    }
}
