using AirTravelPlanning.Logic;
using AirTravelPlanning.Models;
using AirTravelPlanning.Models.Enums;
using AirTravelPlanning.Repositories;
using System;
using System.Collections.Generic;

namespace AirTravelPlanning
{
    class Program
    {

        static void Main(string[] args)
        {
            var repository = new DataRepository();
            var manager = new DataManager(repository);
            manager.DeleteRoute("Vlad", "Ys");
            manager.DeleteRoute("Vlad", "Hb");
            var schedule = new Schedule(manager);
            schedule.SearchRoute("Sp", "Ys", DepartureDays.Monday, new TimeSpan(11,0,0));

            Console.ReadKey();
        }
    }
}
