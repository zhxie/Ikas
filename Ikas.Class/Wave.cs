using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Wave
    {
        public enum WaterLevelType
        {
            normal,
            low,
            high
        }

        public enum EventTypeType
        {
            water_levels,
            rush,
            fog,
            goldie_seeking,
            griller,
            cohock_charge,
            the_mothership,
        }

        public WaterLevelType WaterLevel { get; }
        public EventTypeType EventType { get; }
        public int Quota { get; }
        public int PowerEgg { get; }
        public int GoldenEgg { get; }
        public int GoldenEggPop { get; }
        public List<SpecialWeapon> Specials { get; }
        public Job.ResultType Result { get; }

        public bool IsClear
        {
            get
            {
                return Result == Job.ResultType.clear;
            }
        }

        public Wave(WaterLevelType waterLevel, EventTypeType eventType, int quota, int powerEgg, int goldenEgg, int goldenEggPop, List<SpecialWeapon> specials, Job.ResultType result)
        {
            WaterLevel = waterLevel;
            EventType = eventType;
            Quota = quota;
            PowerEgg = powerEgg;
            GoldenEgg = goldenEgg;
            GoldenEggPop = goldenEggPop;
            Specials = specials;
            Result = result;
        }

        public static WaterLevelType ParseWaterLevel(string s)
        {
            return (WaterLevelType)Enum.Parse(typeof(WaterLevelType), s);
        }
        public static EventTypeType ParseEventType(string s)
        {
            return (EventTypeType)Enum.Parse(typeof(EventTypeType), s.Replace("-", "_"));
        }
    }
}
