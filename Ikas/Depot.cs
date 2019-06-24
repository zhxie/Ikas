using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json.Linq;

using ClassLib;

namespace Ikas
{
    public delegate void AlwaysOnTopChangedEventHandler();
    public delegate void LanguageChangedEventHandler();
    public delegate void ScheduleChangedEventHandler();
    public delegate void ScheduleUpdatedEventHandler();
    public delegate void ScheduleFailedEventHandler();
    public delegate void BattleUpdatedEventHandler();
    public delegate void BattleChangedEventHandler();
    public delegate void BattleFailedEventHandler();
    public static class Depot
    {
        private static string userConfigurationPath = "";
        private static IniData userIniData = new IniData();
        public static string SessionToken
        {
            get
            {
                try
                {
                    return userIniData[FileFolderUrl.UserConfigurationGeneralSection][FileFolderUrl.UserConfigurationSessionToken];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != SessionToken)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationGeneralSection][FileFolderUrl.UserConfigurationSessionToken] = value;
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static string Cookie
        {
            get
            {
                try
                {
                    return userIniData[FileFolderUrl.UserConfigurationGeneralSection][FileFolderUrl.UserConfigurationCookie];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != Cookie)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationGeneralSection][FileFolderUrl.UserConfigurationCookie] = value;
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        private static int level
        {
            set
            {
                if (value != Level)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationLevel] = value.ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static int Level
        {
            get
            {
                try
                {
                    return int.Parse(userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationLevel]);
                }
                catch
                {
                    return -1;
                }
            }
        }
        private static Rank.Key splatZonesRank
        {
            set
            {
                if (value != SplatZonesRank)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationSplatZonesRank] = ((int)value).ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static Rank.Key SplatZonesRank {
            get
            {
                try
                {
                    return (Rank.Key)int.Parse(userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationSplatZonesRank]);
                }
                catch
                {
                    return Rank.Key.rank_unknown;
                }
            }
        }
        private static Rank.Key towerControlRank
        {
            set
            {
                if (value != TowerControlRank)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationTowerControlRank] = ((int)value).ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static Rank.Key TowerControlRank
        {
            get
            {
                try
                {
                    return (Rank.Key)int.Parse(userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationTowerControlRank]);
                }
                catch
                {
                    return Rank.Key.rank_unknown;
                }
            }
        }
        private static Rank.Key rainmakerRank
        {
            set
            {
                if (value != RainmakerRank)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationRainmakerRank] = ((int)value).ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static Rank.Key RainmakerRank
        {
            get
            {
                try
                {
                    return (Rank.Key)int.Parse(userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationRainmakerRank]);
                }
                catch
                {
                    return Rank.Key.rank_unknown;
                }
            }
        }
        private static Rank.Key clamBlitzRank
        {
            set
            {
                if (value != ClamBlitzRank)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationClamBlitzRank] = ((int)value).ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static Rank.Key ClamBlitzRank
        {
            get
            {
                try
                {
                    return (Rank.Key)int.Parse(userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationClamBlitzRank]);
                }
                catch
                {
                    return Rank.Key.rank_unknown;
                }
            }
        }

        private static IniData systemIniData = new IniData();
        public static event AlwaysOnTopChangedEventHandler AlwaysOnTopChanged;
        public static bool AlwaysOnTop
        {
            get
            {
                try
                {
                    return bool.Parse(systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationAlwaysOnTop]);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                if (value != AlwaysOnTop)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationAlwaysOnTop] = value.ToString().ToLower();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                    finally
                    {
                        AlwaysOnTopChanged?.Invoke();
                    }
                }
            }
        }
        public static bool UseProxy
        {
            get
            {
                try
                {
                    return bool.Parse(systemIniData[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxy]);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                if (value != UseProxy)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxy] = value.ToString().ToLower();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static string ProxyHost
        {
            get
            {
                try
                {
                    return systemIniData[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxyHost];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != ProxyHost)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxyHost] = value;
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static int ProxyPort
        {
            get
            {
                try
                {
                    return int.Parse(systemIniData[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxyPort].Trim());
                }
                catch
                {
                    return -1;
                }
            }
            set
            {
                if (value != ProxyPort)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxyPort] = value.ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static WebProxy Proxy
        {
            get
            {
                try
                {
                    if (UseProxy)
                    {
                        if (ProxyHost != "" && ProxyPort > 0)
                        {
                            return new WebProxy(ProxyHost, ProxyPort);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        public static event LanguageChangedEventHandler LanguageChanged;
        public static string Language
        {
            get
            {
                try
                {
                    return systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationLanguage];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value != Language)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationLanguage] = value;
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                    finally
                    {
                        LanguageChanged?.Invoke();
                    }
                }
            }
        }

        private static string authState = "";
        private static string authCodeChallenge = "";
        private static string authCodeVerifier = "";

        public static DownloadManager DownloadManager { get; } = new DownloadManager();

        public static event ScheduleChangedEventHandler ScheduleChanged;
        public static event ScheduleUpdatedEventHandler ScheduleUpdated;
        public static int ScheduleFailedCount { get; set; } = 0;
        public static event ScheduleFailedEventHandler ScheduleFailed;
        private static Mutex ScheduleMutex = new Mutex();
        public static Schedule Schedule { get; set; } = new Schedule();

        public static event BattleChangedEventHandler BattleChanged;
        public static event BattleUpdatedEventHandler BattleUpdated;
        public static int BattleFailedCount { get; set; } = 0;
        public static event BattleFailedEventHandler BattleFailed;
        private static Mutex BattleMutex = new Mutex();
        public static Battle Battle { get; set; } = new Battle();

        private static Mode.Key currentMode = Mode.Key.regular_battle;
        public static Mode.Key CurrentMode
        {
            get
            {
                return currentMode;
            }
            set
            {
                if ((int)value > 2)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (CurrentMode != value)
                {
                    currentMode = value;
                }
                // Raise event
                ScheduleChanged?.Invoke();
                // Update schedule
                GetSchedule();
            }
        }

        /// <summary>
        /// Load user configuration from a file.
        /// </summary>
        /// <param name="file">The file of the user, the default one is user.ini</param>
        /// <returns></returns>
        public static bool LoadUserConfiguration(string file = FileFolderUrl.UserConfiguration)
        {
            userConfigurationPath = file;
            if (!File.Exists(userConfigurationPath))
            {
                userConfigurationPath = System.Environment.CurrentDirectory + userConfigurationPath;
                if (!File.Exists(userConfigurationPath))
                {
                    return false;
                }
            }
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                userIniData = parser.ReadFile(userConfigurationPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Load system configuration from config.ini.
        /// </summary>
        /// <returns></returns>
        public static bool LoadSystemConfiguration()
        {
            if (!File.Exists(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration))
            {
                return false;
            }
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                systemIniData = parser.ReadFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Get current and next Schedule in regular, ranked and league mode.
        /// </summary>
        public static async void GetSchedule()
        {
            // Remove previous Downloader's handlers
            DownloadManager.RemoveDownloaders(Downloader.SourceType.Schedule);
            // Send HTTP GET
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + FileFolderUrl.SplatNetScheduleApi);
            request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                DateTime endTime = new DateTime();
                List<ScheduledStage> stages = new List<ScheduledStage>(), nextStages = new List<ScheduledStage>();
                try
                {
                    // Regular
                    List<ScheduledStage> regularStages = parseScheduledStages(jObject["regular"][0]);
                    List<ScheduledStage> nextRegularStages = parseScheduledStages(jObject["regular"][1]);
                    // Ranked
                    List<ScheduledStage> rankedStages = parseScheduledStages(jObject["gachi"][0]);
                    List<ScheduledStage> nextRankedStages = parseScheduledStages(jObject["gachi"][1]);
                    // League
                    List<ScheduledStage> leagueStages = parseScheduledStages(jObject["league"][0]);
                    List<ScheduledStage> nextLeagueStages = parseScheduledStages(jObject["league"][1]);
                    // Create Schedule
                    endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(int.Parse(jObject["regular"][0]["end_time"].ToString()));
                    stages.AddRange(regularStages);
                    stages.AddRange(rankedStages);
                    stages.AddRange(leagueStages);
                    nextStages.AddRange(nextRegularStages);
                    nextStages.AddRange(nextRankedStages);
                    nextStages.AddRange(nextLeagueStages);
                }
                catch
                {
                    // Update Schedule on error
                    UpdateSchedule(new Schedule());
                    return;
                }
                // Update Schedule
                UpdateSchedule(new Schedule(endTime, stages, nextStages));
            }
            else
            {
                // Update Schedule on error
                UpdateSchedule(new Schedule());
            }
        }
        /// <summary>
        /// Update Schedule.
        /// </summary>
        /// <param name="schedule">Updated Schedule</param>
        /// <returns></returns>
        private static bool UpdateSchedule(Schedule schedule)
        {
            if (schedule.EndTime == new DateTime(0))
            {
                ScheduleFailedCount++;
                ScheduleFailed?.Invoke();
            }
            if (Schedule != schedule)
            {
                ScheduleMutex.WaitOne();
                Schedule = schedule;
                ScheduleMutex.ReleaseMutex();
                // Raise event
                ScheduleUpdated?.Invoke();
                return true;
            }
            else
            {
                // Raise event
                ScheduleUpdated?.Invoke();
                return false;
            }
        }

        /// <summary>
        /// Get last battle result.
        /// </summary>
        public static async void GetLastBattle()
        {
            // Raise event
            BattleChanged?.Invoke();
            // Remove previous Downloader's handlers
            DownloadManager.RemoveDownloaders(Downloader.SourceType.Battle);
            // Send HTTP GET
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + FileFolderUrl.SplatNetBattleApi);
            request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                int battleNumber;
                try
                {
                    battleNumber = int.Parse(jObject["results"][0]["battle_number"].ToString());
                }
                catch
                {
                    // Update Battle on error
                    UpdateBattle(new Battle());
                    return;
                }
                // Same battle
                if (battleNumber == Battle.Number)
                {
                    // Update same Battle
                    UpdateBattle(Battle);
                    return;
                }
                // Send HTTP GET
                request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + string.Format(FileFolderUrl.SplatNetIndividualBattleApi, battleNumber));
                request.Headers.Add("Cookie", "iksm_session=" + Cookie);
                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                    // Parse JSON
                    jObject = JObject.Parse(resultString);
                    try
                    {
                        Mode.Key type = Mode.ParseKey(jObject["type"].ToString());
                        Mode.Key mode = Mode.ParseGameModeKey(jObject["game_mode"]["key"].ToString());
                        Rule.Key rule = Rule.ParseKey(jObject["rule"]["key"].ToString());
                        Stage stage = new Stage((Stage.Key)int.Parse(jObject["stage"]["id"].ToString()), jObject["stage"]["image"].ToString());
                        switch (type)
                        {
                            case Mode.Key.regular_battle:
                                {
                                    // Self
                                    Player selfPlayer = await parsePlayer(jObject["player_result"], true);
                                    // My team players
                                    List<Player> myPlayers = new List<Player>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<Player>> myPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in myPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<Player> playerTask = parsePlayer(playerNode);
                                            myPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(myPlayerTasks);
                                    foreach (Task<Player> playerTask in myPlayerTasks)
                                    {
                                        myPlayers.Add(playerTask.Result);
                                    }
                                    myPlayers.Add(selfPlayer);
                                    myPlayers = sortPlayer(myPlayers, rule);
                                    // Other team players
                                    List<Player> otherPlayers = new List<Player>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<Player>> otherPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in otherPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<Player> playerTask = parsePlayer(playerNode);
                                            otherPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(otherPlayerTasks);
                                    foreach (Task<Player> playerTask in otherPlayerTasks)
                                    {
                                        otherPlayers.Add(playerTask.Result);
                                    }
                                    otherPlayers = sortPlayer(otherPlayers, rule);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    double myScore = double.Parse(jObject["my_team_percentage"].ToString());
                                    double otherScore = double.Parse(jObject["other_team_percentage"].ToString());
                                    UpdateBattle(new RegularBattle(battleNumber, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                        myScore, otherScore) as Battle);
                                }
                                break;
                            case Mode.Key.ranked_battle:
                                {
                                    // Self
                                    RankedPlayer selfPlayer = await parseRankedPlayer(jObject["player_result"], true);
                                    // My team players
                                    List<RankedPlayer> myPlayers = new List<RankedPlayer>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<RankedPlayer>> myPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in myPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<RankedPlayer> playerTask = parseRankedPlayer(playerNode);
                                            myPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(myPlayerTasks);
                                    foreach (Task<RankedPlayer> playerTask in myPlayerTasks)
                                    {
                                        myPlayers.Add(playerTask.Result);
                                    }
                                    myPlayers.Add(selfPlayer);
                                    myPlayers = sortPlayer(myPlayers, rule);
                                    // Other team players
                                    List<RankedPlayer> otherPlayers = new List<RankedPlayer>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<RankedPlayer>> otherPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in otherPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<RankedPlayer> playerTask = parseRankedPlayer(playerNode);
                                            otherPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(otherPlayerTasks);
                                    foreach (Task<RankedPlayer> playerTask in otherPlayerTasks)
                                    {
                                        otherPlayers.Add(playerTask.Result);
                                    }
                                    otherPlayers = sortPlayer(otherPlayers, rule);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    int myScore = int.Parse(jObject["my_team_count"].ToString());
                                    int otherScore = int.Parse(jObject["other_team_count"].ToString());
                                    if (!jObject["x_power"].HasValues)
                                    {
                                        // Ranked Battle
                                        int estimatedRankPower = int.Parse(jObject["estimate_gachi_power"].ToString());
                                        Rank.Key rankAfter;
                                        if (jObject["udemae"]["s_plus_number"].HasValues)
                                        {
                                            rankAfter = Rank.ParseKey(jObject["udemae"]["name"].ToString(), int.Parse(jObject["udemae"]["s_plus_number"].ToString()));
                                        }
                                        else
                                        {
                                            rankAfter = Rank.ParseKey(jObject["udemae"]["name"].ToString());
                                        }
                                        switch (rule)
                                        {
                                            case Rule.Key.splat_zones:
                                                if (rankAfter != SplatZonesRank)
                                                {
                                                    UpdateRank(Rule.Key.splat_zones, rankAfter);
                                                }
                                                break;
                                            case Rule.Key.tower_control:
                                                if (rankAfter != TowerControlRank)
                                                {
                                                    UpdateRank(Rule.Key.tower_control, rankAfter);
                                                }
                                                break;
                                            case Rule.Key.rainmaker:
                                                if (rankAfter != RainmakerRank)
                                                {
                                                    UpdateRank(Rule.Key.rainmaker, rankAfter);
                                                }
                                                break;
                                            case Rule.Key.clam_blitz:
                                                if (rankAfter != ClamBlitzRank)
                                                {
                                                    UpdateRank(Rule.Key.clam_blitz, rankAfter);
                                                }
                                                break;
                                            default:
                                                throw new ArgumentOutOfRangeException();
                                        }
                                        UpdateBattle(new RankedBattle(battleNumber, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                            estimatedRankPower, rankAfter, myScore, otherScore) as Battle);
                                    }
                                    else
                                    {
                                        // Ranked X Battle
                                        int estimatedXPower = int.Parse(jObject["estimate_x_power"].ToString());
                                        double xPowerAfter = double.Parse(jObject["x_power"].ToString());
                                        UpdateBattle(new RankedXBattle(battleNumber, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                            estimatedXPower, xPowerAfter, myScore, otherScore) as Battle);
                                    }
                                }
                                break;
                            case Mode.Key.league_battle:
                                {
                                    // Self
                                    RankedPlayer selfPlayer = await parseRankedPlayer(jObject["player_result"], true);
                                    // My team players
                                    List<RankedPlayer> myPlayers = new List<RankedPlayer>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<RankedPlayer>> myPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in myPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<RankedPlayer> playerTask = parseRankedPlayer(playerNode);
                                            myPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(myPlayerTasks);
                                    foreach (Task<RankedPlayer> playerTask in myPlayerTasks)
                                    {
                                        myPlayers.Add(playerTask.Result);
                                    }
                                    myPlayers.Add(selfPlayer);
                                    myPlayers = sortPlayer(myPlayers, rule);
                                    // Other team players
                                    List<RankedPlayer> otherPlayers = new List<RankedPlayer>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<RankedPlayer>> otherPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in otherPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<RankedPlayer> playerTask = parseRankedPlayer(playerNode);
                                            otherPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(otherPlayerTasks);
                                    foreach (Task<RankedPlayer> playerTask in otherPlayerTasks)
                                    {
                                        otherPlayers.Add(playerTask.Result);
                                    }
                                    otherPlayers = sortPlayer(otherPlayers, rule);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    int myEstimatedLeaguePower = int.Parse(jObject["my_estimate_league_point"].ToString());
                                    int otherEstimatedLeaguePower = int.Parse(jObject["other_estimate_league_point"].ToString());
                                    double leaguePoint;
                                    if (jObject["league_point"].HasValues)
                                    {
                                        leaguePoint = double.Parse(jObject["league_point"].ToString());
                                    }
                                    else
                                    {
                                        leaguePoint = 0;
                                    }
                                    int myScore = int.Parse(jObject["my_team_count"].ToString());
                                    int otherScore = int.Parse(jObject["other_team_count"].ToString());
                                    double maxLeaguePoint = double.Parse(jObject["max_league_point"].ToString());
                                    UpdateBattle(new LeagueBattle(battleNumber, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                        myEstimatedLeaguePower, otherEstimatedLeaguePower, leaguePoint, maxLeaguePoint, myScore, otherScore) as Battle);
                                }
                                break;
                            case Mode.Key.splatfest:
                                {
                                    // Self
                                    Player selfPlayer = await parsePlayer(jObject["player_result"], true);
                                    // My team players
                                    List<Player> myPlayers = new List<Player>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<Player>> myPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in myPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<Player> playerTask = parsePlayer(playerNode);
                                            myPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(myPlayerTasks);
                                    foreach (Task<Player> playerTask in myPlayerTasks)
                                    {
                                        myPlayers.Add(playerTask.Result);
                                    }
                                    myPlayers.Add(selfPlayer);
                                    myPlayers = sortPlayer(myPlayers, rule);
                                    // Other team players
                                    List<Player> otherPlayers = new List<Player>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<Player>> otherPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in otherPlayersNode.Children())
                                    {
                                        if (playerNode.HasValues)
                                        {
                                            Task<Player> playerTask = parsePlayer(playerNode);
                                            otherPlayerTasks.Add(playerTask);
                                        }
                                    }
                                    await Task.WhenAll(otherPlayerTasks);
                                    foreach (Task<Player> playerTask in otherPlayerTasks)
                                    {
                                        otherPlayers.Add(playerTask.Result);
                                    }
                                    otherPlayers = sortPlayer(otherPlayers, rule);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    SplatfestBattle.Key splatfestMode = SplatfestBattle.ParseKey(jObject["fes_mode"]["key"].ToString());
                                    int myEstimatedSplatfestPower = int.Parse(jObject["my_estimate_fes_power"].ToString());
                                    int otherEstimatedSplatfestPower = int.Parse(jObject["other_estimate_fes_power"].ToString());
                                    double splatfestPower = double.Parse(jObject["fes_power"].ToString());
                                    double maxSplatfestPower = double.Parse(jObject["max_fes_power"].ToString());
                                    int contributionPoint = int.Parse(jObject["contribution_point"].ToString());
                                    int totalContributionPoint = int.Parse(jObject["contribution_point_total"].ToString());
                                    double myScore = double.Parse(jObject["my_team_percentage"].ToString());
                                    double otherScore = double.Parse(jObject["other_team_percentage"].ToString());
                                    UpdateBattle(new SplatfestBattle(battleNumber, mode, splatfestMode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                        myEstimatedSplatfestPower, otherEstimatedSplatfestPower, splatfestPower, maxSplatfestPower, contributionPoint, totalContributionPoint, myScore, otherScore) as Battle);
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch
                    {
                        // Update Battle on error
                        UpdateBattle(new Battle());
                        return;
                    }
                }
                else
                {
                    // Update Battle on error
                    UpdateBattle(new Battle());
                }
            }
            else
            {
                // Update Battle on error
                UpdateBattle(new Battle());
            }
        }
        /// <summary>
        ///  Update Battle.
        /// </summary>
        /// <param name="battle">Updated Battle</param>
        /// <returns></returns>
        private static bool UpdateBattle(Battle battle)
        {
            if (battle.Stage == null)
            {
                BattleFailedCount++;
                BattleFailed?.Invoke();
            }
            if (Battle != battle)
            {
                BattleMutex.WaitOne();
                Battle = battle;
                BattleMutex.ReleaseMutex();
                // Raise event
                BattleUpdated?.Invoke();
                return true;
            }
            else
            {
                // Raise event
                BattleUpdated?.Invoke();
                return false;
            }
        }

        /// <summary>
        /// Update level.
        /// </summary>
        /// <param name="level">Updated level</param>
        private static bool UpdateLevel(int level)
        {
            if (Level != level)
            {
                try
                {
                    Depot.level = level;
                    CurrentMode = currentMode;
                    return true;
                }
                catch
                {
                    CurrentMode = currentMode;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Update rank.
        /// </summary>
        /// <param name="rule">Rule of the rank</param>
        /// <param name="rank">Updated rank</param>
        /// <returns></returns>
        private static bool UpdateRank(Rule.Key rule, Rank.Key rank)
        {
            switch (rule)
            {
                case Rule.Key.splat_zones:
                    if (SplatZonesRank != rank)
                    {
                        try
                        {
                            splatZonesRank = rank;
                            CurrentMode = currentMode;
                            return true;
                        }
                        catch
                        {
                            CurrentMode = currentMode;
                            return false;
                        }
                    }
                    break;
                case Rule.Key.tower_control:
                    if (TowerControlRank != rank)
                    {
                        try
                        {
                            towerControlRank = rank;
                            CurrentMode = currentMode;
                            return true;
                        }
                        catch
                        {
                            CurrentMode = currentMode;
                            return false;
                        }
                    }
                    break;
                case Rule.Key.rainmaker:
                    if (RainmakerRank != rank)
                    {
                        try
                        {
                            rainmakerRank = rank;
                            CurrentMode = currentMode;
                            return true;
                        }
                        catch
                        {
                            CurrentMode = currentMode;
                            return false;
                        }
                    }
                    break;
                case Rule.Key.clam_blitz:
                    if (ClamBlitzRank != rank)
                    {
                        try
                        {
                            clamBlitzRank = rank;
                            CurrentMode = currentMode;
                            return true;
                        }
                        catch
                        {
                            CurrentMode = currentMode;
                            return false;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }

        /// <summary>
        /// Get icon of a user.
        /// </summary>
        /// <param name="id">Id of user</param>
        public static async Task<string> GetPlayerIcon(string id)
        {
            // Remove previous Downloader's handlers
            DownloadManager.RemoveDownloaders(Downloader.SourceType.Battle);
            // Send HTTP GET
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + string.Format(FileFolderUrl.SplatNetNicknameAndIconApi, id));
            request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string icon;
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                try
                {
                    icon = jObject["nickname_and_icons"][0]["thumbnail_url"].ToString();
                }
                catch
                {
                    return "";
                }
                // Update icon
                return icon;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Log in to Nintendo
        /// </summary>
        /// <returns></returns>
        public static string LogIn()
        {
            // Auth state
            byte[] authStateBytesRaw = new byte[36];
            (new RNGCryptoServiceProvider()).GetBytes(authStateBytesRaw);
            string authState = ToUrlSafeBase64String(authStateBytesRaw);
            // Auth code verifier
            byte[] authCodeVerifierBytesRaw = new byte[32];
            (new RNGCryptoServiceProvider()).GetBytes(authCodeVerifierBytesRaw);
            string authCodeVerifierRare = ToUrlSafeBase64String(authCodeVerifierBytesRaw);
            byte[] authCodeVerifier = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(authCodeVerifierRare.Replace("=", "")));
            string authCodeChallenge = ToUrlSafeBase64String(authCodeVerifier).Replace("=", "");
            Depot.authState = authState;
            Depot.authCodeChallenge = authCodeChallenge;
            Depot.authCodeVerifier = authCodeVerifierRare.Replace("=", "");
            return string.Format(FileFolderUrl.NintendoAuthorize, authState, authCodeChallenge);
        }
        /// <summary>
        /// Get Session Token from Nintendo
        /// </summary>
        /// <param name="sessionTokenCode">Session Token code</param>
        /// <returns></returns>
        public static async Task<string> GetSessionTokenAsync(string sessionTokenCode)
        {
            // Send HTTP GET
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage requestAuthorize = new HttpRequestMessage(HttpMethod.Get, string.Format(FileFolderUrl.NintendoAuthorize, authState, authCodeChallenge));
            await client.SendAsync(requestAuthorize).ConfigureAwait(false);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, FileFolderUrl.NintendoSessionToken);
            request.Content = new StringContent("{\"client_id\":\"71b963c1b7b6d119\",\"session_token_code\":\"" + sessionTokenCode + "\",\"session_token_code_verifier\":\"" + authCodeVerifier + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                string sessionToken;
                try
                {
                    sessionToken = jObject["session_token"].ToString();
                }
                catch
                {
                    return "";
                }
                return sessionToken;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Sort Players.
        /// Players are sorted by Paint/Sort, Kill + Assist, Special, Death, Kill and Nickname.
        /// </summary>
        /// <param name="players">Player list which is going to be sorted</param>
        /// <returns></returns>
        private static List<Player> sortPlayer(List<Player> players, Rule.Key rule)
        {
            if (rule == Rule.Key.turf_war)
            {
                return players.OrderByDescending(p => p.Paint).ThenByDescending(p => p.Kill + p.Assist).ThenByDescending(p => p.Special).ThenByDescending(p => p.Death).ThenByDescending(p => p.Kill).ThenByDescending(p => p.Nickname).ToList();
            }
            else
            {
                return players.OrderByDescending(p => p.Sort).ThenByDescending(p => p.Kill + p.Assist).ThenByDescending(p => p.Special).ThenByDescending(p => p.Death).ThenByDescending(p => p.Kill).ThenByDescending(p => p.Nickname).ToList();
            }
        }
        /// <summary>
        /// Sort RankedPlayers.
        /// RankedPlayers are sorted by Paint/Sort, Kill + Assist, Special, Death, Kill and Nickname.
        /// </summary>
        /// <param name="players">Player list which is going to be sorted</param>
        /// <returns></returns>
        private static List<RankedPlayer> sortPlayer(List<RankedPlayer> players, Rule.Key rule)
        {
            if (rule == Rule.Key.turf_war)
            {
                return players.OrderByDescending(p => p.Paint).ThenByDescending(p => p.Kill + p.Assist).ThenByDescending(p => p.Special).ThenByDescending(p => p.Death).ThenByDescending(p => p.Kill).ThenByDescending(p => p.Nickname).ToList();
            }
            else
            {
                return players.OrderByDescending(p => p.Sort).ThenByDescending(p => p.Kill + p.Assist).ThenByDescending(p => p.Special).ThenByDescending(p => p.Death).ThenByDescending(p => p.Kill).ThenByDescending(p => p.Nickname).ToList();
            }
        }

        /// <summary>
        /// Parse ScheduledStages from JToken.
        /// </summary>
        /// <param name="node">JToken of a schedule</param>
        private static List<ScheduledStage> parseScheduledStages(JToken node)
        {
            List<ScheduledStage> stages = new List<ScheduledStage>();
            try
            {
                Mode.Key mode = Mode.ParseKey(node["game_mode"]["key"].ToString());
                Rule.Key rule = Rule.ParseKey(node["rule"]["key"].ToString());
                ScheduledStage stage1, stage2;
                if (int.TryParse(node["stage_a"]["id"].ToString(), out int stage1Id))
                {
                    string image = node["stage_a"]["image"].ToString();
                    stage1 = new ScheduledStage(mode, rule, (Stage.Key)stage1Id, image);
                }
                else
                {
                    stage1 = new ScheduledStage(mode, rule);
                }
                if (int.TryParse(node["stage_b"]["id"].ToString(), out int stage2Id))
                {
                    string image = node["stage_b"]["image"].ToString();
                    stage2 = new ScheduledStage(mode, rule, (Stage.Key)stage2Id, image);
                }
                else
                {
                    stage2 = new ScheduledStage(mode, rule);
                }
                stages.Add(stage1);
                stages.Add(stage2);
            }
            catch
            {
                return new List<ScheduledStage>();
            }
            return stages;
        }
        /// <summary>
        /// Parse Player from JToken
        /// </summary>
        /// <param name="node">JToken of a player</param>
        /// <param name="image">Url of the user icon</param>
        /// <param name="isSelf">If the player is player itself</param>
        /// <returns></returns>
        private static Player parsePlayer(JToken node, string image, bool isSelf = false)
        {
            try
            {
                if (image == "")
                {
                    throw new ArgumentNullException();
                }
                // Parse player battle data
                int paint = int.Parse(node["game_paint_point"].ToString());
                int kill = int.Parse(node["kill_count"].ToString());
                int assist = int.Parse(node["assist_count"].ToString());
                int death = int.Parse(node["death_count"].ToString());
                int special = int.Parse(node["special_count"].ToString());
                int sort = int.Parse(node["sort_score"].ToString());
                // Parse player
                JToken playerNode = node["player"];
                string id = playerNode["principal_id"].ToString();
                string nickname = playerNode["nickname"].ToString();
                int level = int.Parse(playerNode["player_rank"].ToString()) + 100 * int.Parse(playerNode["star_rank"].ToString());
                // Parse weapon
                Weapon weapon = parseWeapon(playerNode["weapon"]);
                // Parse gear
                HeadGear headGear = parseGear(playerNode["head"], playerNode["head_skills"], Gear.KindType.Head) as HeadGear;
                ClothesGear clothesGear = parseGear(playerNode["clothes"], playerNode["clothes_skills"], Gear.KindType.Clothes) as ClothesGear;
                ShoesGear shoesGear = parseGear(playerNode["shoes"], playerNode["shoes_skills"], Gear.KindType.Shoes) as ShoesGear;
                return new Player(id, nickname, level, headGear, clothesGear, shoesGear, weapon, paint, kill, assist, death, special, sort, image, isSelf);
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse Player from JToken
        /// </summary>
        /// <param name="node">JToken of a player</param>
        /// <param name="isSelf">If the player is player itself</param>
        /// <returns></returns>
        private static async Task<Player> parsePlayer(JToken node, bool isSelf = false)
        {
            try
            {
                string image = await GetPlayerIcon(node["player"]["principal_id"].ToString()).ConfigureAwait(false);
                Player player = parsePlayer(node, image, isSelf);
                return player;
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse RankedPlayer from JToken
        /// </summary>
        /// <param name="node">JToken of a player</param>
        /// <param name="image">Url of the user icon</param>
        /// <param name="isSelf">If the player is player itself</param>
        /// <returns></returns>
        private static RankedPlayer parseRankedPlayer(JToken node, string image, bool isSelf = false)
        {
            try
            {
                Player player = parsePlayer(node, image, isSelf);
                Rank.Key rank;
                if (node["player"]["udemae"]["s_plus_number"].HasValues)
                {
                    rank = Rank.ParseKey(node["player"]["udemae"]["name"].ToString(), int.Parse(node["player"]["udemae"]["s_plus_number"].ToString()));
                }
                else
                {
                    rank = Rank.ParseKey(node["player"]["udemae"]["name"].ToString());
                }
                return new RankedPlayer(player, rank);
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse RankedPlayer from JToken
        /// </summary>
        /// <param name="node">JToken of a player</param>
        /// <param name="isSelf">If the player is player itself</param>
        /// <returns></returns>
        private static async Task<RankedPlayer> parseRankedPlayer(JToken node, bool isSelf = false)
        {
            try
            {
                string image = await GetPlayerIcon(node["player"]["principal_id"].ToString()).ConfigureAwait(false);
                RankedPlayer player = parseRankedPlayer(node, image, isSelf);
                return player;
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse Weapon from JToken
        /// </summary>
        /// <param name="node">JToken of a weapon</param>
        /// <returns></returns>
        private static Weapon parseWeapon(JToken node)
        {
            try
            {
                SubWeapon sub = parseSubWeapon(node["sub"]);
                SpecialWeapon special = parseSpecialWeapon(node["special"]);
                return new Weapon((Weapon.Key)int.Parse(node["id"].ToString()), sub, special, node["image"].ToString());
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse SubWeapon from JToken
        /// </summary>
        /// <param name="node">JToken of a sub weapon</param>
        /// <returns></returns>
        private static SubWeapon parseSubWeapon(JToken node)
        {
            try
            {
                return new SubWeapon((SubWeapon.Key)int.Parse(node["id"].ToString()), node["image_a"].ToString(), node["image_b"].ToString());
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse SpecialWeapon from JToken
        /// </summary>
        /// <param name="node">JToken of a special weapon</param>
        /// <returns></returns>
        private static SpecialWeapon parseSpecialWeapon(JToken node)
        {
            try
            {
                return new SpecialWeapon((SpecialWeapon.Key)int.Parse(node["id"].ToString()), node["image_a"].ToString(), node["image_b"].ToString()); ;
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse Gear from JToken
        /// </summary>
        /// <param name="gearNode">JToken of a head, clothes or shoes</param>
        /// <param name="skillNode">JToken of a head skills, clothes skills or shoes skills</param>
        /// <param name="kind"></param>
        /// <returns></returns>
        private static Gear parseGear(JToken gearNode, JToken skillNode, Gear.KindType kind)
        {
            try
            {
                Brand brand = new Brand((Brand.Key)int.Parse(gearNode["brand"]["id"].ToString()), gearNode["brand"]["image"].ToString());
                MainSkill mainSkill = parseMainSkill(skillNode["main"]);
                List<SubSkill> subSkills = new List<SubSkill>();
                foreach (JToken subNode in skillNode["subs"])
                {
                    if (subNode.HasValues)
                    {
                        subSkills.Add(parseSubSkill(subNode));
                    }
                }
                switch (kind)
                {
                    case Gear.KindType.Head:
                        HeadGear headGear = new HeadGear((HeadGear.Key)int.Parse(gearNode["id"].ToString()), brand, mainSkill, subSkills, gearNode["image"].ToString());
                        return headGear as Gear;
                    case Gear.KindType.Clothes:
                        ClothesGear clothesGear = new ClothesGear((ClothesGear.Key)int.Parse(gearNode["id"].ToString()), brand, mainSkill, subSkills, gearNode["image"].ToString());
                        return clothesGear as Gear;
                    case Gear.KindType.Shoes:
                        ShoesGear shoesGear = new ShoesGear((ShoesGear.Key)int.Parse(gearNode["id"].ToString()), brand, mainSkill, subSkills, gearNode["image"].ToString());
                        return shoesGear as Gear;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse MainSkill from JToken
        /// </summary>
        /// <param name="node">JToken of a main skill</param>
        /// <returns></returns>
        private static MainSkill parseMainSkill(JToken node)
        {
            try
            {
                return new MainSkill((MainSkill.Key)int.Parse(node["id"].ToString()), node["image"].ToString());
            }
            catch
            {
                throw new FormatException();
            }
        }
        /// <summary>
        /// Parse SubSkill from JToken
        /// </summary>
        /// <param name="node">JToken of a sub skill</param>
        /// <returns></returns>
        private static SubSkill parseSubSkill(JToken node)
        {
            try
            {
                return new SubSkill((SubSkill.Key)int.Parse(node["id"].ToString()), node["image"].ToString());
            }
            catch
            {
                throw new FormatException();
            }
        }

        /// <summary>
        /// Encode to URL safe Base64 string
        /// </summary>
        /// <param name="s">Bytes which is going to be encoded</param>
        /// <returns></returns>
        private static string ToUrlSafeBase64String(byte[] s)
        {
            return Convert.ToBase64String(s).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
