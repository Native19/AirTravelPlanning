using AirTravelPlanning.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models
{
    public class RouteModel
    {
        public string DispatchCity { get; set; }
        public string ArrivalCity { get; set; }
        public List<FlightModel> Flights { get; set; }

        public RouteModel()
        {
            DispatchCity = default;
            ArrivalCity = default;
            Flights = new List<FlightModel>();
        }

        public RouteModel (string dispatchCity, string arrivalCity)
        {
            DispatchCity = dispatchCity;
            ArrivalCity = arrivalCity;
            Flights = new List<FlightModel>();
        }

        [JsonConstructor]
        public RouteModel(string dispatchCity, string arrivalCity, List<FlightModel> flights)
        {
            DispatchCity = dispatchCity;
            ArrivalCity = arrivalCity;
            Flights = flights;
        }

        #region Алгоритм поиска наилучшего перелёта по заданому маршруту
        private TimeSpan TimeUntilEndOfFlight(FlightModel flight, DepartureDays dayOfDeparture, TimeSpan timeOfDeparture)
        {
            int daysUntilFlight = default;
            DepartureDays nearestFlightDay = dayOfDeparture;

            if (timeOfDeparture >= flight.DepartureTime)
            {
                GoToNextDay(ref nearestFlightDay, ref daysUntilFlight);
            }

            while (!flight.DeparturesDays.Contains(nearestFlightDay))
            {
                GoToNextDay(ref nearestFlightDay, ref daysUntilFlight);
            }

            return new TimeSpan(daysUntilFlight, 0, 0, 0) + (flight.DepartureTime - timeOfDeparture) + flight.TakeTimeInAir();
        }

        public (FlightModel Flight, TimeSpan FlightTime) FindBestFlight(DepartureDays dayOfDeparture, TimeSpan timeOfDeparture)
        {
            var minTime = TimeSpan.MaxValue;
            var flightWithMinTime = new FlightModel();


            foreach (var flight in Flights)
            {
                var timeUntilEndOfFlight = TimeUntilEndOfFlight(flight, dayOfDeparture, timeOfDeparture);
                if (timeUntilEndOfFlight < minTime)
                {
                    minTime = timeUntilEndOfFlight;
                    flightWithMinTime = flight;
                }
            }
            return (flightWithMinTime, minTime);
        }
        #endregion

        private void GoToNextDay(ref DepartureDays nearestFlightDay, ref int daysUntilFlight)
        {
            nearestFlightDay = nearestFlightDay.Next();
            ++daysUntilFlight;
        }
    }
}
