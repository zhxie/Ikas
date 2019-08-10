using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Ikas
{
    public delegate void DownloadCompletedEventHandler();
    public class Downloader
    {
        public enum SourceType
        {
            Schedule,
            SalmonRunSchedule,
            Battle,
            SalmonRunBattle,
            Player,
            Gear,
            Weapon
        }

        public string Url { get; }
        public string To { get; }
        public SourceType Source { get; }
        public WebProxy Proxy { get; }
        public bool IsActive { get; private set; }
        private bool isFinished;
        public bool IsFinished
        {
            get
            {
                if (IsActive)
                {
                    return false;
                }
                else
                {
                    return isFinished;
                }
            }
        }
        private bool isSuccess;
        public bool IsSuccess
        {
            get
            {
                if (!IsFinished)
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
            IsActive = false;
            isFinished = false;
            isSuccess = false;
        }

        /// <summary>
        /// Download asynchronously.
        /// </summary>
        /// <param name="isSafeDownload">Download to temporary directory instead of assigned directory before download completed</param>
        public async void DownloadAsync(bool isSafeDownload = true)
        {
            if (!IsActive)
            {
                IsActive = true;
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
                    if (isSafeDownload)
                    {
                        string tempTo = Path.GetTempFileName();
                        await client.DownloadFileTaskAsync(Url, tempTo);
                        File.Move(tempTo, To);
                    }
                    else
                    {
                        await client.DownloadFileTaskAsync(Url, To);
                    }
                }
                catch
                {
                    IsActive = false;
                    isFinished = true;
                    isSuccess = false;
                    DownloadCompleted?.Invoke();
                    return;
                }
                IsActive = false;
                isFinished = true;
                isSuccess = true;
                DownloadSucceeded?.Invoke();
                DownloadCompleted?.Invoke();
            }
        }
    }

    public static class DownloadHelper
    {
        public static int MaxDownload { get; set; } = 0;

        private static Mutex DownloadersMutex = new Mutex();
        public static List<Downloader> Downloaders { get; } = new List<Downloader>();

        /// <summary>
        /// Add a Downloader to Downloaders.
        /// </summary>
        /// <param name="downloader">The Downloader which is going to be added</param>
        /// <param name="handler">Handler for DownloadSucceeded event</param>
        /// <returns></returns>
        public static bool AddDownloader(Downloader downloader, DownloadCompletedEventHandler handler)
        {
            DownloadersMutex.WaitOne();
            // Clean up finished downloader
            int removedCount = Downloaders.RemoveAll(p => p.IsFinished);
            // Add downloader
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
            // Add to downloaders
            Downloaders.Add(downloader);
            DownloadersMutex.ReleaseMutex();
            // Start downloading if no limit
            if (MaxDownload == 0)
            {
                downloader.DownloadAsync();
            }
            // Check downloaders
            CheckDownloaders();
            return true;
        }
        /// <summary>
        /// Remove Downloaders with certain Source.
        /// </summary>
        /// <param name="source">The matching Source</param>
        public static void RemoveDownloaders(Downloader.SourceType source)
        {
            DownloadersMutex.WaitOne();
            bool isFind = false;
            // Remove all handlers of DownloadCompleted event before removing
            foreach (Downloader d in Downloaders.FindAll(p => p.Source == source))
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
                isFind = true;
            }
            // Remove inactive downloaders
            Downloaders.RemoveAll(p => p.Source == source && !p.IsActive);
            DownloadersMutex.ReleaseMutex();
            // Check downloaders
            if (isFind)
            {
                CheckDownloaders();
            }
        }

        /// <summary>
        /// Remove a Downloader.
        /// </summary>
        /// <param name="downloader">The Downloader which is going to be removed</param>
        private static void RemoveDownloader(Downloader downloader)
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
                Downloaders.Remove(downloader);
            }
            DownloadersMutex.ReleaseMutex();
            // Check downloaders
            CheckDownloaders();
        }
        /// <summary>
        /// Check Downloaders and start downloading.
        /// </summary>
        private static void CheckDownloaders()
        {
            // No limit
            if (MaxDownload == 0)
            {
                return;
            }
            DownloadersMutex.WaitOne();
            // Check downloaders
            List<Downloader> startingDownloaders = new List<Downloader>();
            for (int i = 0; i < Math.Min(Downloaders.Count, MaxDownload); ++i)
            {
                Downloader downloader = Downloaders[i];
                if (!downloader.IsActive && !downloader.IsFinished)
                {
                    startingDownloaders.Add(downloader);
                }
            }
            DownloadersMutex.ReleaseMutex();
            // Start downloading
            foreach (Downloader downloader in startingDownloaders)
            {
                downloader.DownloadAsync();
            }
        }
    }
}
