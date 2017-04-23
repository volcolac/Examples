using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class FindQuickPath
    {
       public DataBase Data { get; private set; }
        private List<Flight>[,] Matrix { get; set; }
        private bool[] Usd { get; set; }
        private Time[] Finish { get; set; }
        private int[] Parents { get; set; }
        private int CountCities { get; set; }
        private int[] Cost { get; set; }
        private List<Flight>[] Anses { get; set; } 
 
        public FindQuickPath(DataBase data)
        {
            Data = data;
            CountCities = Data.Cities.Count();
            Usd = new bool[CountCities];
            Anses = new List<Flight>[CountCities];
            Finish = new Time[CountCities];
            Parents = new int[CountCities];
            Matrix = new List<Flight>[CountCities, CountCities];
            Cost = new int[CountCities];
            foreach (var flight in Data.Flights)
            {
                var from = flight.From;
                var to = flight.To;
                if(Matrix[from, to] == null)
                    Matrix[from, to] = new List<Flight>();
                Matrix[from, to].Add(flight);
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
            Anses[start] = new List<Flight>();

            Dfs(start, finish);
            return new Path(Anses[finish].ToArray());
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
                if (Usd[i] || Matrix[id, i] == null)
                    continue;
                for (var j = 0; j < Matrix[id, i].Count(); j++)
                {
                    if (Finish[id] == null)
                    {
                        if (Cost[id] + Matrix[id, i][j].WayInMinute >= Cost[i]) continue;
                        Cost[i] = Cost[id] + Matrix[id, i][j].WayInMinute;
                        Anses[i] = new List<Flight>();
                        foreach (var flight in Anses[id])
                        {
                            Anses[i].Add(flight);
                        }
                        Anses[i].Add(Matrix[id, i][j]);
                        Finish[i] = Matrix[id, i][j].Finish;
                    }
                    else
                    {
                        if (Cost[id] + Matrix[id, i][j].WayInMinute + Matrix[id, i][j].Start.Diff(Finish[id]) >= Cost[i])
                            continue;
                        Cost[i] = Cost[id] + Matrix[id, i][j].WayInMinute + Matrix[id, i][j].Start.Diff(Finish[id]);
                        Anses[i] = new List<Flight>();
                        foreach (var flight in Anses[id])
                        {
                            Anses[i].Add(flight);
                        }
                        Anses[i].Add(Matrix[id, i][j]);
                        Finish[i] = Matrix[id, i][j].Finish;
                    }
                }
            }
        }
    }
}
