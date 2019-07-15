using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ikas.Notification
{
    public delegate void ToastActivatedEventHandler();
    public static class NotificationHelper
    {
        public static event ToastActivatedEventHandler ToastActivated = null;

        public static void InvokeToastActivated()
        {
            if (ToastActivated != null)
            {
                ToastActivated.Invoke();
            }
        }
    }
}
