using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    // Статический класс для управления водителями
    public static class DriverManager
    {
        // Список водителей
        private static List<Driver> drivers = new List<Driver>();

        // Метод для получения списка всех водителей
        public static List<Driver> GetDrivers() => drivers;

        // Метод для поиска водителя по табельному номеру
        public static Driver FindDriver(string employeeNumber)
        {
            // Используем метод Find для поиска водителя с указанным табельным номером
            return drivers.Find(driver => driver.EmployeeNumber == employeeNumber);
        }
    }
}