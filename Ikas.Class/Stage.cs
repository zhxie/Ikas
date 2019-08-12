using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Stage
    {
        public enum Key
        {
            stage_unknown = -1,
            the_reef,
            musselforge_fitness,
            starfish_mainstage,
            sturgeon_shipyard,
            inkblot_art_academy,
            humpback_pump_track,
            manta_maria,
            port_mackerel,
            moray_towers,
            snapper_canal,
            kelp_dome,
            blackbelly_skatepark,
            shellendorf_institute,
            makomart,
            walleye_warehouse,
            arowana_mall,
            camp_triggerfish,
            piranha_pit,
            goby_arena,
            new_albacore_hotel,
            wahoo_world,
            anchov_games,
            skipper_pavilion,
            windmill_house_on_the_pearlie = 100,
            wayslide_cool,
            the_secret_of_splat,
            goosponge,
            cannon_fire_pearl = 105,
            zone_of_glass,
            fancy_spew,
            grapplink_girl,
            zappy_longshocking,
            the_bunker_games,
            a_swiftly_tilting_balance,
            the_switches,
            sweet_valley_tentacles,
            the_bouncey_twins,
            railway_chillin,
            gusher_towns,
            the_maze_dasher,
            flooders_in_the_attic,
            the_splat_in_our_zones,
            the_ink_is_spreading,
            bridge_to_tentaswitchia,
            the_chronicles_of_rolonium,
            furler_in_the_ashes,
            mc_princess_diaries,
            shifty_station = 9999,
            shift_stage_unknown = -2,
            spawning_grounds = 10000,
            marooners_bay,
            lost_outpost,
            salmonid_smokeyard,
            ruins_of_ark_polaris
        }
        public enum ShortName
        {
            stage_unknown = -1,
            reef,
            fitness,
            mainstage,
            shipyard,
            academy,
            track,
            manta,
            port,
            towers,
            canal,
            dome,
            skatepark,
            institute,
            mart,
            warehouse,
            mall,
            camp,
            pit,
            arena,
            hotel,
            world,
            games,
            pavilion,
            shifty_04 = 100,    // shifty_windmill
            shifty_01,          // shifty_wayslide
            shifty_02,          // shifty_splat
            shifty_03,          // shifty_goosponge
            shifty_07 = 105,    // shifty_cannon
            shifty_06,          // shifty_glass
            shifty_05,          // shifty_spew
            shifty_09,          // shifty_grapplink
            shifty_10,          // shifty_longshocking
            shifty_08,          // shifty_bunker
            shifty_11,          // shifty_balance
            shifty_13,          // shifty_switches
            shifty_12,          // shifty_tentacles
            shifty_14,          // shifty_bouncey
            shifty_15,          // shifty_railway
            shifty_16,          // shifty_gusher
            shifty_17,          // shifty_dasher
            shifty_18,          // shifty_flooders
            shifty_19,          // shifty_zones
            shifty_20,          // shifty_spreading
            shifty_21,          // shifty_tentaswitchia
            shifty_22,          // shifty_rolonium
            shifty_23,          // shifty_furler
            shifty_24,          // shifty_diaries
            shifty = 9999,
            shift_stage_unknown = -2,
            grounds = 10000,
            bay,
            outpost,
            smokeyard,
            ark
        }

        public Key Id { get; }
        public string Image { get; }

        public Stage(Key key, string image)
        {
            Id = key;
            Image = image;
        }
        public Stage()
        {
            Id = (Key)(-1);
            Image = "";
        }
    }

    public class ScheduledStage : Stage
    {
        public Mode.Key Mode { get; }
        public Rule.Key Rule { get; }

        public ScheduledStage(Mode.Key mode, Rule.Key rule, Key key, string image) : base(key, image)
        {
            Mode = mode;
            Rule = rule;
        }
        public ScheduledStage(Mode.Key mode, Rule.Key rule) : base()
        {
            Mode = mode;
            Rule = rule;
        }
    }

    public class ShiftStage : Stage
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public List<Weapon> Weapons { get; }

        public ShiftStage(Key key, string image, DateTime startTime, DateTime endTime, List<Weapon> weapons) : base(key, image)
        {
            StartTime = startTime;
            EndTime = endTime;
            Weapons = weapons;
        }

        public static ShiftStage FromUrl(string image, DateTime startTime, DateTime endTime, List<Weapon> weapons)
        {
            switch (image)
            {
                case "/images/coop_stage/65c68c6f0641cc5654434b78a6f10b0ad32ccdee.png":
                    return new ShiftStage(Key.spawning_grounds, image, startTime, endTime, weapons);
                case "/images/coop_stage/e07d73b7d9f0c64e552b34a2e6c29b8564c63388.png":
                    return new ShiftStage(Key.marooners_bay, image, startTime, endTime, weapons);
                case "/images/coop_stage/6d68f5baa75f3a94e5e9bfb89b82e7377e3ecd2c.png":
                    return new ShiftStage(Key.lost_outpost, image, startTime, endTime, weapons);
                case "/images/coop_stage/e9f7c7b35e6d46778cd3cbc0d89bd7e1bc3be493.png":
                    return new ShiftStage(Key.salmonid_smokeyard, image, startTime, endTime, weapons);
                case "/images/coop_stage/50064ec6e97aac91e70df5fc2cfecf61ad8615fd.png":
                    return new ShiftStage(Key.ruins_of_ark_polaris, image, startTime, endTime, weapons);
                default:
                    return new ShiftStage((Key)(-2), image, startTime, endTime, weapons);
            }
        }
    }
}
