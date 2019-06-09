using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Stage
    {
        public enum Key
        {
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
        private enum shortName
        {
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

        public int Id { get; }
        public string Name { get; }
        public string ShortName { get; }

        public Stage(Key key)
        {
            Id = (int)key;
            Name = key.ToString();
            ShortName = ((shortName)key).ToString();
        }
        public Stage()
        {
            Id = -1;
            Name = "unknown";
            ShortName = "unknown";
        }
    }

    public class ScheduledStage : Stage
    {
        public Mode Mode { get; }
        public Rule Rule { get; }

        public ScheduledStage(Mode mode, Rule rule, Key key) : base(key)
        {
            Mode = mode;
            Rule = rule;
        }
        public ScheduledStage(Mode mode, Rule rule) : base()
        {
            Mode = mode;
            Rule = rule;
        }
    }
}
