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
    [Guid("16f7ed7a-c091-418c-8fc4-b447ff9cefb0"), ComVisible(true)]
    public class MyNotificationActivator : NotificationActivator
    {
        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
        {
            
        }
    }
}
