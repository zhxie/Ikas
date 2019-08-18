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
            salGoldie.SetSalmoniod(Salmoniod.Key.goldie);
            salSteelhead.SetSalmoniod(Salmoniod.Key.steelhead);
            salFlyfish.SetSalmoniod(Salmoniod.Key.flyfish);
            salScrapper.SetSalmoniod(Salmoniod.Key.scrapper);
            salSteelEel.SetSalmoniod(Salmoniod.Key.steel_eel);
            salStinger.SetSalmoniod(Salmoniod.Key.stinger);
            salMaws.SetSalmoniod(Salmoniod.Key.maws);
            salGriller.SetSalmoniod(Salmoniod.Key.griller2);
            salDrizzler.SetSalmoniod(Salmoniod.Key.drizzler);
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
            salGoldie.SetKill();
            salSteelhead.SetKill();
            salFlyfish.SetKill();
            salScrapper.SetKill();
            salSteelEel.SetKill();
            salStinger.SetKill();
            salMaws.SetKill();
            salGriller.SetKill();
            salDrizzler.SetKill();
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
                salGoldie.SetKill(Player.GoldieKill, Job.GoldieKill, Job.GoldieCount);
                salSteelhead.SetKill(Player.SteelheadKill, Job.SteelheadKill, Job.SteelheadCount);
                salFlyfish.SetKill(Player.FlyfishKill, Job.FlyfishKill, Job.FlyfishCount);
                salScrapper.SetKill(Player.ScrapperKill, Job.ScrapperKill, Job.ScrapperCount);
                salSteelEel.SetKill(Player.SteelEelKill, Job.SteelEelKill, Job.SteelEelCount);
                salStinger.SetKill(Player.StingerKill, Job.StingerKill, Job.StingerCount);
                salMaws.SetKill(Player.MawsKill, Job.MawsKill, Job.MawsCount);
                salGriller.SetKill(Player.GrillerKill, Job.GrillerKill, Job.GrillerCount);
                salDrizzler.SetKill(Player.DrizzlerKill, Job.DrizzlerKill, Job.DrizzlerCount);
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
