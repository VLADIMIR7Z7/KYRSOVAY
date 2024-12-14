using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    public static class DriverManager
    {
        private static List<Driver> drivers = new List<Driver>();

        public static List<Driver> GetDrivers() => drivers;

        public static Driver FindDriver(string employeeNumber)
        {
            return drivers.Find(driver => driver.EmployeeNumber == employeeNumber);
        }
    }
}