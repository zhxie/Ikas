using System;
using System.Collections.Generic;
using System.Text;

namespace IkasLib
{
    public class Stage
    {
        public enum Key
        {
            unknown = -1,
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
            wayslide_cool = 101,
            the_secret_of_splat,
            goosponge,
            windmill_house_on_the_pearlie,
            fancy_spew,
            zone_of_glass,
            cannon_fire_pearl,
            the_bunker_games,
            grapplink_girl,
            zappy_longshocking,
            a_swiftly_tilting_balance,
            sweet_valley_tentacles,
            the_switches,
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
            shifty_station = 9999
        }
        public enum ShortName
        {
            unknown = -1,
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
            shifty_01 = 101,
            shifty_02,
            shifty_03,
            shifty_04,
            shifty_05,
            shifty_06,
            shifty_07,
            shifty_08,
            shifty_09,
            shifty_10,
            shifty_11,
            shifty_12,
            shifty_13,
            shifty_14,
            shifty_15,
            shifty_16,
            shifty_17,
            shifty_18,
            shifty_19,
            shifty_20,
            shifty_21,
            shifty_22,
            shifty_23,
            shifty_24,
            shifty = 9999
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
}
