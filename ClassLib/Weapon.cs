using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class SubWeapon
    {
        public enum Key
        {
            splat_bomb = 0,
            suction_bomb = 1,
            burst_bomb = 2,
            curling_bomb = 3,
            autobomb = 4,
            ink_mine = 5,
            sprinkler = 6,
            toxic_mist = 7,
            point_sensor = 8,
            splash_wall = 9,
            squid_beakon = 10,
            fizzy_bomb = 11,
            torpedo = 12,
        }

        public Key Id { get; }
        public string Image1 { get; }
        public string Image2 { get; }

        public SubWeapon(Key id, string image1, string image2)
        {
            Id = id;
            Image1 = image1;
            Image2 = image2;
        }
    }

    public class SpecialWeapon
    {
        public enum Key
        {
            tenta_missiles = 0,
            ink_armor = 1,
            splat_bomb_launcher = 2,
            suction_bomb_launcher = 3,
            burst_bomb_launcher = 4,
            curling_bomb_launcher = 5,
            autobomb_launcher = 6,
            sting_ray = 7,
            inkjet = 8,
            splashdown = 9,
            ink_storm = 10,
            baller = 11,
            bubble_blower = 12,
            booyah_bomb = 17,
            ultra_stamp = 18,
        }

        public Key Id { get; }
        public string Image1 { get; }
        public string Image2 { get; }

        public SpecialWeapon(Key id, string image1, string image2)
        {
            Id = id;
            Image1 = image1;
            Image2 = image2;
        }
    }

    public class Weapon
    {
        public enum Key
        {
            // Shooter
            bold = 0,
            bold_neo = 1,
            bold_7 = 2,
            wakaba = 10,
            momiji = 11,
            ochiba = 12,
            sharp = 20,
            sharp_neo = 21,
            promodeler_mg = 30,
            promodeler_rg = 31,
            promodeler_pg = 32,
            sshooter = 40,
            sshooter_collabo = 41,
            sshooter_becchu = 42,
            heroshooter_replica = 45,
            octoshooter_replica = 46,
            _52gal = 50,
            _52gal_deco = 51,
            _52gal_becchu = 52,
            nzap85 = 60,
            nzap89 = 61,
            nzap83 = 62,
            prime = 70,
            prime_collabo = 71,
            prime_becchu = 72,
            _96gal = 80,
            _96gal_deco = 81,
            jetsweeper = 90,
            jetsweeper_custom = 91,
            bottlegeyser = 400,
            bottlegeyser_foil = 401,
            // Blaster
            nova = 200,
            nova_neo = 201,
            nova_becchu = 202,
            hotblaster = 210,
            hotblaster_custom = 211,
            heroblaster_replica = 215,
            longblaster = 220,
            longblaster_custom = 221,
            longblaster_necro = 222,
            clashblaster = 230,
            clashblaster_neo = 231,
            rapid = 240,
            rapid_deco = 241,
            rapid_becchu = 242,
            rapid_elite = 250,
            rapid_elite_deco = 251,
            // Reelgun
            l3reelgun = 300,
            l3reelgun_d = 301,
            l3reelgun_becchu = 302,
            h3reelgun = 310,
            h3reelgun_d = 311,
            h3reelgun_cherry = 312,
            // Roller
            carbon = 1000,
            carbon_deco = 1001,
            splatroller = 1010,
            splatroller_collabo = 1011,
            splatroller_becchu = 1012,
            heroroller_replica = 1015,
            dynamo = 1020,
            dynamo_tesla = 1021,
            dynamo_becchu = 1022,
            variableroller = 1030,
            variableroller_foil = 1031,
            // Brush
            pablo = 1100,
            pablo_hue = 1101,
            pablo_permanent = 1102,
            hokusai = 1110,
            hokusai_hue = 1111,
            hokusai_becchu = 1112,
            herobrush_replica = 1115,
            // Charger
            squiclean_a = 2000,
            squiclean_b = 2001,
            squiclean_g = 2002,
            splatcharger = 2010,
            splatcharger_collabo = 2011,
            splatcharger_becchu = 2012,
            herocharger_replica = 2015,
            splatscope = 2020,
            splatscope_collabo = 2021,
            splatscope_becchu = 2022,
            liter4k = 2030,
            liter4k_custom = 2031,
            liter4k_scope = 2040,
            liter4k_scope_custom = 2041,
            bamboo14mk1 = 2050,
            bamboo14mk2 = 2051,
            bamboo14mk3 = 2052,
            soytuber = 2060,
            soytuber_custom = 2061,
            // Slosher
            bucketslosher = 3000,
            bucketslosher_deco = 3001,
            bucketslosher_soda = 3002,
            heroslosher_replica = 3005,
            hissen = 3010,
            hissen_hue = 3011,
            screwslosher = 3020,
            screwslosher_neo = 3021,
            screwslosher_becchu = 3022,
            furo = 3030,
            furo_deco = 3031,
            explosher = 3040,
            explosher_custom = 3041,
            // Splatling
            splatspinner = 4000,
            splatspinner_collabo = 4001,
            splatspinner_becchu = 4002,
            barrelspinner = 4010,
            barrelspinner_deco = 4011,
            barrelspinner_remix = 4012,
            herospinner_replica = 4015,
            hydra = 4020,
            hydra_custom = 4021,
            kugelschreiber = 4030,
            kugelschreiber_hue = 4031,
            nautilus47 = 4040,
            nautilus79 = 4041,
            // Maneuver
            sputtery = 5000,
            sputtery_hue = 5001,
            sputtery_clear = 5002,
            maneuver = 5010,
            maneuver_collabo = 5011,
            maneuver_becchu = 5012,
            heromaneuver_replica = 5015,
            kelvin525 = 5020,
            kelvin525_deco = 5021,
            kelvin525_becchu = 5022,
            dualsweeper = 5030,
            dualsweeper_custom = 5031,
            quadhopper_black = 5040,
            quadhopper_white = 5041,
            // Brella
            parashelter = 6000,
            parashelter_sorella = 6001,
            heroshelter_replica = 6005,
            campingshelter = 6010,
            campingshelter_sorella = 6011,
            campingshelter_camo = 6012,
            spygadget = 6020,
            spygadget_sorella = 6021,
            spygadget_becchu = 6022,
        }

        public Key Id { get; }
        public SubWeapon SubWeapon { get; }
        public SpecialWeapon SpecialWeapon { get; }
        public string Image { get; }

        public Weapon(Key id, SubWeapon subWeapon, SpecialWeapon specialWeapon, string image)
        {
            Id = id;
            SubWeapon = subWeapon;
            SpecialWeapon = specialWeapon;
            Image = image;
        }
    }
}
