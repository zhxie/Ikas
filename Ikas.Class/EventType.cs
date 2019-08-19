using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class EventType
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

        public Key Id { get; }
        public string Name { get; }

        public EventType(Key id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Key ParseEventType(string s)
        {
            return (Key)Enum.Parse(typeof(Key), s.Replace("-", "_"));
        }
    }
}
