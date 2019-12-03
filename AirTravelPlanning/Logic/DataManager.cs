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
        private readonly DataRepository _dataRepository;
        //private readonly Schedule _schedule;

        public DataManager (/*DataRepository dataRepository*//*, Schedule schedule*/)
        {
            _dataRepository = new DataRepository();
            //_dataRepository = dataRepository;
            //_schedule = schedule;
        }

        //private bool IsScheduleExistRoute(string dispatchCity, string arrivalCity)
        //{
        //    return _schedule.Routes
        //        .Exists(
        //        route => route.ArrivalCity == arrivalCity && 
        //        route.DispatchCity == dispatchCity);
        //}

        //private RouteModel FindRoute(string dispatchCity, string arrivalCity)
        //{
        //    return _schedule.Routes
        //        .Find(
        //        route => route.ArrivalCity == arrivalCity &&
        //        route.DispatchCity == dispatchCity);
        //}

        //public bool AddFlight(string dispatchCity, string arrivalCity, FlightModel newFlight)
        //{
        //    if (dispatchCity == arrivalCity || FindFlightWithNumber(newFlight.FlightNumber) != null)
        //        return false;

        //    AddRoute(dispatchCity, arrivalCity);

        //    FindRoute(dispatchCity, arrivalCity)
        //            .Flights
        //            .Add(newFlight);

        //    _dataRepository.UnloadData(_schedule.Routes);
        //    return true;
        //}

        //public bool AddRoute(string dispatchCity, string arrivalCity)
        //{
        //    if (IsScheduleExistRoute(dispatchCity, arrivalCity))
        //        return false;

        //    _schedule.Routes.Add(new RouteModel(dispatchCity, arrivalCity));
        //    _dataRepository.UnloadData(_schedule.Routes);

        //    return true;
        //}

        //private RouteModel FindRouteWithFlight(int flightNumber)
        //{
        //    return _schedule.Routes
        //        .Find(route => route.Flights
        //            .Exists(flight => flight.FlightNumber == flightNumber));
        //}

        //private FlightModel FindFlightWithNumber(int flightNumber)
        //{
        //    var routeOfFlight = FindRouteWithFlight(flightNumber);

        //    if (routeOfFlight != null)
        //        return routeOfFlight
        //            .Flights
        //                .Find(flight => flight.FlightNumber == flightNumber);
        //    return null;
        //}

        //public bool DeleteFlight(int flightNumber)
        //{
        //    var flight = FindFlightWithNumber(flightNumber);
        //    if (flight == null)
        //        return false;

        //    FindRouteWithFlight(flightNumber).Flights.Remove(flight);

        //    _dataRepository.UnloadData(_schedule.Routes);
        //    return true;                
        //}

        //public bool DeleteRoute(string dispatchCity, string arrivalCity)
        //{
        //    var route = FindRoute(dispatchCity, arrivalCity);
        //    if (route != null)
        //    {
        //        _schedule.Routes.Remove(route);

        //        _dataRepository.UnloadData(_schedule.Routes);
        //        return true;
        //    }
        //    return false;
        //}

        public List<RouteModel> LoadRoutes()
        {
            return _dataRepository.UploadData();
        }
    }
}
