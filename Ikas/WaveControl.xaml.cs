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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Animation;

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// WaveControl.xaml 的交互逻辑
    /// </summary>
    public partial class WaveControl : UserControl
    {
        public string OrangeBackground
        {
            get
            {
                return "#3F" + Design.NeonOrange;
            }
        }

        public volatile Wave Wave;
        public volatile int Number;

        public WaveControl()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(imgGoldenEgg, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial2, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial3, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial4, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial5, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial6, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial7, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial8, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
        }

        #region Control Event

        private void BdMain_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbResult);
            ((Storyboard)FindResource("quick_fade_in")).Begin(spGoldenEggPop);
            ((Storyboard)FindResource("quick_fade_in")).Begin(spSpecial);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbWave);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbTide);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbEvent);
        }

        private void BdMain_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbWave);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbTide);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbEvent);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbResult);
            ((Storyboard)FindResource("quick_fade_out")).Begin(spGoldenEggPop);
            ((Storyboard)FindResource("quick_fade_out")).Begin(spSpecial);
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
            if (Wave != null && Number > 0)
            {
                lbWave.Content = string.Format(Translate("wave_{0}", true), Number);
                lbGoldenEgg.Content = string.Format(Translate("{0}/{1}", true), Wave.GoldenEgg, Wave.Quota);
                lbGolderEggPop.Content = string.Format(Translate("x{0}", true), Wave.GoldenEggPop);
            }
        }

        public void SetWave(Wave wave, int number = 0)
        {
            Wave = wave;
            Number = number;
            ((Storyboard)FindResource("fade_out")).Begin(bdTide);
            ((Storyboard)FindResource("fade_out")).Begin(lbWave);
            ((Storyboard)FindResource("fade_out")).Begin(lbResult);
            ((Storyboard)FindResource("fade_out")).Begin(lbGoldenEgg);
            ((Storyboard)FindResource("fade_out")).Begin(lbTide);
            ((Storyboard)FindResource("fade_out")).Begin(spGoldenEggPop);
            ((Storyboard)FindResource("fade_out")).Begin(lbEvent);
            ((Storyboard)FindResource("fade_out")).Begin(spSpecial);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial1);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial2);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial3);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial4);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial5);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial6);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial7);
            ((Storyboard)FindResource("fade_out")).Begin(gridSpecial8);
            Storyboard sb = (Storyboard)FindResource("resize_height");
            (sb.Children[0] as DoubleAnimation).To = 0;
            sb.Begin(bdTide);
            ((Storyboard)FindResource("fade_out")).Begin(bdTide);
            if (Wave != null && Number > 0)
            {
                lbWave.Content = string.Format(Translate("wave_{0}", true), Number);
                lbResult.SetResourceReference(ContentProperty, Wave.Result.ToString());
                lbGoldenEgg.Content = string.Format(Translate("{0}/{1}", true), Wave.GoldenEgg, Wave.Quota);
                lbGolderEggPop.Content = string.Format(Translate("x{0}", true), Wave.GoldenEggPop);
                lbTide.SetResourceReference(ContentProperty, Wave.WaterLevel.ToString());
                lbEvent.SetResourceReference(ContentProperty, Wave.EventType.ToString());
                if (wave.IsClear)
                {
                    lbResult.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                }
                else
                {
                    lbResult.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                }
                ((Storyboard)FindResource("fade_in")).Begin(lbWave);
                ((Storyboard)FindResource("fade_in")).Begin(lbGoldenEgg);
                ((Storyboard)FindResource("fade_in")).Begin(lbTide);
                ((Storyboard)FindResource("fade_in")).Begin(lbEvent);
                double to;
                switch (wave.WaterLevel)
                {
                    case WaterLevel.Key.low:
                        to = 10;
                        break;
                    case WaterLevel.Key.normal:
                        to = 45;
                        break;
                    case WaterLevel.Key.high:
                        to = 80;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (Wave.Specials.Count > 0)
                {
                    string image = FileFolderUrl.ApplicationData + Wave.Specials[0].Image1;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdSpecial1.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(gridSpecial1);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[0].Image1, image, Downloader.SourceType.Job, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            if (Wave != null && Wave.Specials.Count > 0)
                            {
                                if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Wave.Specials[0].Image1))
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                                    brush.Stretch = Stretch.UniformToFill;
                                    bdSpecial1.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(gridSpecial1);
                                }
                            }
                        }));
                    }
                    if (Wave.Specials.Count > 1)
                    {
                        bdSpecial2.Width = 30;
                        string image2 = FileFolderUrl.ApplicationData + Wave.Specials[1].Image1;
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdSpecial2.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(gridSpecial2);
                        }
                        catch
                        {
                            // Download the image
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[1].Image1, image2, Downloader.SourceType.Job, Depot.Proxy);
                            DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                if (Wave != null && Wave.Specials.Count > 1)
                                {
                                    if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Wave.Specials[1].Image1))
                                    {
                                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                                        brush.Stretch = Stretch.UniformToFill;
                                        bdSpecial2.Background = brush;
                                        ((Storyboard)FindResource("fade_in")).Begin(gridSpecial2);
                                    }
                                }
                            }));
                        }
                        if (Wave.Specials.Count > 2)
                        {
                            bdSpecial3.Width = 30;
                            string image3 = FileFolderUrl.ApplicationData + Wave.Specials[2].Image1;
                            try
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdSpecial3.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(gridSpecial3);
                            }
                            catch
                            {
                                // Download the image
                                Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[2].Image1, image3, Downloader.SourceType.Job, Depot.Proxy);
                                DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                {
                                    if (Wave != null && Wave.Specials.Count > 2)
                                    {
                                        if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Wave.Specials[2].Image1))
                                        {
                                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                                            brush.Stretch = Stretch.UniformToFill;
                                            bdSpecial3.Background = brush;
                                            ((Storyboard)FindResource("fade_in")).Begin(gridSpecial3);
                                        }
                                    }
                                }));
                            }
                            if (Wave.Specials.Count > 3)
                            {
                                bdSpecial4.Width = 30;
                                string image4 = FileFolderUrl.ApplicationData + Wave.Specials[3].Image1;
                                try
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image4)));
                                    brush.Stretch = Stretch.UniformToFill;
                                    bdSpecial4.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(gridSpecial4);
                                }
                                catch
                                {
                                    // Download the image
                                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[3].Image1, image4, Downloader.SourceType.Job, Depot.Proxy);
                                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                    {
                                        if (Wave != null && Wave.Specials.Count > 3)
                                        {
                                            if (System.IO.Path.GetFileName(image4) == System.IO.Path.GetFileName(Wave.Specials[3].Image1))
                                            {
                                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image4)));
                                                brush.Stretch = Stretch.UniformToFill;
                                                bdSpecial4.Background = brush;
                                                ((Storyboard)FindResource("fade_in")).Begin(gridSpecial4);
                                            }
                                        }
                                    }));
                                }
                                if (Wave.Specials.Count > 4)
                                {
                                    bdSpecial5.Width = 30;
                                    string image5 = FileFolderUrl.ApplicationData + Wave.Specials[4].Image1;
                                    try
                                    {
                                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image5)));
                                        brush.Stretch = Stretch.UniformToFill;
                                        bdSpecial5.Background = brush;
                                        ((Storyboard)FindResource("fade_in")).Begin(gridSpecial5);
                                    }
                                    catch
                                    {
                                        // Download the image
                                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[4].Image1, image5, Downloader.SourceType.Job, Depot.Proxy);
                                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                        {
                                            if (Wave != null && Wave.Specials.Count > 4)
                                            {
                                                if (System.IO.Path.GetFileName(image5) == System.IO.Path.GetFileName(Wave.Specials[4].Image1))
                                                {
                                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image5)));
                                                    brush.Stretch = Stretch.UniformToFill;
                                                    bdSpecial5.Background = brush;
                                                    ((Storyboard)FindResource("fade_in")).Begin(gridSpecial5);
                                                }
                                            }
                                        }));
                                    }
                                    if (Wave.Specials.Count > 5)
                                    {
                                        bdSpecial6.Width = 30;
                                        string image6 = FileFolderUrl.ApplicationData + Wave.Specials[5].Image1;
                                        try
                                        {
                                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image6)));
                                            brush.Stretch = Stretch.UniformToFill;
                                            bdSpecial6.Background = brush;
                                            ((Storyboard)FindResource("fade_in")).Begin(gridSpecial6);
                                        }
                                        catch
                                        {
                                            // Download the image
                                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[5].Image1, image6, Downloader.SourceType.Job, Depot.Proxy);
                                            DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                            {
                                                if (Wave != null && Wave.Specials.Count > 5)
                                                {
                                                    if (System.IO.Path.GetFileName(image6) == System.IO.Path.GetFileName(Wave.Specials[5].Image1))
                                                    {
                                                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image6)));
                                                        brush.Stretch = Stretch.UniformToFill;
                                                        bdSpecial6.Background = brush;
                                                        ((Storyboard)FindResource("fade_in")).Begin(gridSpecial6);
                                                    }
                                                }
                                            }));
                                        }
                                        if (Wave.Specials.Count > 6)
                                        {
                                            bdSpecial7.Width = 30;
                                            string image7 = FileFolderUrl.ApplicationData + Wave.Specials[6].Image1;
                                            try
                                            {
                                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image7)));
                                                brush.Stretch = Stretch.UniformToFill;
                                                bdSpecial7.Background = brush;
                                                ((Storyboard)FindResource("fade_in")).Begin(gridSpecial7);
                                            }
                                            catch
                                            {
                                                // Download the image
                                                Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[6].Image1, image7, Downloader.SourceType.Job, Depot.Proxy);
                                                DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                                {
                                                    if (Wave != null && Wave.Specials.Count > 6)
                                                    {
                                                        if (System.IO.Path.GetFileName(image7) == System.IO.Path.GetFileName(Wave.Specials[6].Image1))
                                                        {
                                                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image7)));
                                                            brush.Stretch = Stretch.UniformToFill;
                                                            bdSpecial7.Background = brush;
                                                            ((Storyboard)FindResource("fade_in")).Begin(gridSpecial7);
                                                        }
                                                    }
                                                }));
                                            }
                                            if (Wave.Specials.Count > 7)
                                            {
                                                bdSpecial8.Width = 30;
                                                string image8 = FileFolderUrl.ApplicationData + Wave.Specials[7].Image1;
                                                try
                                                {
                                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image8)));
                                                    brush.Stretch = Stretch.UniformToFill;
                                                    bdSpecial8.Background = brush;
                                                    ((Storyboard)FindResource("fade_in")).Begin(gridSpecial8);
                                                }
                                                catch
                                                {
                                                    // Download the image
                                                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Wave.Specials[7].Image1, image8, Downloader.SourceType.Job, Depot.Proxy);
                                                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                                                    {
                                                        if (Wave != null && Wave.Specials.Count > 7)
                                                        {
                                                            if (System.IO.Path.GetFileName(image8) == System.IO.Path.GetFileName(Wave.Specials[7].Image1))
                                                            {
                                                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image8)));
                                                                brush.Stretch = Stretch.UniformToFill;
                                                                bdSpecial8.Background = brush;
                                                                ((Storyboard)FindResource("fade_in")).Begin(gridSpecial8);
                                                            }
                                                        }
                                                    }));
                                                }
                                                gridSpecial2.Width = 30;
                                                gridSpecial3.Width = 30;
                                                gridSpecial4.Width = 30;
                                                gridSpecial5.Width = 30;
                                                gridSpecial6.Width = 30;
                                                gridSpecial7.Width = 30;
                                                gridSpecial8.Width = 30;
                                                gridSpecial2.Margin = new Thickness(-15.9, 0, 0, 0);
                                                gridSpecial3.Margin = new Thickness(-15.9, 0, 0, 0);
                                                gridSpecial4.Margin = new Thickness(-15.9, 0, 0, 0);
                                                gridSpecial5.Margin = new Thickness(-15.9, 0, 0, 0);
                                                gridSpecial6.Margin = new Thickness(-15.9, 0, 0, 0);
                                                gridSpecial7.Margin = new Thickness(-15.9, 0, 0, 0);
                                                gridSpecial8.Margin = new Thickness(-15.9, 0, 0, 0);
                                            }
                                            else
                                            {
                                                gridSpecial2.Width = 30;
                                                gridSpecial3.Width = 30;
                                                gridSpecial4.Width = 30;
                                                gridSpecial5.Width = 30;
                                                gridSpecial6.Width = 30;
                                                gridSpecial7.Width = 30;
                                                gridSpecial8.Width = 0;
                                                gridSpecial2.Margin = new Thickness(-13.5, 0, 0, 0);
                                                gridSpecial3.Margin = new Thickness(-13.5, 0, 0, 0);
                                                gridSpecial4.Margin = new Thickness(-13.5, 0, 0, 0);
                                                gridSpecial5.Margin = new Thickness(-13.5, 0, 0, 0);
                                                gridSpecial6.Margin = new Thickness(-13.5, 0, 0, 0);
                                                gridSpecial7.Margin = new Thickness(-13.5, 0, 0, 0);
                                                gridSpecial8.Margin = new Thickness(0);
                                            }
                                        }
                                        else
                                        {
                                            gridSpecial2.Width = 30;
                                            gridSpecial3.Width = 30;
                                            gridSpecial4.Width = 30;
                                            gridSpecial5.Width = 30;
                                            gridSpecial6.Width = 30;
                                            gridSpecial7.Width = 0;
                                            gridSpecial8.Width = 0;
                                            gridSpecial2.Margin = new Thickness(-10.2, 0, 0, 0);
                                            gridSpecial3.Margin = new Thickness(-10.2, 0, 0, 0);
                                            gridSpecial4.Margin = new Thickness(-10.2, 0, 0, 0);
                                            gridSpecial5.Margin = new Thickness(-10.2, 0, 0, 0);
                                            gridSpecial6.Margin = new Thickness(-10.2, 0, 0, 0);
                                            gridSpecial7.Margin = new Thickness(0);
                                            gridSpecial8.Margin = new Thickness(0);
                                        }
                                    }
                                    else
                                    {
                                        gridSpecial2.Width = 30;
                                        gridSpecial3.Width = 30;
                                        gridSpecial4.Width = 30;
                                        gridSpecial5.Width = 30;
                                        gridSpecial6.Width = 0;
                                        gridSpecial7.Width = 0;
                                        gridSpecial8.Width = 0;
                                        gridSpecial2.Margin = new Thickness(-5.25, 0, 0, 0);
                                        gridSpecial3.Margin = new Thickness(-5.25, 0, 0, 0);
                                        gridSpecial4.Margin = new Thickness(-5.25, 0, 0, 0);
                                        gridSpecial5.Margin = new Thickness(-5.25, 0, 0, 0);
                                        gridSpecial6.Margin = new Thickness(0);
                                        gridSpecial7.Margin = new Thickness(0);
                                        gridSpecial8.Margin = new Thickness(0);
                                    }
                                }
                                else
                                {
                                    gridSpecial2.Width = 30;
                                    gridSpecial3.Width = 30;
                                    gridSpecial4.Width = 30;
                                    gridSpecial5.Width = 0;
                                    gridSpecial6.Width = 0;
                                    gridSpecial7.Width = 0;
                                    gridSpecial8.Width = 0;
                                    gridSpecial2.Margin = new Thickness(3, 0, 0, 0);
                                    gridSpecial3.Margin = new Thickness(3, 0, 0, 0);
                                    gridSpecial4.Margin = new Thickness(3, 0, 0, 0);
                                    gridSpecial5.Margin = new Thickness(0);
                                    gridSpecial6.Margin = new Thickness(0);
                                    gridSpecial7.Margin = new Thickness(0);
                                    gridSpecial8.Margin = new Thickness(0);
                                }
                            }
                            else
                            {
                                gridSpecial2.Width = 30;
                                gridSpecial3.Width = 30;
                                gridSpecial4.Width = 0;
                                gridSpecial5.Width = 0;
                                gridSpecial6.Width = 0;
                                gridSpecial7.Width = 0;
                                gridSpecial8.Width = 0;
                                gridSpecial2.Margin = new Thickness(5, 0, 0, 0);
                                gridSpecial3.Margin = new Thickness(5, 0, 0, 0);
                                gridSpecial4.Margin = new Thickness(0);
                                gridSpecial5.Margin = new Thickness(0);
                                gridSpecial6.Margin = new Thickness(0);
                                gridSpecial7.Margin = new Thickness(0);
                                gridSpecial8.Margin = new Thickness(0);
                            }
                        }
                        else
                        {
                            gridSpecial2.Width = 30;
                            gridSpecial3.Width = 0;
                            gridSpecial4.Width = 0;
                            gridSpecial5.Width = 0;
                            gridSpecial6.Width = 0;
                            gridSpecial7.Width = 0;
                            gridSpecial8.Width = 0;
                            gridSpecial2.Margin = new Thickness(5, 0, 0, 0);
                            gridSpecial3.Margin = new Thickness(0);
                            gridSpecial4.Margin = new Thickness(0);
                            gridSpecial5.Margin = new Thickness(0);
                            gridSpecial6.Margin = new Thickness(0);
                            gridSpecial7.Margin = new Thickness(0);
                            gridSpecial8.Margin = new Thickness(0);
                        }
                    }
                    else
                    {
                        gridSpecial2.Width = 0;
                        gridSpecial3.Width = 0;
                        gridSpecial4.Width = 0;
                        gridSpecial5.Width = 0;
                        gridSpecial6.Width = 0;
                        gridSpecial7.Width = 0;
                        gridSpecial8.Width = 0;
                        gridSpecial2.Margin = new Thickness(0);
                        gridSpecial3.Margin = new Thickness(0);
                        gridSpecial4.Margin = new Thickness(0);
                        gridSpecial5.Margin = new Thickness(0);
                        gridSpecial6.Margin = new Thickness(0);
                        gridSpecial7.Margin = new Thickness(0);
                        gridSpecial8.Margin = new Thickness(0);
                    }
                }
                (sb.Children[0] as DoubleAnimation).To = to;
                sb.Begin(bdTide);
                ((Storyboard)FindResource("fade_in")).Begin(bdTide);
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("wave_control-" + s);
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
