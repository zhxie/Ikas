using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public static class Rule
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

        public static Key ParseKey(string s)
        {
            return (Key)Enum.Parse(typeof(Key), s);
        }
    }
}
