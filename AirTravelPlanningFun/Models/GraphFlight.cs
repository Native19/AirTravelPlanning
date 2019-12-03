using AirTravelPlanningProc.Models.Enums;
using System;

namespace AirTravelPlanningProc.Models
{
    public class GraphFlight : FlightModel
    {
        public string DispatchCity { get; }
        public string ArrivalCity { get; }
        public DepartureDay? DepartureDay { get; set; }
        public TimeSpan FlightTime { get; }

        public GraphFlight()
        {
            DispatchCity = string.Empty;
            ArrivalCity = string.Empty;
            FlightNumber = default;
            DepartureTime = TimeSpan.Zero;
            ArrivalTime = TimeSpan.Zero;
            DepartureDay = null;
            FlightTime = TimeSpan.Zero;
        }

        public GraphFlight(FlightModel flight, RouteModel route, DepartureDay departureDay, TimeSpan flightTime)
        {
            DispatchCity = route.DispatchCity;
            ArrivalCity = route.ArrivalCity;
            DepartureDay = departureDay;
            FlightTime = flightTime;
            FlightNumber = flight.FlightNumber;
            DepartureTime = flight.DepartureTime;
            ArrivalTime = flight.ArrivalTime;
        }
    }
}
