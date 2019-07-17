using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

using Ikas.Notification.ShellHelpers;

namespace Ikas.Notification
{
    public static class NotificationHelper
    {
        public const string APP_ID = "Ikas";

        /// <summary>
        /// Initialize toast notification.
        /// </summary>
        public static void InitializeNotification()
        {
            RegisterAppForNotificationSupport();
            NotificationActivator.Initialize();
        }

        /// <summary>
        /// Send toast notification with icon, title, and at most 2 lines of text.
        /// </summary>
        /// <param name="title">Title of toast notification</param>
        /// <param name="line1">The first line of text of toast notification</param>
        /// <param name="line2">The second line of text of toast notification</param>
        public static void SendTextNotification(string title, string line1, string line2 = null)
        {
            // Get a toast XML template
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode(title));
            stringElements[1].AppendChild(toastXml.CreateTextNode(line1));
            if (line2 != null)
            {
                stringElements[2].AppendChild(toastXml.CreateTextNode(line2));
            }

            // Specify the absolute path to an image as a URI
            string imagePath = new System.Uri(Path.GetFullPath("Ikas.ico")).AbsoluteUri;
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml);

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }

        /// <summary>
        /// Send toast notification with icon, title, at most 2 lines of text and image.
        /// </summary>
        /// <param name="title">Title of toast notification</param>
        /// <param name="image">Image of toast notification</param>
        /// <param name="line1">The first line of text of toast notification</param>
        /// <param name="line2">The second line of text of toast notification</param>
        public static void SendTextAndImageNotification(string title, string image, string line1 = null, string line2 = null)
        {

        }

        // In order to display toasts, a desktop application must have a shortcut on the Start menu.
        // Also, an AppUserModelID must be set on that shortcut.
        //
        // For the app to be activated from Action Center, it needs to register a COM server with the OS
        // and register the CLSID of that COM server on the shortcut.
        //
        // The shortcut should be created as part of the installer. The following code shows how to create
        // a shortcut and assign the AppUserModelID and ToastActivatorCLSID properties using Windows APIs.
        //
        // Included in this project is a wxs file that be used with the WiX toolkit
        // to make an installer that creates the necessary shortcut. One or the other should be used.
        //
        // This sample doesn't clean up the shortcut or COM registration.

        private static void RegisterAppForNotificationSupport()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Ikas.lnk";
            if (!File.Exists(shortcutPath))
            {
                // Find the path to the current executable
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                InstallShortcut(shortcutPath, exePath);
                RegisterComServer(exePath);
            }
        }
        private static void InstallShortcut(string shortcutPath, string exePath)
        {
            IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

            // Create a shortcut to the exe
            newShortcut.SetPath(exePath);

            // Open the shortcut property store, set the AppUserModelId property
            IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

            PropVariantHelper varAppId = new PropVariantHelper();
            varAppId.SetValue(Ikas.Notification.NotificationHelper.APP_ID);
            newShortcutProperties.SetValue(PROPERTYKEY.AppUserModel_ID, varAppId.Propvariant);

            PropVariantHelper varToastId = new PropVariantHelper();
            varToastId.VarType = VarEnum.VT_CLSID;
            varToastId.SetValue(typeof(NotificationActivator).GUID);

            newShortcutProperties.SetValue(PROPERTYKEY.AppUserModel_ToastActivatorCLSID, varToastId.Propvariant);

            // Commit the shortcut to disk
            IPersistFile newShortcutSave = (IPersistFile)newShortcut;

            newShortcutSave.Save(shortcutPath, true);
        }
        private static void RegisterComServer(string exePath)
        {
            // We register the app process itself to start up when the notification is activated, but
            // other options like launching a background process instead that then decides to launch
            // the UI as needed.
            string regString = string.Format("SOFTWARE\\Classes\\CLSID\\{{{0}}}\\LocalServer32", typeof(NotificationActivator).GUID);
            var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(regString);
            key.SetValue(null, exePath);
        }
    }
}
