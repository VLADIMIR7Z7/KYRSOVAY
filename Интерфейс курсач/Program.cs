using System;
using System.Windows.Forms;

namespace FreightTransportSystem
{
    static class Program
    {
        // Главный метод программы
        [STAThread] // Указывает, что приложение использует однопоточную модель для интерфейса
        static void Main()
        {
            // Включение визуальных стилей для приложения
            Application.EnableVisualStyles();
            // Установка режима совместимости для текстового рендеринга
            Application.SetCompatibleTextRenderingDefault(false);
            // Загрузка данных из хранилища перед запуском приложения
            DataStorage.LoadData();
            // Запуск главной формы приложения
            Application.Run(new MainForm());
            // Сохранение данных в хранилище после завершения работы приложения
            DataStorage.SaveData();
        }
    }
}