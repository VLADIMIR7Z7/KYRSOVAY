using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    public static class OrderManager
    {
        private static List<CargoOrder> orders = new List<CargoOrder>();

        public static List<CargoOrder> GetOrders() => orders;

        public static void AddOrder(CargoOrder order)
        {
            orders.Add(order);
        }

        public static void RemoveOrder(DateTime orderDate)
        {
            orders.RemoveAll(o => o.OrderDate == orderDate);
        }


    }
}