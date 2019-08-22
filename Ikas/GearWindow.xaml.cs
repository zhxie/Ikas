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
    /// GearWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GearWindow : Window
    {
        public Window KeepAliveWindow { get; set; }

        public volatile Gear Gear;

        public GearWindow()
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
            RenderOptions.SetBitmapScalingMode(bdGear, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdBrand, BitmapScalingMode.HighQuality);
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.CenterX = bdGear.Width / 2;
            rotateTransform.CenterY = bdGear.Height / 2;
            rotateTransform.Angle = -7.5;
            bdGear.LayoutTransform = rotateTransform;
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
            // Force refresh labels
            SetGear(Gear);
        }

        public void SetGear(Gear gear)
        {
            // Remove previous Downloader's handlers (Believe every gear has its brand, stop checking)
            //DownloadHelper.RemoveDownloaders(Downloader.SourceType.Gear);
            Gear = gear;
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(tbName);
            ((Storyboard)FindResource("fade_out")).Begin(bdGear);
            ((Storyboard)FindResource("fade_out")).Begin(bdBrand);
            if (Gear != null)
            {
                // Update weapon
                switch (Gear.Kind)
                {
                    case Gear.KindType.Head:
                        tbName.Text = Translate(((HeadGear.Key)Gear.Id).ToString());
                        break;
                    case Gear.KindType.Clothes:
                        tbName.Text = Translate(((ClothesGear.Key)Gear.Id).ToString());
                        break;
                    case Gear.KindType.Shoes:
                        tbName.Text = Translate(((ShoesGear.Key)Gear.Id).ToString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                ((Storyboard)FindResource("fade_in")).Begin(tbName);
                string image = FileFolderUrl.ApplicationData + Gear.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.Uniform;
                    bdGear.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdGear);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.Image, image, Downloader.SourceType.Gear, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Gear.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.Uniform;
                            bdGear.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdGear);
                        }
                    }));
                }
                string image2 = FileFolderUrl.ApplicationData + Gear.Brand.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                    brush.Stretch = Stretch.Uniform;
                    bdBrand.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdBrand);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.Brand.Image, image2, Downloader.SourceType.Gear, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Gear.Brand.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.Uniform;
                            bdBrand.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdBrand);
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
                    return (string)FindResource("gear_window-" + s);
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
