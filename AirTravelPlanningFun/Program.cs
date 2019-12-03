using AirTravelPlanningProc.Models;
using AirTravelPlanningProc.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AirTravelPlanningFun
{
    static class Program
    {
        static string dispatchCity;
        static string arrivalCity;
        static DepartureDay departureDay;
        static TimeSpan departureTime;

        static void Main(string[] args)
        {
            /*
            Алгоритм:
                Загрузить данные о перелётах - LoadFromJson
                Получить все города - GetAllNodes
                Получить входные данные - GetStartData
                Найти кротчайшие пути до каждого города - FindWaysDijkstra
                Найти город прибытия - TakeArrivalNode
                Вывести путь в консоль - OutputInfo
            */

            Graph.Flights = LoadRoutesFromJson();
            Graph.Nodes = GetAllNodes(Graph.Flights);
            GetStartData();

            FindWaysDijkstra(dispatchCity, arrivalCity, departureDay, departureTime);

            var arrivalNode = Graph.Nodes.Find(x => x.Name == arrivalCity);

            PrintStartInfo(dispatchCity, arrivalCity, departureDay, departureTime);
            PrintShortCut(arrivalNode);

            Console.Read();
        }

        static private List<RouteModel> LoadRoutesFromJson()
        {
            /*
            Получить данные из файла
            Сериализовать полученные данные в объект
            Вернуть полученный объект
            */
            var jsonString = File.ReadAllText($@"{Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"))}\Data.json").Normalize();
            var jsonObject = JsonConvert.DeserializeObject<List<RouteModel>>(jsonString);

            return jsonObject;
        }

        static private void GetStartData()
        {
            try
            {
                /* Считать данные с консоли*/
                Console.WriteLine("Введите название города отправления");
                dispatchCity = Console.ReadLine();

                Console.WriteLine("Введите название города прибытия");
                arrivalCity = Console.ReadLine();

                Console.WriteLine("Введите день отправления цифрой");
                departureDay = default;
                bool parseStatus = Enum.TryParse(Console.ReadLine(), out departureDay);

                Console.WriteLine("Введите время отправления");
                departureTime = TimeSpan.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Допущена ошибка: {e.Message}");
                Console.WriteLine("Введите данные повторно");
                GetStartData();
            }

        }

        static private List<GraphNode> GetAllNodes(List<RouteModel> routes)
        {
            /*
            Перебрать все маршруты и получить все города которые в них участвуют
            */
            var nodes = new List<GraphNode>();
            foreach (var route in routes)
            {
                if (nodes.Find(el => el.Name == route.ArrivalCity) == null)
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

        static private GraphNode FindWaysDijkstra(string dispatchCity, string arrivalCity, DepartureDay departureDay, TimeSpan departureTime)
        {
            /* 
            Найти город отправления
            Установить время полёта в ноль
            Проложить кротчайшие пути до всех связанных городов
            Получить путь до города назначения
            */
            var firstNode = Graph.Nodes.Find(node => node.Name == dispatchCity);

            if (firstNode == null)
                return null;

            firstNode.TimeOfRoute = TimeSpan.Zero;

            BuildWaysFromNode(dispatchCity, departureDay, departureTime);

            return Graph.Nodes.Find(node => node.Name == arrivalCity);
        }

        static private void BuildWaysFromNode(string dispatchCity, DepartureDay departureDay, TimeSpan departureTime)
        {
            /* 
            Получить город в который попали
            Пометить как посещённый
            Получить все связанные города
                Для каждого найти лучший рейс
                Для каждого города проверить лучше ли новый путь чем прошлые
                Если лучше, то:
                    Записать Новое лучшее время пути для данного города
                    Записать путь по которому это время достигается
                После проверки каждого города удалить маршруты соеденяюющие города из графа (Для облегчения графа)
            Запустить рекурсию для непосещённых городов
            */
            var currentNode = Graph.Nodes.Find(node => node.Name == dispatchCity);
            currentNode.IsChecked = true;

            var flightsFromNode = Graph.Flights.FindAll(route => route.DispatchCity == dispatchCity);

            foreach (var route in flightsFromNode)
            {
                var bestFlight = FindBestFlight(route.Flights, departureDay, departureTime);
                var arrivalNode = Graph.Nodes.Find(node => node.Name == route.ArrivalCity);

                if (arrivalNode.TimeOfRoute > currentNode.TimeOfRoute + bestFlight.FlightTime)
                {
                    /*Костыль с поиском дня следующего вылета*/
                    var timeUntilFlight = bestFlight.FlightTime - TakeTimeInAir(bestFlight.Flight.DepartureTime, bestFlight.Flight.ArrivalTime);
                    var timeOfDeparture = departureTime + timeUntilFlight;
                    var nextDepartureDay = departureDay;

                    for (var i = 0; i < timeOfDeparture.Days; i++)
                        nextDepartureDay = Next(nextDepartureDay);
                    /*--------------------------------------------*/

                    arrivalNode.TimeOfRoute = currentNode.TimeOfRoute + bestFlight.FlightTime;
                    arrivalNode.RouteToTheNode = new List<GraphFlight>(currentNode.RouteToTheNode);
                    arrivalNode.RouteToTheNode.Add(new GraphFlight(bestFlight.Flight, route, nextDepartureDay, bestFlight.FlightTime));
                }

                Graph.Flights.RemoveAll(flights => flights.DispatchCity == currentNode.Name && flights.ArrivalCity == route.ArrivalCity);
                Graph.Flights.RemoveAll(flights => flights.DispatchCity == route.ArrivalCity && flights.ArrivalCity == currentNode.Name);
            }

            var nextNode = TakeNodeWithMinTimeOfRoute();
            if (nextNode != null)
            {
                var previousNode = nextNode.RouteToTheNode[nextNode.RouteToTheNode.Count - 1];
                var departureDate = TakeArrivalDate(previousNode, (DepartureDay)previousNode.DepartureDay);
                BuildWaysFromNode(nextNode.Name, departureDate.arrivalDay, departureDate.arrivalTime);
            }
        }

        public static (FlightModel Flight, TimeSpan FlightTime) FindBestFlight(List<FlightModel> flights, DepartureDay dayOfDeparture, TimeSpan timeOfDeparture)
        {
            /* 
            Перебрать все рейсы по одному маршруту и найти тот,
            который первым доставит в пункт назначения

            Вернуть рейс и время которое пройдёт до прибытия в следующий город
            */
            var minTime = TimeSpan.MaxValue;
            var flightWithMinTime = new FlightModel();

            foreach (var flight in flights)
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

        private static TimeSpan TimeUntilEndOfFlight(FlightModel flight, DepartureDay dayOfDeparture, TimeSpan timeOfDeparture)
        {
            /*
            Найти ближайший день вылета
            Посчить время оставшие до вылета
            Посчитать время в полёте
            Вернуть общее время до прибытия в пункт назначения
            */
            int daysUntilFlight = default;
            DepartureDay nearestFlightDay = dayOfDeparture;

            if (timeOfDeparture >= flight.DepartureTime)
            {
                GoToNextDay(ref nearestFlightDay, ref daysUntilFlight);
            }

            while (!flight.DeparturesDays.Contains(nearestFlightDay))
            {
                GoToNextDay(ref nearestFlightDay, ref daysUntilFlight);
            }

            return new TimeSpan(daysUntilFlight, 0, 0, 0) + (flight.DepartureTime - timeOfDeparture) + TakeTimeInAir(flight.DepartureTime, flight.ArrivalTime);
        }

        private static void GoToNextDay(ref DepartureDay nearestFlightDay, ref int daysUntilFlight)
        {
            /*
            Добавить день недели и увеличить количество дней в пути на 1
            */
            nearestFlightDay = Next(nearestFlightDay);
            ++daysUntilFlight;
        }

        public static TimeSpan TakeTimeInAir(TimeSpan departureTime, TimeSpan arrivalTime)
        {
            /*
            Посчитать и вернуть время которое самолёт проведёт в воздухе
            */
            return (departureTime < arrivalTime)
                ? arrivalTime - departureTime
                : new TimeSpan(24, 0, 0) + (arrivalTime - departureTime);
        }

        private static GraphNode TakeNodeWithMinTimeOfRoute()
        {
            /*
            Среди всех городов найти непосещённый город, путь до которого самый короткий
            */
            TimeSpan minTime = TimeSpan.MaxValue;
            GraphNode minTimeNode = null;

            foreach (var node in Graph.Nodes)
            {
                if (!node.IsChecked && node.TimeOfRoute < minTime)
                {
                    minTime = node.TimeOfRoute;
                    minTimeNode = node;
                }
            }
            return minTimeNode;
        }

        public static (DepartureDay arrivalDay, TimeSpan arrivalTime) TakeArrivalDate(GraphFlight flight, DepartureDay departureDay)
        {
            /*
            На основании дня вылета и рейса определить день прибытия
            */
            var time = flight.DepartureTime + TakeTimeInAir(flight.DepartureTime, flight.ArrivalTime);

            if (time.Days > 0)
            {
                departureDay = Next(departureDay);
                time -= new TimeSpan(time.Days, 0, 0, 0);
            }

            return (departureDay, time);
        }

        public static T Next<T>(this T src) where T : struct
        {
            /*
            На основе дня недели получить следующий
            */
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }

        public static void PrintStartInfo(string dispatchCity, string arrivalCity, DepartureDay departureDay, TimeSpan departureTime)
        {
            /* Вывести стартовые данные введёные пользователем*/
            Console.WriteLine($"Рассчёт пути из {dispatchCity} до {arrivalCity}!");
            Console.WriteLine($"Дань возможного вылета: {departureDay}, время возможного вылета: {departureTime}!");
        }

        public static void PrintShortCut(GraphNode node)
        {
            /* Вывести кротчайший маршрут, если он есть*/
            if (node == null)
            {
                Console.WriteLine();
                Console.WriteLine($"Невозможно проложить путь!!!");
                return;
            }

            Console.WriteLine($"Общее время в пути: Дней - {node.TimeOfRoute.Days}, Часов - {node.TimeOfRoute.Hours}, Минут - {node.TimeOfRoute.Minutes}, Секунд - {node.TimeOfRoute.Seconds}");
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
