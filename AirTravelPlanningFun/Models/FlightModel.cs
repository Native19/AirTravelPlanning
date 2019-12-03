using AirTravelPlanningProc.Models.Enums;
using System;
using System.Collections.Generic;

namespace AirTravelPlanningProc.Models
{
    public class FlightModel
    {
        public int FlightNumber { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public List<DepartureDay> DeparturesDays { get; set; }

        public FlightModel ()
        {
            FlightNumber = default;
            DepartureTime = default;
            ArrivalTime = default;
            DeparturesDays = new List<DepartureDay>();
        }

        public FlightModel(int flightNumber, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            FlightNumber = flightNumber;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            DeparturesDays = new List<DepartureDay>();
        }
    }
}
