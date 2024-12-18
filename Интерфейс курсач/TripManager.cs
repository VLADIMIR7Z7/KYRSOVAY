using System;
using System.Collections.Generic;
using System.Linq;

namespace FreightTransportSystem
{
    // Класс для управления поездками
    public static class TripManager
    {
        // Список для хранения всех поездок
        private static List<Trip> trips = new List<Trip>();

        // Метод для получения списка всех поездок
        public static List<Trip> GetTrips() => trips;

        private static int lastTripNumber = 0; // Хранит последний номер поездки

        public static string GetNextOrderNumber()
        {
            int newOrderNumber = 1; // Начинаем с 1
            while (GetTrips().Any(trip => trip.OrderNumber == newOrderNumber.ToString()))
            {
                newOrderNumber++; // Увеличиваем номер, пока не найдем уникальный
            }
            return newOrderNumber.ToString(); // Возвращаем новый уникальный номер
        }


        // Метод для добавления новой поездки в список
        public static void AddTrip(Trip trip)
        {
            trips.Add(trip); // Добавляем поездку в список
        }

        // Метод для поиска поездки по номеру автомобиля, времени прибытия и полному имени водителя
        public static Trip FindTrip(string licensePlate, DateTime arrivalTime, string driverFullName)
        {
            // Ищем поездку, соответствующую заданным критериям
            return trips.Find(trip => trip.LicensePlate == licensePlate && trip.ArrivalTime == arrivalTime && trip.DriverFullName == driverFullName);
        }

        // Метод для удаления поездок по номеру заказа
        public static void RemoveTrip(string orderNumber)
        {
            // Удаляем все поездки с указанным номером заказа
            trips.RemoveAll(trip => trip.OrderNumber == orderNumber);
        }
    }
}