using System;
using System.Collections.Generic;


namespace AirTravelPlanningProc.Models
{
    public static class Graph
    {
        public static List<RouteModel> Flights { get; set; }
        public static List<GraphNode> Nodes { get; set; }

        static Graph()
        {
            Flights = new List<RouteModel>();
            Nodes = new List<GraphNode>();
        }
    }
}
