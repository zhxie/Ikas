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
            unknown,
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
    }

    public class Player
    {
        public string Id { get; }
        public string Nickname { get; }
        public int Level { get; }
        public HeadGear HeadGear { get; }
        public ClothesGear ClothesGear { get; }
        public ShoesGear ShoesGear { get; }
        public Weapon Weapon { get; }
        public int Paint { get; }
        public int Kill { get; }
        public int Assist { get; }
        public int Die { get; }
        public int Special { get; }
        public string Image { get; }

        public Player(string id, string nickName, int level, HeadGear headGear, ClothesGear clothesGear, ShoesGear shoesGear, Weapon weapon, int paint, int kill, int assist, int die, int special, string image)
        {
            Id = id;
            Nickname = nickName;
            Level = level;
            HeadGear = headGear;
            ClothesGear = clothesGear;
            ShoesGear = shoesGear;
            Weapon = weapon;
            Paint = paint;
            Kill = kill;
            Assist = assist;
            Die = die;
            Special = special;
            Image = image;
        }
    }

    public class RankedPlayer : Player
    {
        public Rank.Key Rank { get; }

        public RankedPlayer(string id, string nickName, int level, Rank.Key rank HeadGear headGear, ClothesGear clothesGear, ShoesGear shoesGear, Weapon weapon, int paint, int kill, int assist, int die, int special, string image)
            : base(id, nickName, level, headGear, clothesGear, shoesGear, weapon, paint, kill, assist, die, special, image)
        {
            Rank = rank;
        }
    }
}
