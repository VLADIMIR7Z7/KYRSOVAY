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

        // Метод для загрузки данных о машинах
        private static void LoadCars()
        {
            // Проверяем, существует ли файл с данными о машинах
            if (File.Exists(CarsFilePath))
            {
                // Читаем все строки из файла
                string[] lines = File.ReadAllLines(CarsFilePath);
                foreach (string line in lines)
                {
                    // Разбиваем строку на части по разделителю ';'
                    string[] parts = line.Split(';');
                    // Проверяем, что количество частей соответствует ожидаемому количеству
                    if (parts.Length == 8) // Ожидаем 8 частей для Car
                    {
                        try
                        {
                            // Создаем новый объект Car и добавляем его в менеджер автопарка
                            Car newCar = new Car(parts[0], parts[1], Convert.ToDouble(parts[2]), parts[3], Convert.ToInt32(parts[4]), Convert.ToInt32(parts[5]), Convert.ToDouble(parts[6]), parts[7]);
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

        // Метод для сохранения данных о машинах
        private static void SaveCars()
        {
            // Используем StreamWriter для записи данных в файл
            using (StreamWriter writer = new StreamWriter(CarsFilePath))
            {
                foreach (Car car in FleetManager.GetCars())
                {
                    // Записываем строку, представляющую объект Car
                    writer.WriteLine(car.ToString());
                }
            }
        }

        // Метод для загрузки данных о водителях
        private static void LoadDrivers()
        {
            // Проверяем, существует ли файл с данными о водителях
            if (File.Exists(DriversFilePath))
            {
                // Читаем все строки из файла
                string[] lines = File.ReadAllLines(DriversFilePath);
                foreach (string line in lines)
                {
                    // Разбиваем строку на части по разделителю ';'
                    string[] parts = line.Split(';');
                    // Создаем новый объект Driver и добавляем его в менеджер водителей
                    Driver newDriver = new Driver(parts[0], parts[1], Convert.ToInt32(parts[2]), Convert.ToInt32(parts[3]), parts[4], parts[5]);
                    DriverManager.GetDrivers().Add(newDriver);
                }
            }
        }

        // Метод для сохранения данных о водителях
        private static void SaveDrivers()
        {
            // Используем StreamWriter для записи данных в файл
            using (StreamWriter writer = new StreamWriter(DriversFilePath))
            {
                foreach (Driver driver in DriverManager.GetDrivers())
                {
                    // Записываем строку, представляющую объект Driver
                    writer.WriteLine(driver.ToString());
                }
            }
        }

        // Метод для загрузки данных о клиентах
        private static void LoadClients()
        {
            // Проверяем, существует ли файл с данными о клиентах
            if (File.Exists(ClientsFilePath))
            {
                // Читаем все строки из файла
                string[] lines = File.ReadAllLines(ClientsFilePath);
                foreach (string line in lines)
                {
                    // Разбиваем строку на части по разделителю ';'
                    string[] parts = line.Split(';');
                    // Проверяем количество частей и создаем соответствующий объект клиента
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

        // Метод для сохранения данных о клиентах
        private static void SaveClients()
        {
            // Используем StreamWriter для записи данных в файл
            using (StreamWriter writer = new StreamWriter(ClientsFilePath))
            {
                foreach (Client client in ClientManager.GetClients())
                {
                    // Записываем строку, представляющую объект Client
                    writer.WriteLine(client.ToString());
                }
            }
        }


        // Метод для загрузки данных о заказах
        private static void LoadOrders()
        {
            // Проверяем, существует ли файл с данными о заказах
            if (File.Exists(OrdersFilePath))
            {
                // Читаем все строки из файла
                string[] lines = File.ReadAllLines(OrdersFilePath);
                foreach (string line in lines)
                {
                    // Разбиваем строку на части по разделителю ';'
                    string[] parts = line.Split(';');
                    // Проверяем, что количество частей соответствует минимальному количеству для CargoOrder
                    if (parts.Length >= 8)
                    {
                        try
                        {
                            // Считываем данные заказа
                            int orderId = Convert.ToInt32(parts[0]);
                            DateTime orderDate = Convert.ToDateTime(parts[1]);
                            string senderName = parts[2];
                            string loadingAddress = parts[3];
                            string receiverName = parts[4];
                            string unloadingAddress = parts[5];
                            double routeLength = Convert.ToDouble(parts[6]);
                            double orderCost = Convert.ToDouble(parts[7]);

                            // Создаем новый объект CargoOrder
                            CargoOrder newOrder = new CargoOrder(orderId, orderDate, senderName, loadingAddress, receiverName, unloadingAddress, routeLength, orderCost, new List<Cargo>());

                            // Если есть информация о грузах, разбираем ее
                            if (parts.Length > 8)
                            {
                                for (int i = 8; i < parts.Length; i += 5)
                                {
                                    // Считываем данные о грузе
                                    string cargoName = parts[i];
                                    string cargoUnit = parts[i + 1];
                                    double cargoQuantity = Convert.ToDouble(parts[i + 2]);
                                    double cargoWeight = Convert.ToDouble(parts[i + 3]);
                                    double cargoInsuranceValue = Convert.ToDouble(parts[i + 4]);

                                    // Генерация нового уникального номера груза
                                    int cargoId = newOrder.CargoList.Any() ? newOrder.CargoList.Max(c => c.CargoId) + 1 : 1;

                                    // Создаем новый объект Cargo и добавляем его в список груза
                                    Cargo newCargo = new Cargo(cargoId, cargoName, cargoUnit, cargoQuantity, cargoWeight, cargoInsuranceValue);
                                    newOrder.CargoList.Add(newCargo);
                                }
                            }

                            // Добавляем заказ в менеджер заказов
                            OrderManager.AddOrder(newOrder);
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

        // Метод для сохранения данных о заказах
        private static void SaveOrders()
        {
            // Используем StreamWriter для записи данных в файл
            using (StreamWriter writer = new StreamWriter(OrdersFilePath))
            {
                foreach (CargoOrder order in OrderManager.GetOrders())
                {
                    // Формируем строку заказа
                    string cargoInfo = string.Empty;

                    if (order.CargoList.Count > 0)
                    {
                        // Формируем строку с информацией о грузе
                        cargoInfo = string.Join(";", order.CargoList.Select(c => $"{c.Name};{c.Unit};{c.Quantity};{c.Weight};{c.InsuranceValue}"));
                    }

                    // Формируем строку с номером заказа и другой информацией
                    string line = $"{order.OrderId};{order.OrderDate};{order.SenderName};{order.LoadingAddress};{order.ReceiverName};{order.UnloadingAddress};{order.RouteLength};{order.OrderCost}";

                    // Если есть информация о грузе, добавляем ее к строке
                    if (!string.IsNullOrEmpty(cargoInfo))
                    {
                        line += $";{cargoInfo}";
                    }

                    // Записываем строку в файл
                    writer.WriteLine(line);
                }
            }
        }


        public static void LoadTrips()
        {
            // Очистка текущего списка поездок перед загрузкой
            TripManager.GetTrips().Clear();
            // Проверяем, существует ли файл с данными о поездках
            if (File.Exists(TripsFilePath))
            {
                // Читаем все строки из файла
                string[] lines = File.ReadAllLines(TripsFilePath);
                foreach (string line in lines)
                {
                    // Разбиваем строку на части по разделителю ';'
                    string[] parts = line.Split(';');

                    // Проверка корректности формата даты
                    if (parts.Length == 4 && DateTime.TryParse(parts[2], out DateTime arrivalTime)) 
                    {
                        string licensePlate = parts[0]; // Считываем номерной знак
                        string driverFullName = parts[1]; // Считываем полное имя водителя
                        string orderNumber = parts[3]; // Получаем номер заказа

                        // Проверка на дубликаты перед добавлением
                        if (!TripManager.GetTrips().Any(trip => trip.LicensePlate == licensePlate && trip.DriverFullName == driverFullName && trip.ArrivalTime == arrivalTime && trip.OrderNumber == orderNumber))
                        {
                            // Создаем новый объект Trip и добавляем его в менеджер поездок
                            Trip newTrip = new Trip(licensePlate, arrivalTime, driverFullName, orderNumber); // Передаем номер заказа
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

        // Метод для сохранения данных о поездках
        public static void SaveTrips()
        {
            // Используем StreamWriter для записи данных в файл
            using (StreamWriter writer = new StreamWriter(TripsFilePath))
            {
                foreach (Trip trip in TripManager.GetTrips())
                {
                    // Записываем строку, представляющую объект Trip
                    writer.WriteLine($"{trip.LicensePlate};{trip.DriverFullName};{trip.ArrivalTime};{trip.OrderNumber}"); // Добавляем номер заказа
                }
            }
        }
    }
}