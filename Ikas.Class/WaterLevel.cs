using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class WaterLevel
    {
        public enum Key
        {
            normal,
            low,
            high
        }

        public Key Id { get; }
        public string Name { get; }

        public WaterLevel(Key id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Key ParseKey(string s)
        {
            return (Key)Enum.Parse(typeof(Key), s);
        }
    }
}
