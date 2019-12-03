using AirTravelPlanning.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models.FlightGraph
{
    public class GraphFlight : FlightModel
    {
        public string DispatchCity { get; }
        public string ArrivalCity { get; }
        public DepartureDays? DepartureDay { get; set; }
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

        public GraphFlight(FlightModel flight, RouteModel route, DepartureDays departureDay, TimeSpan flightTime)
            :base(flight.FlightNumber, flight.DepartureTime, flight.ArrivalTime)
        {
            DispatchCity = route.DispatchCity;
            ArrivalCity = route.ArrivalCity;
            DepartureDay = departureDay;
            FlightTime = flightTime;
        }
    }
}
