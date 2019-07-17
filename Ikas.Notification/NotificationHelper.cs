﻿using System;
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
        /// <param name="content">Content of te toast notification</param>
        public static void SendTextNotification(string title, string content)
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
                            Source = new Uri(Path.GetFullPath("Ikas.ico")).AbsoluteUri
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
        /// <param name="content">Content of te toast notification</param>
        /// <param name="image">Image of te toast notification</param>
        public static void SendTextAndImageNotification(string title, string content, string image)
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
                            Source = new Uri(Path.GetFullPath("Ikas.ico")).AbsoluteUri
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
