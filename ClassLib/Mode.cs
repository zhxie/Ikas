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

        public Mode(Key key)
        {
            Id = (int)key;
        }

        public static Mode Parse(string s)
        {
            switch (s)
            {
                case "regular":
                    return new Mode(Key.regular_battle);
                case "gachi":
                    return new Mode(Key.ranked_battle);
                case "league":
                case "league_team":
                    return new Mode(Key.league_battle);
                case "private":
                    return new Mode(Key.private_battle);
                case "fes":
                case "fes_solo":
                case "fes_team":
                    return new Mode(Key.splatfest);
                default:
                    throw new InvalidCastException();
            }
        }
    }
}
