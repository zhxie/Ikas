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
        public string OrangeBackground
        {
            get
            {
                return "#3F" + Design.NeonOrange;
            }
        }

        private Job Job;

        private JobPlayerWindow jobPlayerWindow;
        private WeaponWindow weaponWindow;

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
            // Prepare icon and weapon window
            jobPlayerWindow = new JobPlayerWindow();
            jobPlayerWindow.KeepAliveWindow = this;
            jobPlayerWindow.Opacity = 0;
            jobPlayerWindow.Visibility = Visibility.Hidden;
            weaponWindow = new WeaponWindow();
            weaponWindow.KeepAliveWindow = this;
            weaponWindow.Opacity = 0;
            weaponWindow.Visibility = Visibility.Hidden;
            // Rotate loading
            ((Storyboard)FindResource("image_rotate")).Begin(imgLoading);
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

        private void Jp_MouseEnterIcon(object sender, MouseEventArgs e)
        {
            JobPlayer player = (sender as JobPlayerControl).Player;
            if (player != null)
            {
                jobPlayerWindow.Top = e.GetPosition(this).Y + Top - jobPlayerWindow.Height / 2;
                jobPlayerWindow.Left = e.GetPosition(this).X + Left + 10;
                // Restrict in this window
                if (Top - jobPlayerWindow.Top > 30)
                {
                    jobPlayerWindow.Top = Top - 30;
                }
                if (Left - jobPlayerWindow.Left > 30)
                {
                    jobPlayerWindow.Left = Left - 30;
                }
                if (jobPlayerWindow.Top + jobPlayerWindow.Height - (Top + Height) > 30)
                {
                    jobPlayerWindow.Top = Top + Height - jobPlayerWindow.Height + 30;
                }
                if (jobPlayerWindow.Left + jobPlayerWindow.Width - (Left + Width) > 30)
                {
                    jobPlayerWindow.Left = Left + Width - jobPlayerWindow.Width + 30;
                }
                jobPlayerWindow.SetPlayer(player, Depot.Job);
                ((Storyboard)FindResource("window_fade_in")).Begin(jobPlayerWindow);
            }
        }

        private void Jp_MouseLeaveIcon(object sender, MouseEventArgs e)
        {
            JobPlayer player = (sender as JobPlayerControl).Player;
            if (player != null)
            {
                ((Storyboard)FindResource("window_fade_out")).Begin(jobPlayerWindow);
            }
        }

        private void Jp_MouseEnterWeapon(object sender, MouseEventArgs e)
        {
            Weapon weapon = (sender as WeaponControl).Weapon;
            if (weapon != null)
            {
                weaponWindow.Top = e.GetPosition(this).Y + Top - weaponWindow.Height / 2;
                weaponWindow.Left = e.GetPosition(this).X + Left + 10;
                // Restrict in this window
                if (Top - weaponWindow.Top > 30)
                {
                    weaponWindow.Top = Top - 30;
                }
                if (Left - weaponWindow.Left > 90)
                {
                    weaponWindow.Left = Left - 90;
                }
                if (weaponWindow.Top + weaponWindow.Height - (Top + Height) > 30)
                {
                    weaponWindow.Top = Top + Height - weaponWindow.Height + 30;
                }
                if (weaponWindow.Left + weaponWindow.Width - (Left + Width) > 90)
                {
                    weaponWindow.Left = Left + Width - weaponWindow.Width + 90;
                }
                weaponWindow.SetWeapon(weapon, true);
                ((Storyboard)FindResource("window_fade_in")).Begin(weaponWindow);
            }
        }

        private void Jp_MouseLeaveWeapon(object sender, MouseEventArgs e)
        {
            Weapon weapon = (sender as WeaponControl).Weapon;
            if (weapon != null)
            {
                ((Storyboard)FindResource("window_fade_out")).Begin(weaponWindow);
            }
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
            // Force refresh labels
            if (Job != null)
            {
                if (Job.Stage != null)
                {
                    if (Job.HazardLevel != 200)
                    {
                        lbHazardLevel.Content = string.Format("{0}{1}", Job.HazardLevel.ToString(), Translate("%", true));
                    }
                    lbGrizzcoPoints.Content = string.Format(Translate("{0}_X_{1}_=_{2}", true), Job.Score, string.Format("{0}{1}", Job.Rate, Translate("%", true)), Job.GrizzcoPoint);
                }
            }
        }

        public void SetJob(Job job)
        {
            Job = job;
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
            Storyboard sb = (Storyboard)FindResource("resize_width");
            (sb.Children[0] as DoubleAnimation).To = 0;
            sb.Begin(bdHazardLevel);
            ((Storyboard)FindResource("fade_out")).Begin(bdHazardLevel);
            wave1.SetWave(null);
            wave2.SetWave(null);
            wave3.SetWave(null);
            jp1.SetPlayer(null);
            jp2.SetPlayer(null);
            jp3.SetPlayer(null);
            jp4.SetPlayer(null);
            if (Job != null)
            {
                if (Job.Stage != null)
                {
                    // Update current job
                    if (Job.Result == Job.ResultType.clear)
                    {
                        tagResult.SetResourceReference(TagControl.ContentProperty, "job_window-clear");
                    }
                    else
                    {
                        tagResult.SetResourceReference(TagControl.ContentProperty, "job_window-defeat");
                    }
                    lbGrade.SetResourceReference(ContentProperty, Job.Grade.ToString());
                    lbGradePoint.Content = Job.GradePoint.ToString();
                    if (Job.HazardLevel == 200)
                    {
                        lbHazardLevel.SetResourceReference(ContentProperty, "job_window-max");
                    }
                    else
                    {
                        lbHazardLevel.Content = string.Format("{0}{1}", Job.HazardLevel.ToString(), Translate("%", true));
                    }
                    lbGrizzcoPoints.Content = string.Format(Translate("{0}_X_{1}_=_{2}", true), Job.Score, string.Format("{0}{1}", Job.Rate, Translate("%", true)), Job.GrizzcoPoint);
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
                    Stage stage = Job.Stage;
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg.Background = brush;
                        stg.SetResourceReference(StageControl.ContentProperty, stage.Id.ToString());
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
                            stg.SetResourceReference(StageControl.ContentProperty, stage.Id.ToString());
                            ((Storyboard)FindResource("fade_in")).Begin(stg);
                        }));
                    }
                    // Update waves
                    wave1.SetWave(Job.Waves[0], 1);
                    ((Storyboard)FindResource("fade_in")).Begin(wave1);
                    if (Job.Waves.Count > 1)
                    {
                        wave2.Width = 140;
                        wave2.Margin = new Thickness(10, 0, 0, 0);
                        wave2.SetWave(Job.Waves[1], 2);
                        ((Storyboard)FindResource("fade_in")).Begin(wave2);
                        if (Job.Waves.Count > 2)
                        {
                            wave3.Width = 140;
                            wave3.Margin = new Thickness(10, 0, 0, 0);
                            wave3.SetWave(Job.Waves[2], 3);
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
                jp1.SetPlayer(Job.MyPlayer);
                if (Job.OtherPlayers.Count > 0)
                {
                    jp2.SetPlayer(Job.OtherPlayers[0]);
                    if (Job.OtherPlayers.Count > 1)
                    {
                        jp3.SetPlayer(Job.OtherPlayers[1]);
                        if (Job.OtherPlayers.Count > 2)
                        {
                            jp4.SetPlayer(Job.OtherPlayers[2]);
                        }
                    }
                }
                double to = (Job.HazardLevel / 200 * 0.95 + 0.05) * gridHazardLevel.ActualWidth;
                sb = (Storyboard)FindResource("resize_width");
                (sb.Children[0] as DoubleAnimation).To = to;
                sb.Begin(bdHazardLevel);
                ((Storyboard)FindResource("fade_in")).Begin(bdHazardLevel);
                // Fade out loading
                ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
                bdLoading.IsHitTestVisible = false;
            }
        }

        public void StartLoading()
        {
            // Fade in loading
            bdLoading.IsHitTestVisible = true;
            ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
        }

        public void StopLoading()
        {
            // Fade out loading
            ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
            bdLoading.IsHitTestVisible = false;
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
