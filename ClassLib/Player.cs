using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public static class Rank
    {
        public enum Key
        {
            rank_unknown = -1,
            c_minus,
            c,
            c_plus,
            b_minus,
            b,
            b_plus,
            a_minus,
            a,
            a_plus,
            s,
            s_plus_0,
            s_plus_1,
            s_plus_2,
            s_plus_3,
            s_plus_4,
            s_plus_5,
            s_plus_6,
            s_plus_7,
            s_plus_8,
            s_plus_9,
            x
        }

        public static Key ParseKey(string s, int splus = 0)
        {
            switch (s)
            {
                case "C-":
                    return Key.c_minus;
                case "C":
                    return Key.c;
                case "C+":
                    return Key.c_plus;
                case "B-":
                    return Key.b_minus;
                case "B":
                    return Key.b;
                case "B+":
                    return Key.b_plus;
                case "A-":
                    return Key.a_minus;
                case "A":
                    return Key.a;
                case "A+":
                    return Key.a_plus;
                case "S":
                    return Key.s;
                case "S+":
                    return (Key)((int)Key.s_plus_0 + splus);
                case "X":
                    return Key.x;
                default:
                    throw new FormatException();
            }
        }
    }

    public class Player
    {
        public enum SpeciesType
        {
            Inklings,
            Octolings
        }
        public enum StyleType
        {
            Girl,
            Boy
        }

        public string Id { get; }
        public string Nickname { get; }
        public SpeciesType Species { get; }
        public StyleType Style { get; }
        public bool IsSelf { get; }
        public int Level { get; }
        public HeadGear HeadGear { get; }
        public ClothesGear ClothesGear { get; }
        public ShoesGear ShoesGear { get; }
        public Weapon Weapon { get; }
        public int Paint { get; }
        public int Kill { get; }
        public int Assist { get; }
        public int Death { get; }
        public int Special { get; }
        public int Sort { get; }
        public string Image { get; }

        public int DisplayedLevel
        {
            get
            {
                return Level - Level / 100 * 100;
            }
        }
        public int Star
        {
            get
            {
                return Level / 100;
            }
        }
        public int KillAndAssist
        {
            get
            {
                return Kill + Assist;
            }
        }
        public bool IsOffline
        {
            get
            {
                return Paint <= 0;
            }
        }

        public Player(string id, string nickname, SpeciesType species, StyleType style, int level, HeadGear headGear, ClothesGear clothesGear, ShoesGear shoesGear, Weapon weapon, int paint, int kill, int assist, int death, int special, int sort, string image, bool isSelf = false)
        {
            Id = id;
            Nickname = nickname;
            Species = species;
            Style = style;
            IsSelf = isSelf;
            Level = level;
            HeadGear = headGear;
            ClothesGear = clothesGear;
            ShoesGear = shoesGear;
            Weapon = weapon;
            Paint = paint;
            Kill = kill;
            Assist = assist;
            Death = death;
            Special = special;
            Sort = sort;
            Image = image;
        }

        public static SpeciesType ParseSpecies(string s)
        {
            switch (s)
            {
                case "inklings":
                    return SpeciesType.Inklings;
                case "octolings":
                    return SpeciesType.Octolings;
                default:
                    throw new FormatException();
            }
        }
        public static StyleType ParseStyle(string s)
        {
            switch (s)
            {
                case "girl":
                    return StyleType.Girl;
                case "boy":
                    return StyleType.Boy;
                default:
                    throw new FormatException();
            }
        }
    }

    public class RankedPlayer : Player
    {
        public Rank.Key Rank { get; }

        public RankedPlayer(string id, string nickname, SpeciesType species, StyleType style, int level, Rank.Key rank, HeadGear headGear, ClothesGear clothesGear, ShoesGear shoesGear, Weapon weapon, int paint, int kill, int assist, int death, int special, int sort, string image, bool isSelf = false)
            : base(id, nickname, species, style, level, headGear, clothesGear, shoesGear, weapon, paint, kill, assist, death, special, sort, image, isSelf)
        {
            Rank = rank;
        }
        public RankedPlayer(Player player, Rank.Key rank)
            : this(player.Id, player.Nickname, player.Species, player.Style, player.Level, rank, player.HeadGear, player.ClothesGear, player.ShoesGear, player.Weapon, player.Paint, player.Kill, player.Assist, player.Death, player.Special, player.Sort, player.Image, player.IsSelf)
        {

        }
    }
}
