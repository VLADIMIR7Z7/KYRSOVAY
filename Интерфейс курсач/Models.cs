using System;
using System.Collections.Generic;
using System.Linq;

namespace FreightTransportSystem
{
    // Класс, представляющий автомобиль
    public class Car
    {
        public string LicensePlate { get; set; } // Номер автомобиля
        public string Brand { get; set; } // Марка автомобиля
        public double LoadCapacity { get; set; } // Грузоподъемность автомобиля
        public string Purpose { get; set; } // Назначение автомобиля
        public int YearOfManufacture { get; set; } // Год выпуска автомобиля
        public int YearOfRepair { get; set; } // Год последнего ремонта
        public double Mileage { get; set; } // Пробег автомобиля
        public string Photo { get; set; } // Путь к фотографии автомобиля

        // Конструктор для инициализации свойств автомобиля
        public Car(string licensePlate, string brand, double loadCapacity, string purpose, int yearOfManufacture, int yearOfRepair, double mileage, string photo)
        {
            LicensePlate = licensePlate;
            Brand = brand;
            LoadCapacity = loadCapacity;
            Purpose = purpose;
            YearOfManufacture = yearOfManufacture;
            YearOfRepair = yearOfRepair;
            Mileage = mileage;
            Photo = photo;
        }

        // Переопределение метода ToString для удобного отображения информации об автомобиле
        public override string ToString()
        {
            return $"{LicensePlate};{Brand};{LoadCapacity};{Purpose};{YearOfManufacture};{YearOfRepair};{Mileage};{Photo}";
        }
    }

    // Класс, представляющий водителя
    public class Driver
    {
        public string FullName { get; set; } // Полное имя водителя
        public string EmployeeNumber { get; set; } // Табельный номер водителя
        public int YearOfBirth { get; set; } // Год рождения водителя
        public int Experience { get; set; } // Стаж водителя
        public string Category { get; set; } // Категория водительских прав
        public string ClassType { get; set; } // Тип класса автомобиля, который может водить водитель

        // Конструктор для инициализации свойств водителя
        public Driver(string fullName, string employeeNumber, int yearOfBirth, int experience, string category, string classType)
        {
            FullName = fullName;
            EmployeeNumber = employeeNumber;
            YearOfBirth = yearOfBirth;
            Experience = experience;
            Category = category;
            ClassType = classType;
        }

        // Переопределение метода ToString для удобного отображения информации о водителе
        public override string ToString()
        {
            return $"{FullName};{EmployeeNumber};{YearOfBirth};{Experience};{Category};{ClassType}";
        }
    }

    // Абстрактный класс, представляющий клиента
    public abstract class Client
    {
        public string ContactName { get; set; } // Имя контактного лица
        public string Phone { get; set; } // Телефон клиента

        // Конструктор для инициализации свойств клиента
        public Client(string contactName, string phone)
        {
            ContactName = contactName;
            Phone = phone;
        }

        // Абстрактный метод для переопределения в производных классах
        public abstract override string ToString();
    }

    // Класс, представляющий индивидуального клиента
    public class IndividualClient : Client
    {
        public string PassportSeries { get; set; } // Серия паспорта
        public string PassportNumber { get; set; } // Номер паспорта
        public DateTime IssueDate { get; set; } // Дата выдачи паспорта
        public string IssuedBy { get; set; } // Кем выдан паспорт

        // Конструктор для инициализации свойств индивидуального клиента
        public IndividualClient(string contactName, string phone, string passportSeries, string passportNumber, DateTime issueDate, string issuedBy)
            : base(contactName, phone)
        {
            PassportSeries = passportSeries;
            PassportNumber = passportNumber;
            IssueDate = issueDate;
            IssuedBy = issuedBy;
        }

        // Переопределение метода ToString для удобного отображения информации об индивидуальном клиенте
        public override string ToString()
        {
            return $"{ContactName};{Phone};{PassportSeries};{PassportNumber};{IssueDate};{IssuedBy}";
        }
    }

    // Класс, представляющий юридическое лицо
    public class LegalEntityClient : Client
    {
        public string CompanyName { get; set; } // Название компании
        public string DirectorName { get; set; } // Имя директора
        public string LegalAddress { get; set; } // Юридический адрес
        public string BankName { get; set; } // Название банка
        public string AccountNumber { get; set; } // Номер счета
        public string INN { get; set; } // Идентификационный номер налогоплательщика

        // Конструктор для инициализации свойств юридического лица
        public LegalEntityClient(string companyName, string directorName, string legalAddress, string phone, string bankName, string accountNumber, string inn)
            : base(directorName, phone) // Используем directorName как контактное имя
        {
            CompanyName = companyName;
            DirectorName = directorName;
            LegalAddress = legalAddress;
            BankName = bankName;
            AccountNumber = accountNumber;
            INN = inn;
        }

        // Переопределение метода ToString для удобного отображения информации о юридическом лице
        public override string ToString()
        {
            return $"{CompanyName};{DirectorName};{LegalAddress};{Phone};{BankName};{AccountNumber};{INN}";
        }
    }

    // Класс, представляющий груз
    public class Cargo
    {
        public int CargoId { get; set; }
        public string Name { get; set; } // Название груза
        public string Unit { get; set; } // Единица измерения
        public double Quantity { get; set; } // Количество груза
        public double Weight { get; set; } // Вес груза
        public double InsuranceValue { get; set; } // Страховая стоимость груза

        // Конструктор для инициализации свойств груза
        public Cargo(int cargoId, string name, string unit, double quantity, double weight, double insuranceValue)
        {
            CargoId = cargoId; // Исправлено: теперь присваиваем значение правильно
            Name = name;
            Unit = unit;
            Quantity = quantity;
            Weight = weight;
            InsuranceValue = insuranceValue;
        }

        // Переопределение метода ToString для удобного отображения информации о грузе
        public override string ToString()
        {
            return $"{Name};{Unit};{Quantity};{Weight};{InsuranceValue}";
        }
    }

    // Класс, представляющий заказ на грузоперевозку
    public class CargoOrder
    {
        public int OrderId { get; set; } // Номер заказа
        public DateTime OrderDate { get; set; } // Дата заказа
        public string SenderName { get; set; } // Имя отправителя
        public string LoadingAddress { get; set; } // Адрес погрузки
        public string ReceiverName { get; set; } // Имя получателя
        public string UnloadingAddress { get; set; } // Адрес разгрузки
        public double RouteLength { get; set; } // Длина маршрута
        public double OrderCost { get; set; } // Стоимость заказа
        public List<Cargo> CargoList { get; set;  } // Список грузов в заказе

        // Конструктор для инициализации свойств заказа
        public CargoOrder(int orderId, DateTime orderDate, string senderName, string loadingAddress, string receiverName, string unloadingAddress, double routeLength, double orderCost, List<Cargo> cargoList)
        {
            OrderId = orderId; // Инициализация номера заказа
            OrderDate = orderDate;
            SenderName = senderName;
            LoadingAddress = loadingAddress;
            ReceiverName = receiverName;
            UnloadingAddress = unloadingAddress;
            RouteLength = routeLength;
            OrderCost = orderCost;
            CargoList = cargoList;
        }

        // Переопределение метода ToString для удобного отображения информации о заказе
        public override string ToString()
        {
            string cargoDetails = string.Join(",", CargoList.Select(c => c.ToString())); // Преобразуем список грузов в строку
            return $"{OrderId};{OrderDate};{SenderName};{LoadingAddress};{ReceiverName};{UnloadingAddress};{RouteLength};{OrderCost};{cargoDetails}";
        }
    }

    // Класс, представляющий поездку
    public class Trip
    {
        public string LicensePlate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DriverFullName { get; set; }
        public string OrderNumber { get; set; }
        
        // Конструктор
        public Trip(string licensePlate, DateTime arrivalTime, string driverFullName, string orderNumber)
        {
            LicensePlate = licensePlate;
            ArrivalTime = arrivalTime;
            DriverFullName = driverFullName;
            OrderNumber = orderNumber;
           
        }

    }
}