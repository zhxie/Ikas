using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// Reflection invocation class of Ikas.Notification.dll
    /// </summary>
    public static class NotificationHelper
    {
        public static Version osVersion;

        public static void InitializeNotification()
        {
            osVersion = Environment.OSVersion.Version;
            if (osVersion.Major > 10 || (osVersion.Major == 10 && osVersion.Minor >= 0))
            {
                Assembly assembly = Assembly.LoadFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.NotificationDll);
                Type type = assembly.GetType("Ikas.Notification.NotificationHelper");
                type.InvokeMember("InitializeNotification", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new object[] { });
            }
        }

        public static void SendTestNotification()
        {
            if (osVersion.Major > 10 || (osVersion.Major == 10 && osVersion.Minor >= 0))
            {
                SendTextNotification("Ikas", "Ikas、イカす！");
            }
        }

        public static void SendBattleNotification(string title, string content, string scoreTitle, string myScore, string otherScore, double scoreRatio)
        {
            if (osVersion.Major > 10 || (osVersion.Major == 10 && osVersion.Minor >= 0))
            {
                if (osVersion.Major > 10 || (osVersion.Major == 10 && osVersion.Minor > 0) || (osVersion.Major == 10 && osVersion.Minor == 0 && osVersion.Build >= 14393))
                {
                    SendTextAndProgressBarNotification(title, content, scoreTitle, myScore, scoreRatio, otherScore);
                }
                else
                {
                    SendTextNotification(title, scoreTitle);
                }
            }
        }

        private static void SendTextNotification(string title, string content)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.NotificationDll);
            Type type = assembly.GetType("Ikas.Notification.NotificationHelper");
            type.InvokeMember("SendTextNotification", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new string[] { title, content });
        }

        private static void SendTextNotification(string title, string content, string image)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.NotificationDll);
            Type type = assembly.GetType("Ikas.Notification.NotificationHelper");
            type.InvokeMember("SendTextAndImageNotification", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new string[] { title, content, image });
        }

        private static void SendTextAndProgressBarNotification(string title, string content, string progressTitle, string status, double value, string valueString)
        {
            Assembly assembly = Assembly.LoadFile(Directory.GetParent(Assembly.GetExecutingAssembly().Location) + FileFolderUrl.NotificationDll);
            Type type = assembly.GetType("Ikas.Notification.NotificationHelper");
            type.InvokeMember("SendTextAndProgressBarNotification", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new object[] { title, content, progressTitle, status, value, valueString });
        }
    }
}
