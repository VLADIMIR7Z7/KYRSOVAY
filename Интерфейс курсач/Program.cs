using System;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataStorage.LoadData();
            Application.Run(new MainForm());
            DataStorage.SaveData();
        }
    }
}