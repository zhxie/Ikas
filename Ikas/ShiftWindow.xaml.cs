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
        private DispatcherTimer tmLoading;
        private int loadingRotationAngle;

        public string OrangeForeground
        {
            get
            {
                return "#FF" + Design.NeonOrange;
            }
        }

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
            RenderOptions.SetBitmapScalingMode(bdWeapon11, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon12, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon13, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon14, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(stg2, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon21, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon22, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon23, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon24, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.ShiftChanged += new ContentChangedEventHandler(ShiftChanged);
            Depot.ShiftUpdated += new ContentUpdatedEventHandler(ShiftUpdated);
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

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            ((Storyboard)FindResource("window_fade_in")).Begin(this);
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(this);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
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

        private void ShiftChanged()
        {
            // Fade in loading
            bdLoading.IsHitTestVisible = true;
            ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(tagStatus1);
            ((Storyboard)FindResource("fade_out")).Begin(lbTime1);
            ((Storyboard)FindResource("fade_out")).Begin(stg1);
            ((Storyboard)FindResource("fade_out")).Begin(lbWeapon1);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon11);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon12);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon13);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon14);
            ((Storyboard)FindResource("fade_out")).Begin(tagStatus2);
            ((Storyboard)FindResource("fade_out")).Begin(lbTime2);
            ((Storyboard)FindResource("fade_out")).Begin(stg2);
            ((Storyboard)FindResource("fade_out")).Begin(lbWeapon2);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon21);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon22);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon23);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon24);
        }

        private void ShiftUpdated()
        {
            Shift shift = Depot.Shift;
            if (shift.Stages.Count > 0)
            {
                if (shift.IsOpen)
                {
                    tagStatus1.Content = Translate("open", true);
                    tagStatus2.Content = Translate("next", true);
                }
                else
                {
                    tagStatus1.Content = Translate("next", true);
                    tagStatus2.Content = Translate("future", true);
                }
            }
            // Fade in labels
            ((Storyboard)FindResource("fade_in")).Begin(imgMode);
            ((Storyboard)FindResource("fade_in")).Begin(lbMode);
            ((Storyboard)FindResource("fade_in")).Begin(tagStatus1);
            ((Storyboard)FindResource("fade_in")).Begin(tagStatus2);
            // Update shift
            ShiftStage stage = shift.Stages[0];
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
                stg1.Content = Translate((stage.Id).ToString());
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
                    stg1.Content = Translate((stage.Id).ToString());
                    ((Storyboard)FindResource("fade_in")).Begin(stg1);
                }));
            }
            if (stage.Weapons.Count > 0)
            {
                // Update weapon
                Weapon weapon11 = stage.Weapons[0];
                string image2 = FileFolderUrl.ApplicationData + weapon11.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdWeapon11.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon11);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon11.Image, image2, Downloader.SourceType.Shift, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdWeapon11.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdWeapon11);
                    }));
                }
                if (stage.Weapons.Count > 1)
                {
                    // Update weapon 2
                    Weapon weapon12 = stage.Weapons[1];
                    string image3 = FileFolderUrl.ApplicationData + weapon12.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdWeapon12.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdWeapon12);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon12.Image, image3, Downloader.SourceType.Shift, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdWeapon12.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdWeapon12);
                        }));
                    }
                    if (stage.Weapons.Count > 2)
                    {
                        // Update weapon 3
                        Weapon weapon13 = stage.Weapons[2];
                        string image4 = FileFolderUrl.ApplicationData + weapon13.Image;
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image4)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdWeapon13.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdWeapon13);
                        }
                        catch
                        {
                            // Download the image
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon13.Image, image4, Downloader.SourceType.Shift, Depot.Proxy);
                            DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image4)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdWeapon14.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdWeapon14);
                            }));
                        }
                        if (stage.Weapons.Count > 3)
                        {
                            // Update weapon 4
                            Weapon weapon14 = stage.Weapons[3];
                            string image5 = FileFolderUrl.ApplicationData + weapon14.Image;
                            try
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image5)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdWeapon14.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdWeapon14);
                            }
                            catch
                            {
                                // Download the image
                                Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon14.Image, image5, Downloader.SourceType.Shift, Depot.Proxy);
                                DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image5)));
                                    brush.Stretch = Stretch.UniformToFill;
                                    bdWeapon14.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon14);
                                }));
                            }
                        }
                    }
                }
            }
            if (shift.Stages.Count > 1)
            {
                // Update next shift
                ShiftStage stage2 = shift.Stages[1];
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
                    stg2.Content = Translate((stage2.Id).ToString());
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
                        stg2.Content = Translate((stage2.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(stg2);
                    }));
                }
                if (stage2.Weapons.Count > 0)
                {
                    // Update next weapon
                    Weapon weapon21 = stage2.Weapons[0];
                    string image7 = FileFolderUrl.ApplicationData + weapon21.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image7)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdWeapon21.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdWeapon21);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon21.Image, image7, Downloader.SourceType.Shift, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image7)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdWeapon21.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdWeapon21);
                        }));
                    }
                    if (stage2.Weapons.Count > 1)
                    {
                        // Update next weapon 2
                        Weapon weapon22 = stage2.Weapons[1];
                        string image8 = FileFolderUrl.ApplicationData + weapon22.Image;
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image8)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdWeapon22.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdWeapon22);
                        }
                        catch
                        {
                            // Download the image
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon22.Image, image8, Downloader.SourceType.Shift, Depot.Proxy);
                            DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image8)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdWeapon22.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdWeapon22);
                            }));
                        }
                        if (stage2.Weapons.Count > 2)
                        {
                            // Update next weapon 3
                            Weapon weapon23 = stage2.Weapons[2];
                            string image9 = FileFolderUrl.ApplicationData + weapon23.Image;
                            try
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image9)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdWeapon23.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdWeapon23);
                            }
                            catch
                            {
                                // Download the image
                                Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon23.Image, image9, Downloader.SourceType.Shift, Depot.Proxy);
                                DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image9)));
                                    brush.Stretch = Stretch.UniformToFill;
                                    bdWeapon23.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon23);
                                }));
                            }
                            if (stage2.Weapons.Count > 3)
                            {
                                // Update next weapon 4
                                Weapon weapon24 = stage2.Weapons[3];
                                string image10 = FileFolderUrl.ApplicationData + weapon24.Image;
                                try
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image10)));
                                    brush.Stretch = Stretch.UniformToFill;
                                    bdWeapon24.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon24);
                                }
                                catch
                                {
                                    // Download the image
                                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + weapon24.Image, image10, Downloader.SourceType.Shift, Depot.Proxy);
                                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                    {
                                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image10)));
                                        brush.Stretch = Stretch.UniformToFill;
                                        bdWeapon24.Background = brush;
                                        ((Storyboard)FindResource("fade_in")).Begin(bdWeapon24);
                                    }));
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

        private void CookieUpdated()
        {
            // Update shift
            Depot.ForceGetShift();
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
