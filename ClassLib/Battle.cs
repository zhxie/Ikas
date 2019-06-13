using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class Battle
    {
        public Mode.Key Mode { get; }
        public Rule.Key Rule { get; }
        public Stage Stage { get; }
        public List<Player> MyPlayers { get; }
        public List<Player> OtherPlayers { get; }
        public double MyScore { get; }
        public double OtherScore { get; }

        public Battle(Mode.Key mode, Rule.Key rule, Stage stage, List<Player> myPlayers, List<Player> otherPlayers, double myScore, double otherScore)
        {
            Mode = mode;
            Rule = rule;
            Stage = stage;
            MyPlayers = myPlayers;
            OtherPlayers = otherPlayers;
            MyScore = myScore;
            OtherScore = otherScore;
        }
    }

    public class RankedBattle : Battle
    {
        public double EstimatedRankPower { get; }

        public RankedBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, double estimatedRankPower, double myScore, double otherScore)
            :base(mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), myScore, otherScore)
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
        public double XPower { get; }

        public RankedXBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, double estimatedXPower, double xPower, double myScore, double otherScore)
            : base(mode, rule, stage, myPlayers, otherPlayers, estimatedXPower, myScore, otherScore)
        {
            XPower = xPower;
        }
    }

    public class LeagueBattle : Battle
    {
        public double MyEstimatedLeaguePoint { get; }
        public double OtherEstimatedLeaguePoint { get; }
        public double LeaguePoint { get; }
        public double MaxLeaguePoint { get; }

        public LeagueBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, double myEstimatedLeaguePoint, double otherEstimatedLeaguePoint, double leaguePoint, double maxLeaguePoint, double myScore, double otherScore)
            : base(mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), myScore, otherScore)
        {
            MyEstimatedLeaguePoint = myEstimatedLeaguePoint;
            OtherEstimatedLeaguePoint = otherEstimatedLeaguePoint;
            LeaguePoint = leaguePoint;
            MaxLeaguePoint = maxLeaguePoint;
        }
    }

    public class PrivateBattle : Battle
    {
        public PrivateBattle(Mode.Key mode, Rule.Key rule, Stage stage, List<RankedPlayer> myPlayers, List<RankedPlayer> otherPlayers, double myScore, double otherScore)
            : base(mode, rule, stage, myPlayers.Cast<Player>().ToList(), otherPlayers.Cast<Player>().ToList(), myScore, otherScore)
        {
            
        }
    }
}
