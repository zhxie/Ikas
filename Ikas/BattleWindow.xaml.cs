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

using IkasLib;

namespace Ikas
{
    /// <summary>
    /// BattleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BattleWindow : Window
    {
        private PlayerWindow playerWindow;
        private WeaponWindow weaponWindow;

        private DispatcherTimer tmLoading;
        private int loadingRotationAngle;

        public BattleWindow()
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
            RenderOptions.SetBitmapScalingMode(imgMode, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(stg, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.BattleChanged += new BattleChangedEventHandler(BattleChanged);
            Depot.BattleFound += new BattleFoundEventHandler(BattleFound);
            Depot.BattleUpdated += new BattleUpdatedEventHandler(BattleUpdated);
            // Prepare Icon and Weapon window
            playerWindow = new PlayerWindow();
            playerWindow.KeepAliveWindow = this;
            playerWindow.Opacity = 0;
            playerWindow.Visibility = Visibility.Hidden;
            weaponWindow = new WeaponWindow();
            weaponWindow.KeepAliveWindow = this;
            weaponWindow.Opacity = 0;
            weaponWindow.Visibility = Visibility.Hidden;
            // Create timers
            loadingRotationAngle = 0;
            tmLoading = new DispatcherTimer();
            tmLoading.Tick += new EventHandler((object source, EventArgs e) =>
            {
                imgLoading.RenderTransform = new RotateTransform(loadingRotationAngle, imgLoading.Source.Width / 2, imgLoading.Source.Height / 2);
                if (loadingRotationAngle >= 359)
                {
                    loadingRotationAngle = 0;
                }
                else
                {
                    loadingRotationAngle++;
                }
            });
            tmLoading.Interval = new TimeSpan(0, 0, 0, 0, 10);
        }

        #region Control Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start timers
            tmLoading.Start();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
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

        private void Player_MouseEnterIcon(object sender, MouseEventArgs e)
        {
            Player player = (sender as PlayerControl).Player;
            if (player != null)
            {
                playerWindow.Top = e.GetPosition(this).Y + Top - playerWindow.Height / 2;
                playerWindow.Left = e.GetPosition(this).X + Left + 10;
                // Restrict in this window
                if (Top - playerWindow.Top > 30)
                {
                    playerWindow.Top = Top - 30;
                }
                if (Left - playerWindow.Left > 30)
                {
                    playerWindow.Left = Left - 30;
                }
                if (playerWindow.Top + playerWindow.Height - (Top + Height) > 30)
                {
                    playerWindow.Top = Top + Height - playerWindow.Height + 30;
                }
                if (playerWindow.Left + playerWindow.Width - (Left + Width) > 30)
                {
                    playerWindow.Left = Left + Width - playerWindow.Width + 30;
                }
                playerWindow.SetPlayer(player);
                ((Storyboard)FindResource("window_fade_in")).Begin(playerWindow);
            }
        }

        private void Player_MouseLeaveIcon(object sender, MouseEventArgs e)
        {
            Player player = (sender as PlayerControl).Player;
            if (player != null)
            {
                ((Storyboard)FindResource("window_fade_out")).Begin(playerWindow);
            }
        }

        private void Player_MouseEnterWeapon(object sender, MouseEventArgs e)
        {
            Player player = (sender as PlayerControl).Player;
            if (player != null)
            {
                weaponWindow.Top = e.GetPosition(this).Y + Top - weaponWindow.Height / 2;
                weaponWindow.Left = e.GetPosition(this).X + Left + 10;
                // Restrict in this window
                if (Top - weaponWindow.Top > 30)
                {
                    weaponWindow.Top = Top - 30;
                }
                if (Left - weaponWindow.Left > 30)
                {
                    weaponWindow.Left = Left - 30;
                }
                if (weaponWindow.Top + weaponWindow.Height - (Top + Height) > 30)
                {
                    weaponWindow.Top = Top + Height - weaponWindow.Height + 30;
                }
                if (weaponWindow.Left + weaponWindow.Width - (Left + Width) > 30)
                {
                    weaponWindow.Left = Left + Width - weaponWindow.Width + 30;
                }
                weaponWindow.SetWeapon(player.Weapon);
                ((Storyboard)FindResource("window_fade_in")).Begin(weaponWindow);
            }
        }

        private void Player_MouseLeaveWeapon(object sender, MouseEventArgs e)
        {
            Player player = (sender as PlayerControl).Player;
            if (player != null)
            {
                ((Storyboard)FindResource("window_fade_out")).Begin(weaponWindow);
            }
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

        private void BattleChanged()
        {
            // Fade in loading
            ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
        }

        private void BattleFound()
        {
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbRule);
            ((Storyboard)FindResource("fade_out")).Begin(tagResult);
            ((Storyboard)FindResource("fade_out")).Begin(lbPowerName);
            ((Storyboard)FindResource("fade_out")).Begin(lbPower);
            ((Storyboard)FindResource("fade_out")).Begin(tagWin);
            ((Storyboard)FindResource("fade_out")).Begin(lbWinEstimatedPower);
            ((Storyboard)FindResource("fade_out")).Begin(tagLose);
            ((Storyboard)FindResource("fade_out")).Begin(lbLoseEstimatedPower);
            ((Storyboard)FindResource("fade_out")).Begin(stg);
            plWin1.SetPlayer(null, true);
            plWin2.SetPlayer(null, true);
            plWin3.SetPlayer(null, true);
            plWin4.SetPlayer(null, true);
            plLose1.SetPlayer(null, false);
            plLose2.SetPlayer(null, false);
            plLose3.SetPlayer(null, false);
            plLose4.SetPlayer(null, false);
        }

        private void BattleUpdated()
        {
            Battle battle = Depot.Battle;
            if (battle.Stage != null)
            {
                // Update current Battle
                switch (battle.Mode)
                {
                    case Mode.Key.regular_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_regular");
                        lbPowerName.Content = "";
                        lbPower.Content = "";
                        lbWinEstimatedPower.Content = "";
                        lbLoseEstimatedPower.Content = "";
                        break;
                    case Mode.Key.ranked_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_ranked");
                        lbPowerName.Content = "";
                        lbPower.Content = "";
                        lbWinEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as RankedBattle).EstimatedRankPower);
                        lbLoseEstimatedPower.Content = "";
                        break;
                    case Mode.Key.league_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_league");
                        lbPowerName.Content = Translate("league_power", true);
                        if ((battle as LeagueBattle).IsCalculating)
                        {
                            lbPower.Content = Translate("calculating", true);
                        }
                        else
                        {
                            lbPower.Content = string.Format(Translate("{0}/{1}", true), (battle as LeagueBattle).LeaguePoint, (battle as LeagueBattle).MaxLeaguePoint);
                        }
                        if (battle.IsWin)
                        {
                            lbWinEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as LeagueBattle).MyEstimatedLeaguePower);
                            lbLoseEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as LeagueBattle).OtherEstimatedLeaguePower);
                        }
                        else
                        {
                            lbWinEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as LeagueBattle).OtherEstimatedLeaguePower);
                            lbLoseEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as LeagueBattle).MyEstimatedLeaguePower);
                        }
                        break;
                    case Mode.Key.private_battle:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_private");
                        lbPowerName.Content = "";
                        lbPower.Content = "";
                        lbWinEstimatedPower.Content = "";
                        lbLoseEstimatedPower.Content = "";
                        break;
                    case Mode.Key.splatfest:
                        imgMode.Source = (BitmapImage)FindResource("image_battle_splatfest");
                        switch ((battle as SplatfestBattle).SplatfestMode)
                        {
                            case SplatfestBattle.Key.regular:
                                lbPowerName.Content = "";
                                lbPower.Content = "";
                                break;
                            case SplatfestBattle.Key.challenge:
                                lbPowerName.Content = Translate("splatfest_power", true);
                                if ((battle as SplatfestBattle).IsCalculating)
                                {
                                    lbPower.Content = Translate("calculating", true);
                                }
                                else
                                {
                                    lbPower.Content = string.Format(Translate("{0}/{1}", true), (battle as SplatfestBattle).SplatfestPower, (battle as SplatfestBattle).MaxSplatfestPower);
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        if (battle.IsWin)
                        {
                            lbWinEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as SplatfestBattle).MyEstimatedSplatfestPower);
                            lbLoseEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as SplatfestBattle).OtherEstimatedSplatfestPower);
                        }
                        else
                        {
                            lbWinEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as SplatfestBattle).OtherEstimatedSplatfestPower);
                            lbLoseEstimatedPower.Content = string.Format(Translate("estimated {0}", true), (battle as SplatfestBattle).MyEstimatedSplatfestPower);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                lbRule.Content = Translate(battle.Rule.ToString());
                if (battle.IsWin)
                {
                    tagResult.Content = Translate("Win!", true);
                    tagResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                    switch (battle.Type)
                    {
                        case Mode.Key.regular_battle:
                        case Mode.Key.splatfest:
                            tagWin.Content = battle.MyScore.ToString();
                            tagWin.Content2 = Translate("%", true);
                            tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            tagLose.Content = battle.OtherScore.ToString();
                            tagLose.Content2 = Translate("%", true);
                            tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            break;
                        case Mode.Key.ranked_battle:
                            if ((battle as RankedBattle).IsKo)
                            {
                                tagWin.Content = Translate("knock_out", true);
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.OtherScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            }
                            else
                            {
                                tagWin.Content = string.Format(Translate("{0} count", true), battle.MyScore.ToString());
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.OtherScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            }
                            break;
                        case Mode.Key.league_battle:
                            if ((battle as LeagueBattle).IsKo)
                            {
                                tagWin.Content = Translate("knock_out", true);
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.OtherScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            }
                            else
                            {
                                tagWin.Content = string.Format(Translate("{0} count", true), battle.MyScore.ToString());
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.OtherScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    tagResult.Content = Translate("Lose..", true);
                    tagResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                    switch (battle.Type)
                    {
                        case Mode.Key.regular_battle:
                        case Mode.Key.splatfest:
                            tagWin.Content = battle.OtherScore.ToString();
                            tagWin.Content2 = Translate("%", true);
                            tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            tagLose.Content = battle.MyScore.ToString();
                            tagLose.Content2 = Translate("%", true);
                            tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            break;
                        case Mode.Key.ranked_battle:
                            if ((battle as RankedBattle).IsBeKoed)
                            {
                                tagWin.Content = Translate("knock_out", true);
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.MyScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            }
                            else
                            {
                                tagWin.Content = string.Format(Translate("{0} count", true), battle.OtherScore.ToString());
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.MyScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            }
                            break;
                        case Mode.Key.league_battle:
                            if ((battle as LeagueBattle).IsBeKoed)
                            {
                                tagWin.Content = Translate("knock_out", true);
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.MyScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            }
                            else
                            {
                                tagWin.Content = string.Format(Translate("{0} count", true), battle.OtherScore.ToString());
                                tagWin.Content2 = "";
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = string.Format(Translate("{0} count", true), battle.MyScore.ToString());
                                tagLose.Content2 = "";
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            ((Storyboard)FindResource("fade_in")).Begin(imgMode);
                ((Storyboard)FindResource("fade_in")).Begin(lbRule);
                ((Storyboard)FindResource("fade_in")).Begin(tagResult);
                ((Storyboard)FindResource("fade_in")).Begin(lbPowerName);
                ((Storyboard)FindResource("fade_in")).Begin(lbPower);
                ((Storyboard)FindResource("fade_in")).Begin(tagWin);
                ((Storyboard)FindResource("fade_in")).Begin(lbWinEstimatedPower);
                ((Storyboard)FindResource("fade_in")).Begin(tagLose);
                ((Storyboard)FindResource("fade_in")).Begin(lbLoseEstimatedPower);
                // Update Stage
                Stage stage = battle.Stage;
                string image = FileFolderUrl.ApplicationData + stage.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    stg.Background = brush;
                    stg.Content = Translate((stage.Id).ToString());
                    ((Storyboard)FindResource("fade_in")).Begin(stg);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Battle, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg.Background = brush;
                        stg.Content = Translate((stage.Id).ToString());
                        ((Storyboard)FindResource("fade_in")).Begin(stg);
                    }));
                }
                // Update Players
                if (battle.IsWin)
                {
                    if (battle.MyPlayers.Count > 0)
                    {
                        plWin1.SetPlayer(battle.MyPlayers[0], true);
                        if (battle.MyPlayers.Count > 1)
                        {
                            plWin2.SetPlayer(battle.MyPlayers[1], true);
                            if (battle.MyPlayers.Count > 2)
                            {
                                plWin3.SetPlayer(battle.MyPlayers[2], true);
                                if (battle.MyPlayers.Count > 3)
                                {
                                    plWin4.SetPlayer(battle.MyPlayers[3], true);
                                }
                            }
                        }
                    }
                    if (battle.OtherPlayers.Count > 0)
                    {
                        plLose1.SetPlayer(battle.OtherPlayers[0], false);
                        if (battle.OtherPlayers.Count > 1)
                        {
                            plLose2.SetPlayer(battle.OtherPlayers[1], false);
                            if (battle.OtherPlayers.Count > 2)
                            {
                                plLose3.SetPlayer(battle.OtherPlayers[2], false);
                                if (battle.OtherPlayers.Count > 3)
                                {
                                    plLose4.SetPlayer(battle.OtherPlayers[3], false);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (battle.OtherPlayers.Count > 0)
                    {
                        plWin1.SetPlayer(battle.OtherPlayers[0], false);
                        if (battle.OtherPlayers.Count > 1)
                        {
                            plWin2.SetPlayer(battle.OtherPlayers[1], false);
                            if (battle.OtherPlayers.Count > 2)
                            {
                                plWin3.SetPlayer(battle.OtherPlayers[2], false);
                                if (battle.OtherPlayers.Count > 3)
                                {
                                    plWin4.SetPlayer(battle.OtherPlayers[3], false);
                                }
                            }
                        }
                    }
                    if (battle.MyPlayers.Count > 0)
                    {
                        plLose1.SetPlayer(battle.MyPlayers[0], true);
                        if (battle.MyPlayers.Count > 1)
                        {
                            plLose2.SetPlayer(battle.MyPlayers[1], true);
                            if (battle.MyPlayers.Count > 2)
                            {
                                plLose3.SetPlayer(battle.MyPlayers[2], true);
                                if (battle.MyPlayers.Count > 3)
                                {
                                    plLose4.SetPlayer(battle.MyPlayers[3], true);
                                }
                            }
                        }
                    }
                }
            }
            // Fade out loading
            ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("battle_window-" + s);
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
