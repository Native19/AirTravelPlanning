using AirTravelPlanning.Interfaces;
using AirTravelPlanning.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AirTravelPlanning.Repositories;

namespace AirTravelPlanning.Logic
{
    public class DataManager : IDataManager
    {
        private readonly IRepisitory _dataRepository;
        public List<RouteModel> Routes { get; set; }

        public DataManager (IRepisitory dataRepository)
        {
            _dataRepository = dataRepository;
            Routes = _dataRepository.UploadData();
        }

        public List<RouteModel> LoadRoutes()
        {
            return Routes;
        }

        private RouteModel FindRoute(string dispatchCity, string arrivalCity)
        {
            if (IsScheduleExistRoute(dispatchCity, arrivalCity))
                return Routes
                    .Find(
                        route => route.ArrivalCity == arrivalCity &&
                        route.DispatchCity == dispatchCity);
            return null;
        }

        private bool IsScheduleExistRoute(string dispatchCity, string arrivalCity)
        {
            return Routes
                .Exists(
                route => route.ArrivalCity == arrivalCity &&
                route.DispatchCity == dispatchCity);
        }

        private RouteModel FindRouteWithFlight(int flightNumber)
        {
            return Routes
                .Find(route => route.Flights
                    .Exists(flight => flight.FlightNumber == flightNumber));
        }

        private FlightModel FindFlightWithNumber(int flightNumber)
        {
            var routeOfFlight = FindRouteWithFlight(flightNumber);

            if (routeOfFlight != null)
                return routeOfFlight
                    .Flights
                        .Find(flight => flight.FlightNumber == flightNumber);
            return null;
        }

        public bool AddRoute(string dispatchCity, string arrivalCity)
        {
                if (IsScheduleExistRoute(dispatchCity, arrivalCity))
                    return false;

                Routes.Add(new RouteModel(dispatchCity, arrivalCity));
                _dataRepository.UnloadData(Routes);

                return true;
        }

        public bool AddFlight(string dispatchCity, string arrivalCity, FlightModel newFlight)
        {
            if (dispatchCity == arrivalCity || FindFlightWithNumber(newFlight.FlightNumber) != null)
                return false;

            if (!IsScheduleExistRoute(dispatchCity, arrivalCity))
                AddRoute(dispatchCity, arrivalCity);
            

            FindRoute(dispatchCity, arrivalCity)
                    .Flights
                    .Add(newFlight);

            _dataRepository.UnloadData(Routes);
            return true;
        }

        public bool DeleteRoute(string dispatchCity, string arrivalCity)
        {
            var route = FindRoute(dispatchCity, arrivalCity);
            if (route != null)
            {
                Routes.Remove(route);

                _dataRepository.UnloadData(Routes);
                return true;
            }
            return false;
        }

        public bool DeleteFlight(string dispatchCity, string arrivalCity, int flightNumber)
        {
            var flight = FindFlightWithNumber(flightNumber);
            if (flight == null)
                return false;

            FindRouteWithFlight(flightNumber).Flights.Remove(flight);

            _dataRepository.UnloadData(Routes);
            return true;
        }

        public bool UpdateRoute(string dispatchCityUntilUpdate, string arrivalCityUntilUpdate, string dispatchCityUpdated, string arrivalCityUpdated)
        {
            var route = FindRoute(dispatchCityUntilUpdate, arrivalCityUntilUpdate);
            if (route != null)
            {
                route.DispatchCity = dispatchCityUpdated;
                route.ArrivalCity = arrivalCityUpdated;

                _dataRepository.UnloadData(Routes);
                return true;
            }
            return false;
        }

        public bool UpdateFlight(string dispatchCity, string arrivalCity, FlightModel updatedFlight)
        {
            var flight = FindFlightWithNumber(updatedFlight.FlightNumber);
            if (flight == null)
                return false;

            var routeFlights = FindRouteWithFlight(updatedFlight.FlightNumber).Flights;
            routeFlights.Remove(flight);
            routeFlights.Add(updatedFlight);

            _dataRepository.UnloadData(Routes);
            return true;
        }
    }
}
