using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    // Статический класс для управления клиентами
    public static class ClientManager
    {
        // Список клиентов, который будет хранить всех клиентов
        private static List<Client> clients = new List<Client>();

        // Метод для получения списка всех клиентов
        public static List<Client> GetClients() => clients;

        // Метод для поиска клиента по имени контакта
        public static Client FindClient(string contactName)
        {
            // Используем метод Find для поиска клиента с указанным именем
            return clients.Find(client => client.ContactName == contactName);
        }
    }
}