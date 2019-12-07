using AirTravelPlanning.Interfaces;
using AirTravelPlanning.Models;
using AirTravelPlanning.Models.Enums;
using AirTravelPlanning.Models.FlightGraph;
using AirTravelPlanning.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Logic
{
    public class Schedule
    {
        public List<RouteModel> Routes;
        private Graph graph;
        private readonly IDataManager _dataManager;

        public Schedule(IDataManager dataManager)
        {
            _dataManager = dataManager;
            Routes = _dataManager.LoadRoutes();
            graph = new Graph(Routes);
        }

        public void SearchRoute(string dispatchCity, string arrivalCity, DepartureDays departureDay, TimeSpan departureTime)
        {
            var node = graph.FindWaysDijkstra(dispatchCity, arrivalCity, departureDay, departureTime);

            Console.WriteLine($"Рассчёт пути из {dispatchCity} до {arrivalCity}!");
            Console.WriteLine($"Дань возможного вылета: {departureDay}, время возможного вылета: {departureTime}!");

            if (node == null)
            {
                Console.WriteLine();
                Console.WriteLine($"Невозможно проложить путь!!!");
                return;
            }

            if (node.TimeOfRoute != TimeSpan.Zero)
                Console.WriteLine(
                    $"Общее время в пути: " +
                    $"Дней - {node.TimeOfRoute.Days}, " +
                    $"Часов - {node.TimeOfRoute.Hours}, " +
                    $"Минут - {node.TimeOfRoute.Minutes}, " +
                    $"Секунд - {node.TimeOfRoute.Seconds}");
            else
                Console.WriteLine("Время не определено");

            Console.WriteLine();
            Console.WriteLine($"Построенный маршрут:");

            foreach (var element in node.RouteToTheNode)
            {
                Console.WriteLine(
                    $"Вылет из { element.DispatchCity} " +
                    $"Прибытие в { element.ArrivalCity} ");

                Console.WriteLine(
                    $"День вылета - { element.DepartureDay} " +
                    $"Время вылета - { element.DepartureTime} " +
                    $"Время прибытия - { element.ArrivalTime}");

                Console.WriteLine();
            }
        }
    }
}
