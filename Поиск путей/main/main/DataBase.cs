using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class DataBase
    {
        public City[] Cities { get; private set; }
        public Flight[] Flights { get; private set; }

        public DataBase()
        {
            var lines = File.ReadAllLines("City.txt");
            var pointer = 0;
            Cities = new City[int.Parse(lines[0])];
            foreach (var a in lines.Select(line => line.Split(' ')).Where(a => a.Count() != 1))
            {
                Cities[pointer] = new City(int.Parse(a[0]), int.Parse(a[2]), int.Parse(a[3]), a[1]);
                pointer++;
            }

            lines = File.ReadAllLines("Flight.txt");
            pointer = 0;
            Flights = new Flight[2 * int.Parse(lines[0])];
            foreach (var a in lines.Select(line => line.Split(' ')).Where(a => a.Count() != 1))
            {
                Flights[pointer] = new Flight(a[0], a[1], a[2], a[3], a[4], a[5]);
                pointer++;
                Flights[pointer] = new Flight(a[1], a[0], a[2], a[3], a[4], a[5]);
                pointer++;
            }
        }

        public Point[] FindPoints(int[] id)
        {
            var count = id.Count();
            var points = new Point[count];
            for (var i = 0; i < count; i++)
            {
                points[i] = Cities[id[i]].Coordinat;
            }
            return points;
        }
    }
}
