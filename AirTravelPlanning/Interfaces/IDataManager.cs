using AirTravelPlanning.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Interfaces
{
    public interface IDataManager
    {
        List<RouteModel> LoadRoutes();
        //    bool AddRoute(string dispatchCity, string arrivalCity);

        //    bool AddFlight(string dispatchCity, string arrivalCity, FlightModel newFlight);

        //    bool DeleteRoute(string dispatchCity, string arrivalCity);

        //    bool DeleteFlight(int flightNumber);
        //}
    }
}
