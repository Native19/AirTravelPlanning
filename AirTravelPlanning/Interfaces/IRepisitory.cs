using AirTravelPlanning.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Interfaces
{
    public interface IRepisitory
    {
        List<RouteModel> LoadData();
        RouteModel AddRoute();

        void DeleteRoute();

        void DeleteFlight();

        void UpdateRoute();
        void UdateFlight();
    }
}
