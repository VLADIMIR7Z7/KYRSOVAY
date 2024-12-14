using FreightTransportSystem;
using System.Collections.Generic;

public static class TripManager
{
    private static List<Trip> trips = new List<Trip>();

    public static List<Trip> GetTrips() => trips;

    public static void ClearTrips()
    {
        trips.Clear();
    }
}