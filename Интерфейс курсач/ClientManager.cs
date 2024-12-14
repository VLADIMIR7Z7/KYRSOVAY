using System;
using System.Collections.Generic;

namespace FreightTransportSystem
{
    public static class ClientManager
    {
        private static List<Client> clients = new List<Client>();

        public static List<Client> GetClients() => clients;

        public static Client FindClient(string contactName)
        {
            return clients.Find(client => client.ContactName == contactName);
        }
    }
}