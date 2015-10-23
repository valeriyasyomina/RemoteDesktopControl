using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using User32Lib;

namespace Client
{
    public class KeyBoard
    {
        public static void PressKey(byte key)
        {
            KeyboardUser32.PressKey(key);
            KeyboardUser32.ReleaseKey(key);
        }
        public static void PressShiftKey(byte key)
        {
            KeyboardUser32.PressShift();
            KeyboardUser32.PressKey(key);
            KeyboardUser32.ReleaseShift();
        }
        public static void ChangeKeyBoardLayout(KeyboardUser32.Languages language)
        {
            KeyboardUser32.ChangeLanguage(language);
        }
    }
}
