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
    /// ShiftWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ShiftWindow : Window
    {
        public string OrangeForeground
        {
            get
            {
                return "#FF" + Design.NeonOrange;
            }
        }

        private Shift Shift;

        private WeaponWindow weaponWindow;

        private DispatcherTimer tmLoading;
        private int loadingRotationAngle;

        public ShiftWindow()
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
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            // Prepare weapon window
            weaponWindow = new WeaponWindow();
            weaponWindow.KeepAliveWindow = this;
            weaponWindow.Opacity = 0;
            weaponWindow.Visibility = Visibility.Hidden;
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

        private void Weapon_MouseEnter(object sender, MouseEventArgs e)
        {
            Weapon weapon = (sender as WeaponControl).Weapon;
            if (weapon != null)
            {
                weaponWindow.Top = e.GetPosition(this).Y + Top - weaponWindow.Height / 2;
                weaponWindow.Left = e.GetPosition(this).X + Left - 10 - weaponWindow.Width;
                // Restrict in this window
                if (Top - weaponWindow.Top > 30)
                {
                    weaponWindow.Top = Top - 30;
                }
                if (Left - weaponWindow.Left > 40)
                {
                    weaponWindow.Left = Left - 40;
                }
                if (weaponWindow.Top + weaponWindow.Height - (Top + Height) > 30)
                {
                    weaponWindow.Top = Top + Height - weaponWindow.Height + 30;
                }
                if (weaponWindow.Left + weaponWindow.Width - (Left + Width) > 30)
                {
                    weaponWindow.Left = Left + Width - weaponWindow.Width + 30;
                }
                weaponWindow.SetWeapon(weapon, false);
                ((Storyboard)FindResource("window_fade_in")).Begin(weaponWindow);
            }
        }

        private void Weapon_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(weaponWindow);
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
            if (Shift != null)
            {
                if (Shift.Stages.Count > 0)
                {
                    ShiftStage stage = Shift.Stages[0];
                    DateTime startTime = stage.StartTime.ToLocalTime();
                    DateTime endTime = stage.EndTime.ToLocalTime();
                    lbTime1.Content = string.Format(Translate("{0}_-_{1}", true), startTime.ToString("M/dd HH:mm"), endTime.ToString("M/dd HH:mm"));
                    if (Shift.Stages.Count > 1)
                    {
                        ShiftStage stage2 = Shift.Stages[1];
                        startTime = stage2.StartTime.ToLocalTime();
                        endTime = stage2.EndTime.ToLocalTime();
                        lbTime2.Content = string.Format(Translate("{0}_-_{1}", true), startTime.ToString("M/dd HH:mm"), endTime.ToString("M/dd HH:mm"));
                    }
                }
            }
        }

        public void SetShift(Shift shift)
        {
            Shift = shift;
            // Fade in loading
            bdLoading.IsHitTestVisible = true;
            ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(tagOpenOrSoon);
            ((Storyboard)FindResource("fade_out")).Begin(lbTime1);
            ((Storyboard)FindResource("fade_out")).Begin(stg1);
            ((Storyboard)FindResource("fade_out")).Begin(lbWeapon1);
            ((Storyboard)FindResource("fade_out")).Begin(tagNext);
            ((Storyboard)FindResource("fade_out")).Begin(lbTime2);
            ((Storyboard)FindResource("fade_out")).Begin(stg2);
            ((Storyboard)FindResource("fade_out")).Begin(lbWeapon2);
            wp11.SetWeapon(null);
            wp12.SetWeapon(null);
            wp13.SetWeapon(null);
            wp14.SetWeapon(null);
            wp21.SetWeapon(null);
            wp22.SetWeapon(null);
            wp23.SetWeapon(null);
            wp24.SetWeapon(null);
            if (Shift != null)
            {
                // Update shift
                if (Shift.Stages.Count > 0)
                {
                    if (Shift.IsOpen)
                    {
                        tagOpenOrSoon.SetResourceReference(TagControl.ContentProperty, "shift_window-open");
                    }
                    else
                    {
                        tagOpenOrSoon.SetResourceReference(TagControl.ContentProperty, "shift_window-soon");
                    }
                    // Fade in labels
                    ((Storyboard)FindResource("fade_in")).Begin(imgMode);
                    ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                    ((Storyboard)FindResource("fade_in")).Begin(tagOpenOrSoon);
                    ((Storyboard)FindResource("fade_in")).Begin(tagNext);
                    ShiftStage stage = Shift.Stages[0];
                    DateTime startTime = stage.StartTime.ToLocalTime();
                    DateTime endTime = stage.EndTime.ToLocalTime();
                    lbTime1.Content = string.Format(Translate("{0}_-_{1}", true), startTime.ToString("M/dd HH:mm"), endTime.ToString("M/dd HH:mm"));
                    ((Storyboard)FindResource("fade_in")).Begin(lbTime1);
                    ((Storyboard)FindResource("fade_in")).Begin(lbWeapon1);
                    // Update stage
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg1.Background = brush;
                        stg1.SetResourceReference(StageControl.ContentProperty, stage.Id.ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(stg1);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Shift, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            stg1.Background = brush;
                            stg1.SetResourceReference(StageControl.ContentProperty, stage.Id.ToString());
                            ((Storyboard)FindResource("fade_in")).Begin(stg1);
                        }));
                    }
                    if (stage.Weapons.Count > 0)
                    {
                        // Update weapons
                        wp11.SetWeapon(stage.Weapons[0]);
                        if (stage.Weapons.Count > 1)
                        {
                            wp12.SetWeapon(stage.Weapons[1]);
                            if (stage.Weapons.Count > 2)
                            {
                                wp13.SetWeapon(stage.Weapons[2]);
                                if (stage.Weapons.Count > 3)
                                {
                                    wp14.SetWeapon(stage.Weapons[3]);
                                }
                            }
                        }
                    }
                    if (Shift.Stages.Count > 1)
                    {
                        // Update next shift
                        ShiftStage stage2 = Shift.Stages[1];
                        startTime = stage2.StartTime.ToLocalTime();
                        endTime = stage2.EndTime.ToLocalTime();
                        lbTime2.Content = string.Format(Translate("{0}_-_{1}", true), startTime.ToString("M/dd HH:mm"), endTime.ToString("M/dd HH:mm"));
                        ((Storyboard)FindResource("fade_in")).Begin(lbTime2);
                        ((Storyboard)FindResource("fade_in")).Begin(lbWeapon2);
                        // Update next stage
                        string image6 = FileFolderUrl.ApplicationData + stage2.Image;
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image6)));
                            brush.Stretch = Stretch.UniformToFill;
                            stg2.Background = brush;
                            stg2.SetResourceReference(StageControl.ContentProperty, stage2.Id.ToString());
                            ((Storyboard)FindResource("fade_in")).Begin(stg2);
                        }
                        catch
                        {
                            // Download the image
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage2.Image, image6, Downloader.SourceType.Shift, Depot.Proxy);
                            DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image6)));
                                brush.Stretch = Stretch.UniformToFill;
                                stg2.Background = brush;
                                stg2.SetResourceReference(StageControl.ContentProperty, stage2.Id.ToString());
                                ((Storyboard)FindResource("fade_in")).Begin(stg2);
                            }));
                        }
                        if (stage2.Weapons.Count > 0)
                        {
                            // Update weapons
                            wp21.SetWeapon(stage2.Weapons[0]);
                            if (stage2.Weapons.Count > 1)
                            {
                                wp22.SetWeapon(stage2.Weapons[1]);
                                if (stage2.Weapons.Count > 2)
                                {
                                    wp23.SetWeapon(stage2.Weapons[2]);
                                    if (stage2.Weapons.Count > 3)
                                    {
                                        wp24.SetWeapon(stage2.Weapons[3]);
                                    }
                                }
                            }
                        }
                    }
                }
                // Fade out loading
                ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
                bdLoading.IsHitTestVisible = false;
            }
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
                    return (string)FindResource("shift_window-" + s);
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
