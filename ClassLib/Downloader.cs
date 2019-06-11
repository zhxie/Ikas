using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace ClassLib
{
    public delegate void DownloadCompletedEventHandler();
    public class Downloader
    {
        public enum SourceType
        {
            Schedule,
            Battle,
            Player,
            Weapon
        }

        public string Url { get; }
        public string To { get; }
        public SourceType Source { get; }
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

        public Downloader(string url, string to)
        {
            Url = url;
            To = to;
            isActive = false;
            isSuccess = false;
        }

        public async void DownloadAsync()
        {
            isActive = true;
            // Create folder of To if not exists
            if (!Directory.Exists(FileFolderUrl.GetFolder(To)))
            {
                Directory.CreateDirectory(FileFolderUrl.GetFolder(To));
            }
            WebClient client = new WebClient();
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

        public bool AddDownloader(Downloader downloader, DownloadCompletedEventHandler handler)
        {
            DownloadersMutex.WaitOne();
            // Clean up inactive download
            downloaders.RemoveAll(p => p.IsActive);
            foreach (Downloader d in Downloaders)
            {
                if (d.To == downloader.To)
                {
                    d.DownloadSucceeded += handler;
                }
                DownloadersMutex.ReleaseMutex();
                return false;
            }
            downloader.DownloadCompleted += new DownloadCompletedEventHandler(() =>
            {
                RemoveDownloader(downloader);
            });
            downloader.DownloadSucceeded += handler;
            downloaders.Add(downloader);
            // Start download
            downloader.DownloadAsync();
            DownloadersMutex.ReleaseMutex();
            return true;
        }
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
