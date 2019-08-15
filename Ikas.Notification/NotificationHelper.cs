using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Ikas.Notification.DesktopNotificationManager;

namespace Ikas.Notification
{
    public static class NotificationHelper
    {
        /// <summary>
        /// Initialize toast notification.
        /// </summary>
        public static void InitializeNotification()
        {
            // Register AUMID, COM server, and activator
            DesktopNotificationManagerCompat.RegisterAumidAndComServer<IkasNotificationActivator>(IkasNotificationActivator.AppId);
            DesktopNotificationManagerCompat.RegisterActivator<IkasNotificationActivator>();
        }

        /// <summary>
        /// Send text toast notification.
        /// </summary>
        /// <param name="title">Title of the toast notification</param>
        /// <param name="content">Content of the toast notification</param>
        /// <param name="icon">Icon of the toast notification</param>
        /// <param name="iconCrop">Hint crop of the icon of the toast notification</param>
        public static void SendTextNotification(string title, string content, string icon, bool iconCrop = false)
        {
            // Construct the toast content
            ToastContent toastContent = new ToastContent
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = new Uri(Path.GetFullPath(icon)).AbsoluteUri,
                            HintCrop = iconCrop ? ToastGenericAppLogoCrop.Circle : ToastGenericAppLogoCrop.Default
                        }
                    }
                }
            };
            // Send the toast notification
            SendNotification(toastContent);
        }

        /// <summary>
        /// Send text and image toast notification.
        /// </summary>
        /// <param name="title">Title of the toast notification</param>
        /// <param name="content">Content of the toast notification</param>
        /// <param name="image">Image of the toast notification</param>
        /// <param name="icon">Icon of the toast notification</param>
        /// <param name="iconCrop">Hint crop of the icon of the toast notification</param>
        public static void SendTextAndImageNotification(string title, string content, string image, string icon, bool iconCrop = false)
        {
            // Construct the toast content
            ToastContent toastContent = new ToastContent
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            },
                            new AdaptiveImage()
                            {
                                Source = image
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = new Uri(Path.GetFullPath(icon)).AbsoluteUri,
                            HintCrop = iconCrop ? ToastGenericAppLogoCrop.Circle : ToastGenericAppLogoCrop.Default
                        }
                    }
                }
            };
            // Send the toast notification
            SendNotification(toastContent);
        }

        /// <summary>
        /// Send progress bar toast notification.
        /// </summary>
        /// <param name="title">Title of the toast notification</param>
        /// <param name="progressTitle">Title of the progress of the toast notification</param>
        /// <param name="status">Status of the progress of the toast notification</param>
        /// <param name="value">Value of the progress of the toast notification</param>
        /// <param name="valueString">Value string of the progress of the toast notification</param>
        /// <param name="icon">Icon of the toast notification</param>
        /// <param name="iconCrop">Hint crop of the icon of the toast notification</param>
        public static void SendProgressBarNotification(string title, string progressTitle, string status, double value, string valueString, string icon, bool iconCrop = false)
        {
            // Construct the toast content
            ToastContent toastContent = new ToastContent
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title
                            },
                            new AdaptiveProgressBar()
                            {
                                Title = progressTitle,
                                Value = value,
                                ValueStringOverride = valueString,
                                Status = status
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = new Uri(Path.GetFullPath(icon)).AbsoluteUri,
                            HintCrop = iconCrop ? ToastGenericAppLogoCrop.Circle : ToastGenericAppLogoCrop.Default
                        }
                    }
                }
            };
            // Send the toast notification
            SendNotification(toastContent);
        }

        /// <summary>
        /// Send text and progress bar toast notification.
        /// </summary>
        /// <param name="title">Title of the toast notification</param>
        /// <param name="content">Content of the toast notification</param>
        /// <param name="progressTitle">Title of the progress of the toast notification</param>
        /// <param name="status">Status of the progress of the toast notification</param>
        /// <param name="value">Value of the progress of the toast notification</param>
        /// <param name="valueString">Value string of the progress of the toast notification</param>
        /// <param name="icon">Icon of the toast notification</param>
        /// <param name="iconCrop">Hint crop of the icon of the toast notification</param>
        public static void SendTextAndProgressBarNotification(string title, string content, string progressTitle, string status, double value, string valueString, string icon, bool iconCrop = false)
        {
            // Construct the toast content
            ToastContent toastContent = new ToastContent
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            },
                            new AdaptiveProgressBar()
                            {
                                Title = progressTitle,
                                Value = value,
                                ValueStringOverride = valueString,
                                Status = status
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = new Uri(Path.GetFullPath(icon)).AbsoluteUri,
                            HintCrop = iconCrop ? ToastGenericAppLogoCrop.Circle : ToastGenericAppLogoCrop.Default
                        }
                    }
                }
            };
            // Send the toast notification
            SendNotification(toastContent);
        }

        /// <summary>
        /// Send toast notification.
        /// </summary>
        /// <param name="toastContent">XML document of the toast notification</param>
        private static void SendNotification(ToastContent toastContent)
        {
            // Make sure to use Windows.Data.Xml.Dom
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(toastContent.GetContent());

            // And create the toast notification
            ToastNotification toast = new ToastNotification(doc);

            // And then show it
            DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);
        }
    }
}
