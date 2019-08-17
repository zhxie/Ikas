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

using System.Windows.Media.Animation;
using System.Windows.Threading;

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// ScheduleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        private DispatcherTimer tmLoading;
        private int loadingRotationAngle;

        public ScheduleWindow()
        {
            // Load language
            if (Depot.Language != null && Depot.Language != "")
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
            RenderOptions.SetBitmapScalingMode(stg1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(stg2, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(stgNext1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(stgNext2, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.ScheduleChanged += new ContentChangedEventHandler(ScheduleChanged);
            Depot.ScheduleUpdated += new ContentUpdatedEventHandler(ScheduleUpdated);
            Depot.CookieUpdated += new CookieUpdatedEventHandler(CookieUpdated);
            // Create timers
            loadingRotationAngle = 0;
            tmLoading = new DispatcherTimer();
            tmLoading.Tick += new EventHandler((object source, EventArgs e) =>
            {
                imgLoading.RenderTransform = new RotateTransform(loadingRotationAngle, imgLoading.Source.Width / 2, imgLoading.Source.Height / 2);
                if (loadingRotationAngle >= 359)
                {
                    loadingRotationAngle = 0;
                }
                else
                {
                    loadingRotationAngle++;
                }
            });
            tmLoading.Interval = new TimeSpan(0, 0, 0, 0, 10);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start timers
            tmLoading.Start();
        }

        #region Control Event

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_in")).Begin(this);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(this);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            /*
            if (Top < 0)
            {
                Top = 0;
            }
            if (Top + Height > WpfScreen.GetScreenFrom(this).DeviceBounds.Height)
            {
                Top = WpfScreen.GetScreenFrom(this).DeviceBounds.Height - Height;
            }
            */
        }

        #endregion

        private void LanguageChanged()
        {
            ResourceDictionary lang = (ResourceDictionary)Application.LoadComponent(new Uri(@"assets/lang/" + Depot.Language + ".xaml", UriKind.Relative));
            if (Resources.MergedDictionaries.Count > 0)
            {
                Resources.MergedDictionaries.Clear();
            }
            Resources.MergedDictionaries.Add(lang);
        }

        private void ScheduleChanged()
        {
            // Fade in loading
            bdLoading.IsHitTestVisible = true;
            ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbRule);
            ((Storyboard)FindResource("fade_out")).Begin(lbTime);
            ((Storyboard)FindResource("fade_out")).Begin(stg1);
            ((Storyboard)FindResource("fade_out")).Begin(stg2);
            ((Storyboard)FindResource("fade_out")).Begin(tagNext);
            ((Storyboard)FindResource("fade_out")).Begin(lbNextRule);
            ((Storyboard)FindResource("fade_out")).Begin(lbNextTime);
            ((Storyboard)FindResource("fade_out")).Begin(stgNext1);
            ((Storyboard)FindResource("fade_out")).Begin(stgNext2);
        }

        private void ScheduleUpdated()
        {
            Schedule schedule = Depot.Schedule;
            Mode.Key mode = Mode.Key.regular_battle;
            switch (Depot.CurrentMode)
            {
                case Depot.Mode.regular_battle:
                case Depot.Mode.salmon_run:
                    mode = Mode.Key.regular_battle;
                    break;
                case Depot.Mode.ranked_battle:
                    mode = Mode.Key.ranked_battle;
                    break;
                case Depot.Mode.league_battle:
                    mode = Mode.Key.league_battle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // Update current Schedule
            List<ScheduledStage> scheduledStages = schedule.GetStages(mode);
            if (scheduledStages.Count > 0)
            {
                // Change UI
                switch (mode)
                {
                    case Mode.Key.regular_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_regular");
                        tagNext.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                        break;
                    case Mode.Key.ranked_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_ranked");
                        tagNext.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                        break;
                    case Mode.Key.league_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_league");
                        tagNext.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (Depot.TranslateProperNoun)
                {
                    lbMode.Content = Translate(scheduledStages[0].Mode.Id.ToString());
                    lbRule.Content = Translate(scheduledStages[0].Rule.Id.ToString());
                }
                else
                {
                    lbMode.Content = scheduledStages[0].Mode.Name;
                    lbRule.Content = scheduledStages[0].Rule.Name;
                }
                DateTime startTime = schedule.EndTime.AddHours(-2).ToLocalTime();
                DateTime endTime = schedule.EndTime.ToLocalTime();
                lbTime.Content = string.Format(Translate("{0}_-_{1}", true), startTime.ToString("HH:mm"), endTime.ToString("HH:mm"));
                // Fade in labels
                ((Storyboard)FindResource("fade_in")).Begin(imgMode);
                ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                ((Storyboard)FindResource("fade_in")).Begin(lbRule);
                ((Storyboard)FindResource("fade_in")).Begin(lbTime);
                ((Storyboard)FindResource("fade_in")).Begin(tagNext);
                // Update stages
                Stage stage = scheduledStages[0];
                string image = FileFolderUrl.ApplicationData + stage.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    stg1.Background = brush;
                    if (Depot.TranslateProperNoun)
                    {
                        stg1.Content = Translate(stage.Id.ToString());
                    }
                    else
                    {
                        stg1.Content = stage.Name;
                    }
                    ((Storyboard)FindResource("fade_in")).Begin(stg1);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg1.Background = brush;
                        if (Depot.TranslateProperNoun)
                        {
                            stg1.Content = Translate(stage.Id.ToString());
                        }
                        else
                        {
                            stg1.Content = stage.Name;
                        }
                        ((Storyboard)FindResource("fade_in")).Begin(stg1);
                    }));
                }
                if (scheduledStages.Count > 1)
                {
                    Stage stage2 = scheduledStages[1];
                    string image2 = FileFolderUrl.ApplicationData + stage2.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg2.Background = brush;
                        if (Depot.TranslateProperNoun)
                        {
                            stg2.Content = Translate(stage2.Id.ToString());
                        }
                        else
                        {
                            stg2.Content = stage2.Name;
                        }
                        ((Storyboard)FindResource("fade_in")).Begin(stg2);
                    }
                    catch
                    {
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage2.Image, image2, Downloader.SourceType.Schedule, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.UniformToFill;
                            stg2.Background = brush;
                            if (Depot.TranslateProperNoun)
                            {
                                stg2.Content = Translate(stage2.Id.ToString());
                            }
                            else
                            {
                                stg2.Content = stage2.Name;
                            }
                            ((Storyboard)FindResource("fade_in")).Begin(stg2);
                        }));
                    }
                }
            }
            // Update next schedule
            List<ScheduledStage> nextScheduledStages = schedule.GetNextStages(mode);
            if (nextScheduledStages.Count > 0)
            {
                // Change UI
                if (Depot.TranslateProperNoun)
                {
                    lbNextRule.Content = Translate(nextScheduledStages[0].Rule.Id.ToString());
                }
                else
                {
                    lbNextRule.Content = nextScheduledStages[0].Rule.Name;
                }
                TimeSpan dTime = schedule.EndTime - DateTime.UtcNow;
                if (dTime.Hours > 0)
                {
                    lbNextTime.Content = string.Format(Translate("in_{0}_hour_{1}_min", true), dTime.Hours, dTime.Minutes);
                }
                else
                {
                    lbNextTime.Content = string.Format(Translate("in_{0}_min", true), dTime.Minutes);
                }
                // Fade in labels
                ((Storyboard)FindResource("fade_in")).Begin(lbNextRule);
                ((Storyboard)FindResource("fade_in")).Begin(lbNextTime);
                // Update Stages
                Stage stage = nextScheduledStages[0];
                string image = FileFolderUrl.ApplicationData + stage.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    stgNext1.Background = brush;
                    if (Depot.TranslateProperNoun)
                    {
                        stgNext1.Content = Translate(stage.Id.ToString());
                    }
                    else
                    {
                        stgNext1.Content = stage.Name;
                    }
                    ((Storyboard)FindResource("fade_in")).Begin(stgNext1);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stgNext1.Background = brush;
                        if (Depot.TranslateProperNoun)
                        {
                            stgNext1.Content = Translate(stage.Id.ToString());
                        }
                        else
                        {
                            stgNext1.Content = stage.Name;
                        }
                        ((Storyboard)FindResource("fade_in")).Begin(stgNext1);
                    }));
                }
                if (nextScheduledStages.Count > 1)
                {
                    Stage stage2 = nextScheduledStages[1];
                    string image2 = FileFolderUrl.ApplicationData + stage2.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                        brush.Stretch = Stretch.UniformToFill;
                        stgNext2.Background = brush;
                        if (Depot.TranslateProperNoun)
                        {
                            stgNext2.Content = Translate(stage2.Id.ToString());
                        }
                        else
                        {
                            stgNext2.Content = stage2.Name;
                        }
                        ((Storyboard)FindResource("fade_in")).Begin(stgNext2);
                    }
                    catch
                    {
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage2.Image, image2, Downloader.SourceType.Schedule, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.UniformToFill;
                            stgNext2.Background = brush;
                            if (Depot.TranslateProperNoun)
                            {
                                stgNext2.Content = Translate(stage2.Id.ToString());
                            }
                            else
                            {
                                stgNext2.Content = stage2.Name;
                            }
                            ((Storyboard)FindResource("fade_in")).Begin(stgNext2);
                        }));
                    }
                }
            }
            // Fade out loading
            ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
            bdLoading.IsHitTestVisible = false;
        }

        private void CookieUpdated()
        {
            // Update schedule
            Depot.ForceGetSchedule();
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
