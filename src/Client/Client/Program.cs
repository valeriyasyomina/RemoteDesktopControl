using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Timers;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Facade.Init();
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
          
            Application.Run(new ClientForm());            
        }
    }
}
