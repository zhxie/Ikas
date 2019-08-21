using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public static class EventType
    {
        public enum Key
        {
            water_levels,
            rush,
            fog,
            goldie_seeking,
            griller,
            cohock_charge,
            the_mothership
        }

        public static Key ParseKey(string s)
        {
            return (Key)Enum.Parse(typeof(Key), s.Replace("-", "_"));
        }
    }
}
