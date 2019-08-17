using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Rule
    {
        public enum Key
        {
            turf_war,
            splat_zones,
            tower_control,
            rainmaker,
            clam_blitz
        }
        public enum ShortName
        {
            turf,
            zones,
            tower,
            rain,
            clam
        }

        public Key Id { get; }
        public string Name { get; }

        public Rule(Key id, string name)
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
