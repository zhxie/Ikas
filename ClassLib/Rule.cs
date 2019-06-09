using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
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

        public int Id { get; }

        public Rule(Key key)
        {
            Id = (int)key;
        }

        public static Rule Parse(string s)
        {
            return new Rule((Key)Enum.Parse(typeof(Key), s));
        }
    }
}
