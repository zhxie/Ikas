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

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// WeaponWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WeaponWindow : Window
    {
        public Window KeepAliveWindow { get; set; }

        public volatile Weapon Weapon;

        public WeaponWindow()
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
            RenderOptions.SetBitmapScalingMode(bdWeapon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSubWeapon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecialWeapon, BitmapScalingMode.HighQuality);
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.CenterX = bdWeapon.Width / 2;
            rotateTransform.CenterY = bdWeapon.Height / 2;
            rotateTransform.Angle = -7.5;
            bdWeapon.LayoutTransform = rotateTransform;
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
        }

        #region Control Event

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (KeepAliveWindow != null)
            {
                ((Storyboard)FindResource("window_fade_in")).Begin(KeepAliveWindow);
            }
            // ((Storyboard)FindResource("window_fade_in")).Begin(this);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            // ((Storyboard)FindResource("window_fade_out")).Begin(this);
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

        public void SetWeapon(Weapon weapon)
        {
            // Remove previous Downloader's handlers (Believe every weapon has its sub weapon and special weapon, stop checking)
            //DownloadHelper.RemoveDownloaders(Downloader.SourceType.Weapon);
            Weapon = weapon;
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(tbName);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon);
            ((Storyboard)FindResource("fade_out")).Begin(bdSubWeapon);
            ((Storyboard)FindResource("fade_out")).Begin(bdSubWeapon);
            if (Weapon != null)
            {
                // Update weapon
                tbName.Text = Translate(Weapon.Id.ToString());
                ((Storyboard)FindResource("fade_in")).Begin(tbName);
                string image = FileFolderUrl.ApplicationData + Weapon.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.Uniform;
                    bdWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Weapon.Image, image, Downloader.SourceType.Weapon, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Weapon.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.Uniform;
                            bdWeapon.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                        }
                    }));
                }
                string image2 = FileFolderUrl.ApplicationData + Weapon.SubWeapon.Image1;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                    brush.Stretch = Stretch.Uniform;
                    bdSubWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdSubWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Weapon.SubWeapon.Image1, image2, Downloader.SourceType.Weapon, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Weapon.SubWeapon.Image1))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.Uniform;
                            bdSubWeapon.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdSubWeapon);
                        }
                    }));
                }
                string image3 = FileFolderUrl.ApplicationData + Weapon.SpecialWeapon.Image1;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                    brush.Stretch = Stretch.Uniform;
                    bdSpecialWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdSpecialWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Weapon.SpecialWeapon.Image1, image3, Downloader.SourceType.Weapon, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Weapon.SpecialWeapon.Image1))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                            brush.Stretch = Stretch.Uniform;
                            bdSpecialWeapon.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdSpecialWeapon);
                        }
                    }));
                }
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("weapon_window-" + s);
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
