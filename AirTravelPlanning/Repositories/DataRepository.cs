using AirTravelPlanning.Interfaces;
using AirTravelPlanning.Models;
using AirTravelPlanning.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AirTravelPlanning.Repositories
{
    public class DataRepository : IRepisitory
    {
        private readonly string _dataPath = $@"{Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"))}\Data.json";

        public List<RouteModel> UploadData()
        {
            var jsonString = File.ReadAllText(_dataPath).Normalize();
            var jsonObject = JsonConvert.DeserializeObject<List<RouteModel>>(jsonString);

            return jsonObject;
            //if (ValidateData(jsonObject))
            //    return jsonObject;
            //else
            //    return new List<RouteModel>();
        }

        public void UnloadData(List<RouteModel> routeModels)
        {
            var json = JsonConvert.SerializeObject(routeModels);
            File.WriteAllText(_dataPath, json);
        }

        private bool ValidateData (List<RouteModel> data)
        {
            var flightNumbersList = new List<int>();
            var flightRoutesList = new List<(string arrivalCity, string dispatchCity)>();

            foreach (var route in data)
            {
                if ((route.ArrivalCity == route.DispatchCity) || (flightRoutesList.Contains((route.ArrivalCity, route.DispatchCity))))
                    return false;

                flightRoutesList.Add((route.ArrivalCity, route.DispatchCity));

                foreach (var flight in route.Flights)
                {
                    if ((flight.DeparturesDays.Contains(DepartureDays.Friday) && flight.DeparturesDays.Count > 1) ||
                        flight.DeparturesDays.Count == 0)
                        return false;

                    if (flightNumbersList.Contains(flight.FlightNumber))
                        return false;
                    else
                        flightNumbersList.Add(flight.FlightNumber);

                }
            }
            return true;
        }
    }
}
