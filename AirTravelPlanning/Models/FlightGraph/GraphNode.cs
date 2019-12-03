using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models.FlightGraph
{
    public class GraphNode
    {
        public string Name { get; }
        public List<GraphFlight> RouteToTheNode { get; set; }
        public bool IsChecked { get; set; }
        public TimeSpan TimeOfRoute { get; set; }

        public GraphNode()
        {
            Name = null;
            RouteToTheNode = null;
            TimeOfRoute = TimeSpan.MaxValue;
            IsChecked = false;
        }
        public GraphNode(string name)
        {
            Name = name;
            RouteToTheNode = new List<GraphFlight>();
            TimeOfRoute = TimeSpan.MaxValue;
            IsChecked = false;
        }
    }
}
