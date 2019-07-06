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
