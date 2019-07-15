using System;
using System.Windows;

using System.Runtime.InteropServices;

using Ikas.Notification.ShellHelpers;

namespace Ikas.Notification
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("16F7ED7A-C091-418C-8FC4-B447FF9CEFB0"), ComVisible(true)]
    public class NotificationActivator : INotificationActivationCallback
    {
        public void Activate(string appUserModelId, string invokedArgs, NOTIFICATION_USER_INPUT_DATA[] data, uint dataCount)
        {
            NotificationHelper.InvokeToastActivated();
        }

        public static void Initialize()
        {
            regService = new RegistrationServices();

            cookie = regService.RegisterTypeForComClients(
                typeof(NotificationActivator),
                RegistrationClassContext.LocalServer,
                RegistrationConnectionType.MultipleUse);
        }
        public static void Uninitialize()
        {
            if (cookie != -1 && regService != null)
            {
                regService.UnregisterTypeForComClients(cookie);
            }
        }

        private static int cookie = -1;
        private static RegistrationServices regService = null;
    }
}
