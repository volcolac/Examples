using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Flight
    {
        public int From { get; private set; }
        public int To { get; private set; }
        public int WayInMinute { get; private set; }
        public int Cost { get; private set; }
        public Time Start { get; private set; }
        public Time Finish { get; private set; }
        public int Seats { get; private set; }
        public int FreeSeats { get; private set; }

        public Flight(int from, int to, string start, string finish, int cost, int seats)
        {
            var rand = new Random();
            From = from;
            To = to;
            Cost = cost;
            var a = start.Split(':');
            Start = new Time(int.Parse(a[0]), int.Parse(a[1]));
            a = finish.Split(':');
            Finish = new Time(int.Parse(a[0]), int.Parse(a[1]));
            WayInMinute = Finish.Diff(Start);
            Seats = seats;
            FreeSeats = rand.Next()%Seats;
        }

        public Flight(string from, string to, string start, string finish, string cost, string seats)
        {
            var rand = new Random();
            From = int.Parse(from);
            To = int.Parse(to);
            Cost = int.Parse(cost);
            var a = start.Split(':');
            Start = new Time(int.Parse(a[0]), int.Parse(a[1]));
            a = finish.Split(':');
            Finish = new Time(int.Parse(a[0]), int.Parse(a[1]));
            WayInMinute = Finish.Diff(Start);
            Seats = int.Parse(seats);
            FreeSeats = rand.Next() % Seats;
        }
    }
}
