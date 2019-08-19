using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Job : Base
    {
        public enum ResultType
        {
            clear,
            time_limit,
            wipe_out
        }

        public int Number { get; }
        public DateTime StartTime { get; }
        public double HazardLevel { get; }
        public ShiftStage Stage { get; }
        public List<Wave> Waves { get; }
        public JobPlayer MyPlayer { get; }
        public List<JobPlayer> OtherPlayers { get; }
        public int SteelheadCount { get; }
        public int FlyfishCount { get; }
        public int SteelEelCount { get; }
        public int DrizzlerCount { get; }
        public int StingerCount { get; }
        public int ScrapperCount { get; }
        public int MawsCount { get; }
        public int GrillerCount { get; }
        public int GoldieCount { get; }
        public int Score { get; }
        public int GradePointDelta { get; }
        public ResultType Result { get; }

        public bool IsClear
        {
            get
            {
                return Result == ResultType.clear;
            }
        }
        public int GoldenEgg
        {
            get
            {
                int count = 0;
                foreach (Wave wave in Waves)
                {
                    count += wave.GoldenEgg;
                }
                return count;
            }
        }
        public int Quota
        {
            get
            {
                int count = 0;
                foreach (Wave wave in Waves)
                {
                    count += wave.Quota;
                }
                return count;
            }
        }
        public Grade Grade
        {
            get
            {
                return MyPlayer.Grade;
            }
        }
        public int GradePoint
        {
            get
            {
                return MyPlayer.GradePoint;
            }
        }
        public int Rate
        {
            get
            {
                return MyPlayer.Rate;
            }
        }
        public double PayGrade
        {
            get
            {
                return Rate * 1.0 / 100;
            }
        }
        public int GrizzcoPoint
        {
            get
            {
                return (int)Math.Round(PayGrade * Score, MidpointRounding.AwayFromZero);
            }
        }
        public int FailureWave
        {
            get
            {
                if (Result == ResultType.clear)
                {
                    return -1;
                }
                else
                {
                    return Waves.Count;
                }
            }
        }
        public int BossCount
        {
            get
            {
                return SteelheadCount + FlyfishCount + SteelEelCount + DrizzlerCount + StingerCount + ScrapperCount + MawsCount + GrillerCount + GoldieCount;
            }
        }
        public int SteelheadKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.SteelheadKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.SteelheadKill;
                }
                return count;
            }
        }
        public int FlyfishKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.FlyfishKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.FlyfishKill;
                }
                return count;
            }
        }
        public int SteelEelKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.SteelEelKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.SteelEelKill;
                }
                return count;
            }
        }
        public int DrizzlerKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.DrizzlerKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.DrizzlerKill;
                }
                return count;
            }
        }
        public int StingerKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.StingerKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.StingerKill;
                }
                return count;
            }
        }
        public int ScrapperKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.ScrapperKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.ScrapperKill;
                }
                return count;
            }
        }
        public int MawsKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.MawsKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.MawsKill;
                }
                return count;
            }
        }
        public int GrillerKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.GrillerKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.GrillerKill;
                }
                return count;
            }
        }
        public int GoldieKill
        {
            get
            {
                int count = 0;
                count += MyPlayer.GoldieKill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count += player.GoldieKill;
                }
                return count;
            }
        }
        public int BossKill
        {
            get
            {
                return SteelheadKill + FlyfishKill + SteelEelKill + DrizzlerKill + StingerKill + ScrapperKill + MawsKill + GrillerKill + GoldieKill;
            }
        }

        public Job(int number, DateTime startTime, double hazardLevel, ShiftStage stage, List<Wave> waves, JobPlayer myPlayer, List<JobPlayer> otherPlayers,
            int steelheadCount, int flyfishCount, int steelEelCount, int drizzlerCount, int stingerCount, int scrapperCount, int mawsCount, int grillerCount, int goldieCount,
            int score, int gradePointDelta, ResultType result)
        {
            Number = number;
            StartTime = startTime;
            HazardLevel = hazardLevel;
            Stage = stage;
            Waves = waves;
            MyPlayer = myPlayer;
            OtherPlayers = otherPlayers;
            SteelheadCount = steelheadCount;
            FlyfishCount = flyfishCount;
            SteelEelCount = steelEelCount;
            DrizzlerCount = drizzlerCount;
            StingerCount = stingerCount;
            ScrapperCount = scrapperCount;
            MawsCount = mawsCount;
            GrillerCount = grillerCount;
            GoldieCount = goldieCount;
            Score = score;
            GradePointDelta = gradePointDelta;
            Result = result;
        }
        public Job()
        {
            Number = -1;
            StartTime = new DateTime(0);
            HazardLevel = -1;
            Waves = new List<Wave>();
            OtherPlayers = new List<JobPlayer>();
            SteelheadCount = -1;
            FlyfishCount = -1;
            SteelEelCount = -1;
            DrizzlerCount = -1;
            StingerCount = -1;
            ScrapperCount = -1;
            MawsCount = -1;
            GrillerCount = -1;
            GoldieCount = -1;
            Score = -1;
            GradePointDelta = -1;
            Result = ResultType.clear;
        }
        public Job(ErrorType error) : base(error)
        {
            Number = -1;
            StartTime = new DateTime(0);
            HazardLevel = -1;
            Waves = new List<Wave>();
            OtherPlayers = new List<JobPlayer>();
            SteelheadCount = -1;
            FlyfishCount = -1;
            SteelEelCount = -1;
            DrizzlerCount = -1;
            StingerCount = -1;
            ScrapperCount = -1;
            MawsCount = -1;
            GrillerCount = -1;
            GoldieCount = -1;
            Score = -1;
            GradePointDelta = -1;
            Result = ResultType.clear;
        }

        public static ResultType ParseResultType(string s)
        {
            return (ResultType)Enum.Parse(typeof(ResultType), s);
        }
    }
}
