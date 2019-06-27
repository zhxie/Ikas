using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Battle
    {
        public int Number { get; }
        public Mode.Key Type { get; }
        public Mode.Key Mode { get; }
        public Rule.Key Rule { get; }
        public Stage Stage { get; }
        public List<Player> MyPlayers { get; }
        public List<Player> OtherPlayers { get; }
        public int LevelAfter { get; }
        public double MyScore { get; }
        public double OtherScore { get; }

        public bool IsWin
        {
            get
            {
                return MyScore > OtherScore;
            }
        }

        public Battle(int number, Mode.Key type, Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, int levelAfter, double myScore, double otherScore)
        {
            Number = number;
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
        public Battle(int error)
        {
            Number = error;
            MyPlayers = new List<Player>();
            OtherPlayers = new List<Player>();
            LevelAfter = -1;
            MyScore = -1;
            OtherScore = -1;
        }
    }

    public class RegularBattle : Battle
    {
        public RegularBattle(int number, Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, int levelAfter, double myScore, double otherScore)
            : base(number, ClassLib.Mode.Key.regular_battle, mode, rule, stage, myPlayers, otherPlayers, levelAfter, myScore, otherScore)
        {

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

        public RankedBattle(int number, Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, int levelAfter,
            double estimatedRankPower, Rank.Key rankAfter, double myScore, double otherScore)
            :base(number, ClassLib.Mode.Key.ranked_battle, mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), levelAfter, myScore, otherScore)
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

        public RankedXBattle(int number, Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, int levelAfter,
            double estimatedXPower, double xPowerAfter, double myScore, double otherScore)
            : base(number, mode, rule, stage, myPlayers.Cast<RankedPlayer>().ToList(), otherPlayers.Cast<RankedPlayer>().ToList(), levelAfter, estimatedXPower, Rank.Key.x, myScore, otherScore)
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

        public LeagueBattle(int number, Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, int levelAfter,
            int myEstimatedLeaguePower, int otherEstimatedLeaguePower, double leaguePoint, double maxLeaguePoint, double myScore, double otherScore)
            : base(number, ClassLib.Mode.Key.league_battle, mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), levelAfter, myScore, otherScore)
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

        public SplatfestBattle(int number, Mode.Key mode, Key splatfestMode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, int levelAfter,
            int myEstimatedSplatfestPower, int otherEstimatedSplatfestPower, double splatfestPower, double maxSplatfestPower, int contributionPoint, int totalContributionPoint, double myScore, double otherScore)
            :base(number, ClassLib.Mode.Key.splatfest, mode, rule, stage, myPlayers, otherPlayers, levelAfter, myScore, otherScore)
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

}
