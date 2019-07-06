using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace IkasLib
{
    public delegate void DownloadCompletedEventHandler();
    public class Downloader
    {
        public enum SourceType
        {
            Schedule,
            Battle,
            Player,
            Gear,
            Weapon
        }

        public string Url { get; }
        public string To { get; }
        public SourceType Source { get; }
        public WebProxy Proxy { get; }
        private bool isActive;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }
        private bool isSuccess;
        public bool IsSuccess
        {
            get
            {
                if (IsActive)
                {
                    return false;
                }
                else
                {
                    return isSuccess;
                }
            }
        }

        public event DownloadCompletedEventHandler DownloadCompleted;
        public event DownloadCompletedEventHandler DownloadSucceeded;

        public Downloader(string url, string to, SourceType source, WebProxy proxy = null)
        {
            Url = url;
            To = to;
            Source = source;
            Proxy = proxy;
            isActive = false;
            isSuccess = false;
        }

        public async void DownloadAsync()
        {
            isActive = true;
            // Create folder of To if not exists
            if (!Directory.Exists(Path.GetDirectoryName(To)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(To));
            }
            WebClient client = new WebClient();
            if (Proxy != null)
            {
                client.Proxy = Proxy;
            }
            try
            {
                await client.DownloadFileTaskAsync(Url, To);
            }
            catch
            {
                isActive = false;
                isSuccess = false;
                DownloadCompleted?.Invoke();
                return;
            }
            isActive = false;
            isSuccess = true;
            DownloadSucceeded?.Invoke();
            DownloadCompleted?.Invoke();
        }
    }

    public class DownloadManager
    {
        private Mutex DownloadersMutex;
        private List<Downloader> downloaders;
        public List<Downloader> Downloaders
        {
            get
            {
                return downloaders;
            }
        }

        public DownloadManager()
        {
            DownloadersMutex = new Mutex();
            DownloadersMutex.WaitOne();
            downloaders = new List<Downloader>();
            DownloadersMutex.ReleaseMutex();
        }

        /// <summary>
        /// Add a Downloader to the DownloadManager and start downloading.
        /// </summary>
        /// <param name="downloader">The Downloader which is going to be added</param>
        /// <param name="handler">Handler for DownloadSucceeded event</param>
        /// <returns></returns>
        public bool AddDownloader(Downloader downloader, DownloadCompletedEventHandler handler)
        {
            DownloadersMutex.WaitOne();
            // Clean up inactive download
            downloaders.RemoveAll(p => !p.IsActive);
            foreach (Downloader d in Downloaders)
            {
                if (d.To == downloader.To)
                {
                    d.DownloadSucceeded += handler;
                    DownloadersMutex.ReleaseMutex();
                    return false;
                }
            }
            downloader.DownloadCompleted += new DownloadCompletedEventHandler(() =>
            {
                RemoveDownloader(downloader);
            });
            downloader.DownloadSucceeded += handler;
            downloaders.Add(downloader);
            // Start download
            DownloadersMutex.ReleaseMutex();
            downloader.DownloadAsync();
            return true;
        }
        /// <summary>
        /// Remove Downloaders with certain Source.
        /// </summary>
        /// <param name="source">The matching Source</param>
        public void RemoveDownloaders(Downloader.SourceType source)
        {
            DownloadersMutex.WaitOne();
            // Remove all handlers of DownloadCompleted event before removing
            foreach (Downloader d in downloaders.FindAll(p => p.Source == source))
            {
                FieldInfo fi = null;
                Type type = d.GetType();
                while (d.GetType() != null)
                {
                    // Find events defined as field 
                    fi = type.GetField("DownloadSucceeded", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (fi != null && (fi.FieldType == typeof(MulticastDelegate) || fi.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                    {
                        break;
                    }
                    // Find events defined as property
                    fi = type.GetField("EVENT_" + "DownloadSucceeded".ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (fi != null)
                    {
                        break;
                    }
                    type = type.BaseType;
                }
                if (fi != null)
                {
                    fi.SetValue(d, null);
                }
            }
            // Remove inactive downloaders
            downloaders.RemoveAll(p => p.Source == source && !p.IsActive);
            DownloadersMutex.ReleaseMutex();
        }

        /// <summary>
        /// Remove a Downloader.
        /// </summary>
        /// <param name="downloader">The Downloader which is going to be removed</param>
        private void RemoveDownloader(Downloader downloader)
        {
            DownloadersMutex.WaitOne();
            // Remove all handlers of DownloaCompleted event before removing
            FieldInfo fi = null;
            Type type = downloader.GetType();
            while (downloader.GetType() != null)
            {
                // Find events defined as field 
                fi = type.GetField("DownloadSucceeded", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null && (fi.FieldType == typeof(MulticastDelegate) || fi.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
                {
                    break;
                }
                // Find events defined as property
                fi = type.GetField("EVENT_" + "DownloadSucceeded".ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fi != null)
                {
                    break;
                }
                type = type.BaseType;
            }
            if (fi != null)
            {
                fi.SetValue(downloader, null);
            }
            // Remove downloader if inactive
            if (!downloader.IsActive)
            {
                downloaders.Remove(downloader);
            }
            DownloadersMutex.ReleaseMutex();
        }
    }
}
