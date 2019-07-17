using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Windows;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Ikas.Notification.DesktopNotificationManager;

namespace Ikas.Notification
{
    // The GUID CLSID must be unique to your app. Create a new GUID if copying this code.
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("a6f70341-f83c-4ba8-97de-1c6947715679"), ComVisible(true)]
    public class IkasNotificationActivator : NotificationActivator
    {
        public const string AppId = "Ikas";

        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            
        }
    }
}
