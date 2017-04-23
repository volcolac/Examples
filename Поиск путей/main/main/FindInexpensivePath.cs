using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    class FindInexpensivePath
    {
        public DataBase Data { get; private set; }
        private Flight[,] Matrix { get; set; }
        private bool[] Usd { get; set; }
        private int[] Parents { get; set; }
        private int CountCities { get; set; }
        private int[] Cost { get; set; }
 
        public FindInexpensivePath(DataBase data)
        {
            Data = data;
            CountCities = Data.Cities.Count();
            Usd = new bool[CountCities];
            Parents = new int[CountCities];
            Matrix = new Flight[CountCities, CountCities];
            Cost = new int[CountCities];
            foreach (var flight in Data.Flights)
            {
                var from = flight.From;
                var to = flight.To;
                if (Matrix[from, to] == null)
                    Matrix[from, to] = flight;
                else if (Matrix[from, to].Cost > flight.Cost)
                    Matrix[from, to] = flight;
            }
        }

        public Path FindPath(string startS, string finishS)
        {
            var start = 0;
            var finish = 0;
            foreach (var city in Data.Cities)
            {
                if (city.Name == startS)
                    start = city.Id;
                if (city.Name == finishS)
                    finish = city.Id;
            }
            Usd = Usd.Select(a => false).ToArray();
            Parents = Parents.Select(a => -1).ToArray();
            Cost = Cost.Select(a => 999999).ToArray();
            Cost[start] = 0;
            Usd[start] = true;

            Dfs(start, finish);

            return new Path(Recovery(finish).ToArray());
        }

        private void Dfs(int current, int finish)
        {
            Relax(current);
            var min = 999999;
            var minId = 0;
            for (var i = 0; i < CountCities; i++)
            {
                if(Usd[i])
                    continue;
                if (Cost[i] >= min) continue;
                min = Cost[i];
                minId = i;
            }
            Usd[minId] = true;
            if (minId == finish)
            {
                return;
            }
            Dfs(minId, finish);
        }

        private void Relax(int id)
        {
            for (var i = 0; i < CountCities; i++)
            {
                if (Usd[i])
                    continue;
                if (Matrix[id, i] != null && Cost[id] + Matrix[id, i].Cost < Cost[i])
                {
                    Cost[i] = Cost[id] + Matrix[id, i].Cost;
                    Parents[i] = id;
                }
            }
        }

        private List<Flight> Recovery(int finish)
        {
            var ans = new List<int>();
            var current = finish;
            while (current != -1)
            {
                ans.Add(current);
                current = Parents[current];
            }
            var flights = new List<Flight>();
            for(var i = ans.Count - 1; i > 0; i--)
                flights.Add(Matrix[ans[i], ans[i - 1]]);
            return flights;
        }
    }
}
