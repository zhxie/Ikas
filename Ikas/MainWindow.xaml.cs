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
using System.Windows.Threading;

using ClassLib;

namespace Ikas
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScheduleWindow scheduleWindow;
        private BattleWindow battleWindow;
        private SettingsWindow settingsWindow;

        private DispatcherTimer tmSchedule;
        private DispatcherTimer tmBattle;

        public MainWindow()
        {
            // Load user and system configuration
            Depot.LoadSystemConfiguration();
            Depot.LoadUserConfiguration();
            // Load language
            if (Depot.Language != "")
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
            Topmost = Depot.AlwaysOnTop;
            RenderOptions.SetBitmapScalingMode(bdStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage2, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.AlwaysOnTopChanged += new AlwaysOnTopChangedEventHandler(AlwaysOnTopChanged);
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.ScheduleChanged += new ScheduleChangedEventHandler(ScheduleChanged);
            Depot.ScheduleUpdated += new ScheduleUpdatedEventHandler(ScheduleUpdated);
            Depot.ScheduleFailed += new ScheduleFailedEventHandler(ScheduleFailed);
            Depot.BattleFailed += new BattleFailedEventHandler(BattleFailed);
            // Prepare windows
            scheduleWindow = new ScheduleWindow();
            scheduleWindow.Opacity = 0;
            scheduleWindow.Visibility = Visibility.Hidden;
            battleWindow = new BattleWindow();
            battleWindow.Opacity = 0;
            battleWindow.Visibility = Visibility.Hidden;
            settingsWindow = new SettingsWindow();
            settingsWindow.Opacity = 0;
            settingsWindow.Visibility = Visibility.Hidden;
            // Create timers
            tmSchedule = new DispatcherTimer();
            tmSchedule.Tick += new EventHandler((object source, EventArgs e) => { Depot.GetSchedule(); });
            tmSchedule.Interval = new TimeSpan(0, 0, 15);
            tmBattle = new DispatcherTimer();
            tmBattle.Tick += new EventHandler((object source, EventArgs e) => {
                if (battleWindow.Visibility == Visibility.Hidden)
                {
                    Depot.GetLastBattle();
                }
            });
            tmBattle.Interval = new TimeSpan(0, 0, 30);
        }

        #region Control Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Check cookie
            if (Depot.Cookie == "")
            {
                MessageBox.Show(Translate("Welcome to Ikas! To use Ikas, you may set up your Cookie first.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                MenuItemSetting_Click(null, null);
            }
            // Update Schedule
            Depot.GetSchedule();
            // Start timers
            tmSchedule.Start();
            tmBattle.Start();
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(scheduleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(battleWindow);
            DragMove();
        }

        private void LbMode_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (Depot.CurrentMode)
            {
                case Mode.Key.regular_battle:
                    Depot.CurrentMode = Mode.Key.ranked_battle;
                    break;
                case Mode.Key.ranked_battle:
                    Depot.CurrentMode = Mode.Key.league_battle;
                    break;
                case Mode.Key.league_battle:
                    Depot.CurrentMode = Mode.Key.regular_battle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BdStage_MouseEnter(object sender, MouseEventArgs e)
        {
            scheduleWindow.Top = Top + Height + 10;
            scheduleWindow.Left = Left;
            ((Storyboard)FindResource("window_fade_in")).Begin(scheduleWindow);
        }

        private void BdStage_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(scheduleWindow);
        }

        private void LbLevel_MouseEnter(object sender, MouseEventArgs e)
        {
            Depot.GetLastBattle();
            battleWindow.Top = Top + Height + 10;
            battleWindow.Left = Left;
            ((Storyboard)FindResource("window_fade_in")).Begin(battleWindow);
        }

        private void LbLevel_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(battleWindow);
        }

        private void MenuItemSetting_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_in")).Begin(settingsWindow);
            settingsWindow.ShowDialog();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        private void AlwaysOnTopChanged()
        {
            Topmost = Depot.AlwaysOnTop;
        }

        private void LanguageChanged()
        {
            ResourceDictionary lang = (ResourceDictionary)Application.LoadComponent(new Uri(@"assets/lang/" + Depot.Language + ".xaml", UriKind.Relative));
            if (Resources.MergedDictionaries.Count > 0)
            {
                Resources.MergedDictionaries.Clear();
            }
            Resources.MergedDictionaries.Add(lang);
        }

        private void ScheduleChanged()
        {
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbLevel);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage1);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage2);
        }

        private void ScheduleUpdated()
        {
            Schedule schedule = Depot.Schedule;
            List<ScheduledStage> scheduledStages = schedule.GetStages(Depot.CurrentMode);
            if (scheduledStages.Count > 0 || Depot.CurrentMode == Mode.Key.regular_battle)
            {
                if (scheduledStages.Count > 0)
                {
                    // Change UI
                    switch (Depot.CurrentMode)
                    {
                        case Mode.Key.regular_battle:
                            lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            break;
                        case Mode.Key.ranked_battle:
                            lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                            break;
                        case Mode.Key.league_battle:
                            lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    lbMode.Content = Translate(((Rule.ShortName)scheduledStages[0].Rule).ToString());
                    switch (scheduledStages[0].Rule)
                    {
                        case Rule.Key.turf_war:
                            if (Depot.Level > 0)
                            {
                                if (Depot.Level > 100)
                                {
                                    tbLevel.Text = (Depot.Level - Depot.Level / 100 * 100).ToString();
                                    tbStar.Text = Translate("★", true);
                                }
                                else
                                {
                                    tbLevel.Text = Depot.Level.ToString();
                                    tbStar.Text = "";
                                }
                            }
                            else
                            {
                                tbLevel.Text = Translate("--", true);
                                tbStar.Text = "";
                            }
                            break;
                        case Rule.Key.splat_zones:
                            tbLevel.Text = Translate(Depot.SplatZonesRank.ToString());
                            tbStar.Text = "";
                            break;
                        case Rule.Key.tower_control:
                            tbLevel.Text = Translate(Depot.TowerControlRank.ToString());
                            tbStar.Text = "";
                            break;
                        case Rule.Key.rainmaker:
                            tbLevel.Text = Translate(Depot.RainmakerRank.ToString());
                            tbStar.Text = "";
                            break;
                        case Rule.Key.clam_blitz:
                            tbLevel.Text = Translate(Depot.ClamBlitzRank.ToString());
                            tbStar.Text = "";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    // Fade in labels
                    ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                    ((Storyboard)FindResource("fade_in")).Begin(lbLevel);
                    // Update Stages
                    Stage stage = scheduledStages[0];
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage1.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdStage1);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                        Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdStage1.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdStage1);
                        }));
                    }
                    if (scheduledStages.Count > 1)
                    {
                        Stage stage2 = scheduledStages[1];
                        string image2 = FileFolderUrl.ApplicationData + stage2.Image;
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdStage2.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdStage2);
                        }
                        catch
                        {
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage2.Image, image2, Downloader.SourceType.Schedule, Depot.Proxy);
                            Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdStage2.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdStage2);
                            }));
                        }
                    }
                }
            }
            else
            {
                // Current mode do not has a schedule, switch to regular battle
                Depot.CurrentMode = Mode.Key.regular_battle;
            }
        }

        private void ScheduleFailed()
        {
            if (Depot.ScheduleFailedCount <= 1)
            {
                MessageBox.Show(string.Format(Translate("{0} {1}\n{2}\n{3}\n{4}", true),
                    Translate("Ikas can not get schdule.", true),
                    Translate("Please check:", true),
                    Translate("1. Your network and network settings", true),
                    Translate("2. Your Cookie (If you set up your Session Token, you may update cookie in settings)", true),
                    Translate("After you solve the problems above, if this error message continues to appear, please consider submitting the issue.", true)
                    ),"Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BattleFailed()
        {
            if (Depot.BattleFailedCount <= 1)
            {
                MessageBox.Show(string.Format(Translate("{0} {1}\n{2}\n{3}\n{4}", true),
                    Translate("Ikas can not get the latest battle.", true),
                    Translate("Please check:", true),
                    Translate("1. Your network and network settings", true),
                    Translate("2. Your Cookie (If you set up your Session Token, you may update cookie in settings)", true),
                    Translate("After you solve the problems above, if this error message continues to appear, please consider submitting the issue.", true)
                    ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("main_window-" + s);
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
