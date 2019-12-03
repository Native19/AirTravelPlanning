using AirTravelPlanning.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirTravelPlanning.Models
{
    public class FlightModel
    {
        public int FlightNumber { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public List<DepartureDays> DeparturesDays { get; set; }

        public FlightModel ()
        {
            FlightNumber = default;
            DepartureTime = default;
            ArrivalTime = default;
            DeparturesDays = new List<DepartureDays>();
        }

        public FlightModel(int flightNumber, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            FlightNumber = flightNumber;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            DeparturesDays = new List<DepartureDays>();
        }

        public TimeSpan TakeTimeInAir()
        {
            return (DepartureTime < ArrivalTime)
                ? ArrivalTime - DepartureTime
                : new TimeSpan(24, 0, 0) + (ArrivalTime - DepartureTime);
        }

        public (DepartureDays arrivalDay, TimeSpan arrivalTime) TakeArrivalDate(DepartureDays departureDay)
        {
            var time = DepartureTime + TakeTimeInAir();

            if (time.Days > 0)
            {
                departureDay = departureDay.Next();
                time -= new TimeSpan(time.Days, 0, 0, 0);
            }

            return (departureDay, time);
        }

        //public (DepartureDays arrivalDay, TimeSpan arrivalTime) TakeArrivalDate(DepartureDays departureDay)
        //{
        //    return (x) => x == DepartureTime + TakeTimeInAir()
            

        //        ((DepartureTime + TakeTimeInAir()).Days > 0)
        //        ? (departureDay.Next(), DepartureTime + TakeTimeInAir() - new TimeSpan(time.Days, 0, 0, 0))

        //    var time = DepartureTime + TakeTimeInAir();

        //    if (time.Days > 0)
        //    {
        //        departureDay = departureDay.Next();
        //        time -= new TimeSpan(time.Days, 0, 0, 0);
        //    }

        //    return (departureDay, time);
        //}
    }
}
