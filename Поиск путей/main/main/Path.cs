using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Path
    {
        public int Start { get; private set; }
        public int Finish { get; private set; }
        public int[] Cities { get; private set; }
        public Flight[] Flights { get; private set; }
        public int Cost { get; private set; }
        public int WayInMinute { get; private set; }
        public int TimeInFly { get; private set; }

        public Path(Flight[] flights)
        {
            Cost = 0;
            TimeInFly = 0;
            foreach (var flight in flights)
            {
                Cost += flight.Cost;
                TimeInFly += flight.WayInMinute;
            }
            Flights = flights;
            var cities = flights.Select(flight => flight.From).ToList();
            cities.Add(flights[flights.Count() - 1].To);
            Cities = cities.ToArray();
            Start = cities[0];
            Finish = cities[cities.Count - 1];
            WayInMinute = 0;
            for (var i = 0; i < Flights.Count(); i++)
            {
                WayInMinute += Flights[i].WayInMinute;
                if (i > 0)
                    WayInMinute += Flights[i].Start.Diff(Flights[i - 1].Finish);
            }
        }
    }
}
