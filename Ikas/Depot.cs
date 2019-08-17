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
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json.Linq;

using Ikas.Class;

namespace Ikas
{
    public delegate void AlwaysOnTopChangedEventHandler();
    public delegate void LanguageChangedEventHandler();
    public delegate void ContentChangedEventHandler();
    public delegate void ContentFoundEventHandler();
    public delegate void ContentUpdatedEventHandler();
    public delegate void ContentFailedEventHandler(Base.ErrorType error);
    public delegate void ContentNotifyingHandler();
    public delegate void AccessGetEventHandler(Base.ErrorType error, string access = null);
    public delegate void CookieUpdatedEventHandler();
    public static class Depot
    {
        public enum Mode
        {
            regular_battle,
            ranked_battle,
            league_battle,
            salmon_run
        }

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
                    // Raise event
                    CookieUpdated?.Invoke();
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
        private static JobPlayer.GradeType salmonRunGrade
        {
            set
            {
                if (value != SalmonRunGrade)
                {
                    try
                    {
                        userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationSalmonRunGrade] = ((int)value).ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(userConfigurationPath, userIniData);
                    }
                    catch { }
                }
            }
        }
        public static JobPlayer.GradeType SalmonRunGrade
        {
            get
            {
                try
                {
                    return (JobPlayer.GradeType)int.Parse(userIniData[FileFolderUrl.UserConfigurationStatisticsSection][FileFolderUrl.UserConfigurationSalmonRunGrade]);
                }
                catch
                {
                    return JobPlayer.GradeType.grade_unknown;
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
                    return bool.Parse(systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationAlwaysOnTop]);
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
                        systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationAlwaysOnTop] = value.ToString().ToLower();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                    finally
                    {
                        AlwaysOnTopChanged?.Invoke();
                    }
                }
            }
        }
        public static bool Notification
        {
            get
            {
                try
                {
                    return bool.Parse(systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationNotification]);
                }
                catch
                {
                    return true;
                }
            }
            set
            {
                if (value != Notification)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationNotification] = value.ToString().ToLower();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static Mode StartMode
        {
            get
            {
                try
                {
                    return (Mode)int.Parse(systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationStartMode]);
                }
                catch
                {
                    return Mode.regular_battle;
                }
            }
            set
            {
                if (value != StartMode)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationStartMode] = ((int)value).ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static double StartX
        {
            get
            {
                try
                {
                    return double.Parse(systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationStartX]);
                }
                catch
                {
                    return double.MinValue;
                }
            }
            set
            {
                if (value != StartX)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationStartX] = value.ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static double StartY
        {
            get
            {
                try
                {
                    return double.Parse(systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationStartY]);
                }
                catch
                {
                    return double.MinValue;
                }
            }
            set
            {
                if (value != StartY)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationAppearanceSection][FileFolderUrl.SystemConfigurationStartY] = value.ToString();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
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
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
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
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
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
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
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
                        if (ProxyHost != null && ProxyHost != "" && ProxyPort > 0)
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
                    if (systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationLanguage] != null)
                    {
                        return systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationLanguage];
                    }
                    else
                    {
                        return "en-US";
                    }
                }
                catch
                {
                    return "en-US";
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
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                    finally
                    {
                        LanguageChanged?.Invoke();
                    }
                }
                else
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationLanguage] = value;
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }
        public static bool TranslateProperNoun
        {
            get
            {
                return true;
            }
        }
        public static bool InUse
        {
            get
            {
                try
                {
                    return bool.Parse(systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationInUse]);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                if (value != InUse)
                {
                    try
                    {
                        systemIniData[FileFolderUrl.SystemConfigurationGeneralSection][FileFolderUrl.SystemConfigurationInUse] = value.ToString().ToLower();
                        FileIniDataParser parser = new FileIniDataParser();
                        parser.WriteFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration, systemIniData);
                    }
                    catch { }
                }
            }
        }

        private static string authState = "";
        private static string authCodeChallenge = "";
        private static string authCodeVerifier = "";

        public static event ContentChangedEventHandler ScheduleChanged;
        public static event ContentUpdatedEventHandler ScheduleUpdated;
        public static event ContentFailedEventHandler ScheduleFailed;
        private static Mutex ScheduleMutex = new Mutex();
        public static Schedule Schedule { get; set; } = new Schedule();

        public static event ContentChangedEventHandler ShiftChanged;
        public static event ContentUpdatedEventHandler ShiftUpdated;
        public static event ContentFailedEventHandler ShiftFailed;
        private static Mutex ShiftMutex = new Mutex();
        public static Shift Shift { get; set; } = new Shift();

        public static event ContentChangedEventHandler BattleChanged;
        public static event ContentFoundEventHandler BattleFound;
        public static event ContentUpdatedEventHandler BattleUpdated;
        public static event ContentFailedEventHandler BattleFailed;
        public static event ContentNotifyingHandler BattleNotifying;
        private static Mutex BattleMutex = new Mutex();
        public static Battle Battle { get; set; } = new Battle();
        private static int notifiedBattleNumber = 0;

        public static event ContentChangedEventHandler JobChanged;
        public static event ContentFoundEventHandler JobFound;
        public static event ContentUpdatedEventHandler JobUpdated;
        public static event ContentFailedEventHandler JobFailed;
        public static event ContentNotifyingHandler JobNotifying;
        private static Mutex JobMutex = new Mutex();
        public static Job Job { get; set; } = new Job();
        private static int notifiedJobNumber = 0;

        public static event AccessGetEventHandler SessionTokenGet;
        public static event AccessGetEventHandler CookieGet;
        public static event CookieUpdatedEventHandler CookieUpdated;

        public static Mode CurrentMode { get; set; } = Mode.regular_battle;

        /// <summary>
        /// Load user configuration from a file.
        /// </summary>
        /// <param name="file">The file of the user, the default one is user.ini</param>
        /// <returns></returns>
        public static bool LoadUserConfiguration(string file = FileFolderUrl.UserConfiguration)
        {
            userConfigurationPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + file;
            if (!File.Exists(userConfigurationPath))
            {
                return false;
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
            if (!File.Exists(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration))
            {
                return false;
            }
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                systemIniData = parser.ReadFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.SystemConfiguration);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get current and next schedule in regular, ranked and league mode.
        /// </summary>
        public static async void GetSchedule()
        {
            // Remove previous downloader's handlers
            DownloadHelper.RemoveDownloaders(Downloader.SourceType.Schedule);
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
            try
            {
                request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            }
            catch
            {
                // Update schedule on error
                UpdateSchedule(new Schedule(Base.ErrorType.cookie_is_empty));
                return;
            }
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request);
            }
            catch
            {
                // Update schedule on error
                UpdateSchedule(new Schedule(Base.ErrorType.network_cannot_be_reached));
                return;
            }
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
                    // Create schedules
                    endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(int.Parse(jObject["regular"][0]["end_time"].ToString()));
                    stages.AddRange(regularStages);
                    stages.AddRange(rankedStages);
                    stages.AddRange(leagueStages);
                    nextStages.AddRange(nextRegularStages);
                    nextStages.AddRange(nextRankedStages);
                    nextStages.AddRange(nextLeagueStages);
                }
                catch (Exception ex)
                {
                    if (Base.TryParseErrorType(ex.Message, out _))
                    {
                        // Update schedule on error
                        UpdateSchedule(new Schedule(Base.ParseErrorType(ex.Message)));
                    }
                    else
                    {
                        // Update schedule on error
                        UpdateSchedule(new Schedule(Base.ErrorType.schedule_cannot_be_resolved));
                    }
                    return;
                }
                // Update schedule
                UpdateSchedule(new Schedule(endTime, stages, nextStages));
            }
            else
            {
                // Update schedule on error
                UpdateSchedule(new Schedule(Base.ErrorType.network_cannot_be_reached_or_cookie_is_invalid_or_expired));
            }
        }
        /// <summary>
        /// Get current and next schedule in regular, ranked and league mode, also raise ScheduleChanged event.
        /// </summary>
        public static void ForceGetSchedule()
        {
            // Raise event
            ScheduleChanged?.Invoke();
            // Update schedule
            GetSchedule();
        }
        /// <summary>
        /// Update schedule.
        /// </summary>
        /// <param name="schedule">Updated schedule</param>
        /// <returns></returns>
        private static bool UpdateSchedule(Schedule schedule)
        {
            if (schedule.Error >= 0)
            {
                ScheduleFailed?.Invoke(schedule.Error);
            }
            else if (schedule.EndTime == new DateTime(0))
            {
                ScheduleFailed?.Invoke(Base.ErrorType.schedule_is_not_ready);
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
        /// Get current and next shift.
        /// </summary>
        public static async void GetShift()
        {
            // Remove previous downloader's handlers
            DownloadHelper.RemoveDownloaders(Downloader.SourceType.Shift);
            // Send HTTP GET
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + FileFolderUrl.SplatNetSalmonRunScheduleApi);
            try
            {
                request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            }
            catch
            {
                // Update shift on error
                UpdateShift(new Shift(Base.ErrorType.cookie_is_empty));
                return;
            }
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request);
            }
            catch
            {
                // Update shift on error
                UpdateShift(new Shift(Base.ErrorType.network_cannot_be_reached));
                return;
            }
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                JToken details = jObject["details"];
                List<ShiftStage> stages = new List<ShiftStage>();
                try
                {
                    foreach (JToken node in details)
                    {
                        stages.Add(parseShiftStage(node));
                    }
                }
                catch (Exception ex)
                {
                    if (Base.TryParseErrorType(ex.Message, out _))
                    {
                        // Update shift on error
                        UpdateShift(new Shift(Base.ParseErrorType(ex.Message)));
                    }
                    else
                    {
                        // Update shift on error
                        UpdateShift(new Shift(Base.ErrorType.schedule_cannot_be_resolved));
                    }
                    return;
                }
                // Update shift
                UpdateShift(new Shift(stages));
            }
            else
            {
                // Update shift on error
                UpdateShift(new Shift(Base.ErrorType.network_cannot_be_reached_or_cookie_is_invalid_or_expired));
            }
        }
        /// <summary>
        /// Get current and next shift, also raise ShiftChanged event.
        /// </summary>
        public static void ForceGetShift()
        {
            // Raise event
            ShiftChanged?.Invoke();
            // Update shift
            GetShift();
        }
        /// <summary>
        /// Update shift.
        /// </summary>
        /// <param name="shift">Updated shift</param>
        /// <returns></returns>
        private static bool UpdateShift(Shift shift)
        {
            if (shift.Error >= 0)
            {
                ShiftFailed?.Invoke(shift.Error);
            }
            else if (shift.Stages.Count == 0)
            {
                ShiftFailed?.Invoke(Base.ErrorType.shift_is_not_ready);
            }
            if (Shift != shift)
            {
                ShiftMutex.WaitOne();
                Shift = shift;
                ShiftMutex.ReleaseMutex();
                // Raise event
                ShiftUpdated?.Invoke();
                return true;
            }
            else
            {
                // Raise event
                ShiftUpdated?.Invoke();
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
            // Remove previous downloader's handlers
            DownloadHelper.RemoveDownloaders(Downloader.SourceType.Battle);
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
            try
            {
                request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            }
            catch
            {
                // Update battle on error
                UpdateBattle(new Battle(Base.ErrorType.cookie_is_empty));
                return;
            }
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request);
            }
            catch
            {
                // Update Battle on error
                UpdateBattle(new Battle(Base.ErrorType.network_cannot_be_reached));
                return;
            }
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                int battleNumber = -1;
                try
                {
                    battleNumber = int.Parse(jObject["results"][0]["battle_number"].ToString());
                    try
                    {
                        bool isSplatZonesUpdated = false;
                        bool isTowerControlUpdated = false;
                        bool isRainmakerUpdated = false;
                        bool isClamBlitzUpdated = false;
                        foreach (JToken battle in jObject["results"])
                        {
                            int thisBattleNumber = int.Parse(battle["battle_number"].ToString());
                            if (thisBattleNumber > Battle.Number)
                            {
                                Class.Mode.Key type = Class.Mode.ParseKey(battle["type"].ToString());
                                if (type == Class.Mode.Key.ranked_battle || type == Class.Mode.Key.league_battle)
                                {
                                    Rule.Key rule = Rule.ParseKey(battle["rule"]["key"].ToString());
                                    Rank.Key rankAfter;
                                    bool isSPlus = int.TryParse(battle["udemae"]["s_plus_number"].ToString(), out _);
                                    if (isSPlus)
                                    {
                                        rankAfter = Rank.ParseKey(battle["udemae"]["name"].ToString(), int.Parse(battle["udemae"]["s_plus_number"].ToString()));
                                    }
                                    else
                                    {
                                        rankAfter = Rank.ParseKey(battle["udemae"]["name"].ToString());
                                    }
                                    switch (rule)
                                    {
                                        case Rule.Key.splat_zones:
                                            if (rankAfter != SplatZonesRank && !isSplatZonesUpdated)
                                            {
                                                UpdateRank(Rule.Key.splat_zones, rankAfter);
                                            }
                                            isSplatZonesUpdated = true;
                                            break;
                                        case Rule.Key.tower_control:
                                            if (rankAfter != TowerControlRank && !isTowerControlUpdated)
                                            {
                                                UpdateRank(Rule.Key.tower_control, rankAfter);
                                            }
                                            isTowerControlUpdated = true;
                                            break;
                                        case Rule.Key.rainmaker:
                                            if (rankAfter != RainmakerRank && !isRainmakerUpdated)
                                            {
                                                UpdateRank(Rule.Key.rainmaker, rankAfter);
                                            }
                                            isRainmakerUpdated = true;
                                            break;
                                        case Rule.Key.clam_blitz:
                                            if (rankAfter != ClamBlitzRank && !isClamBlitzUpdated)
                                            {
                                                UpdateRank(Rule.Key.clam_blitz, rankAfter);
                                            }
                                            isClamBlitzUpdated = true;
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    // Update Battle on error
                    UpdateBattle(new Battle(Base.ErrorType.battles_cannot_be_resolved));
                    return;
                }
                // Same battle
                if (battleNumber == Battle.Number)
                {
                    // Update same Battle
                    UpdateBattle(Battle);
                    return;
                }
                else
                {
                    // Raise event
                    BattleFound?.Invoke();
                }
                // Send HTTP GET
                request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + string.Format(FileFolderUrl.SplatNetIndividualBattleApi, battleNumber));
                try
                {
                    request.Headers.Add("Cookie", "iksm_session=" + Cookie);
                }
                catch
                {
                    // Update Battle on error
                    UpdateBattle(new Battle(Base.ErrorType.cookie_is_empty));
                    return;
                }
                try
                {
                    response = await client.SendAsync(request);
                }
                catch
                {
                    // Update Battle on error
                    UpdateBattle(new Battle(Base.ErrorType.network_cannot_be_reached));
                    return;
                }
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                    // Parse JSON
                    jObject = JObject.Parse(resultString);
                    try
                    {
                        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddSeconds(long.Parse(jObject["start_time"].ToString()));
                        double elapsedTime;
                        Class.Mode.Key type = Class.Mode.ParseKey(jObject["type"].ToString());
                        Class.Mode mode = new Class.Mode(Class.Mode.ParseGameModeKey(jObject["game_mode"]["key"].ToString()), jObject["game_mode"]["name"].ToString());
                        Rule rule = new Rule(Rule.ParseKey(jObject["rule"]["key"].ToString()), jObject["rule"]["name"].ToString());
                        switch (rule.Id)
                        {
                            case Rule.Key.turf_war:
                                elapsedTime = 180;
                                break;
                            case Rule.Key.splat_zones:
                            case Rule.Key.tower_control:
                            case Rule.Key.rainmaker:
                            case Rule.Key.clam_blitz:
                                elapsedTime = double.Parse(jObject["elapsed_time"].ToString());
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        Stage stage = new Stage((Stage.Key)int.Parse(jObject["stage"]["id"].ToString()), jObject["stage"]["name"].ToString(), jObject["stage"]["image"].ToString());
                        switch (type)
                        {
                            case Class.Mode.Key.regular_battle:
                                {
                                    // Self
                                    Player selfPlayer = await parsePlayer(jObject["player_result"], true);
                                    // My team players
                                    List<Player> myPlayers = new List<Player>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<Player>> myPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in myPlayersNode)
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
                                    myPlayers = sortPlayer(myPlayers, rule.Id);
                                    // Other team players
                                    List<Player> otherPlayers = new List<Player>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<Player>> otherPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in otherPlayersNode)
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
                                    otherPlayers = sortPlayer(otherPlayers, rule.Id);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    double winMeter = double.Parse(jObject["win_meter"].ToString());
                                    double myScore = double.Parse(jObject["my_team_percentage"].ToString());
                                    double otherScore = double.Parse(jObject["other_team_percentage"].ToString());
                                    UpdateBattle(new RegularBattle(battleNumber, startTime, elapsedTime, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                        winMeter, myScore, otherScore) as Battle);
                                }
                                break;
                            case Class.Mode.Key.ranked_battle:
                                {
                                    // Self
                                    RankedPlayer selfPlayer = await parseRankedPlayer(jObject["player_result"], true);
                                    // My team players
                                    List<RankedPlayer> myPlayers = new List<RankedPlayer>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<RankedPlayer>> myPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in myPlayersNode)
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
                                    myPlayers = sortPlayer(myPlayers, rule.Id);
                                    // Other team players
                                    List<RankedPlayer> otherPlayers = new List<RankedPlayer>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<RankedPlayer>> otherPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in otherPlayersNode)
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
                                    otherPlayers = sortPlayer(otherPlayers, rule.Id);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    int myScore = int.Parse(jObject["my_team_count"].ToString());
                                    int otherScore = int.Parse(jObject["other_team_count"].ToString());
                                    bool isXPower = double.TryParse(jObject["x_power"].ToString(), out _);
                                    if (!isXPower)
                                    {
                                        // Ranked Battle
                                        int estimatedRankPower = int.Parse(jObject["estimate_gachi_power"].ToString());
                                        Rank.Key rankAfter;
                                        bool isSPlus = int.TryParse(jObject["udemae"]["s_plus_number"].ToString(), out _);
                                        if (isSPlus)
                                        {
                                            rankAfter = Rank.ParseKey(jObject["udemae"]["name"].ToString(), int.Parse(jObject["udemae"]["s_plus_number"].ToString()));
                                        }
                                        else
                                        {
                                            rankAfter = Rank.ParseKey(jObject["udemae"]["name"].ToString());
                                        }
                                        switch (rule.Id)
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
                                        UpdateBattle(new RankedBattle(battleNumber, startTime, elapsedTime, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                            estimatedRankPower, rankAfter, myScore, otherScore) as Battle);
                                    }
                                    else
                                    {
                                        // Ranked X Battle
                                        int estimatedXPower = int.Parse(jObject["estimate_x_power"].ToString());
                                        double xPowerAfter = double.Parse(jObject["x_power"].ToString());
                                        switch (rule.Id)
                                        {
                                            case Rule.Key.splat_zones:
                                                if (SplatZonesRank != Rank.Key.x)
                                                {
                                                    UpdateRank(Rule.Key.splat_zones, Rank.Key.x);
                                                }
                                                break;
                                            case Rule.Key.tower_control:
                                                if (TowerControlRank != Rank.Key.x)
                                                {
                                                    UpdateRank(Rule.Key.tower_control, Rank.Key.x);
                                                }
                                                break;
                                            case Rule.Key.rainmaker:
                                                if (RainmakerRank != Rank.Key.x)
                                                {
                                                    UpdateRank(Rule.Key.rainmaker, Rank.Key.x);
                                                }
                                                break;
                                            case Rule.Key.clam_blitz:
                                                if (ClamBlitzRank != Rank.Key.x)
                                                {
                                                    UpdateRank(Rule.Key.clam_blitz, Rank.Key.x);
                                                }
                                                break;
                                            default:
                                                throw new ArgumentOutOfRangeException();
                                        }
                                        UpdateBattle(new RankedXBattle(battleNumber, startTime, elapsedTime, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                            estimatedXPower, xPowerAfter, myScore, otherScore) as Battle);
                                    }
                                }
                                break;
                            case Class.Mode.Key.league_battle:
                                {
                                    // Self
                                    RankedPlayer selfPlayer = await parseRankedPlayer(jObject["player_result"], true);
                                    // My team players
                                    List<RankedPlayer> myPlayers = new List<RankedPlayer>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<RankedPlayer>> myPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in myPlayersNode)
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
                                    myPlayers = sortPlayer(myPlayers, rule.Id);
                                    // Other team players
                                    List<RankedPlayer> otherPlayers = new List<RankedPlayer>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<RankedPlayer>> otherPlayerTasks = new List<Task<RankedPlayer>>();
                                    foreach (JToken playerNode in otherPlayersNode)
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
                                    otherPlayers = sortPlayer(otherPlayers, rule.Id);
                                    // Other battle data
                                    int levelAfter = int.Parse(jObject["star_rank"].ToString()) * 100 + int.Parse(jObject["player_rank"].ToString());
                                    if (levelAfter != Level)
                                    {
                                        UpdateLevel(levelAfter);
                                    }
                                    int myEstimatedLeaguePower = int.Parse(jObject["my_estimate_league_point"].ToString());
                                    int otherEstimatedLeaguePower = int.Parse(jObject["other_estimate_league_point"].ToString());
                                    double leaguePoint;
                                    bool isLeaguePoint = double.TryParse(jObject["league_point"].ToString(), out _);
                                    if (isLeaguePoint)
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
                                    Rank.Key rankAfter;
                                    bool isSPlus = int.TryParse(jObject["udemae"]["s_plus_number"].ToString(), out _);
                                    if (isSPlus)
                                    {
                                        rankAfter = Rank.ParseKey(jObject["udemae"]["name"].ToString(), int.Parse(jObject["udemae"]["s_plus_number"].ToString()));
                                    }
                                    else
                                    {
                                        rankAfter = Rank.ParseKey(jObject["udemae"]["name"].ToString());
                                    }
                                    switch (rule.Id)
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
                                    UpdateBattle(new LeagueBattle(battleNumber, startTime, elapsedTime, mode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                        myEstimatedLeaguePower, otherEstimatedLeaguePower, leaguePoint, maxLeaguePoint, myScore, otherScore) as Battle);
                                }
                                break;
                            case Class.Mode.Key.splatfest:
                                {
                                    // Self
                                    Player selfPlayer = await parsePlayer(jObject["player_result"], true);
                                    // My team players
                                    List<Player> myPlayers = new List<Player>();
                                    JToken myPlayersNode = jObject["my_team_members"];
                                    List<Task<Player>> myPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in myPlayersNode)
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
                                    myPlayers = sortPlayer(myPlayers, rule.Id);
                                    // Other team players
                                    List<Player> otherPlayers = new List<Player>();
                                    JToken otherPlayersNode = jObject["other_team_members"];
                                    List<Task<Player>> otherPlayerTasks = new List<Task<Player>>();
                                    foreach (JToken playerNode in otherPlayersNode)
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
                                    otherPlayers = sortPlayer(otherPlayers, rule.Id);
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
                                    UpdateBattle(new SplatfestBattle(battleNumber, startTime, elapsedTime, mode, splatfestMode, rule, stage, myPlayers, otherPlayers, levelAfter,
                                        myEstimatedSplatfestPower, otherEstimatedSplatfestPower, splatfestPower, maxSplatfestPower, contributionPoint, totalContributionPoint, myScore, otherScore) as Battle);
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Base.TryParseErrorType(ex.Message, out _))
                        {
                            // Update Battle on error
                            UpdateBattle(new Battle(Base.ParseErrorType(ex.Message)));
                        }
                        else
                        {
                            // Update Battle on error
                            UpdateBattle(new Battle(Base.ErrorType.battle_cannot_be_resolved));
                        }
                    }
                }
                else
                {
                    // Update Battle on error
                    UpdateBattle(new Battle(Base.ErrorType.network_cannot_be_reached_or_cookie_is_invalid_or_expired));
                }
            }
            else
            {
                // Update Battle on error
                UpdateBattle(new Battle(Base.ErrorType.network_cannot_be_reached_or_cookie_is_invalid_or_expired));
            }
        }
        /// <summary>
        ///  Update battle.
        /// </summary>
        /// <param name="battle">Updated battle</param>
        /// <returns></returns>
        private static bool UpdateBattle(Battle battle)
        {
            if (battle.Error >= 0)
            {
                BattleFailed?.Invoke(battle.Error);
            }
            else if (battle.Number == -1)
            {
                BattleFailed?.Invoke(Base.ErrorType.battle_is_not_ready);
            }
            if (Battle != battle)
            {
                BattleMutex.WaitOne();
                Battle = battle;
                // Notify
                if (Battle.Number > notifiedBattleNumber)
                {
                    notifiedBattleNumber = Battle.Number;
                    BattleNotifying?.Invoke();
                }
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
        /// Get last job result.
        /// </summary>
        public static async void GetLastJob()
        {
            // Raise event
            JobChanged?.Invoke();
            // Remove previous downloader's handlers
            DownloadHelper.RemoveDownloaders(Downloader.SourceType.Job);
            // Send HTTP GET
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + FileFolderUrl.SplatNetSalmonRunBattleApi);
            try
            {
                request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            }
            catch
            {
                // Update job on error
                UpdateJob(new Job(Base.ErrorType.cookie_is_empty));
                return;
            }
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request);
            }
            catch
            {
                // Update job on error
                UpdateJob(new Job(Base.ErrorType.network_cannot_be_reached));
                return;
            }
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                int battleNumber = -1;
                try
                {
                    battleNumber = int.Parse(jObject["results"][0]["job_id"].ToString());
                    JobPlayer.GradeType grade = (JobPlayer.GradeType)int.Parse(jObject["results"][0]["grade"]["id"].ToString());
                    if (grade != SalmonRunGrade)
                    {
                        salmonRunGrade = grade;
                        ForceGetShift();
                    }
                }
                catch
                {
                    // Update job on error
                    UpdateJob(new Job(Base.ErrorType.jobs_cannot_be_resolved));
                    return;
                }
                // Same job
                if (battleNumber == Job.Number)
                {
                    // Update same job
                    UpdateJob(Job);
                    return;
                }
                else
                {
                    // Raise event
                    JobFound?.Invoke();
                }
                // Send HTTP GET
                request = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet + string.Format(FileFolderUrl.SplatNetSalmonRunIndividualBattleApi, battleNumber));
                try
                {
                    request.Headers.Add("Cookie", "iksm_session=" + Cookie);
                }
                catch
                {
                    // Update job on error
                    UpdateJob(new Job(Base.ErrorType.cookie_is_empty));
                    return;
                }
                try
                {
                    response = await client.SendAsync(request);
                }
                catch
                {
                    // Update job on error
                    UpdateJob(new Job(Base.ErrorType.network_cannot_be_reached));
                    return;
                }
                if (response.IsSuccessStatusCode)
                {
                    resultString = await response.Content.ReadAsStringAsync();
                    // Parse JSON
                    jObject = JObject.Parse(resultString);
                    try
                    {
                        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddSeconds(long.Parse(jObject["play_time"].ToString()));
                        double hazardLevel = double.Parse(jObject["danger_rate"].ToString());
                        ShiftStage stage = parseShiftStage(jObject["schedule"]);
                        // Player should be parsed first to acquire specials used in wave
                        JobPlayer myPlayer = await parseJobPlayer(jObject["my_result"], true, (JobPlayer.GradeType)int.Parse(jObject["grade"]["id"].ToString()), int.Parse(jObject["grade_point"].ToString()));
                        List<JobPlayer> otherPlayers = new List<JobPlayer>();
                        List<Task<JobPlayer>> otherPlayerTasks = new List<Task<JobPlayer>>();
                        foreach (JToken node in jObject["other_results"])
                        {
                            if (node.HasValues)
                            {
                                Task<JobPlayer> playerTask = parseJobPlayer(node);
                                otherPlayerTasks.Add(playerTask);
                            }
                        }
                        await Task.WhenAll(otherPlayerTasks);
                        foreach (Task<JobPlayer> playerTask in otherPlayerTasks)
                        {
                            otherPlayers.Add(playerTask.Result);
                        }
                        // Result should be parsed first to acquire result of waves
                        Job.ResultType result = Job.ResultType.clear;
                        if (!bool.Parse(jObject["job_result"]["is_clear"].ToString()))
                        {
                            result = Job.ParseResultType(jObject["job_result"]["failure_reason"].ToString());
                        }
                        List<Wave> waves = new List<Wave>();
                        for (int i = 0; i < jObject["wave_details"].Count(); ++i)
                        {
                            JToken node = jObject["wave_details"][i];
                            List<SpecialWeapon> specials = new List<SpecialWeapon>();
                            if (myPlayer.SpecialWeaponCount[i] != 0)
                            {
                                for (int j = 0; j < myPlayer.SpecialWeaponCount[i]; ++j)
                                {
                                    specials.Add(myPlayer.Weapons[i].SpecialWeapon);
                                }
                            }
                            foreach (JobPlayer player in otherPlayers)
                            {
                                if (player.SpecialWeaponCount[i] != 0)
                                {
                                    for (int j = 0; j < player.SpecialWeaponCount[i]; ++j)
                                    {
                                        specials.Add(player.Weapons[i].SpecialWeapon);
                                    }
                                }
                            }
                            if (i != jObject["wave_details"].Count() - 1)
                            {
                                waves.Add(parseWave(node, specials, Job.ResultType.clear));
                            }
                            else
                            {
                                waves.Add(parseWave(node, specials, result));
                            }
                        }
                        int steelheadCount = int.Parse(jObject["boss_counts"]["6"]["count"].ToString());
                        int flyfishCount = int.Parse(jObject["boss_counts"]["9"]["count"].ToString());
                        int steelEelCount = int.Parse(jObject["boss_counts"]["13"]["count"].ToString());
                        int drizzlerCount = int.Parse(jObject["boss_counts"]["21"]["count"].ToString());
                        int stingerCount = int.Parse(jObject["boss_counts"]["14"]["count"].ToString());
                        int scrapperCount = int.Parse(jObject["boss_counts"]["12"]["count"].ToString());
                        int mawsCount = int.Parse(jObject["boss_counts"]["15"]["count"].ToString());
                        int grillerCount = int.Parse(jObject["boss_counts"]["16"]["count"].ToString());
                        int goldieCount = int.Parse(jObject["boss_counts"]["3"]["count"].ToString());
                        int score = int.Parse(jObject["job_score"].ToString());
                        int gradePointDelta = int.Parse(jObject["grade_point_delta"].ToString());
                        UpdateJob(new Job(battleNumber, startTime, hazardLevel, stage, waves, myPlayer, otherPlayers,
                            steelheadCount, flyfishCount, steelEelCount, drizzlerCount, stingerCount, scrapperCount, mawsCount, grillerCount, goldieCount,
                            score, gradePointDelta, result));
                    }
                    catch (Exception ex)
                    {
                        if (Base.TryParseErrorType(ex.Message, out _))
                        {
                            // Update job on error
                            UpdateJob(new Job(Base.ParseErrorType(ex.Message)));
                        }
                        else
                        {
                            // Update job on error
                            UpdateJob(new Job(Base.ErrorType.job_cannot_be_resolved));
                        }
                    }
                }
                else
                {
                    // Update job on error
                    UpdateJob(new Job(Base.ErrorType.network_cannot_be_reached_or_cookie_is_invalid_or_expired));
                }
            }
            else
            {
                // Update job on error
                UpdateJob(new Job(Base.ErrorType.network_cannot_be_reached_or_cookie_is_invalid_or_expired));
            }
        }
        /// <summary>
        /// Update job.
        /// </summary>
        /// <param name="job">Updated job</param>
        /// <returns></returns>
        private static bool UpdateJob(Job job)
        {
            if (job.Error >= 0)
            {
                JobFailed?.Invoke(job.Error);
            }
            else if (job.Number == -1)
            {
                JobFailed?.Invoke(Base.ErrorType.job_is_not_ready);
            }
            if (Job != job)
            {
                JobMutex.WaitOne();
                Job = job;
                // Notify
                if (Job.Number > notifiedJobNumber)
                {
                    notifiedJobNumber = Job.Number;
                    JobNotifying?.Invoke();
                }
                JobMutex.ReleaseMutex();
                // Raise event
                JobUpdated?.Invoke();
                return true;
            }
            else
            {
                // Raise event
                JobUpdated?.Invoke();
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
                    ForceGetSchedule();
                    return true;
                }
                catch
                {
                    ForceGetSchedule();
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
                            ForceGetSchedule();
                            return true;
                        }
                        catch
                        {
                            ForceGetSchedule();
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
                            ForceGetSchedule();
                            return true;
                        }
                        catch
                        {
                            ForceGetSchedule();
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
                            ForceGetSchedule();
                            return true;
                        }
                        catch
                        {
                            ForceGetSchedule();
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
                            ForceGetSchedule();
                            return true;
                        }
                        catch
                        {
                            ForceGetSchedule();
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
            DownloadHelper.RemoveDownloaders(Downloader.SourceType.Battle);
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
            try
            {
                request.Headers.Add("Cookie", "iksm_session=" + Cookie);
            }
            catch
            {
                return "";
            }
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request).ConfigureAwait(false);
            }
            catch
            {
                return "";
            }
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string icon = "";
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
        /// Log in to Nintendo.
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
        /// Get session token from Nintendo.
        /// </summary>
        /// <param name="sessionTokenCode">Session token code</param>
        /// <returns></returns>
        public static async void GetSessionToken(string sessionTokenCode)
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
            try
            {
                await client.SendAsync(requestAuthorize);
            }
            catch
            {
                SessionTokenGet?.Invoke(Base.ErrorType.network_cannot_be_reached);
                return;
            }
            // Send HTTP POST
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, FileFolderUrl.NintendoSessionToken);
            request.Content = new StringContent("{\"client_id\":\"71b963c1b7b6d119\",\"session_token_code\":\"" + sessionTokenCode + "\",\"session_token_code_verifier\":\"" + authCodeVerifier + "\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage response;
            try
            {
                response = await client.SendAsync(request);
            }
            catch
            {
                SessionTokenGet?.Invoke(Base.ErrorType.network_cannot_be_reached);
                return;
            }
            if (response.IsSuccessStatusCode)
            {
                string resultString = await response.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultString);
                string sessionToken = "";
                try
                {
                    sessionToken = jObject["session_token"].ToString();
                }
                catch
                {
                    SessionTokenGet?.Invoke(Base.ErrorType.session_token_cannot_be_resolved);
                    return;
                }
                SessionTokenGet?.Invoke(Base.ErrorType.no_error, sessionToken);
                return;
            }
            else
            {
                SessionTokenGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_link_is_invalid_or_expired);
                return;
            }
        }
        /// <summary>
        /// Get cookie from Nintendo (3rd Party services was used).
        /// </summary>
        /// <param name="sessionToken">Session token of Nintendo (3rd party services was used)</param>
        /// <returns></returns>
        public static async void GetCookie(string sessionToken)
        {
            // THIS METHOD USES 3RD PARTY SERVICES, WHICH MAY EXPOSE USERS TO HARM.
            // API DOCS HERE https://github.com/frozenpandaman/splatnet2statink/wiki/api-docs
            // Send HTTP POST
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = false;
            if (Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage requestToken = new HttpRequestMessage(HttpMethod.Post, FileFolderUrl.NintendoToken);
            requestToken.Content = new StringContent("{\"client_id\":\"71b963c1b7b6d119\",\"session_token\":\"" + sessionToken + "\",\"grant_type\":\"urn:ietf:params:oauth:grant-type:jwt-bearer-session-token\"}", Encoding.UTF8, "application/json");
            HttpResponseMessage responseToken;
            try
            {
                responseToken = await client.SendAsync(requestToken);
            }
            catch
            {
                CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                return;
            }
            if (responseToken.IsSuccessStatusCode)
            {
                string resultTokenString = await responseToken.Content.ReadAsStringAsync();
                // Parse JSON
                JObject jObject = JObject.Parse(resultTokenString);
                string token = "";
                string idToken = "";
                try
                {
                    token = jObject["access_token"].ToString();
                    idToken = jObject["id_token"].ToString();
                }
                catch
                {
                    CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved_1_7);
                    return;
                }
                // Send HTTP GET
                HttpRequestMessage requestUserInfo = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.NintendoUserInfo);
                requestUserInfo.Headers.Add("Authorization", string.Format("Bearer {0}", token));
                HttpResponseMessage responseUserInfo;
                try
                {
                    responseUserInfo = await client.SendAsync(requestUserInfo);
                }
                catch
                {
                    CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                    return;
                }
                if (responseUserInfo.IsSuccessStatusCode)
                {
                    string resultUserInfoString = await responseUserInfo.Content.ReadAsStringAsync();
                    // Parse JSON
                    jObject = JObject.Parse(resultUserInfoString);
                    string country = "";
                    string birthday = "";
                    string language = "";
                    try
                    {
                        JToken userInfo = jObject;
                        country = userInfo["country"].ToString();
                        birthday = userInfo["birthday"].ToString();
                        language = userInfo["language"].ToString();
                    }
                    catch
                    {
                        CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved_2_7);
                        return;
                    }
                    // Send 3rd Party HTTP POST
                    long timestamp = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
                    HttpRequestMessage requestHash = new HttpRequestMessage(HttpMethod.Post, FileFolderUrl.eliFesslerApi);
                    requestHash.Headers.Add("User-Agent", "Ikas/0.2.2");
                    List<KeyValuePair<string, string>> requestHashContent = new List<KeyValuePair<string, string>>();
                    requestHashContent.Add(new KeyValuePair<string, string>("naIdToken", idToken));
                    requestHashContent.Add(new KeyValuePair<string, string>("timestamp", timestamp.ToString()));
                    requestHash.Content = new FormUrlEncodedContent(requestHashContent);
                    HttpResponseMessage responseHash;
                    try
                    {
                        responseHash = await client.SendAsync(requestHash);
                    }
                    catch
                    {
                        CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                        return;
                    }
                    if (responseHash.IsSuccessStatusCode)
                    {
                        string resultHashString = await responseHash.Content.ReadAsStringAsync();
                        // Parse JSON
                        jObject = JObject.Parse(resultHashString);
                        string hash = "";
                        try
                        {
                            hash = jObject["hash"].ToString();
                        }
                        catch
                        {
                            CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved_3_7);
                            return;
                        }
                        // Send 3rd Party HTTP GET
                        string guid = Guid.NewGuid().ToString();
                        byte[] iisRaw = new byte[8];
                        (new RNGCryptoServiceProvider()).GetNonZeroBytes(iisRaw);
                        char[] iisChar = new char[8];
                        for (int i = 0; i < 8; ++i)
                        {
                            double doubleN = (double)iisRaw[i] / 256.0 * 62.0;
                            int intN = (int)Math.Ceiling(doubleN);
                            if (intN >= 1 && intN <= 10)
                            {
                                iisChar[i] = (char)(47 + intN);
                            }
                            else if (intN >= 11 && intN <= 36)
                            {
                                iisChar[i] = (char)(54 + intN);
                            }
                            else if (intN >= 37 && intN <= 62)
                            {
                                iisChar[i] = (char)(60 + intN);
                            }
                            else
                            {
                                Debug.Assert(false);
                                iisChar[i] = (char)48;
                            }
                        }
                        string iid = new string(iisChar);
                        HttpRequestMessage request3rd = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.FlapgApi);
                        request3rd.Headers.Add("x-token", idToken);
                        request3rd.Headers.Add("x-time", timestamp.ToString());
                        request3rd.Headers.Add("x-guid", guid);
                        request3rd.Headers.Add("x-hash", hash);
                        request3rd.Headers.Add("x-ver", "2");
                        request3rd.Headers.Add("x-iid", iid);
                        HttpResponseMessage response3rd;
                        try
                        {
                            response3rd = await client.SendAsync(request3rd);
                        }
                        catch
                        {
                            CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                            return;
                        }
                        if (response3rd.IsSuccessStatusCode)
                        {
                            string result3rdString = await response3rd.Content.ReadAsStringAsync();
                            // Parse JSON
                            jObject = JObject.Parse(result3rdString);
                            string loginNsoF = "";
                            string loginNsoP1 = "";
                            string loginNsoP2 = "";
                            string loginNsoP3 = "";
                            string loginAppF = "";
                            string loginAppP1 = "";
                            string loginAppP2 = "";
                            string loginAppP3 = "";
                            try
                            {
                                JToken loginNso = jObject["login_nso"];
                                loginNsoF = loginNso["f"].ToString();
                                loginNsoP1 = loginNso["p1"].ToString();
                                loginNsoP2 = loginNso["p2"].ToString();
                                loginNsoP3 = loginNso["p3"].ToString();
                                JToken loginApp = jObject["login_app"];
                                loginAppF = loginApp["f"].ToString();
                                loginAppP1 = loginApp["p1"].ToString();
                                loginAppP2 = loginApp["p2"].ToString();
                                loginAppP3 = loginApp["p3"].ToString();
                            }
                            catch
                            {
                                CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved_4_7);
                                return;
                            }
                            // Send HTTP POST
                            HttpRequestMessage requestAccessToken = new HttpRequestMessage(HttpMethod.Post, FileFolderUrl.NintendoAccessToken);
                            requestAccessToken.Headers.Add("Authorization", "Bearer");
                            requestAccessToken.Headers.Add("X-ProductVersion", "1.5.2");
                            requestAccessToken.Headers.Add("X-Platform", "Android");
                            requestAccessToken.Content = new StringContent("{\"parameter\":{\"f\":\"" + loginNsoF +
                                "\",\"naIdToken\":\"" + loginNsoP1 +
                                "\",\"timestamp\":\"" + loginNsoP2 +
                                "\",\"requestId\":\"" + loginNsoP3 +
                                "\",\"naCountry\":\"" + country +
                                "\",\"naBirthday\":\"" + birthday +
                                "\",\"language\":\"" + language + "\"}}", Encoding.UTF8, "application/json");
                            HttpResponseMessage responseAccessToken;
                            try
                            {
                                responseAccessToken = await client.SendAsync(requestAccessToken);
                            }
                            catch
                            {
                                CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                                return;
                            }
                            if (responseAccessToken.IsSuccessStatusCode)
                            {
                                string resultAccessTokenString = await responseAccessToken.Content.ReadAsStringAsync();
                                // Parse JSON
                                jObject = JObject.Parse(resultAccessTokenString);
                                string accessToken = "";
                                try
                                {
                                    accessToken = jObject["result"]["webApiServerCredential"]["accessToken"].ToString();
                                }
                                catch
                                {
                                    CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved_5_7);
                                    return;
                                }
                                // Send HTTP POST
                                HttpRequestMessage requestSplatoonAccessToken = new HttpRequestMessage(HttpMethod.Post, FileFolderUrl.NintendoSplatoonAccessToken);
                                requestSplatoonAccessToken.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
                                requestSplatoonAccessToken.Content = new StringContent("{\"parameter\":{\"id\":\"5741031244955648" +
                                "\",\"f\":\"" + loginAppF +
                                "\",\"registrationToken\":\"" + loginAppP1 +
                                "\",\"timestamp\":\"" + loginAppP2 +
                                "\",\"requestId\":\"" + loginAppP3 + "\"}}", Encoding.UTF8, "application/json");
                                HttpResponseMessage responseSplatoonAccessToken;
                                try
                                {
                                    responseSplatoonAccessToken = await client.SendAsync(requestSplatoonAccessToken);
                                }
                                catch
                                {
                                    CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                                    return;
                                }
                                if (responseSplatoonAccessToken.IsSuccessStatusCode)
                                {
                                    string resultSplatoonAccessTokenString = await responseSplatoonAccessToken.Content.ReadAsStringAsync();
                                    // Parse JSON
                                    jObject = JObject.Parse(resultSplatoonAccessTokenString);
                                    string splatoonAccessToken = "";
                                    try
                                    {
                                        splatoonAccessToken = jObject["result"]["accessToken"].ToString();
                                    }
                                    catch
                                    {
                                        CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved_6_7);
                                        return;
                                    }
                                    // Send HTTP GET
                                    CookieContainer cookieContainer = new CookieContainer();
                                    HttpClientHandler handlerCookie = new HttpClientHandler();
                                    handlerCookie.CookieContainer = cookieContainer;
                                    if (Proxy != null)
                                    {
                                        handlerCookie.UseProxy = true;
                                        handlerCookie.Proxy = Proxy;
                                    }
                                    HttpClient clientCookie = new HttpClient(handlerCookie);
                                    HttpRequestMessage requestCookie = new HttpRequestMessage(HttpMethod.Get, FileFolderUrl.SplatNet);
                                    requestCookie.Headers.Add("X-GameWebToken", splatoonAccessToken);
                                    HttpResponseMessage responseCookie;
                                    try
                                    {
                                        responseCookie = await clientCookie.SendAsync(requestCookie);
                                    }
                                    catch
                                    {
                                        CookieGet.Invoke(Base.ErrorType.network_cannot_be_reached);
                                        return;
                                    }
                                    if (responseCookie.IsSuccessStatusCode)
                                    {
                                        List<Cookie> cookies = cookieContainer.GetCookies(new Uri(FileFolderUrl.SplatNet)).Cast<Cookie>().ToList();
                                        string cookie = "";
                                        try
                                        {
                                            cookie = cookies[0].Value;
                                        }
                                        catch
                                        {
                                            CookieGet?.Invoke(Base.ErrorType.cookie_cannot_be_resolved);
                                            return;
                                        }
                                        CookieGet?.Invoke(Base.ErrorType.no_error, cookie);
                                        return;
                                    }
                                    else
                                    {
                                        CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                                        return;
                                    }
                                }
                                else
                                {
                                    CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                                    return;
                                }
                            }
                            else
                            {
                                CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                                return;
                            }
                        }
                        else
                        {
                            CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                            return;
                        }
                    }
                    else
                    {
                        CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                        return;
                    }
                }
                else
                {
                    CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                    return;
                }
            }
            else
            {
                CookieGet?.Invoke(Base.ErrorType.network_cannot_be_reached_or_session_token_is_invalid_or_expired);
                return;
            }
        }

        /// <summary>
        /// Sort players.
        /// Players are sorted by paint/sort, kill + assist, special, death, kill and nickname.
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
        /// RankedPlayers are sorted by paint/sort, kill + assist, special, death, kill and nickname.
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
            try
            {
                List<ScheduledStage> stages = new List<ScheduledStage>();
                Class.Mode mode = new Class.Mode(Class.Mode.ParseKey(node["game_mode"]["key"].ToString()), node["game_mode"]["name"].ToString());
                Rule rule = new Rule(Rule.ParseKey(node["rule"]["key"].ToString()), node["rule"]["name"].ToString());
                ScheduledStage stage1, stage2;
                if (int.TryParse(node["stage_a"]["id"].ToString(), out int stage1Id))
                {
                    string name = node["stage_a"]["name"].ToString();
                    string image = node["stage_a"]["image"].ToString();

                    stage1 = new ScheduledStage(mode, rule, (Stage.Key)stage1Id, name, image);
                }
                else
                {
                    stage1 = new ScheduledStage(mode, rule);
                }
                if (int.TryParse(node["stage_b"]["id"].ToString(), out int stage2Id))
                {
                    string name = node["stage_b"]["name"].ToString();
                    string image = node["stage_b"]["image"].ToString();
                    stage2 = new ScheduledStage(mode, rule, (Stage.Key)stage2Id, name, image);
                }
                else
                {
                    stage2 = new ScheduledStage(mode, rule);
                }
                stages.Add(stage1);
                stages.Add(stage2);
                return stages;
            }
            catch
            {
                throw new FormatException(Base.ErrorType.stage_cannot_be_resolved.ToString());
            }
        }
        /// <summary>
        /// Parse ShiftStage from JToken
        /// </summary>
        /// <param name="node">JToken of a detail schedule of salmon run</param>
        /// <returns></returns>
        private static ShiftStage parseShiftStage(JToken node)
        {
            try
            {
                string name = node["stage"]["name"].ToString();
                string image = node["stage"]["image"].ToString();
                DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(int.Parse(node["start_time"].ToString()));
                DateTime endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(int.Parse(node["end_time"].ToString()));
                List<Weapon> weapons = new List<Weapon>();
                JToken weaponsNode = node["weapons"];
                foreach (JToken weaponNode in weaponsNode)
                {
                    weapons.Add(parseShiftWeapon(weaponNode));
                }
                return ShiftStage.FromUrl(name, image, startTime, endTime, weapons);
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.shift_stage_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse wave from JToken
        /// </summary>
        /// <param name="node">JToken of a wave</param>
        /// <param name="specials">Special weapons used in the wave</param>
        /// <param name="result">Result of the wave</param>
        /// <returns></returns>
        private static Wave parseWave(JToken node, List<SpecialWeapon> specials, Job.ResultType result)
        {
            try
            {
                Wave.WaterLevelType waterLevel = Wave.ParseWaterLevel(node["water_level"]["key"].ToString());
                Wave.EventTypeType eventType = Wave.ParseEventType(node["event_type"]["key"].ToString());
                int quota = int.Parse(node["quota_num"].ToString());
                int powerEgg = int.Parse(node["ikura_num"].ToString());
                int goldenEgg = int.Parse(node["golden_ikura_num"].ToString());
                int goldenEggPop = int.Parse(node["golden_ikura_pop_num"].ToString());
                return new Wave(waterLevel, eventType, quota, powerEgg, goldenEgg, goldenEggPop, specials, result);
            }
            catch
            {
                throw new FormatException(Base.ErrorType.wave_cannot_be_resolved.ToString());
            }
        }
        /// <summary>
        /// Parse player from JToken
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
                JToken playerTypeNode = playerNode["player_type"];
                Player.SpeciesType species = Player.ParseSpecies(playerTypeNode["species"].ToString());
                Player.StyleType style = Player.ParseStyle(playerTypeNode["style"].ToString());
                int level = int.Parse(playerNode["player_rank"].ToString()) + 100 * int.Parse(playerNode["star_rank"].ToString());
                // Parse weapon
                Weapon weapon = parseWeapon(playerNode["weapon"]);
                // Parse gear
                HeadGear headGear = parseGear(playerNode["head"], playerNode["head_skills"], Gear.KindType.Head) as HeadGear;
                ClothesGear clothesGear = parseGear(playerNode["clothes"], playerNode["clothes_skills"], Gear.KindType.Clothes) as ClothesGear;
                ShoesGear shoesGear = parseGear(playerNode["shoes"], playerNode["shoes_skills"], Gear.KindType.Shoes) as ShoesGear;
                return new Player(id, nickname, species, style, level, headGear, clothesGear, shoesGear, weapon, paint, kill, assist, death, special, sort, image, isSelf);
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.player_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse player from JToken
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
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.player_cannot_be_resolved.ToString());
                }
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
                bool isSPlus = int.TryParse(node["player"]["udemae"]["s_plus_number"].ToString(), out _);
                if (isSPlus)
                {
                    rank = Rank.ParseKey(node["player"]["udemae"]["name"].ToString(), int.Parse(node["player"]["udemae"]["s_plus_number"].ToString()));
                }
                else
                {
                    rank = Rank.ParseKey(node["player"]["udemae"]["name"].ToString());
                }
                return new RankedPlayer(player, rank);
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.player_cannot_be_resolved.ToString());
                }
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
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.player_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse JobPlayer from JToken
        /// </summary>
        /// <param name="node">JToken of a player in salmon run</param>
        /// <param name="image">Url of the user icon</param>
        /// <param name="isSelf">If the player is player itself</param>
        /// <param name="grade">Grade of the player</param>
        /// <param name="gradePoint">Grade point of the player</param>
        /// <returns></returns>
        private static JobPlayer parseJobPlayer(JToken node, string image, bool isSelf = false, JobPlayer.GradeType grade = JobPlayer.GradeType.intern, int gradePoint = 0)
        {
            try
            {
                if (image == "")
                {
                    throw new ArgumentNullException();
                }
                string id = node["pid"].ToString();
                string nickname = node["name"].ToString();
                JobPlayer.SpeciesType species = JobPlayer.ParseSpecies(node["player_type"]["species"].ToString());
                JobPlayer.StyleType style = JobPlayer.ParseStyle(node["player_type"]["style"].ToString());
                List<Weapon> weapons = new List<Weapon>();
                foreach (JToken weaponNode in node["weapon_list"])
                {
                    weapons.Add(parseJobWeapon(weaponNode["weapon"], node["special"]));
                }
                List<int> specialWeaponCount = new List<int>();
                foreach (JToken count in node["special_counts"])
                {
                    specialWeaponCount.Add(int.Parse(count.ToString()));
                }
                int steelheadKill = int.Parse(node["boss_kill_counts"]["6"]["count"].ToString());
                int flyfishKill = int.Parse(node["boss_kill_counts"]["9"]["count"].ToString());
                int steelEelKill = int.Parse(node["boss_kill_counts"]["13"]["count"].ToString());
                int drizzlerKill = int.Parse(node["boss_kill_counts"]["21"]["count"].ToString());
                int stingerKill = int.Parse(node["boss_kill_counts"]["14"]["count"].ToString());
                int scrapperKill = int.Parse(node["boss_kill_counts"]["12"]["count"].ToString());
                int mawsKill = int.Parse(node["boss_kill_counts"]["15"]["count"].ToString());
                int grillerKill = int.Parse(node["boss_kill_counts"]["16"]["count"].ToString());
                int goldieKill = int.Parse(node["boss_kill_counts"]["3"]["count"].ToString());
                int help = int.Parse(node["help_count"].ToString());
                int dead = int.Parse(node["dead_count"].ToString());
                int powerEgg = int.Parse(node["ikura_num"].ToString());
                int goldenEgg = int.Parse(node["golden_ikura_num"].ToString());
                return new JobPlayer(id, nickname, species, style, grade, gradePoint, weapons, specialWeaponCount,
                    steelheadKill, flyfishKill, steelEelKill, drizzlerKill, stingerKill, mawsKill, grillerKill, goldieKill, help, dead, powerEgg, goldenEgg, image, isSelf);
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.job_player_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse JobPlayer from JToken
        /// </summary>
        /// <param name="node">JToken of a player in salmon run</param>
        /// <param name="isSelf">If the player is player itself</param>
        /// <param name="grade">Grade of the player</param>
        /// <param name="gradePoint">Grade point of the player</param>
        /// <returns></returns>
        private static async Task<JobPlayer> parseJobPlayer(JToken node, bool isSelf = false, JobPlayer.GradeType grade = JobPlayer.GradeType.intern, int gradePoint = 0)
        {
            try
            {
                string image = await GetPlayerIcon(node["pid"].ToString()).ConfigureAwait(false);
                JobPlayer player = parseJobPlayer(node, image, isSelf, grade, gradePoint);
                return player;
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.job_player_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse weapon from JToken
        /// </summary>
        /// <param name="node">JToken of a weapon</param>
        /// <returns></returns>
        private static Weapon parseWeapon(JToken node)
        {
            try
            {
                SubWeapon sub = parseSubWeapon(node["sub"]);
                SpecialWeapon special = parseSpecialWeapon(node["special"]);
                return new Weapon((Weapon.Key)int.Parse(node["id"].ToString()), node["name"].ToString(), sub, special, node["image"].ToString());
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.weapon_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse shift weapon from JToken
        /// </summary>
        /// <param name="node">JToken of a weapon in salmon run schedule</param>
        /// <returns></returns>
        private static Weapon parseShiftWeapon(JToken node)
        {
            try
            {
                JToken weaponNode;
                if (int.Parse(node["id"].ToString()) < 0)
                {
                    weaponNode = node["coop_special_weapon"];
                    return new Weapon((Weapon.Key)int.Parse(node["id"].ToString()), weaponNode["name"].ToString(), null, null, weaponNode["image"].ToString());
                }
                else
                {
                    weaponNode = node["weapon"];
                    return new Weapon((Weapon.Key)int.Parse(weaponNode["id"].ToString()), weaponNode["name"].ToString(), null, null, weaponNode["image"].ToString());
                }
            }
            catch
            {
                throw new FormatException(Base.ErrorType.shift_weapon_cannot_be_resolved.ToString());
            }
        }
        /// <summary>
        /// Parse job weapon from JToken
        /// </summary>
        /// <param name="node">JToken of a weapon in salmon run</param>
        /// <param name="specialNode">JToken of a special weapon in salmon run</param>
        /// <returns></returns>
        private static Weapon parseJobWeapon(JToken node, JToken specialNode)
        {
            try
            {
                SpecialWeapon special = parseSpecialWeapon(specialNode);
                return new Weapon((Weapon.Key)int.Parse(node["id"].ToString()), node["name"].ToString(), null, special, node["image"].ToString());
            }
            catch (Exception ex)
            {
                if (Base.ParseErrorType(ex.Message) == Base.ErrorType.special_weapon_cannot_be_resolved)
                {
                    throw new FormatException(Base.ErrorType.job_special_weapon_cannot_be_resolved.ToString());
                }
                else if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.job_weapon_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse sub weapon from JToken
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
                throw new FormatException(Base.ErrorType.sub_weapon_cannot_be_resolved.ToString());
            }
        }
        /// <summary>
        /// Parse special weapon from JToken
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
                throw new FormatException(Base.ErrorType.special_weapon_cannot_be_resolved.ToString());
            }
        }
        /// <summary>
        /// Parse gear from JToken
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
                PrimaryAbility primaryAbility = parsePrimaryAbility(skillNode["main"]);
                List<SecondaryAbility> secondaryAbilities = new List<SecondaryAbility>();
                foreach (JToken subNode in skillNode["subs"])
                {
                    if (subNode.HasValues)
                    {
                        secondaryAbilities.Add(parseSecondaryAbility(subNode));
                    }
                }
                switch (kind)
                {
                    case Gear.KindType.Head:
                        HeadGear headGear = new HeadGear((HeadGear.Key)int.Parse(gearNode["id"].ToString()), gearNode["name"].ToString(), brand, primaryAbility, secondaryAbilities, gearNode["image"].ToString());
                        return headGear as Gear;
                    case Gear.KindType.Clothes:
                        ClothesGear clothesGear = new ClothesGear((ClothesGear.Key)int.Parse(gearNode["id"].ToString()), gearNode["name"].ToString(), brand, primaryAbility, secondaryAbilities, gearNode["image"].ToString());
                        return clothesGear as Gear;
                    case Gear.KindType.Shoes:
                        ShoesGear shoesGear = new ShoesGear((ShoesGear.Key)int.Parse(gearNode["id"].ToString()), gearNode["name"].ToString(), brand, primaryAbility, secondaryAbilities, gearNode["image"].ToString());
                        return shoesGear as Gear;
                    default:
                        throw new ArgumentOutOfRangeException(Base.ErrorType.gear_cannot_be_resolved.ToString());
                }
            }
            catch (Exception ex)
            {
                if (Base.TryParseErrorType(ex.Message, out _))
                {
                    throw new FormatException(ex.Message);
                }
                else
                {
                    throw new FormatException(Base.ErrorType.gear_cannot_be_resolved.ToString());
                }
            }
        }
        /// <summary>
        /// Parse primary ability from JToken
        /// </summary>
        /// <param name="node">JToken of a main skill</param>
        /// <returns></returns>
        private static PrimaryAbility parsePrimaryAbility(JToken node)
        {
            try
            {
                return new PrimaryAbility((PrimaryAbility.Key)int.Parse(node["id"].ToString()), node["image"].ToString());
            }
            catch
            {
                throw new FormatException(Base.ErrorType.primary_ability_cannot_be_resolved.ToString());
            }
        }
        /// <summary>
        /// Parse secondary ability from JToken
        /// </summary>
        /// <param name="node">JToken of a sub skill</param>
        /// <returns></returns>
        private static SecondaryAbility parseSecondaryAbility(JToken node)
        {
            try
            {
                return new SecondaryAbility((SecondaryAbility.Key)int.Parse(node["id"].ToString()), node["image"].ToString());
            }
            catch
            {
                throw new FormatException(Base.ErrorType.secondary_ability_cannot_be_resolved.ToString());
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
