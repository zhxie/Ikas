﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
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
                case "":
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

    public abstract class BasePlayer
    {
        public enum SpeciesType
        {
            inklings,
            octolings
        }
        public enum StyleType
        {
            girl,
            boy
        }
    }

    public class Player : BasePlayer
    {
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
        public double KillDeathRatio
        {
            get
            {
                if (Death == 0)
                {
                    if (Kill == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 99.99;
                    }
                }
                else
                {
                    return Math.Min(Kill * 1.0 / Death, 99.99);
                }
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
            return (SpeciesType)Enum.Parse(typeof(SpeciesType), s);
        }
        public static StyleType ParseStyle(string s)
        {
            return (StyleType)Enum.Parse(typeof(StyleType), s);
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

    public class SalmonRunPlayer : BasePlayer
    {
        public enum GradeType
        {
            intern,
            apprentice,
            part_timer,
            go_getter,
            overachiever,
            profeshional
        }

        public string Id { get; }
        public string Nickname { get; }
        public SpeciesType Species { get; }
        public StyleType Style { get; }
        public bool IsSelf { get; }
        public GradeType Grade { get; }
        public int GradePoint { get; }
        public List<Weapon> Weapons { get; }
        public List<int> SpecialWeaponCount { get; }
        public int SteelheadKill { get; }
        public int FlyfishKill { get; }
        public int SteelEelKill { get; }
        public int DrizzlerKill { get; }
        public int StingerKill { get; }
        public int ScrapperKill { get; }
        public int MawsKill { get; }
        public int GrillerKill { get; }
        public int GoldieKill { get; }
        public int Help { get; }
        public int Dead { get; }
        public int PowerEgg { get; }
        public int GoldenEgg { get; }
        public string Image { get; }

        public int Rate
        {
            get
            {
                if (Grade == GradeType.intern)
                {
                    return 90;
                }
                else
                {
                    return 65 + 25 * (int)Grade + 5 * (GradePoint / 20);
                }
            }
        }
        public int SpecialWeaponRemained
        {
            get
            {
                int use = 0;
                foreach (int count in SpecialWeaponCount)
                {
                    use = use + count;
                }
                return 2 - use;
            }
        }

        public SalmonRunPlayer(string id, string nickname, SpeciesType species, StyleType style, GradeType grade, int gradePoint, List<Weapon> weapons, List<int> specialWeaponCount,
            int steelHeadKill, int flyFishKill, int steelEelKill, int drizzlerKill, int stingerKill, int mawsKill, int grillerKill, int goldieKill, int help, int dead, int powerEgg, int goldenEgg, string image, bool isSelf = false)
        {
            Id = id;
            Nickname = nickname;
            Species = species;
            Style = style;
            IsSelf = isSelf;
            Grade = grade;
            GradePoint = gradePoint;
            Weapons = weapons;
            SpecialWeaponCount = specialWeaponCount;
            SteelheadKill = steelHeadKill;
            FlyfishKill = flyFishKill;
            SteelEelKill = steelEelKill;
            DrizzlerKill = drizzlerKill;
            StingerKill = stingerKill;
            MawsKill = mawsKill;
            GrillerKill = grillerKill;
            GoldieKill = goldieKill;
            Help = help;
            Dead = dead;
            PowerEgg = powerEgg;
            GoldenEgg = goldenEgg;
            Image = image;
        }
    }
}
