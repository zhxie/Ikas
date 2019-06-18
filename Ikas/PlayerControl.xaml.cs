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

using ClassLib;

namespace Ikas
{
    public delegate void MouseEnterIconEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseLeaveIconEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseEnterWeaponEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseLeaveWeaponEventHandler(object sender, MouseEventArgs e);
    /// <summary>
    /// PlayerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public volatile Player Player;

        public string OfflineColor
        {
            get
            {
                return "#7F" + Design.NeonOrange;
            }
        }

        public event MouseEnterIconEventHandler MouseEnterIcon;
        public event MouseLeaveIconEventHandler MouseLeaveIcon;
        public event MouseEnterWeaponEventHandler MouseEnterWeapon;
        public event MouseLeaveWeaponEventHandler MouseLeaveWeapon;

        public PlayerControl()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdIcon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial, BitmapScalingMode.HighQuality);
        }

        #region Control Event

        private void BdIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterIcon?.Invoke(this, e);
        }

        private void BdIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeaveIcon?.Invoke(this, e);
        }

        private void BdWeapon_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterWeapon?.Invoke(this, e);
        }

        private void BdWeapon_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeaveWeapon?.Invoke(this, e);
        }

        #endregion

        public void SetPlayer(Player player, bool isMy)
        {
            Player = player;
            ((Storyboard)FindResource("fade_out")).Begin(gridMain);
            ((Storyboard)FindResource("fade_out")).Begin(bdIcon);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon);
            ((Storyboard)FindResource("fade_out")).Begin(bdSpecial);
            ((Storyboard)FindResource("bg_to_black")).Begin(bdMain);
            if (Player != null)
            {
                // Background change
                bdOffline.Background = new SolidColorBrush(Colors.Transparent);
                if (Player.IsSelf)
                {
                    ((Storyboard)FindResource("bg_to_white")).Begin(bdMain);
                }
                else if (Player.IsOffline)
                {
                    bdOffline.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F000000"));
                }
                // Level or Rank
                if (Player is RankedPlayer)
                {
                    tbLevel.Text = Translate((Player as RankedPlayer).Rank.ToString());
                    tbStar.Text = "";
                }
                else
                {
                    if (Player.Level >= 100)
                    {
                        tbLevel.Text = (Player.Level - 100 * Player.Star).ToString();
                        tbStar.Text = Translate("★", true);
                    }
                    else
                    {
                        tbLevel.Text = Player.Level.ToString();
                        tbLevel.Foreground = new SolidColorBrush(Colors.White);
                        tbStar.Text = "";
                    }
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
                    Downloader downloader = new Downloader(Player.Image, image, Downloader.SourceType.Battle, Depot.Proxy);
                    Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
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
                // Weapon image
                string image2 = FileFolderUrl.ApplicationData + Player.Weapon.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.Image, image2, Downloader.SourceType.Battle, Depot.Proxy);
                    Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Player.Weapon.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdWeapon.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                        }
                    }));
                }
                // Nickname and paint
                tbNickname.Text = Player.Nickname;
                lbPaint.Content = string.Format(Translate("{0}p", true), Player.Paint.ToString());
                // Kill, death and special
                tbKillAndAssist.Text = Player.KillAndAssist.ToString();
                tbDeath.Text = player.Death.ToString();
                if (Player.Assist > 0)
                {
                    tbAssist.Text = string.Format(Translate("({0})", true), Player.Assist);
                }
                else
                {
                    tbAssist.Text = "";
                }
                tbSpecial.Text = Player.Special.ToString();
                // Special image
                if (isMy)
                {
                    string image3 = FileFolderUrl.ApplicationData + Player.Weapon.SpecialWeapon.Image1;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                        brush.Stretch = Stretch.Uniform;
                        bdSpecial.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.SpecialWeapon.Image1, image3, Downloader.SourceType.Battle, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Player.Weapon.SpecialWeapon.Image1))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                                brush.Stretch = Stretch.Uniform;
                                bdSpecial.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                            }
                        }));
                    }
                }
                else
                {
                    string image3 = FileFolderUrl.ApplicationData + Player.Weapon.SpecialWeapon.Image2;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                        brush.Stretch = Stretch.Uniform;
                        bdSpecial.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.SpecialWeapon.Image2, image3, Downloader.SourceType.Battle, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Player.Weapon.SpecialWeapon.Image2))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                                brush.Stretch = Stretch.Uniform;
                                bdSpecial.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                            }
                        }));
                    }
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
                    return (string)FindResource("player_control-" + s);
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
