
using AirTravelPlanning.Models.Enums;
using AirTravelPlanning.Models.FlightGraph;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models.FlightGraph
{
    public class Graph
    {
        private List<RouteModel> Flights { get; set; }
        private List<GraphNode> Nodes { get; set; }

        #region Конструктор и инциализация
        public Graph(List<RouteModel> routes)
        {
            Flights = routes;
            Nodes = GetAllNodes(routes);
        }

        private List<GraphNode> GetAllNodes(List<RouteModel> routes)
        {
            var nodes = new List<GraphNode>();
            foreach (var route in routes)
            {
                if (nodes.Find(el=> el.Name == route.ArrivalCity) == null)
                {
                    nodes.Add(new GraphNode(route.ArrivalCity));
                }

                if (nodes.Find(el => el.Name == route.DispatchCity) == null)
                {
                    nodes.Add(new GraphNode(route.DispatchCity));
                }
            }
            return nodes;
        }
        #endregion

        #region Алгоритм дейкстры и вспомагательный рекурсивный метод
        public GraphNode FindWaysDijkstra(string dispatchCity, string arrivalCity, DepartureDays departureDay, TimeSpan departureTime)
        {
            var firstNode = Nodes.Find(node => node.Name == dispatchCity);

            if (firstNode == null)
                return null;

            firstNode.TimeOfRoute = TimeSpan.Zero;

            BuildWaysFromNode(dispatchCity, departureDay, departureTime);

            return Nodes.Find(node => node.Name == arrivalCity);
        }

        private void BuildWaysFromNode(string dispatchCity, DepartureDays departureDay, TimeSpan departureTime)
        {
            var currentNode = Nodes.Find(node => node.Name == dispatchCity);
            currentNode.IsChecked = true;

            var flightsFromNode = Flights.FindAll(route => route.DispatchCity == dispatchCity);

            foreach (var route in flightsFromNode)
            {
                var bestFlight = route.FindBestFlight(departureDay, departureTime);
                var arrivalNode = Nodes.Find(node => node.Name == route.ArrivalCity);

                if (arrivalNode.TimeOfRoute > currentNode.TimeOfRoute + bestFlight.FlightTime)
                {
                    /*Костыль с поиском дня следующего вылета*/
                    var timeUntilFlight = bestFlight.FlightTime - bestFlight.Flight.TakeTimeInAir();
                    var timeOfDeparture = departureTime + timeUntilFlight;
                    var nextDepartureDay = departureDay;

                    for (var i = 0; i < timeOfDeparture.Days; i++)
                        nextDepartureDay = nextDepartureDay.Next();
                    /*--------------------------------------------*/

                    arrivalNode.TimeOfRoute = currentNode.TimeOfRoute + bestFlight.FlightTime;
                    arrivalNode.RouteToTheNode = new List<GraphFlight>(currentNode.RouteToTheNode);
                    arrivalNode.RouteToTheNode.Add(new GraphFlight(bestFlight.Flight, route, nextDepartureDay, bestFlight.FlightTime));
                }

                Flights.RemoveAll(flights => flights.DispatchCity == currentNode.Name && flights.ArrivalCity == route.ArrivalCity);
                Flights.RemoveAll(flights => flights.DispatchCity == route.ArrivalCity && flights.ArrivalCity == currentNode.Name);
            }

            var nextNode = TakeNodeWithMinTimeOfRoute();
            if (nextNode != null)
            {
                var previousNode = nextNode.RouteToTheNode[nextNode.RouteToTheNode.Count - 1];
                var departureDate = previousNode.TakeArrivalDate((DepartureDays)previousNode.DepartureDay);
                BuildWaysFromNode(nextNode.Name, departureDate.arrivalDay, departureDate.arrivalTime);
            }
        }
        #endregion

        #region Вспомагательные методы

        private GraphNode TakeNodeWithMinTimeOfRoute()
        {
            TimeSpan minTime = TimeSpan.MaxValue;
            GraphNode minTimeNode = null;

            foreach (var node in Nodes)
            {
                if (!node.IsChecked && node.TimeOfRoute < minTime)
                {
                    minTime = node.TimeOfRoute;
                    minTimeNode = node;
                }
            }
            return minTimeNode;
        }
        #endregion
    }
}
