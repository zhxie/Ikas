using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Battle
    {
        public Mode.Key Type { get; }
        public Mode.Key Mode { get; }
        public Rule.Key Rule { get; }
        public Stage Stage { get; }
        public List<Player> MyPlayers { get; }
        public List<Player> OtherPlayers { get; }
        public double MyScore { get; }
        public double OtherScore { get; }

        public bool IsWin
        {
            get
            {
                return MyScore > OtherScore;
            }
        }

        public Battle(Mode.Key type, Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, double myScore, double otherScore)
        {
            Type = type;
            Mode = mode;
            Rule = rule;
            Stage = stage;
            MyPlayers = myPlayers;
            OtherPlayers = otherPlayers;
            MyScore = myScore;
            OtherScore = otherScore;
        }
        public Battle()
        {
            MyPlayers = new List<Player>();
            OtherPlayers = new List<Player>();
            MyScore = -1;
            OtherScore = -1;
        }
    }

    public class RegularBattle : Battle
    {
        public RegularBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, double myScore, double otherScore)
            : base(ClassLib.Mode.Key.regular_battle, mode, rule, stage, myPlayers, otherPlayers, myScore, otherScore)
        {

        }
    }

    public class RankedBattle : Battle
    {
        public double EstimatedRankPower { get; }

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

        public RankedBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers,
            double estimatedRankPower, double myScore, double otherScore)
            :base(ClassLib.Mode.Key.ranked_battle, mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), myScore, otherScore)
        {
            EstimatedRankPower = estimatedRankPower;
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

        public RankedXBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedXPlayer> myPlayers, List<RankedXPlayer> otherPlayers,
            double estimatedXPower, double xPower, double myScore, double otherScore)
            : base(mode, rule, stage, myPlayers.Cast<RankedPlayer>().ToList(), otherPlayers.Cast<RankedPlayer>().ToList(), estimatedXPower, myScore, otherScore)
        {
            
        }
    }

    public class LeagueBattle : Battle
    {
        public int MyEstimatedLeaguePower { get; }
        public int OtherEstimatedLeaguePower { get; }
        public double LeaguePoint { get; }
        public double MaxLeaguePoint { get; }

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

        public LeagueBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers,
            int myEstimatedLeaguePower, int otherEstimatedLeaguePower, double leaguePoint, double maxLeaguePoint, double myScore, double otherScore)
            : base(ClassLib.Mode.Key.league_battle, mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), myScore, otherScore)
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

        public SplatfestBattle(Mode.Key mode, Key splatfestMode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers,
            int myEstimatedSplatfestPower, int otherEstimatedSplatfestPower, double splatfestPower, double maxSplatfestPower, int contributionPoint, int totalContributionPoint, double myScore, double otherScore)
            :base(ClassLib.Mode.Key.splatfest, mode, rule, stage, myPlayers, otherPlayers, myScore, otherScore)
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
