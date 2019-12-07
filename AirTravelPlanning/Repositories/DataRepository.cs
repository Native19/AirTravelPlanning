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
        }

        public void UnloadData(List<RouteModel> routeModels)
        {
            var json = JsonConvert.SerializeObject(routeModels);
            File.WriteAllText(_dataPath, json);
        }
    }
}
