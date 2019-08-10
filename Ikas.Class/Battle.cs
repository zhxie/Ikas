using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ikas.Class
{
    public class Battle : Base
    {
        public int Number { get; }
        public DateTime StartTime { get; }
        public double ElapsedTime { get; }
        public Mode.Key Type { get; }
        public Mode.Key Mode { get; }
        public Rule.Key Rule { get; }
        public Stage Stage { get; }
        public List<Player> MyPlayers { get; }
        public List<Player> OtherPlayers { get; }
        public int LevelAfter { get; }
        public double MyScore { get; }
        public double OtherScore { get; }

        public Player SelfPlayer
        {
            get
            {
                return MyPlayers.Find(p => p.IsSelf);
            }
        }
        public double MyAveragePaint
        {
            get
            {
                if (MyPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in MyPlayers)
                    {
                        average = average + player.Paint;
                    }
                    return average / MyPlayers.Count;
                }
            }
        }
        public double MyAverageKillAndAssist
        {
            get
            {
                if (MyPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in MyPlayers)
                    {
                        average = average + player.KillAndAssist;
                    }
                    return average / MyPlayers.Count;
                }
            }
        }
        public double MyAverageAssist
        {
            get
            {
                if (MyPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in MyPlayers)
                    {
                        average = average + player.Assist;
                    }
                    return average / MyPlayers.Count;
                }
            }
        }
        public double MyAverageDeath
        {
            get
            {
                if (MyPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in MyPlayers)
                    {
                        average = average + player.Death;
                    }
                    return average / MyPlayers.Count;
                }
            }
        }
        public double MyAverageSpecial
        {
            get
            {
                if (MyPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in MyPlayers)
                    {
                        average = average + player.Special;
                    }
                    return average / MyPlayers.Count;
                }
            }
        }
        public double OtherAveragePaint
        {
            get
            {
                if (OtherPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in OtherPlayers)
                    {
                        average = average + player.Paint;
                    }
                    return average / OtherPlayers.Count;
                }
            }
        }
        public double OtherAverageKillAndAssist
        {
            get
            {
                if (OtherPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in OtherPlayers)
                    {
                        average = average + player.KillAndAssist;
                    }
                    return average / OtherPlayers.Count;
                }
            }
        }
        public double OtherAverageAssist
        {
            get
            {
                if (OtherPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in OtherPlayers)
                    {
                        average = average + player.Assist;
                    }
                    return average / OtherPlayers.Count;
                }
            }
        }
        public double OtherAverageDeath
        {
            get
            {
                if (OtherPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in OtherPlayers)
                    {
                        average = average + player.Death;
                    }
                    return average / OtherPlayers.Count;
                }
            }
        }
        public double OtherAverageSpecial
        {
            get
            {
                if (OtherPlayers.Count == 0)
                {
                    return 0;
                }
                else
                {
                    double average = 0;
                    foreach (Player player in OtherPlayers)
                    {
                        average = average + player.Special;
                    }
                    return average / OtherPlayers.Count;
                }
            }
        }
        public double ScoreRatio
        {
            get
            {
                if (MyScore == 0 && OtherScore == 0)
                {
                    return 0.5;
                }
                else
                {
                    return MyScore / (MyScore + OtherScore);
                }
            }
        }

        public bool IsWin
        {
            get
            {
                return MyScore > OtherScore;
            }
        }

        public Battle(int number, DateTime startTime, double elapsedTime, Mode.Key type, Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, int levelAfter, double myScore, double otherScore)
        {
            Number = number;
            StartTime = startTime;
            ElapsedTime = elapsedTime;
            Type = type;
            Mode = mode;
            Rule = rule;
            Stage = stage;
            MyPlayers = myPlayers;
            OtherPlayers = otherPlayers;
            LevelAfter = levelAfter;
            MyScore = myScore;
            OtherScore = otherScore;
        }
        public Battle()
        {
            Number = -1;
            StartTime = new DateTime(0);
            ElapsedTime = -1;
            MyPlayers = new List<Player>();
            OtherPlayers = new List<Player>();
            LevelAfter = -1;
            MyScore = -1;
            OtherScore = -1;
        }
        public Battle(ErrorType error) : base(error)
        {
            Number = -1;
            StartTime = new DateTime(0);
            ElapsedTime = -1;
            MyPlayers = new List<Player>();
            OtherPlayers = new List<Player>();
            LevelAfter = -1;
            MyScore = -1;
            OtherScore = -1;
        }
    }

    public class RegularBattle : Battle
    {
        public enum FreshnessKey
        {
            dry,
            raw,
            fresh,
            superfresh,
            superfresh2,
            superfresh3
        }

        public double WinMeter { get; }
        public FreshnessKey Freshness
        {
            get
            {
                if (WinMeter < 5)
                {
                    return FreshnessKey.dry;
                }
                else if (WinMeter < 10)
                {
                    return FreshnessKey.raw;
                }
                else if (WinMeter < 15)
                {
                    return FreshnessKey.fresh;
                }
                else if (WinMeter < 20)
                {
                    return FreshnessKey.superfresh;
                }
                else if (WinMeter < 50)
                {
                    return FreshnessKey.superfresh2;
                }
                else
                {
                    return FreshnessKey.superfresh3;
                }
            }
        }

        public RegularBattle(int number, DateTime startTime, double elapsedTime, Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, int levelAfter,
            double winMeter, double myScore, double otherScore)
            : base(number, startTime, elapsedTime, Class.Mode.Key.regular_battle, mode, rule, stage, myPlayers, otherPlayers, levelAfter, myScore, otherScore)
        {
            WinMeter = winMeter;
        }
    }

    public class RankedBattle : Battle
    {
        public double EstimatedRankPower { get; }
        public Rank.Key RankAfter { get; }

        public bool IsKo
        {
            get
            {
                return MyScore == 100;
            }
        }
        public bool IsBeKoed
        {
            get
            {
                return OtherScore == 100;
            }
        }

        public RankedBattle(int number, DateTime startTime, double elapsedTime, Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, int levelAfter,
            double estimatedRankPower, Rank.Key rankAfter, double myScore, double otherScore)
            : base(number, startTime, elapsedTime, Ikas.Class.Mode.Key.ranked_battle, mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), levelAfter, myScore, otherScore)
        {
            EstimatedRankPower = estimatedRankPower;
            RankAfter = rankAfter;
        }
    }

    public class RankedXBattle : RankedBattle
    {
        public double EstimatedXPower
        {
            get
            {
                return EstimatedRankPower;
            }
        }
        public double XPowerAfter { get; }

        public RankedXBattle(int number, DateTime startTime, double elapsedTime, Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, int levelAfter,
            double estimatedXPower, double xPowerAfter, double myScore, double otherScore)
            : base(number, startTime, elapsedTime, mode, rule, stage, myPlayers.Cast<RankedPlayer>().ToList(), otherPlayers.Cast<RankedPlayer>().ToList(), levelAfter, estimatedXPower, Rank.Key.x, myScore, otherScore)
        {
            XPowerAfter = xPowerAfter;
        }
    }

    public class LeagueBattle : Battle
    {
        public int MyEstimatedLeaguePower { get; }
        public int OtherEstimatedLeaguePower { get; }
        public double LeaguePoint { get; }
        public double MaxLeaguePoint { get; }
        public bool IsCalculating
        {
            get
            {
                return LeaguePoint == 0;
            }
        }

        public bool IsKo
        {
            get
            {
                return MyScore == 100;
            }
        }
        public bool IsBeKoed
        {
            get
            {
                return OtherScore == 100;
            }
        }

        public LeagueBattle(int number, DateTime startTime, double elapsedTime, Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, int levelAfter,
            int myEstimatedLeaguePower, int otherEstimatedLeaguePower, double leaguePoint, double maxLeaguePoint, double myScore, double otherScore)
            : base(number, startTime, elapsedTime, Ikas.Class.Mode.Key.league_battle, mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), levelAfter, myScore, otherScore)
        {
            MyEstimatedLeaguePower = myEstimatedLeaguePower;
            OtherEstimatedLeaguePower = otherEstimatedLeaguePower;
            LeaguePoint = leaguePoint;
            MaxLeaguePoint = maxLeaguePoint;
        }
    }

    public class SplatfestBattle : Battle
    {
        public enum Key
        {
            regular,
            challenge
        }

        public Key SplatfestMode { get; }
        public int MyEstimatedSplatfestPower { get; }
        public int OtherEstimatedSplatfestPower { get; }
        public double SplatfestPower { get; }
        public double MaxSplatfestPower { get; }
        public int ContributionPoint { get; }
        public int TotalContributionPoint { get; }
        public bool IsCalculating
        {
            get
            {
                return SplatfestPower == 0;
            }
        }

        public SplatfestBattle(int number, DateTime startTime, double elapsedTime, Mode.Key mode, Key splatfestMode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, int levelAfter,
            int myEstimatedSplatfestPower, int otherEstimatedSplatfestPower, double splatfestPower, double maxSplatfestPower, int contributionPoint, int totalContributionPoint, double myScore, double otherScore)
            : base(number, startTime, elapsedTime, Ikas.Class.Mode.Key.splatfest, mode, rule, stage, myPlayers, otherPlayers, levelAfter, myScore, otherScore)
        {
            SplatfestMode = splatfestMode;
            MyEstimatedSplatfestPower = myEstimatedSplatfestPower;
            OtherEstimatedSplatfestPower = otherEstimatedSplatfestPower;
            SplatfestPower = splatfestPower;
            MaxSplatfestPower = maxSplatfestPower;
            ContributionPoint = contributionPoint;
            TotalContributionPoint = totalContributionPoint;
        }

        public static Key ParseKey(string s)
        {
            if (s.Contains("challenge"))
            {
                return Key.challenge;
            }
            if (s.Contains("regular"))
            {
                return Key.regular;
            }
            throw new FormatException();
        }
    }

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

        public Wave(WaterLevelType waterLevel, EventTypeType eventType, int quota, int powerEgg, int goldenEgg, int goldenEggPop)
        {
            WaterLevel = waterLevel;
            EventType = eventType;
            Quota = quota;
            PowerEgg = powerEgg;
            GoldenEgg = goldenEgg;
            GoldenEggPop = goldenEggPop;
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

    public class SalmonRunBattle : Base
    {
        public enum ResultType
        {
            clear,
            time_limit,
            wipe_out
        }

        public int Number { get; }
        public DateTime StartTime { get; }
        public double DangerRate { get; }
        public SalmonRunStage Stage { get; }
        public List<Wave> Waves { get; }
        public SalmonRunPlayer MyPlayer { get; }
        public List<SalmonRunPlayer> OtherPlayers { get; }
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
        public int FailureWave { get; }

        public bool IsClear
        {
            get
            {
                return Result == ResultType.clear;
            }
        }
        public SalmonRunPlayer.GradeType Grade
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
        public int BearPoint
        {
            get
            {
                return Rate / 100 * Score;
            }
        }

        public SalmonRunBattle(int number, DateTime startTime, double dangerRate, SalmonRunStage stage, List<Wave> waves, SalmonRunPlayer myPlayer, List<SalmonRunPlayer> otherPlayers,
            int steelheadCount, int flyfishCount, int steelEelCount, int drizzlerCount, int stingerCount, int scrapperCount, int mawsCount, int grillerCount, int goldieCount,
            int score, int gradePointDelta, ResultType result, int failureWave)
        {
            Number = number;
            StartTime = startTime;
            DangerRate = dangerRate;
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
            FailureWave = failureWave;
        }
        public SalmonRunBattle(ErrorType error) : base(error)
        {
            Number = -1;
            StartTime = new DateTime(0);
            DangerRate = -1;
            Waves = new List<Wave>();
            OtherPlayers = new List<SalmonRunPlayer>();
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
            FailureWave = -1;
        }

        public static ResultType ParseResultType(string s)
        {
            return (ResultType)Enum.Parse(typeof(ResultType), s);
        }
    }
}
