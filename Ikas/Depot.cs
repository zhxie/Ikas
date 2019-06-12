using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json.Linq;

using ClassLib;

namespace Ikas
{
    public delegate void ScheduleUpdatedEventHandler();
    public delegate void CurrentModeChangedEventHandler();
    public static class Depot
    {
        private static string cookie = "";
        public static string Cookie
        {
            get
            {
                return cookie;
            }
        }

        private static WebProxy proxy = null;
        public static WebProxy Proxy
        {
            get
            {
                return proxy;
            }
        }
        public static DownloadManager DownloadManager { get; } = new DownloadManager();

        public static event ScheduleUpdatedEventHandler ScheduleUpdated;
        private static Mutex ScheduleMutex = new Mutex();
        public static Schedule Schedule { get; set; } = new Schedule();

        public static event CurrentModeChangedEventHandler CurrentModeChanged;
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
                CurrentModeChanged?.Invoke();
            }
        }

        /// <summary>
        /// Load user configuration from a file.
        /// </summary>
        /// <param name="file">The file of the user, the default one is user.ini</param>
        /// <returns></returns>
        public static bool LoadUserConfiguration(string file = FileFolderUrl.UserConfiguration)
        {
            string newFile = file;
            if (!File.Exists(newFile))
            {
                newFile = System.Environment.CurrentDirectory + newFile;
                if (!File.Exists(newFile))
                {
                    return false;
                }
            }
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile(newFile);
                cookie = data[FileFolderUrl.UserConfigurationUserSection][FileFolderUrl.UserConfigurationCookie].Trim();
                if (Cookie == "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
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
                IniData data = parser.ReadFile(System.Environment.CurrentDirectory + FileFolderUrl.SystemConfiguration);
                bool useProxy = bool.Parse(data[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxy].Trim());
                if (useProxy)
                {
                    string host = data[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxyHost].Trim();
                    int port = int.Parse(data[FileFolderUrl.SystemConfigurationNetworkSection][FileFolderUrl.SystemConfigurationUseProxyPort].Trim());
                    proxy = new WebProxy(host, port);
                }
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
                    DateTime endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(int.Parse(jObject["regular"][0]["end_time"].ToString()));
                    List<ScheduledStage> stages = new List<ScheduledStage>(), nextStages = new List<ScheduledStage>();
                    stages.AddRange(regularStages);
                    stages.AddRange(rankedStages);
                    stages.AddRange(leagueStages);
                    nextStages.AddRange(nextRegularStages);
                    nextStages.AddRange(nextRankedStages);
                    nextStages.AddRange(nextLeagueStages);
                    // Update Schedule
                    UpdateSchedule(new Schedule(endTime, stages, nextStages));
                }
                catch
                {
                    // Update Schedule
                    UpdateSchedule(new Schedule());
                }
            }
            else
            {
                // Update Schedule
                UpdateSchedule(new Schedule());
            }
        }
        /// <summary>
        /// Update Schedule.
        /// </summary>
        public static bool UpdateSchedule(Schedule schedule)
        {
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
        /// Parse ScheduledStages from JToken.
        /// </summary>
        /// <param name="node">JToken of a schedule of certain mode</param>
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
    }
}
