using AirTravelPlanning.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models
{
    public class FlightModel
    {
        public Guid FlightNumber { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public List<DepartureDays> DeparturesDays { get; set; }
    }
}
