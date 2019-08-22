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
    public delegate void MouseEnterIconEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseLeaveIconEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseEnterWeaponEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseLeaveWeaponEventHandler(object sender, MouseEventArgs e);
    /// <summary>
    /// PlayerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public string YellowForeground
        {
            get
            {
                return "#FF" + Design.NeonYellow;
            }
        }

        public volatile Player Player;
        public volatile bool IsMy;

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
            RenderOptions.SetBitmapScalingMode(imgInklingsKill, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgOctolingsKill, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgInklingsDeath, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(imgOctolingsDeath, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
        }

        #region Control Event

        private void BdMain_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Player is RankedPlayer)
            {
                ((Storyboard)FindResource("quick_fade_in")).Begin(lbLevel);
                ((Storyboard)FindResource("quick_fade_out")).Begin(lbRank);
            }
            ((Storyboard)FindResource("quick_fade_in")).Begin(gridKD);
            ((Storyboard)FindResource("quick_fade_out")).Begin(gridKill);
            ((Storyboard)FindResource("quick_fade_out")).Begin(gridDeath);
        }

        private void BdMain_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Player is RankedPlayer)
            {
                ((Storyboard)FindResource("quick_fade_in")).Begin(lbRank);
                ((Storyboard)FindResource("quick_fade_out")).Begin(lbLevel);
            }
            ((Storyboard)FindResource("quick_fade_in")).Begin(gridKill);
            ((Storyboard)FindResource("quick_fade_in")).Begin(gridDeath);
            ((Storyboard)FindResource("quick_fade_out")).Begin(gridKD);
        }

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
                lbPaint.Content = string.Format(Translate("{0}p", true), Player.Paint.ToString());
                if (Player.Assist > 0)
                {
                    tbAssist.Text = string.Format(Translate("({0})", true), Player.Assist);
                }
            }
        }

        public void SetPlayer(Player player, bool isMy)
        {
            Player = player;
            IsMy = isMy;
            ((Storyboard)FindResource("fade_out")).Begin(gridMain);
            ((Storyboard)FindResource("fade_out")).Begin(bdIcon);
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon);
            ((Storyboard)FindResource("fade_out")).Begin(bdSpecial);
            Storyboard sb = (Storyboard)FindResource("resize_width");
            (sb.Children[0] as DoubleAnimation).To = 0;
            sb.Begin(bdKillDeathRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdKillDeathRatio);
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
                if (Player.Level >= 100)
                {
                    tbLevel.Text = (Player.Level - 100 * Player.Star).ToString();
                    tbStar.SetResourceReference(Run.TextProperty, "player_control-★");
                    tbRank.Text = "";
                }
                else
                {
                    tbLevel.Text = Player.Level.ToString();
                    tbStar.Text = "";
                    tbRank.Text = "";
                }
                ((Storyboard)FindResource("fade_in")).Begin(lbLevel);
                ((Storyboard)FindResource("fade_out")).Begin(lbRank);
                if (Player is RankedPlayer)
                {
                    tbRank.SetResourceReference(TextBlock.TextProperty, (Player as RankedPlayer).Rank.ToString());
                    ((Storyboard)FindResource("fade_in")).Begin(lbRank);
                    ((Storyboard)FindResource("fade_out")).Begin(lbLevel);
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
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (Player != null)
                        {
                            if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Player.Weapon.Image))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdWeapon.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                            }
                        }
                    }));
                }
                // Nickname and paint
                tbNickname.Text = Player.Nickname;
                lbPaint.Content = string.Format(Translate("{0}p", true), Player.Paint.ToString());
                // Kill, death and special
                switch (player.Species)
                {
                    case Player.SpeciesType.inklings:
                        ((Storyboard)FindResource("fade_in")).Begin(imgInklingsKill);
                        ((Storyboard)FindResource("fade_in")).Begin(imgInklingsDeath);
                        ((Storyboard)FindResource("fade_out")).Begin(imgOctolingsKill);
                        ((Storyboard)FindResource("fade_out")).Begin(imgOctolingsDeath);
                        break;
                    case Player.SpeciesType.octolings:
                        ((Storyboard)FindResource("fade_in")).Begin(imgOctolingsKill);
                        ((Storyboard)FindResource("fade_in")).Begin(imgOctolingsDeath);
                        ((Storyboard)FindResource("fade_out")).Begin(imgInklingsKill);
                        ((Storyboard)FindResource("fade_out")).Begin(imgInklingsDeath);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                tbKillAndAssist.Text = Player.KillAndAssist.ToString();
                tbDeath.Text = Player.Death.ToString();
                tbKD.Text = string.Format("{0:f2}", Player.KillDeathRatio);
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
                string image3;
                if (IsMy)
                {
                    image3 = FileFolderUrl.ApplicationData + Player.Weapon.SpecialWeapon.Image1;
                }
                else
                {
                    image3 = FileFolderUrl.ApplicationData + Player.Weapon.SpecialWeapon.Image2;
                }
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
                    Downloader downloader;
                    if (IsMy)
                    {
                        downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.SpecialWeapon.Image1, image3, Downloader.SourceType.Battle, Depot.Proxy);
                    }
                    else
                    {
                        downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.SpecialWeapon.Image2, image3, Downloader.SourceType.Battle, Depot.Proxy);
                    }
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (Player != null)
                        {
                            if (IsMy)
                            {
                                if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Player.Weapon.SpecialWeapon.Image1))
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                                    brush.Stretch = Stretch.Uniform;
                                    bdSpecial.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                                }
                            }
                            else
                            {
                                if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Player.Weapon.SpecialWeapon.Image2))
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                                    brush.Stretch = Stretch.Uniform;
                                    bdSpecial.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                                }
                            }
                        }
                    }));
                }
                // Kill death ratio
                if (IsMy)
                {
                    bdKillDeathRatio.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2F" + Design.NeonRed));
                }
                else
                {
                    bdKillDeathRatio.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2F" + Design.NeonGreen));
                }
                double to;
                if (player.IsOffline)
                {
                    to = 0;
                }
                else
                {
                    if (player.KillDeathRatio <= 2)
                    {
                        to = bdMain.ActualWidth * (0.05 + 0.35 * player.KillDeathRatio);
                    }
                    else if (player.KillDeathRatio <= 8)
                    {
                        to = bdMain.ActualWidth * (0.75 + 0.25 * (Math.Log(player.KillDeathRatio) / Math.Log(2)) / 3);
                    }
                    else
                    {
                        to = bdMain.ActualWidth;
                    }
                }
                (sb.Children[0] as DoubleAnimation).To = to;
                sb.Begin(bdKillDeathRatio);
                ((Storyboard)FindResource("fade_in")).Begin(bdKillDeathRatio);
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
