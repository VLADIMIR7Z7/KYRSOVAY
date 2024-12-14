using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FreightTransportSystem
{
    public static class DataStorage
    {
        private const string CarsFilePath = "cars.txt";
        private const string DriversFilePath = "drivers.txt";
        private const string ClientsFilePath = "clients.txt";
        private const string OrdersFilePath = "orders.txt";
        private const string TripsFilePath = "trips.txt";

        private static bool isDataLoaded = false; // Флаг для отслеживания загрузки данных

        public static void LoadData()
        {
            if (!isDataLoaded) // Проверка, были ли данные уже загружены
            {
                LoadCars();
                LoadDrivers();
                LoadClients();
                LoadOrders();
                LoadTrips();
                isDataLoaded = true; // Устанавливаем флаг в true после загрузки данных
            }
        }

        public static void SaveData()
        {
            SaveCars();
            SaveDrivers();
            SaveClients();
            SaveOrders();
            SaveTrips();
        }

        private static void LoadCars()
        {
            if (File.Exists(CarsFilePath))
            {
                string[] lines = File.ReadAllLines(CarsFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    // Проверяем, что количество частей соответствует ожидаемому количеству
                    if (parts.Length == 8) // Ожидаем 8 частей для Car
                    {
                        try
                        {
                            Car newCar = new Car( parts[0], parts[1], Convert.ToDouble(parts[2]), parts[3], Convert.ToInt32(parts[4]), Convert.ToInt32(parts[5]), Convert.ToDouble(parts[6]), parts[7] );
                            FleetManager.GetCars().Add(newCar);
                        }
                        catch (FormatException ex)
                        {
                            // Обработка ошибок преобразования
                            Console.WriteLine($"Ошибка преобразования данных для строки: {line}. {ex.Message}");
                        }
                    }
                    else
                    {
                        // Логирование или обработка некорректной строки
                        Console.WriteLine($"Пропущена некорректная строка: {line}");
                    }
                }
            }
        }

        private static void SaveCars()
        {
            using (StreamWriter writer = new StreamWriter(CarsFilePath))
            {
                foreach (Car car in FleetManager.GetCars())
                {
                    writer.WriteLine(car.ToString());
                }
            }
        }

        private static void LoadDrivers()
        {
            if (File.Exists(DriversFilePath))
            {
                string[] lines = File.ReadAllLines(DriversFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    Driver newDriver = new Driver(parts[0], parts[1], Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]), parts[4], parts[5]);
                    DriverManager.GetDrivers().Add(newDriver);
                }
            }
        }

        private static void SaveDrivers()
        {
            using (StreamWriter writer = new StreamWriter(DriversFilePath))
            {
                foreach (Driver driver in DriverManager.GetDrivers())
                {
                    writer.WriteLine(driver.ToString());
                }
            }
        }

        private static void LoadClients()
        {
            if (File.Exists(ClientsFilePath))
            {
                string[] lines = File.ReadAllLines(ClientsFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 6)
                    {
                        IndividualClient newClient = new IndividualClient(parts[0], parts[1], parts[2], parts[3], Convert.ToDateTime(parts[4]), parts[5]);
                        ClientManager.GetClients().Add(newClient);
                    }
                    else if (parts.Length == 7)
                    {
                        LegalEntityClient newClient = new LegalEntityClient(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6]);
                        ClientManager.GetClients().Add(newClient);
                    }
                }
            }
        }

        private static void SaveClients()
        {
            using (StreamWriter writer = new StreamWriter(ClientsFilePath))
            {
                foreach (Client client in ClientManager.GetClients())
                {
                    writer.WriteLine(client.ToString());
                }
            }
        }


        private static void LoadOrders()
        {
            if (File.Exists(OrdersFilePath))
            {
                string[] lines = File.ReadAllLines(OrdersFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length >= 7) // Ожидаем минимум 7 частей для CargoOrder
                    {
                        try
                        {
                            DateTime orderDate = Convert.ToDateTime(parts[0]);
                            string senderName = parts[1];
                            string loadingAddress = parts[2];
                            string receiverName = parts[3];
                            string unloadingAddress = parts[4];
                            double routeLength = Convert.ToDouble(parts[5]);
                            double orderCost = Convert.ToDouble(parts[6]);

                            // Создаем новый заказ
                            CargoOrder newOrder = new CargoOrder(orderDate, senderName, loadingAddress, receiverName, unloadingAddress, routeLength, orderCost, new List<Cargo>());

                            // Если есть информация о грузах, разбираем ее
                            if (parts.Length > 7)
                            {
                               
                               for (int i = 7;i < parts.Count() - 1; i=i+5)
                                {
                                    string cargoName = parts[i];
                                    string cargoUnit = parts[i+1];
                                    double cargoQuantity = Convert.ToDouble(parts[i+2]);
                                    double cargoWeight = Convert.ToDouble(parts[i+3]);
                                    double cargoInsuranceValue = Convert.ToDouble(parts[i+4]);

                                    Cargo newCargo = new Cargo(cargoName, cargoUnit, cargoQuantity, cargoWeight, cargoInsuranceValue);
                                    newOrder.CargoList.Add(newCargo);
                                }
                                
                            }

                            OrderManager.AddOrder(newOrder); // Добавляем заказ в менеджер заказов
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Ошибка преобразования данных для строки: {line}. {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Пропущена некорректная строка: {line}");
                    }
                }
            }
        }

        private static void SaveOrders()
        {
            using (StreamWriter writer = new StreamWriter(OrdersFilePath))
            {
                foreach (CargoOrder order in OrderManager.GetOrders())
                {
                    // Формируем строку заказа
                    string cargoInfo = string.Empty;

                    if (order.CargoList.Count > 0)
                    {
                        cargoInfo = string.Join(";", order.CargoList.Select(c => $"{c.Name};{c.Unit};{c.Quantity};{c.Weight};{c.InsuranceValue}"));
                    }

                    string line = $"{order.OrderDate};{order.SenderName};{order.LoadingAddress};{order.ReceiverName};{order.UnloadingAddress};{order.RouteLength};{order.OrderCost}";

                    // Если есть информация о грузе, добавляем ее к строке
                    if (!string.IsNullOrEmpty(cargoInfo))
                    {
                        line += $";{cargoInfo}";
                    }

                    writer.WriteLine(line);
                }
            }
        }


        public static void LoadTrips()
        {
            // Очистка текущего списка поездок перед загрузкой
            TripManager.GetTrips().Clear();

            if (File.Exists(TripsFilePath))
            {
                string[] lines = File.ReadAllLines(TripsFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');

                    // Проверка корректности формата даты
                    if (parts.Length == 3 && DateTime.TryParse(parts[2], out DateTime arrivalTime))
                    {
                        string licensePlate = parts[0];
                        string driverFullName = parts[1]; // Исправлено: убрано преобразование в int

                        // Проверка на дубликаты перед добавлением
                        if (!TripManager.GetTrips().Any(trip => trip.LicensePlate == licensePlate && trip.DriverFullName == driverFullName && trip.ArrivalTime == arrivalTime))
                        {
                            Trip newTrip = new Trip(licensePlate, arrivalTime, driverFullName);
                            TripManager.GetTrips().Add(newTrip);
                        }
                        else
                        {
                            // Логирование или обработка дубликата
                            Console.WriteLine($"Пропущена дублирующая запись: {line}");
                        }
                    }
                    else
                    {
                        // Логирование или обработка некорректной строки
                        Console.WriteLine($"Пропущена некорректная строка: {line}");
                    }
                }
            }
        }

        public static void SaveTrips()
        {
            using (StreamWriter writer = new StreamWriter(TripsFilePath))
            {
                foreach (Trip trip in TripManager.GetTrips())
                {
                    writer.WriteLine($"{trip.LicensePlate};{trip.DriverFullName};{trip.ArrivalTime}");
                }
            }
        }
    }
}