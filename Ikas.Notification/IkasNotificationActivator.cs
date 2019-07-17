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
#if DEBUG
    [Guid("a6f70341-f83c-4ba8-97de-1c6947715679"), ComVisible(true)]
#else
    [Guid("f9fa6e5b-ea57-4d16-9cf1-adeb57e110a1"), ComVisible(true)]
#endif
    public class IkasNotificationActivator : NotificationActivator
    {
#if DEBUG
        public const string AppId = "Ikas (Debug)";
#else
        public const string AppId = "Ikas";
#endif

        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            
        }
    }
}
