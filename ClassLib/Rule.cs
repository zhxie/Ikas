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
        private enum shortName
        {
            turf,
            zones,
            tower,
            rain,
            clam
        }

        public int Id { get; }
        public string Name { get; }
        public string ShortName { get; }

        public Rule(Key key)
        {
            Id = (int)key;
            Name = key.ToString();
            ShortName = ((shortName)key).ToString();
        }
    }
}
