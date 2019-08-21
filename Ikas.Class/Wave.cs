using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Wave
    {
        public WaterLevel.Key WaterLevel { get; }
        public EventType.Key EventType { get; }
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

        public Wave(WaterLevel.Key waterLevel, EventType.Key eventType, int quota, int powerEgg, int goldenEgg, int goldenEggPop, List<SpecialWeapon> specials, Job.ResultType result)
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
    }
}
