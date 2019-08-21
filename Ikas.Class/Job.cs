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
        public List<SalmoniodCount> SalmoniodAppearances { get; }
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
        public Grade.Key Grade
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
        public int Appearance
        {
            get
            {
                int count = 0;
                foreach (SalmoniodCount salmoniod in SalmoniodAppearances)
                {
                    count = count + salmoniod.Count;
                }
                return count;
            }
        }
        public int Kill
        {
            get
            {
                int count = 0;
                count = count + MyPlayer.Kill;
                foreach (JobPlayer player in OtherPlayers)
                {
                    count = count + player.Kill;
                }
                return count;
            }
        }

        public Job(int number, DateTime startTime, double hazardLevel, ShiftStage stage, List<Wave> waves, JobPlayer myPlayer, List<JobPlayer> otherPlayers, List<SalmoniodCount> salmoniodAppearances, int score, int gradePointDelta, ResultType result)
        {
            Number = number;
            StartTime = startTime;
            HazardLevel = hazardLevel;
            Stage = stage;
            Waves = waves;
            MyPlayer = myPlayer;
            OtherPlayers = otherPlayers;
            SalmoniodAppearances = salmoniodAppearances;
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
            SalmoniodAppearances = new List<SalmoniodCount>();
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
            SalmoniodAppearances = new List<SalmoniodCount>();
            Score = -1;
            GradePointDelta = -1;
            Result = ResultType.clear;
        }

        public int GetSalmoniodKill(Salmoniod.Key id)
        {
            int count = 0;
            count = count + MyPlayer.SalmoniodKills.Find(p => p.Salmoniod == id).Count;
            foreach (JobPlayer player in OtherPlayers)
            {
                count = count + player.SalmoniodKills.Find(p => p.Salmoniod == id).Count;
            }
            return count;
        }

        public static ResultType ParseResultType(string s)
        {
            return (ResultType)Enum.Parse(typeof(ResultType), s);
        }
    }
}
