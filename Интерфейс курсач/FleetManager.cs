using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    public static class FleetManager
    {
        private static List<Car> cars = new List<Car>();

        public static List<Car> GetCars() => cars;

        public static Car FindCar(string licensePlate)
        {
            return cars.Find(car => car.LicensePlate == licensePlate);
        }

        public static void AddCar(Car car)
        {
            cars.Add(car);
        }

        public static void RemoveCar(string licensePlate)
        {
            Car carToRemove = FindCar(licensePlate);
            if (carToRemove != null)
            {
                cars.Remove(carToRemove);
            }
        }
    }
}