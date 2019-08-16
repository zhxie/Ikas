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
    /// JobPlayerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JobPlayerWindow : Window
    {
        public string SalmonYellowBackground
        {
            get
            {
                return "#7F" + Design.NeonSalmonYellow;
            }
        }

        public Window KeepAliveWindow { get; set; }

        public volatile JobPlayer Player;
        public volatile Job Job;

        public JobPlayerWindow()
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
            RenderOptions.SetBitmapScalingMode(bdIcon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgGoldie, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgSteelhead, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgFlyfish, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgScrapper, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgSteelEel, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgStinger, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgMaws, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgGriller, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgDrizzler, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgSalmonRun, BitmapScalingMode.HighQuality);
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
            ((Storyboard)FindResource("window_fade_in")).Begin(this);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(this);
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

        private void BdGoldie_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbGoldie);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbGoldieName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbGoldieShort);
        }

        private void BdGoldie_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbGoldie);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbGoldieName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbGoldieShort);
        }

        private void BdSteelhead_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbSteelhead);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbSteelheadName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbSteelheadShort);
        }

        private void BdSteelhead_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbSteelhead);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbSteelheadName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbSteelheadShort);
        }

        private void BdFlyfish_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbFlyfish);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbFlyfishName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbFlyfishShort);
        }

        private void BdFlyfish_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbFlyfish);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbFlyfishName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbFlyfishShort);
        }

        private void BdScrapper_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbScrapper);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbScrapperName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbScrapperShort);
        }

        private void BdScrapper_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbScrapper);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbScrapperName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbScrapperShort);
        }

        private void BdSteelEel_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbSteelEel);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbSteelEelName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbSteelEelShort);
        }

        private void BdSteelEel_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbSteelEel);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbSteelEelName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbSteelEelShort);
        }

        private void BdStinger_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbStinger);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbStingerName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbStingerShort);
        }

        private void BdStinger_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbStinger);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbStingerName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbStingerShort);
        }

        private void BdMaws_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbMaws);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbMawsName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbMawsShort);
        }

        private void BdMaws_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbMaws);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbMawsName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbMawsShort);
        }

        private void BdGriller_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbGriller);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbGrillerName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbGrillerShort);
        }

        private void BdGriller_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbGriller);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbGrillerName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbGrillerShort);
        }

        private void BdDrizzler_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbDrizzler);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbDrizzlerName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbDrizzlerShort);
        }

        private void BdDrizzler_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbDrizzler);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbDrizzlerName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbDrizzlerShort);
        }

        private void BdTotal_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbTotal);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbTotalName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbTotalShort);
        }

        private void BdTotal_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbTotal);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbTotalName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbTotalShort);
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

        public void SetPlayer(JobPlayer player, Job job)
        {
            Player = player;
            Job = job;
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(tbName);
            ((Storyboard)FindResource("fade_out")).Begin(bdIcon);
            ((Storyboard)FindResource("fade_out")).Begin(gridGoldie);
            ((Storyboard)FindResource("fade_out")).Begin(gridSteelhead);
            ((Storyboard)FindResource("fade_out")).Begin(gridFlyfish);
            ((Storyboard)FindResource("fade_out")).Begin(gridScrapper);
            ((Storyboard)FindResource("fade_out")).Begin(gridSteelEel);
            ((Storyboard)FindResource("fade_out")).Begin(gridStinger);
            ((Storyboard)FindResource("fade_out")).Begin(gridMaws);
            ((Storyboard)FindResource("fade_out")).Begin(gridGriller);
            ((Storyboard)FindResource("fade_out")).Begin(gridDrizzler);
            ((Storyboard)FindResource("fade_out")).Begin(gridTotal);
            Storyboard sb = (Storyboard)FindResource("resize_width");
            (sb.Children[0] as DoubleAnimation).To = 0;
            sb.Begin(bdGoldieRatio);
            sb.Begin(bdSteelheadRatio);
            sb.Begin(bdFlyfishRatio);
            sb.Begin(bdScrapperRatio);
            sb.Begin(bdSteelEelRatio);
            sb.Begin(bdStingerRatio);
            sb.Begin(bdMawsRatio);
            sb.Begin(bdGrillerRatio);
            sb.Begin(bdDrizzlerRatio);
            sb.Begin(bdTotalRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdGoldieRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdSteelheadRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdFlyfishRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdScrapperRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdSteelEelRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdStingerRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdMawsRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdGrillerRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdDrizzlerRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdTotalRatio);
            if (Player != null && Job != null)
            {
                // Update player
                tbName.Text = Player.Nickname;
                ((Storyboard)FindResource("fade_in")).Begin(tbName);
                string image = FileFolderUrl.ApplicationData + FileFolderUrl.IconFolder + @"\" + System.IO.Path.GetFileName(Player.Image) + ".jpg";
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdIcon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdIcon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(Player.Image, image, Downloader.SourceType.Player, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Player.Image) + ".jpg")
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdIcon.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdIcon);
                        }
                    }));
                }
                // Update salmoniods
                lbGoldie.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.GoldieKill, Job.GoldieKill, Job.GoldieCount);
                lbGoldieShort.Content = string.Format(Translate("x{0}", true), Player.GoldieKill);
                lbSteelhead.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.SteelheadKill, Job.SteelheadKill, Job.SteelheadCount);
                lbSteelheadShort.Content = string.Format(Translate("x{0}", true), Player.SteelheadKill);
                lbFlyfish.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.FlyfishKill, Job.FlyfishKill, Job.FlyfishCount);
                lbFlyfishShort.Content = string.Format(Translate("x{0}", true), Player.FlyfishKill);
                lbScrapper.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.ScrapperKill, Job.ScrapperKill, Job.ScrapperCount);
                lbScrapperShort.Content = string.Format(Translate("x{0}", true), Player.ScrapperKill);
                lbSteelEel.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.SteelEelKill, Job.SteelEelKill, Job.SteelEelCount);
                lbSteelEelShort.Content = string.Format(Translate("x{0}", true), Player.SteelEelKill);
                lbStinger.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.StingerKill, Job.StingerKill, Job.StingerCount);
                lbStingerShort.Content = string.Format(Translate("x{0}", true), Player.StingerKill);
                lbMaws.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.MawsKill, Job.MawsKill, Job.MawsCount);
                lbMawsShort.Content = string.Format(Translate("x{0}", true), Player.MawsKill);
                lbGriller.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.GrillerKill, Job.GrillerKill, Job.GrillerCount);
                lbGrillerShort.Content = string.Format(Translate("x{0}", true), Player.GrillerKill);
                lbDrizzler.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.DrizzlerKill, Job.DrizzlerKill, Job.DrizzlerCount);
                lbDrizzlerShort.Content = string.Format(Translate("x{0}", true), Player.DrizzlerKill);
                lbTotal.Content = string.Format(Translate("x{0}/{1}/{2}", true), Player.BossKill, Job.BossKill, Job.BossCount);
                lbTotalShort.Content = string.Format(Translate("x{0}", true), Player.BossKill);
                ((Storyboard)FindResource("fade_in")).Begin(gridGoldie);
                ((Storyboard)FindResource("fade_in")).Begin(gridSteelhead);
                ((Storyboard)FindResource("fade_in")).Begin(gridFlyfish);
                ((Storyboard)FindResource("fade_in")).Begin(gridScrapper);
                ((Storyboard)FindResource("fade_in")).Begin(gridSteelEel);
                ((Storyboard)FindResource("fade_in")).Begin(gridStinger);
                ((Storyboard)FindResource("fade_in")).Begin(gridMaws);
                ((Storyboard)FindResource("fade_in")).Begin(gridGriller);
                ((Storyboard)FindResource("fade_in")).Begin(gridDrizzler);
                ((Storyboard)FindResource("fade_in")).Begin(gridTotal);
                double to;
                if (Job.GoldieKill != 0)
                {
                    to = (Player.GoldieKill * 0.9 / Job.GoldieKill + 0.1) * bdGoldie.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdGoldieRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdGoldieRatio);
                }
                else
                {
                    to = 0.1 * bdGoldie.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdGoldieRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdGoldieRatio);
                }
                if (Job.SteelheadKill != 0)
                {
                    to = (Player.SteelheadKill * 0.9 / Job.SteelheadKill + 0.1) * bdSteelhead.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdSteelheadRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdSteelheadRatio);
                }
                else
                {
                    to = 0.1 * bdSteelhead.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdSteelheadRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdSteelheadRatio);
                }
                if (Job.FlyfishKill != 0)
                {
                    to = (Player.FlyfishKill * 0.9 / Job.FlyfishKill + 0.1) * bdFlyfish.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdFlyfishRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdFlyfishRatio);
                }
                else
                {
                    to = 0.1 * bdFlyfish.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdFlyfishRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdFlyfishRatio);
                }
                if (Job.ScrapperKill != 0)
                {
                    to = (Player.ScrapperKill * 0.9 / Job.ScrapperKill + 0.1) * bdScrapper.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdScrapperRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdScrapperRatio);
                }
                else
                {
                    to = 0.1 * bdScrapper.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdScrapperRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdScrapperRatio);
                }

                if (Job.SteelEelKill != 0)
                {
                    to = (Player.SteelEelKill * 0.9 / Job.SteelEelKill + 0.1) * bdSteelEel.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdSteelEelRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdSteelEelRatio);
                }
                else
                {
                    to = 0.1 * bdSteelEel.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdSteelEelRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdSteelEelRatio);
                }
                if (Job.StingerKill != 0)
                {
                    to = (Player.StingerKill * 0.9 / Job.StingerKill + 0.1) * bdStinger.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdStingerRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdStingerRatio);
                }
                else
                {
                    to = 0.1 * bdStinger.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdStingerRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdStingerRatio);
                }
                if (Job.MawsKill != 0)
                {
                    to = (Player.MawsKill * 0.9 / Job.MawsKill + 0.1) * bdMaws.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdMawsRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdMawsRatio);
                }
                else
                {
                    to = 0.1 * bdMaws.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdMawsRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdMawsRatio);
                }
                if (Job.GrillerKill != 0)
                {
                    to = (Player.GrillerKill * 0.9 / Job.GrillerKill + 0.1) * bdGriller.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdGrillerRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdGrillerRatio);
                }
                else
                {
                    to = 0.1 * bdGriller.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdGrillerRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdGrillerRatio);
                }
                if (Job.DrizzlerKill != 0)
                {
                    to = (Player.DrizzlerKill * 0.9 / Job.DrizzlerKill + 0.1) * bdDrizzler.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdDrizzlerRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdDrizzlerRatio);
                }
                else
                {
                    to = 0.1 * bdDrizzler.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdDrizzlerRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdDrizzlerRatio);
                }
                if (Job.BossKill != 0)
                {
                    to = (Player.BossKill * 0.9 / Job.BossKill + 0.1) * bdTotal.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdTotalRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdTotalRatio);
                }
                else
                {
                    to = 0.1 * bdTotal.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdTotalRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdTotalRatio);
                }
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("job_player_window-" + s);
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
