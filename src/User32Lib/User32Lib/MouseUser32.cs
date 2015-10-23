using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace User32Lib
{
    public class MouseUser32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; // нажатие левой кнопки мыши
        public const int MOUSEEVENTF_LEFTUP = 0x0004; // отпустили левую кнопку мыши
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08; // нажатие правой кнопки мыши
        public const int MOUSEEVENTF_RIGHTUP = 0x10; // отпустили правую кнопку мыши
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000; // перемещение курсора
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; // нажатие средней кнопки мыши
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040; // отпустили среднюю кнопку мыши
        public const int MOUSEEVENTF_MOVE = 0x0001; // произошло движение мыши
        public const int MOUSEEVENTF_WHEEL = 0x0800; // прокрутили колесо мыши. dwData содержит насколько прокрутили
        public const int MOUSEEVENTF_XDOWN = 0x0080; // нажата кнопка Х (для горячих клавиш может использоваться)
        public const int MOUSEEVENTF_XUP = 0x0100;  // отпущена кнопка Х (для горячих клавиш может использоваться)
        public const int MOUSEEVENTF_HWHEEL = 0x01000; // ещё один вариант для прокрутки колеса мыши
    }
}
