using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.IO;
using System.Windows.Media.Animation;

using ClassLib;

namespace Ikas
{
    /// <summary>
    /// ScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ScheduleWindow()
        {
            // Load language
            if (Depot.Language != null)
            {
                try
                {
                    ResourceDictionary lang = (ResourceDictionary)Application.LoadComponent(new Uri(@"assets/lang/" + Depot.Language + ".xaml", UriKind.Relative));
                    if (Resources.MergedDictionaries.Count > 0)
                    {
                        Resources.MergedDictionaries.Clear();
                    }
                    Resources.MergedDictionaries.Add(lang);
                }
                catch { }
            }
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(imgMode, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage2, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdNextStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdNextStage2, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.CurrentModeChanged += new CurrentModeChangedEventHandler(CurrentModeChanged);
            Depot.ScheduleUpdated += new ScheduleUpdatedEventHandler(ScheduleUpdated);
        }

        #region Control Event

        #endregion

        private void CurrentModeChanged()
        {
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbRule);
            ((Storyboard)FindResource("fade_out")).Begin(lbTime);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage1);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage2);
            ((Storyboard)FindResource("fade_out")).Begin(lbStage1Name);
            ((Storyboard)FindResource("fade_out")).Begin(lbStage2Name);
            ((Storyboard)FindResource("fade_out")).Begin(bdNext);
            ((Storyboard)FindResource("fade_out")).Begin(lbNextRule);
            ((Storyboard)FindResource("fade_out")).Begin(lbNextTime);
            ((Storyboard)FindResource("fade_out")).Begin(bdNextStage1);
            ((Storyboard)FindResource("fade_out")).Begin(bdNextStage2);
            ((Storyboard)FindResource("fade_out")).Begin(lbNextStage1Name);
            ((Storyboard)FindResource("fade_out")).Begin(lbNextStage2Name);
            // Update Schedule
            Depot.GetSchedule();
        }

        private void ScheduleUpdated()
        {
            Schedule schedule = Depot.Schedule;
            // Update current Schedule
            List<ScheduledStage> scheduledStages = schedule.GetStages(Depot.CurrentMode);
            if (scheduledStages.Count > 0)
            {
                // Change UI
                switch (Depot.CurrentMode)
                {
                    case Mode.Key.regular_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_regular");
                        bdNext.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                        break;
                    case Mode.Key.ranked_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_ranked");
                        bdNext.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                        break;
                    case Mode.Key.league_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_league");
                        bdNext.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                lbMode.Content = Translate(scheduledStages[0].Mode.ToString());
                lbRule.Content = Translate(scheduledStages[0].Rule.ToString());
                DateTime startTime = schedule.EndTime.AddHours(-2).ToLocalTime();
                DateTime endTime = schedule.EndTime.ToLocalTime();
                lbTime.Content = string.Format(Translate("{0} - {1}", true), startTime.ToString("HH:mm"), endTime.ToString("HH:mm"));
                // Fade in labels
                ((Storyboard)FindResource("fade_in")).Begin(imgMode);
                ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                ((Storyboard)FindResource("fade_in")).Begin(lbRule);
                ((Storyboard)FindResource("fade_in")).Begin(lbTime);
                ((Storyboard)FindResource("fade_in")).Begin(bdNext);
                // Update Stages
                Stage stage = scheduledStages[0];
                string image = FileFolderUrl.ApplicationData + stage.Image;
                if (File.Exists(image))
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdStage1.Background = brush;
                    lbStage1Name.Content = Translate(((Stage.Key)stage.Id).ToString());
                    ((Storyboard)FindResource("fade_in")).Begin(bdStage1);
                    ((Storyboard)FindResource("fade_in")).Begin(lbStage1Name);
                }
                else
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                    Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage1.Background = brush;
                        lbStage1Name.Content = Translate(((Stage.Key)stage.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(bdStage1);
                        ((Storyboard)FindResource("fade_in")).Begin(lbStage1Name);
                    }));
                }
                if (scheduledStages.Count > 1)
                {
                    stage = scheduledStages[1];
                    image = FileFolderUrl.ApplicationData + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage2.Background = brush;
                        lbStage2Name.Content = Translate(((Stage.Key)stage.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(bdStage2);
                        ((Storyboard)FindResource("fade_in")).Begin(lbStage2Name);
                    }
                    else
                    {
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdStage2.Background = brush;
                            lbStage2Name.Content = Translate(((Stage.Key)stage.Id).ToString());
                            ((Storyboard)FindResource("fade_in")).Begin(bdStage2);
                            ((Storyboard)FindResource("fade_in")).Begin(lbStage2Name);
                        }));
                    }
                }
            }
            // Update next Schedule
            List<ScheduledStage> nextScheduledStages = schedule.GetNextStages(Depot.CurrentMode);
            if (nextScheduledStages.Count > 0)
            {
                // Change UI
                lbNextRule.Content = Translate(nextScheduledStages[0].Rule.ToString());
                TimeSpan dTime = schedule.EndTime - DateTime.UtcNow;
                if (dTime.Hours > 0)
                {
                    lbNextTime.Content = string.Format(Translate("in {0} hour {1} min", true), dTime.Hours, dTime.Minutes);
                }
                else
                {
                    lbNextTime.Content = string.Format(Translate("in {0} min", true), dTime.Minutes);
                }
                // Fade in labels
                ((Storyboard)FindResource("fade_in")).Begin(lbNextRule);
                ((Storyboard)FindResource("fade_in")).Begin(lbNextTime);
                // Update Stages
                Stage stage = nextScheduledStages[0];
                string image = FileFolderUrl.ApplicationData + stage.Image;
                if (File.Exists(image))
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdNextStage1.Background = brush;
                    lbNextStage1Name.Content = Translate(((Stage.ShortName)stage.Id).ToString());
                    ((Storyboard)FindResource("fade_in")).Begin(bdNextStage1);
                    ((Storyboard)FindResource("fade_in")).Begin(lbNextStage1Name);
                }
                else
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                    Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdNextStage1.Background = brush;
                        lbNextStage1Name.Content = Translate(((Stage.ShortName)stage.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(bdNextStage1);
                        ((Storyboard)FindResource("fade_in")).Begin(lbNextStage1Name);
                    }));
                }
                if (nextScheduledStages.Count > 1)
                {
                    stage = nextScheduledStages[1];
                    image = FileFolderUrl.ApplicationData + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdNextStage2.Background = brush;
                        lbNextStage2Name.Content = Translate(((Stage.ShortName)stage.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(bdNextStage2);
                        ((Storyboard)FindResource("fade_in")).Begin(lbNextStage2Name);
                    }
                    else
                    {
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdNextStage2.Background = brush;
                            lbNextStage2Name.Content = Translate(((Stage.ShortName)stage.Id).ToString());
                            ((Storyboard)FindResource("fade_in")).Begin(bdNextStage2);
                            ((Storyboard)FindResource("fade_in")).Begin(lbNextStage2Name);
                        }));
                    }
                }
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("schedule_window-" + s);
                }
                else
                {
                    return (string)FindResource(s);
                }
            }
            catch
            {
                return s;
            }
        }
    }
}
