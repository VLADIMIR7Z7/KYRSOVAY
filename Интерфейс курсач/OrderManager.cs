using FreightTransportSystem; // Подключение пространства имен для работы с системой грузоперевозок
using System.Collections.Generic; // Подключение пространства имен для работы с коллекциями

public static class OrderManager
{
    // Список для хранения всех заказов на грузоперевозки
    private static List<CargoOrder> orders = new List<CargoOrder>();

    // Метод для получения списка всех заказов
    public static List<CargoOrder> GetOrders() => orders;

    // Метод для добавления нового заказа в список
    public static void AddOrder(CargoOrder order)
    {
        orders.Add(order); // Добавление заказа в список
    }

    // Метод для удаления заказа по его идентификатору
    public static void RemoveOrder(int orderId)
    {
        // Удаление заказа по номеру заказа
        orders.RemoveAll(o => o.OrderId == orderId);
    }
}