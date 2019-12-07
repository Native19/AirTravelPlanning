using AirTravelPlanning.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Interfaces
{
    public interface IRepisitory
    {
        List<RouteModel> UploadData();
        void UnloadData(List<RouteModel> routeModels);
    }
}
