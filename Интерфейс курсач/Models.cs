using System;
using System.Collections.Generic;
using System.Linq;

namespace FreightTransportSystem
{
    public class Car
    {
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public double LoadCapacity { get; set; }

        public string Purpose { get; set; }
        public int YearOfManufacture { get; set; }
        public int YearOfRepair { get; set; }
        public double Mileage { get; set; }
        public string Photo { get; set; }

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

        public override string ToString()
        {
            return $"{LicensePlate};{Brand};{LoadCapacity};{Purpose};{YearOfManufacture};{YearOfRepair};{Mileage};{Photo}";
        }
    }

    public class Driver
    {
        public string FullName { get; set; }
        public string EmployeeNumber { get; set; }
        public int YearOfBirth { get; set; }
        public int Experience { get; set; }
        public string Category { get; set; }
        public string ClassType { get; set; }

        public Driver(string fullName, string employeeNumber, int yearOfBirth, int experience, string category, string classType)
        {
            FullName = fullName;
            EmployeeNumber = employeeNumber;
            YearOfBirth = yearOfBirth;
            Experience = experience;
            Category = category;
            ClassType = classType;
        }

        public override string ToString()
        {
            return $"{FullName};{EmployeeNumber};{YearOfBirth};{Experience};{Category};{ClassType}";
        }
    }

    public abstract class Client
    {
        public string ContactName { get; set; }
        public string Phone { get; set; }

        public Client(string contactName, string phone)
        {
            ContactName = contactName;
            Phone = phone;
        }

        public abstract override string ToString();
    }

    public class IndividualClient : Client
    {
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssuedBy { get; set; }

        public IndividualClient(string contactName, string phone, string passportSeries, string passportNumber, DateTime issueDate, string issuedBy)
            : base(contactName, phone)
        {
            PassportSeries = passportSeries;
            PassportNumber = passportNumber;
            IssueDate = issueDate;
            IssuedBy = issuedBy;
        }

        public override string ToString()
        {
            return $"{ContactName};{Phone};{PassportSeries};{PassportNumber};{IssueDate};{IssuedBy}";
        }
    }

    public class LegalEntityClient : Client
    {
        public string CompanyName { get; set; }
        public string DirectorName { get; set; }
        public string LegalAddress { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string INN { get; set; }

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

        public override string ToString()
        {
            return $"{CompanyName};{DirectorName};{LegalAddress};{Phone};{BankName};{AccountNumber};{INN}";
        }
    }

    public class Cargo
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public double Weight { get; set; }
        public double InsuranceValue { get; set; }

        public Cargo(string name, string unit, double quantity, double weight, double insuranceValue)
        {
            Name = name;
            Unit = unit;
            Quantity = quantity;
            Weight = weight;
            InsuranceValue = insuranceValue;
        }

        public override string ToString()
        {
            return $"{Name};{Unit};{Quantity};{Weight};{InsuranceValue}";
        }
    }

    public class CargoOrder
    {
        public DateTime OrderDate { get; }
        public string SenderName { get; }
        public string LoadingAddress { get; }
        public string ReceiverName { get; }
        public string UnloadingAddress { get; }
        public double RouteLength { get; }
        public double OrderCost { get; }
        public List<Cargo> CargoList { get; }

        public CargoOrder(DateTime orderDate, string senderName, string loadingAddress, string receiverName, string unloadingAddress, double routeLength, double orderCost, List<Cargo> cargoList)
        {
            OrderDate = orderDate;
            SenderName = senderName;
            LoadingAddress = loadingAddress;
            ReceiverName = receiverName;
            UnloadingAddress = unloadingAddress;
            RouteLength = routeLength;
            OrderCost = orderCost;
            CargoList = cargoList;
        }

        public override string ToString()
        {
            string cargoDetails = string.Join(",", CargoList.Select(c => c.ToString())); // Преобразуем список грузов в строку
            return $"{OrderDate};{SenderName};{LoadingAddress};{ReceiverName};{UnloadingAddress};{RouteLength};{OrderCost};{cargoDetails}";
        }
    }




    public class Trip
    {
        public string LicensePlate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DriverFullName { get; set; } // Изменено с DriverCount на DriverFullName

        public Trip(string licensePlate, DateTime arrivalTime, string driverFullName)
        {
            LicensePlate = licensePlate;
            ArrivalTime = arrivalTime;
            DriverFullName = driverFullName; // Сохраняем ФИО водителя
        }
    }
}