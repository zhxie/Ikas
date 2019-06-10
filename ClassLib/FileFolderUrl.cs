using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace ClassLib
{
    public static class FileFolderUrl
    {
        #region File

        public const string Config = @"\config.txt";

        #endregion

        #region Folder

        public static string ApplicationData
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + Assembly.GetEntryAssembly().GetName().Name;
            }
        }

        #endregion

        #region URL

        public const string SplatNet = @"https://app.splatoon2.nintendo.net";
        public const string SplatNetScheduleApi = @"/api/schedules";
        public const string SplatNetBattleApi = @"/api/results/{}";

        #endregion
    }
}
