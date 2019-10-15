using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models
{
    public class RouteModel
    {
        public string DispatchCity { get; set; }
        public string ArrivalCity { get; set; }
        public List<FlightModel> Flights { get; set; }
    }
}
