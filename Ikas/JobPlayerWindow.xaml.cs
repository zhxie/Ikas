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
            salGoldie.SetImage(Salmoniod.Key.goldie);
            salSteelhead.SetImage(Salmoniod.Key.steelhead);
            salFlyfish.SetImage(Salmoniod.Key.flyfish);
            salScrapper.SetImage(Salmoniod.Key.scrapper);
            salSteelEel.SetImage(Salmoniod.Key.steel_eel);
            salStinger.SetImage(Salmoniod.Key.stinger);
            salMaws.SetImage(Salmoniod.Key.maws);
            salGriller.SetImage(Salmoniod.Key.griller2);
            salDrizzler.SetImage(Salmoniod.Key.drizzler);
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
            salGoldie.SetKill(Salmoniod.Key.goldie);
            salSteelhead.SetKill(Salmoniod.Key.steelhead);
            salFlyfish.SetKill(Salmoniod.Key.flyfish);
            salScrapper.SetKill(Salmoniod.Key.scrapper);
            salSteelEel.SetKill(Salmoniod.Key.steel_eel);
            salStinger.SetKill(Salmoniod.Key.stinger);
            salMaws.SetKill(Salmoniod.Key.maws);
            salGriller.SetKill(Salmoniod.Key.griller2);
            salDrizzler.SetKill(Salmoniod.Key.drizzler);
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
                salGoldie.SetKill(Salmoniod.Key.goldie, Player, Job);
                salSteelhead.SetKill(Salmoniod.Key.steelhead, Player, Job);
                salFlyfish.SetKill(Salmoniod.Key.flyfish, Player, Job);
                salScrapper.SetKill(Salmoniod.Key.scrapper, Player, Job);
                salSteelEel.SetKill(Salmoniod.Key.steel_eel, Player, Job);
                salStinger.SetKill(Salmoniod.Key.stinger, Player, Job);
                salMaws.SetKill(Salmoniod.Key.maws, Player, Job);
                salGriller.SetKill(Salmoniod.Key.griller2, Player, Job);
                salDrizzler.SetKill(Salmoniod.Key.drizzler, Player, Job);
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
