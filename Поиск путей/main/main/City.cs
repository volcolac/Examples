using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class City
    {
        public Point Coordinat { get; private set; }
        public string Name { get; private set; }
        public int Id { get; private set; }

        public City(int id, int x, int y, string name)
        {
            Id = id;
            Name = name;
            Coordinat = new Point(x, y);

        }

    }
}
