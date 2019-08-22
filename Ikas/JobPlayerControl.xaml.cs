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
    /// JobPlayerControl.xaml 的交互逻辑
    /// </summary>
    public partial class JobPlayerControl : UserControl
    {
        public volatile JobPlayer Player;
        public volatile bool IsMy;

        public event MouseEnterIconEventHandler MouseEnterIcon;
        public event MouseLeaveIconEventHandler MouseLeaveIcon;
        public event MouseEnterWeaponEventHandler MouseEnterWeapon;
        public event MouseLeaveWeaponEventHandler MouseLeaveWeapon;

        public JobPlayerControl()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdIcon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecialWeapon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgGoldenEgg, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgPowerEgg, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgInklingsSave, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgOctolingsSave, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgInklingsDead, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgOctolingsDead, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
        }

        #region Control Event

        private void BdMain_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void BdMain_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void BdIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterIcon?.Invoke(this, e);
        }

        private void BdIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeaveIcon?.Invoke(this, e);
        }

        private void Wp_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterWeapon?.Invoke(sender, e);
        }

        private void Wp_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeaveWeapon?.Invoke(sender, e);
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
            if (Player != null)
            {
                lbGoldenEgg.Content = string.Format(Translate("x{0}", true), Player.GoldenEgg);
                lbPowerEgg.Content = string.Format(Translate("x{0}", true), Player.PowerEgg);
                lbHelp.Content = string.Format(Translate("x{0}", true), Player.Help);
                lbDead.Content = string.Format(Translate("x{0}", true), Player.Dead);
            }
        }

        public void SetPlayer(JobPlayer player, bool isMy)
        {
            Player = player;
            IsMy = isMy;
            // TODO: My gray color
            ((Storyboard)FindResource("fade_out")).Begin(gridMain);
            ((Storyboard)FindResource("fade_out")).Begin(bdIcon);
            ((Storyboard)FindResource("fade_out")).Begin(bdSpecialWeapon);
            wp1.SetWeapon(null);
            wp2.SetWeapon(null);
            wp3.SetWeapon(null);
            ((Storyboard)FindResource("bg_to_black")).Begin(bdMain);
            if (Player != null)
            {
                // Background change
                if (Player.IsSelf)
                {
                    ((Storyboard)FindResource("bg_to_white")).Begin(bdMain);
                }
                // Icon
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
                    Downloader downloader = new Downloader(Player.Image, image, Downloader.SourceType.Job, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (Player != null)
                        {
                            if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Player.Image) + ".jpg")
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdIcon.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdIcon);
                            }
                        }
                    }));
                }
                // Nickname
                tbNickname.Text = Player.Nickname;
                // Weapons
                wp1.SetWeapon(Player.Weapons[0]);
                if (Player.Weapons.Count > 1)
                {
                    wp2.SetWeapon(Player.Weapons[1]);
                    if (Player.Weapons.Count > 2)
                    {
                        wp3.SetWeapon(Player.Weapons[2]);
                    }
                }
                // Special
                string image2 = FileFolderUrl.ApplicationData + Player.Weapons[0].SpecialWeapon.Image1;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdSpecialWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdSpecialWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapons[0].SpecialWeapon.Image1, image2, Downloader.SourceType.Battle, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (Player != null)
                        {
                            if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Player.Weapons[0].SpecialWeapon.Image1))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdSpecialWeapon.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdSpecialWeapon);
                            }
                        }
                    }));
                }
                // Golden egg, power egg, save, dead
                lbGoldenEgg.Content = string.Format(Translate("x{0}", true), Player.GoldenEgg);
                lbPowerEgg.Content = string.Format(Translate("x{0}", true), Player.PowerEgg);
                lbHelp.Content = string.Format(Translate("x{0}", true), Player.Help);
                lbDead.Content = string.Format(Translate("x{0}", true), Player.Dead);
                switch (player.Species)
                {
                    case JobPlayer.SpeciesType.inklings:
                        ((Storyboard)FindResource("fade_in")).Begin(imgInklingsSave);
                        ((Storyboard)FindResource("fade_in")).Begin(imgInklingsDead);
                        ((Storyboard)FindResource("fade_out")).Begin(imgOctolingsSave);
                        ((Storyboard)FindResource("fade_out")).Begin(imgOctolingsDead);
                        break;
                    case JobPlayer.SpeciesType.octolings:
                        ((Storyboard)FindResource("fade_in")).Begin(imgOctolingsSave);
                        ((Storyboard)FindResource("fade_in")).Begin(imgOctolingsDead);
                        ((Storyboard)FindResource("fade_out")).Begin(imgInklingsSave);
                        ((Storyboard)FindResource("fade_out")).Begin(imgInklingsDead);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                // Show all
                ((Storyboard)FindResource("fade_in")).Begin(gridMain);
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("job_player_control-" + s);
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
