using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

namespace Ikas.Class
{
    public static class FileFolderUrl
    {
        #region File

        public const string UserConfiguration = @"\user.ini";
        public const string SystemConfiguration = @"\config.ini";

        public const string UserConfigurationGeneralSection = "Ikas";
        public const string UserConfigurationSessionToken = "SessionToken";
        public const string UserConfigurationCookie = "Cookie";
        public const string UserConfigurationStatisticsSection = "Statistics";
        public const string UserConfigurationLevel = "Level";
        public const string UserConfigurationSplatZonesRank = "SplatZonesRank";
        public const string UserConfigurationTowerControlRank = "TowerControlRank";
        public const string UserConfigurationRainmakerRank = "RainmakerRank";
        public const string UserConfigurationClamBlitzRank = "ClamBlitzRank";
        public const string SystemConfigurationGeneralSection = "Ikas";
        public const string SystemConfigurationLanguage = "Language";
        public const string SystemConfigurationInUse = "InUse";
        public const string SystemConfigurationAppearanceSection = "Appearance";
        public const string SystemConfigurationAlwaysOnTop = "AlwaysOnTop";
        public const string SystemConfigurationNotification = "Notification";
        public const string SystemConfigurationStartMode = "StartMode";
        public const string SystemConfigurationStartX = "StartX";
        public const string SystemConfigurationStartY = "StartY";
        public const string SystemConfigurationNetworkSection = "Network";
        public const string SystemConfigurationUseProxy = "UseProxy";
        public const string SystemConfigurationUseProxyHost = "UseProxyHost";
        public const string SystemConfigurationUseProxyPort = "UseProxyPort";

        public const string NotificationDll = @"\Ikas.Notification.dll";

        #endregion

        #region Folder

        public static string ApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + Assembly.GetEntryAssembly().GetName().Name;
            }
        }
        public const string IconFolder = @"\icon";

        #endregion

        #region URL

        public const string SplatNet = @"https://app.splatoon2.nintendo.net";
        public const string SplatNetScheduleApi = @"/api/schedules";
        public const string SplatNetSalmonRunScheduleApi = @"/api/coop_schedules";
        public const string SplatNetBattleApi = @"/api/results";
        public const string SplatNetIndividualBattleApi = @"/api/results/{0}";
        public const string SplatNetNicknameAndIconApi = @"/api/nickname_and_icon?id={0}";

        public const string NintendoAuthorize = @"https://accounts.nintendo.com/connect/1.0.0/authorize?state={0}&redirect_uri=npf71b963c1b7b6d119%3A%2F%2Fauth&client_id=71b963c1b7b6d119&scope=openid+user+user.birthday+user.mii+user.screenName&response_type=session_token_code&session_token_code_challenge={1}&session_token_code_challenge_method=S256&theme=login_form";
        public const string NintendoSessionToken = @"https://accounts.nintendo.com/connect/1.0.0/api/session_token";
        public const string NintendoToken = @"https://accounts.nintendo.com/connect/1.0.0/api/token";
        public const string NintendoUserInfo = @"https://api.accounts.nintendo.com/2.0.0/users/me";
        public const string eliFesslerApi = @"https://elifessler.com/s2s/api/gen2";
        public const string FlapgApi = @"https://flapg.com/ika2/api/login";
        public const string NintendoAccessToken = @"https://api-lp1.znc.srv.nintendo.net/v1/Account/Login";
        public const string NintendoSplatoonAccessToken = @"https://api-lp1.znc.srv.nintendo.net/v2/Game/GetWebServiceToken";

        public const string MitmInstruction = @"https://github.com/frozenpandaman/splatnet2statink/wiki/mitmproxy-instructions";

        #endregion
    }
}
