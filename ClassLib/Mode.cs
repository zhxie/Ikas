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
                case "fes":
                    return Key.splatfest;
                default:
                    throw new FormatException();
            }
        }
        
        public static Key ParseGameModeKey(string s)
        {
            switch (s)
            {
                case "regular":
                    return Key.regular_battle;
                case "gachi":
                    return Key.ranked_battle;
                case "league":
                case "league_team":
                    return Key.league_battle;
                case "fes_solo":
                case "fes_team":
                    return Key.splatfest;
                case "private":
                    return Key.private_battle;
                default:
                    throw new FormatException();
            }
        }
    }
}
