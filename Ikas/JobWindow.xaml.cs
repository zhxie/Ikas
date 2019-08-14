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
    /// JobWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JobWindow : Window
    {
        private DispatcherTimer tmLoading;
        private int loadingRotationAngle;

        public JobWindow()
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
            RenderOptions.SetBitmapScalingMode(stg, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.JobChanged += new ContentChangedEventHandler(JobChanged);
            Depot.JobFound += new ContentFoundEventHandler(JobFound);
            Depot.JobUpdated += new ContentUpdatedEventHandler(JobUpdated);
            Depot.JobNotifying += new ContentNotifyingHandler(JobNotifying);
            // TODO: Prepare icon and weapon window
            // TODO: Create timers
            loadingRotationAngle = 0;
            tmLoading = new DispatcherTimer();
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

        private void JobChanged()
        {
            // TODO: Fade in loading
            // bdLoading.IsHitTestVisible = true;
            // ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
        }

        private void JobFound()
        {
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(tagResult);
            ((Storyboard)FindResource("fade_out")).Begin(stg);
            ((Storyboard)FindResource("fade_out")).Begin(lbGrade);
            ((Storyboard)FindResource("fade_out")).Begin(lbGradePoint);
            ((Storyboard)FindResource("fade_out")).Begin(lbHazardLevelName);
            ((Storyboard)FindResource("fade_out")).Begin(lbHazardLevel);
            ((Storyboard)FindResource("fade_out")).Begin(lbGrizzcoPointsName);
            ((Storyboard)FindResource("fade_out")).Begin(lbGrizzcoPoints);
            ((Storyboard)FindResource("fade_out")).Begin(wave1);
            ((Storyboard)FindResource("fade_out")).Begin(wave2);
            ((Storyboard)FindResource("fade_out")).Begin(wave3);
            wave1.SetWave(null);
            wave2.SetWave(null);
            wave3.SetWave(null);
        }

        private void JobUpdated()
        {
            Job job = Depot.Job;
            if (job.Stage != null)
            {
                // Update current job
                if (job.Result == Job.ResultType.clear)
                {
                    tagResult.Content = Translate("clear", true);
                }
                else
                {
                    tagResult.Content = Translate("defeat", true);
                }
                lbGrade.Content = Translate(job.Grade.ToString());
                lbGradePoint.Content = job.GradePoint.ToString();
                if (job.HazardLevel == 200)
                {
                    lbHazardLevel.Content = Translate("max", true);
                }
                else
                {
                    lbHazardLevel.Content = string.Format("{0}{1}", job.HazardLevel.ToString(), Translate("%", true));
                }
                lbGrizzcoPoints.Content = string.Format(Translate("{0}_X_{1}_=_{2}", true), job.Score, string.Format("{0}{1}", job.Rate, Translate("%", true)), job.GrizzcoPoint);
                ((Storyboard)FindResource("fade_in")).Begin(imgMode);
                ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                ((Storyboard)FindResource("fade_in")).Begin(tagResult);
                ((Storyboard)FindResource("fade_in")).Begin(lbGrade);
                ((Storyboard)FindResource("fade_in")).Begin(lbGradePoint);
                ((Storyboard)FindResource("fade_in")).Begin(lbHazardLevelName);
                ((Storyboard)FindResource("fade_in")).Begin(lbHazardLevel);
                ((Storyboard)FindResource("fade_in")).Begin(lbGrizzcoPointsName);
                ((Storyboard)FindResource("fade_in")).Begin(lbGrizzcoPoints);
                // Update stage
                Stage stage = job.Stage;
                string image = FileFolderUrl.ApplicationData + stage.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    stg.Background = brush;
                    stg.Content = Translate((stage.Id).ToString());
                    ((Storyboard)FindResource("fade_in")).Begin(stg);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Battle, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg.Background = brush;
                        stg.Content = Translate((stage.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(stg);
                    }));
                }
                // Update waves
                wave1.SetWave(job.Waves[0], 1);
                ((Storyboard)FindResource("fade_in")).Begin(wave1);
                if (job.Waves.Count > 1)
                {
                    wave2.Width = 140;
                    wave2.Margin = new Thickness(10, 0, 0, 0);
                    wave2.SetWave(job.Waves[1], 2);
                    ((Storyboard)FindResource("fade_in")).Begin(wave2);
                    if (job.Waves.Count > 2)
                    {
                        wave3.Width = 140;
                        wave3.Margin = new Thickness(10, 0, 0, 0);
                        wave3.SetWave(job.Waves[2], 3);
                        ((Storyboard)FindResource("fade_in")).Begin(wave3);
                    }
                    else
                    {
                        wave3.Width = 0;
                        wave3.Margin = new Thickness(0);
                    }
                }
                else
                {
                    wave2.Width = 0;
                    wave2.Margin = new Thickness(0);
                    wave3.Width = 0;
                    wave3.Margin = new Thickness(0);
                }
            }
            // TODO: Fade out loading
            // ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
            // bdLoading.IsHitTestVisible = false;
        }

        private void JobNotifying()
        {

        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("job_window-" + s);
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
