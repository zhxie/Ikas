using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public static class Mode
    {
        public enum Key
        {
            regular_battle,
            ranked_battle,
            league_battle,
            private_battle,
            splatfest
        }

        public static Key ParseKey(string s)
        {
            switch (s)
            {
                case "regular":
                    return Key.regular_battle;
                case "gachi":
                    return Key.ranked_battle;
                case "league":
                    return Key.league_battle;
                default:
                    throw new InvalidCastException();
            }
        }
    }
}
