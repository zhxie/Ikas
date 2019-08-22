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
    /// BattleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BattleWindow : Window
    {
        public string YellowForeground
        {
            get
            {
                return "#FF" + Design.NeonYellow;
            }
        }

        public Battle Battle;

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
            // Prepare icon and weapon window
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
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(this);
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
            bool isMy = (sender as PlayerControl).IsMy;
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
                weaponWindow.SetWeapon(player.Weapon, isMy);
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
            // Force refresh labels
            if (Battle != null)
            {
                if (Battle.Stage != null)
                {
                    switch (Battle.Mode)
                    {
                        case Mode.Key.regular_battle:
                            break;
                        case Mode.Key.ranked_battle:
                            lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as RankedBattle).EstimatedRankPower);
                            break;
                        case Mode.Key.league_battle:
                            if (!(Battle as LeagueBattle).IsCalculating)
                            {
                                tbPower.Text = string.Format(Translate("{0:0.0}/{1:0.0}", true), (Battle as LeagueBattle).LeaguePoint, (Battle as LeagueBattle).MaxLeaguePoint);
                            }
                            if (Battle.IsWin)
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).MyEstimatedLeaguePower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).OtherEstimatedLeaguePower);
                            }
                            else
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).OtherEstimatedLeaguePower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).MyEstimatedLeaguePower);
                            }
                            break;
                        case Mode.Key.private_battle:
                            break;
                        case Mode.Key.splatfest:
                            switch ((Battle as SplatfestBattle).SplatfestMode)
                            {
                                case SplatfestBattle.Key.regular:
                                    break;
                                case SplatfestBattle.Key.challenge:
                                    if (!(Battle as SplatfestBattle).IsCalculating)
                                    {
                                        tbPower.Text = string.Format(Translate("{0:0.0}/{1:0.0}", true), (Battle as SplatfestBattle).SplatfestPower, (Battle as SplatfestBattle).MaxSplatfestPower);
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            if (Battle.IsWin)
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).MyEstimatedSplatfestPower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).OtherEstimatedSplatfestPower);
                            }
                            else
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).OtherEstimatedSplatfestPower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).MyEstimatedSplatfestPower);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (Battle.IsWin)
                    {
                        switch (Battle.Type)
                        {
                            case Mode.Key.regular_battle:
                            case Mode.Key.splatfest:
                                break;
                            case Mode.Key.ranked_battle:
                                if ((Battle as RankedBattle).IsKo)
                                {
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                }
                                break;
                            case Mode.Key.league_battle:
                                if ((Battle as LeagueBattle).IsKo)
                                {
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        switch (Battle.Type)
                        {
                            case Mode.Key.regular_battle:
                            case Mode.Key.splatfest:
                                break;
                            case Mode.Key.ranked_battle:
                                if ((Battle as RankedBattle).IsBeKoed)
                                {
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                }
                                break;
                            case Mode.Key.league_battle:
                                if ((Battle as LeagueBattle).IsBeKoed)
                                {
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }

        public void SetBattle(Battle battle)
        {
            Battle = battle;
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(imgMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbRule);
            ((Storyboard)FindResource("fade_out")).Begin(tagResult);
            ((Storyboard)FindResource("fade_out")).Begin(stg);
            ((Storyboard)FindResource("fade_out")).Begin(lbPowerName);
            ((Storyboard)FindResource("fade_out")).Begin(lbPower);
            ((Storyboard)FindResource("fade_out")).Begin(tagWin);
            ((Storyboard)FindResource("fade_out")).Begin(lbWinEstimatedPower);
            ((Storyboard)FindResource("fade_out")).Begin(tagLose);
            ((Storyboard)FindResource("fade_out")).Begin(lbLoseEstimatedPower);
            plWin1.SetPlayer(null, true);
            plWin2.SetPlayer(null, true);
            plWin3.SetPlayer(null, true);
            plWin4.SetPlayer(null, true);
            plLose1.SetPlayer(null, false);
            plLose2.SetPlayer(null, false);
            plLose3.SetPlayer(null, false);
            plLose4.SetPlayer(null, false);
            if (Battle != null)
            {
                if (Battle.Stage != null)
                {
                    // Update current Battle
                    switch (Battle.Mode)
                    {
                        case Mode.Key.regular_battle:
                            imgMode.Source = (BitmapImage)FindResource("image_battle_regular");
                            lbPowerName.SetResourceReference(ContentProperty, (Battle as RegularBattle).Freshness.ToString());
                            lbPower.FontFamily = FindResource("splatfont_2") as FontFamily;
                            switch ((Battle as RegularBattle).Freshness)
                            {
                                case RegularBattle.FreshnessKey.raw:
                                    lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDBDBDB"));
                                    break;
                                case RegularBattle.FreshnessKey.dry:
                                    lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.MeterGreen));
                                    break;
                                case RegularBattle.FreshnessKey.fresh:
                                    lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.MeterBronze));
                                    break;
                                case RegularBattle.FreshnessKey.superfresh:
                                case RegularBattle.FreshnessKey.superfresh2:
                                    lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.MeterSilver));
                                    break;
                                case RegularBattle.FreshnessKey.superfresh3:
                                    lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.MeterGold));
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            lbPower.Margin = new Thickness(0, -20, 0, 0);
                            tbPower.Foreground = new SolidColorBrush();
                            tbPower.FontSize = 36;
                            tbPower.Text = (Battle as RegularBattle).WinMeter.ToString("0.0");
                            tbPowerSub.Text = "";
                            lbWinEstimatedPower.Content = "";
                            lbLoseEstimatedPower.Content = "";
                            break;
                        case Mode.Key.ranked_battle:
                            imgMode.Source = (BitmapImage)FindResource("image_battle_ranked");
                            lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDBDBDB"));
                            if (Battle is RankedXBattle)
                            {
                                lbPowerName.SetResourceReference(ContentProperty, "battle_window-x_power");
                                lbPower.FontFamily = FindResource("splatfont_2") as FontFamily;
                                lbPower.Margin = new Thickness(0, -10, 0, 0);
                                tbPower.FontSize = 28;
                                if ((Battle as RankedXBattle).XPowerAfter >= 0)
                                {
                                    tbPower.Text = (Battle as RankedXBattle).XPowerAfter.ToString();
                                }
                                else
                                {
                                    tbPower.SetResourceReference(Run.TextProperty, "battle_window-calculating");
                                }
                                tbPowerSub.Text = "";
                            }
                            else
                            {
                                lbPowerName.SetResourceReference(ContentProperty, "battle_window-rank");
                                lbPower.Margin = new Thickness(0, -20, 0, 0);
                                tbPower.FontSize = 36;
                                lbPower.FontFamily = FindResource("splatfont") as FontFamily;
                                var a = (Battle as RankedBattle).RankAfter.ToString();
                                tbPower.SetResourceReference(Run.TextProperty, (Battle as RankedBattle).RankAfter.ToString());
                                if ((Battle as RankedBattle).RankAfter > Rank.Key.s && (Battle as RankedBattle).RankAfter < Rank.Key.x)
                                {
                                    tbPowerSub.Text = ((Battle as RankedBattle).RankAfter - Rank.Key.s_plus_0).ToString();
                                }
                                else
                                {
                                    tbPowerSub.Text = "";
                                }
                            }
                            lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as RankedBattle).EstimatedRankPower);
                            lbLoseEstimatedPower.Content = "";
                            break;
                        case Mode.Key.league_battle:
                            imgMode.Source = (BitmapImage)FindResource("image_battle_league");
                            lbPowerName.SetResourceReference(ContentProperty, "battle_window-league_power");
                            lbPower.FontFamily = FindResource("splatfont_2") as FontFamily;
                            lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDBDBDB"));
                            lbPower.Margin = new Thickness(0, -10, 0, 0);
                            tbPower.FontSize = 28;
                            if ((Battle as LeagueBattle).IsCalculating)
                            {
                                tbPower.SetResourceReference(Run.TextProperty, "battle_window-calculating");
                                tbPowerSub.Text = "";
                            }
                            else
                            {
                                tbPower.Text = string.Format(Translate("{0:0.0}/{1:0.0}", true), (Battle as LeagueBattle).LeaguePoint, (Battle as LeagueBattle).MaxLeaguePoint);
                                tbPowerSub.Text = "";
                            }
                            if (Battle.IsWin)
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).MyEstimatedLeaguePower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).OtherEstimatedLeaguePower);
                            }
                            else
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).OtherEstimatedLeaguePower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as LeagueBattle).MyEstimatedLeaguePower);
                            }
                            break;
                        case Mode.Key.private_battle:
                            imgMode.Source = (BitmapImage)FindResource("image_battle_private");
                            lbPowerName.Content = "";
                            lbPower.FontFamily = FindResource("splatfont_2") as FontFamily;
                            lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDBDBDB"));
                            lbPower.Margin = new Thickness(0, -10, 0, 0);
                            tbPower.FontSize = 28;
                            tbPower.Text = "";
                            tbPowerSub.Text = "";
                            lbWinEstimatedPower.Content = "";
                            lbLoseEstimatedPower.Content = "";
                            break;
                        case Mode.Key.splatfest:
                            imgMode.Source = (BitmapImage)FindResource("image_battle_splatfest");
                            lbPower.FontFamily = FindResource("splatfont_2") as FontFamily;
                            lbPower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDBDBDB"));
                            lbPower.Margin = new Thickness(0, -10, 0, 0);
                            tbPower.FontSize = 28;
                            switch ((Battle as SplatfestBattle).SplatfestMode)
                            {
                                case SplatfestBattle.Key.regular:
                                    lbPowerName.Content = "";
                                    tbPower.Text = "";
                                    tbPowerSub.Text = "";
                                    break;
                                case SplatfestBattle.Key.challenge:
                                    lbPowerName.SetResourceReference(ContentProperty, "battle_window-splatfest_power");
                                    if ((Battle as SplatfestBattle).IsCalculating)
                                    {
                                        tbPower.SetResourceReference(Run.TextProperty, "battle_window-calculating");
                                        tbPowerSub.Text = "";
                                    }
                                    else
                                    {
                                        tbPower.Text = string.Format(Translate("{0:0.0}/{1:0.0}", true), (Battle as SplatfestBattle).SplatfestPower, (Battle as SplatfestBattle).MaxSplatfestPower);
                                        tbPowerSub.Text = "";
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            if (Battle.IsWin)
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).MyEstimatedSplatfestPower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).OtherEstimatedSplatfestPower);
                            }
                            else
                            {
                                lbWinEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).OtherEstimatedSplatfestPower);
                                lbLoseEstimatedPower.Content = string.Format(Translate("estimated_{0}", true), (Battle as SplatfestBattle).MyEstimatedSplatfestPower);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    lbRule.SetResourceReference(ContentProperty, Battle.Rule.ToString());
                    if (Battle.IsWin)
                    {
                        tagResult.SetResourceReference(TagControl.ContentProperty, "battle_window-win");
                        tagResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                        switch (Battle.Type)
                        {
                            case Mode.Key.regular_battle:
                            case Mode.Key.splatfest:
                                tagWin.Content = Battle.MyScore.ToString("0.0");
                                tagWin.SetResourceReference(TagControl.Content2Property, "battle_window-%");
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                tagLose.Content = Battle.OtherScore.ToString("0.0");
                                tagLose.SetResourceReference(TagControl.Content2Property, "battle_window-%");
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                break;
                            case Mode.Key.ranked_battle:
                                if ((Battle as RankedBattle).IsKo)
                                {
                                    tagWin.SetResourceReference(TagControl.ContentProperty, "battle_window-knock_out");
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagLose.Content2 = "";
                                    tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagLose.Content2 = "";
                                    tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                }
                                break;
                            case Mode.Key.league_battle:
                                if ((Battle as LeagueBattle).IsKo)
                                {
                                    tagWin.SetResourceReference(TagControl.ContentProperty, "battle_window-knock_out");
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagLose.Content2 = "";
                                    tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
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
                        tagResult.SetResourceReference(TagControl.ContentProperty, "battle_window-lose");
                        tagResult.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                        switch (Battle.Type)
                        {
                            case Mode.Key.regular_battle:
                            case Mode.Key.splatfest:
                                tagWin.Content = Battle.OtherScore.ToString("0.0");
                                tagWin.SetResourceReference(TagControl.Content2Property, "battle_window-%");
                                tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                tagLose.Content = Battle.MyScore.ToString("0.0");
                                tagLose.SetResourceReference(TagControl.Content2Property, "battle_window-%");
                                tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                break;
                            case Mode.Key.ranked_battle:
                                if ((Battle as RankedBattle).IsBeKoed)
                                {
                                    tagWin.SetResourceReference(TagControl.ContentProperty, "battle_window-knock_out");
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagLose.Content2 = "";
                                    tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagLose.Content2 = "";
                                    tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                }
                                break;
                            case Mode.Key.league_battle:
                                if ((Battle as LeagueBattle).IsBeKoed)
                                {
                                    tagWin.SetResourceReference(TagControl.ContentProperty, "battle_window-knock_out");
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
                                    tagLose.Content2 = "";
                                    tagLose.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                }
                                else
                                {
                                    tagWin.Content = string.Format(Translate("{0}_count", true), Battle.OtherScore.ToString());
                                    tagWin.Content2 = "";
                                    tagWin.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                                    tagLose.Content = string.Format(Translate("{0}_count", true), Battle.MyScore.ToString());
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
                    // Update stage
                    Stage stage = Battle.Stage;
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        stg.Background = brush;
                        stg.SetResourceReference(StageControl.ContentProperty, stage.Id.ToString());
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
                            stg.SetResourceReference(StageControl.ContentProperty, stage.Id.ToString());
                            ((Storyboard)FindResource("fade_in")).Begin(stg);
                        }));
                    }
                    // Update players
                    if (Battle.IsWin)
                    {
                        if (Battle.MyPlayers.Count > 0)
                        {
                            plWin1.SetPlayer(Battle.MyPlayers[0], true);
                            if (Battle.MyPlayers.Count > 1)
                            {
                                plWin2.SetPlayer(Battle.MyPlayers[1], true);
                                if (Battle.MyPlayers.Count > 2)
                                {
                                    plWin3.SetPlayer(Battle.MyPlayers[2], true);
                                    if (Battle.MyPlayers.Count > 3)
                                    {
                                        plWin4.SetPlayer(Battle.MyPlayers[3], true);
                                    }
                                }
                            }
                        }
                        if (Battle.OtherPlayers.Count > 0)
                        {
                            plLose1.SetPlayer(Battle.OtherPlayers[0], false);
                            if (Battle.OtherPlayers.Count > 1)
                            {
                                plLose2.SetPlayer(Battle.OtherPlayers[1], false);
                                if (Battle.OtherPlayers.Count > 2)
                                {
                                    plLose3.SetPlayer(Battle.OtherPlayers[2], false);
                                    if (Battle.OtherPlayers.Count > 3)
                                    {
                                        plLose4.SetPlayer(Battle.OtherPlayers[3], false);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Battle.OtherPlayers.Count > 0)
                        {
                            plWin1.SetPlayer(Battle.OtherPlayers[0], false);
                            if (Battle.OtherPlayers.Count > 1)
                            {
                                plWin2.SetPlayer(Battle.OtherPlayers[1], false);
                                if (Battle.OtherPlayers.Count > 2)
                                {
                                    plWin3.SetPlayer(Battle.OtherPlayers[2], false);
                                    if (Battle.OtherPlayers.Count > 3)
                                    {
                                        plWin4.SetPlayer(Battle.OtherPlayers[3], false);
                                    }
                                }
                            }
                        }
                        if (Battle.MyPlayers.Count > 0)
                        {
                            plLose1.SetPlayer(Battle.MyPlayers[0], true);
                            if (Battle.MyPlayers.Count > 1)
                            {
                                plLose2.SetPlayer(Battle.MyPlayers[1], true);
                                if (Battle.MyPlayers.Count > 2)
                                {
                                    plLose3.SetPlayer(Battle.MyPlayers[2], true);
                                    if (Battle.MyPlayers.Count > 3)
                                    {
                                        plLose4.SetPlayer(Battle.MyPlayers[3], true);
                                    }
                                }
                            }
                        }
                    }
                }
                // Fade out loading
                ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
                bdLoading.IsHitTestVisible = false;
            }
        }

        public void StartLoading()
        {
            // Fade in loading
            bdLoading.IsHitTestVisible = true;
            ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
        }

        public void StopLoading()
        {
            // Fade out loading
            ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
            bdLoading.IsHitTestVisible = false;
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
