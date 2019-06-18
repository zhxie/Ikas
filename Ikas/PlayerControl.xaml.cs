﻿using System;
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

        public PlayerControl()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdImage, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdWeapon, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSpecial, BitmapScalingMode.HighQuality);
        }

        public void SetPlayer(Player player, bool isMy)
        {
            Player = player;
            ((Storyboard)FindResource("fade_out")).Begin(gridMain);
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
                    bdImage.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdImage);
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
                            bdImage.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdImage);
                        }
                    }));
                }
                // Weapon image
                image = FileFolderUrl.ApplicationData + Player.Weapon.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.Image, image, Downloader.SourceType.Battle, Depot.Proxy);
                    Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Player.Weapon.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
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
                    image = FileFolderUrl.ApplicationData + Player.Weapon.SpecialWeapon.Image1;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.Uniform;
                        bdSpecial.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.SpecialWeapon.Image1, image, Downloader.SourceType.Battle, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Player.Weapon.SpecialWeapon.Image1))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                                brush.Stretch = Stretch.Uniform;
                                bdSpecial.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                            }
                        }));
                    }
                }
                else
                {
                    image = FileFolderUrl.ApplicationData + Player.Weapon.SpecialWeapon.Image2;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.Uniform;
                        bdSpecial.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdSpecial);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Player.Weapon.SpecialWeapon.Image2, image, Downloader.SourceType.Battle, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Player.Weapon.SpecialWeapon.Image2))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
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