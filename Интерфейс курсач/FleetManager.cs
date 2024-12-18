using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    // Статический класс для управления автопарком
    public static class FleetManager
    {
        // Список автомобилей в автопарке
        private static List<Car> cars = new List<Car>();

        // Метод для получения списка всех автомобилей
        public static List<Car> GetCars() => cars;

        // Метод для поиска автомобиля по номеру лицензии
        public static Car FindCar(string licensePlate)
        {
            // Используем метод Find для поиска автомобиля с указанным номером лицензии
            return cars.Find(car => car.LicensePlate == licensePlate);
        }

        // Метод для добавления нового автомобиля в автопарк
        public static void AddCar(Car car)
        {
            cars.Add(car); // Добавляем автомобиль в список
        }

        // Метод для удаления автомобиля из автопарка по номеру лицензии
        public static void RemoveCar(string licensePlate)
        {
            // Находим автомобиль по номеру лицензии
            Car carToRemove = FindCar(licensePlate);
            if (carToRemove != null) // Если автомобиль найден
            {
                cars.Remove(carToRemove); // Удаляем его из списка
            }
        }
    }
}