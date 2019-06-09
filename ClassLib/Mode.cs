using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Mode
    {
        public enum Key
        {
            regular_battle,
            ranked_battle,
            league_battle,
            private_battle,
            splatfest
        }

        public int Id { get; }
        public string Name { get; }

        public Mode(Key key)
        {
            Id = (int)key;
            Name = key.ToString();
        }
    }
}
