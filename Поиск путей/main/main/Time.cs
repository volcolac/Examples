using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Time
    {
       
        public int Hour { get; private set; }
        public int Minute { get; private set; }

        public override string ToString()
        {
            var s = new StringBuilder();
            if (Hour.ToString().Length > 1)
                s.Append(Hour.ToString());
            else
            {
                s.Append("0");
                s.Append(Hour.ToString());
            }
            s.Append(":");
            if (Minute.ToString().Length > 1)
                s.Append(Minute.ToString());
            else
            {
                s.Append("0");
                s.Append(Minute.ToString());
            }
            return s.ToString();
        }

        public Time(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public Time(int minute)
        {
            Hour = minute/60;
            Minute = minute%60;
        }

        public void Plus(int minute)
        {
            var plus = new Time(minute);
            Plus(plus);
        }

        public void Plus(Time plus)
        {
            Minute += plus.Minute;
            if (Minute > 60)
            {
                Hour++;
                Minute -= 60;
            }
            Hour += plus.Hour;
            Hour = Hour%24;
        }

        public int Diff(Time t)
        {
            var ans = (Hour - t.Hour)*60 + (Minute - t.Minute);
            if (ans < 0)
            {
                return new Time(24, 0).Diff(t) + this.Diff(new Time(0));
            }
            return ans;
        }
    }
}
