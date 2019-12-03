using Newtonsoft.Json;
using System.Collections.Generic;

namespace AirTravelPlanningProc.Models
{
    public class RouteModel
    {
        public string DispatchCity { get; set; }
        public string ArrivalCity { get; set; }
        public List<FlightModel> Flights { get; set; }

        public RouteModel()
        {
            DispatchCity = default;
            ArrivalCity = default;
            Flights = new List<FlightModel>();
        }

        public RouteModel (string dispatchCity, string arrivalCity)
        {
            DispatchCity = dispatchCity;
            ArrivalCity = arrivalCity;
            Flights = new List<FlightModel>();
        }

        [JsonConstructor]
        public RouteModel(string dispatchCity, string arrivalCity, List<FlightModel> flights)
        {
            DispatchCity = dispatchCity;
            ArrivalCity = arrivalCity;
            Flights = flights;
        }
    }
}
