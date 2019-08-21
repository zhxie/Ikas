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

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string YellowForeground
        {
            get
            {
                return "#FF" + Design.NeonYellow;
            }
        }

        private ScheduleWindow scheduleWindow;
        private ShiftWindow shiftWindow;
        private BattleWindow battleWindow;
        private JobWindow jobWindow;
        private SettingsWindow settingsWindow;

        private DispatcherTimer tmScheduleAndShift;
        private DispatcherTimer tmBattle;
        private DispatcherTimer tmJob;

        public MainWindow()
        {
            // Add handler for unhandled exception
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
            // Load user and system configuration
            Depot.LoadSystemConfiguration();
            Depot.LoadUserConfiguration();
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
            RenderOptions.SetBitmapScalingMode(bdStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage2, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.AlwaysOnTopChanged += new AlwaysOnTopChangedEventHandler(AlwaysOnTopChanged);
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.ScheduleChanged += new ContentChangedEventHandler(ScheduleChanged);
            Depot.ScheduleUpdated += new ContentUpdatedEventHandler(ScheduleUpdated);
            Depot.ScheduleFailed += new ContentFailedEventHandler(ScheduleFailed);
            Depot.ShiftChanged += new ContentChangedEventHandler(ShiftChanged);
            Depot.ShiftUpdated += new ContentUpdatedEventHandler(ShiftUpdated);
            Depot.ShiftFailed += new ContentFailedEventHandler(ShiftFailed);
            Depot.BattleFailed += new ContentFailedEventHandler(BattleFailed);
            Depot.JobFailed += new ContentFailedEventHandler(JobFailed);
            Depot.CookieUpdated += new CookieUpdatedEventHandler(CookieUpdated);
            // Prepare windows
            scheduleWindow = new ScheduleWindow();
            scheduleWindow.Opacity = 0;
            scheduleWindow.Visibility = Visibility.Hidden;
            shiftWindow = new ShiftWindow();
            shiftWindow.Opacity = 0;
            shiftWindow.Visibility = Visibility.Hidden;
            battleWindow = new BattleWindow();
            battleWindow.Opacity = 0;
            battleWindow.Visibility = Visibility.Hidden;
            jobWindow = new JobWindow();
            jobWindow.Opacity = 0;
            jobWindow.Visibility = Visibility.Hidden;
            settingsWindow = new SettingsWindow();
            settingsWindow.Opacity = 0;
            settingsWindow.Visibility = Visibility.Hidden;
            // Create timers
            tmScheduleAndShift = new DispatcherTimer();
            tmScheduleAndShift.Tick += new EventHandler((object source, EventArgs e) =>
            {
                Depot.GetSchedule();
                Depot.GetShift();
            });
            tmScheduleAndShift.Interval = new TimeSpan(0, 0, 15);
            tmBattle = new DispatcherTimer();
            tmBattle.Tick += new EventHandler((object source, EventArgs e) =>
            {
                if (battleWindow.Visibility == Visibility.Hidden)
                {
                    Depot.GetLastBattle();
                }
            });
            tmBattle.Interval = new TimeSpan(0, 0, 30);
            tmJob = new DispatcherTimer();
            tmJob.Tick += new EventHandler((object source, EventArgs e) =>
            {
                if (jobWindow.Visibility == Visibility.Hidden)
                {
                    Depot.GetLastJob();
                }
            });
            tmJob.Interval = new TimeSpan(0, 0, 15);
            // Initialize notification
            NotificationHelper.InitializeNotification();
        }

        #region Control Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set properties for controls (1/2)
            Topmost = Depot.AlwaysOnTop;
            if (!Depot.InUse)
            {
                if (Depot.StartX != double.MinValue && Depot.StartY != double.MinValue)
                {
                    Left = Depot.StartX;
                    Top = Depot.StartY;
                }
            }
            Depot.CurrentMode = Depot.StartMode;
            // In use
#if DEBUG
            // Do not popup in use message in Debug
#else
            if (Depot.InUse)
            {
                MessageBox.Show(string.Format(Translate("{0}._{1}", true),
                    Translate("ikas_has_started,_or_ikas_failed_to_exit_normally"),
                    Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                    ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
#endif
            Depot.InUse = true;
            // Check cookie
            if (Depot.Cookie == null || Depot.Cookie == "")
            {
                MessageBox.Show(Translate("welcome_to_ikas!_to_use_ikas,_you_may_set_up_your_cookie_first.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                MenuItemSettings_Click(null, null);
            }
            // Automatic schedule and shift, battle, and job update
            tmScheduleAndShift.Start();
            tmBattle.Start();
            tmJob.Start();
            // Update schedule or shift
            switch (Depot.CurrentMode)
            {
                case Depot.Mode.regular_battle:
                case Depot.Mode.ranked_battle:
                case Depot.Mode.league_battle:
                    Depot.ForceGetSchedule();
                    break;
                case Depot.Mode.salmon_run:
                    Depot.ForceGetShift();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(scheduleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(shiftWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(battleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(jobWindow);
            DragMove();
        }

        private void LbMode_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (Depot.CurrentMode)
            {
                case Depot.Mode.regular_battle:
                    Depot.CurrentMode = Depot.Mode.ranked_battle;
                    // Update schedule
                    Depot.ForceGetSchedule();
                    ((Storyboard)FindResource("fade_out")).Begin(spJob);
                    // Automatic schedule and shift update
                    tmScheduleAndShift.Start();
                    break;
                case Depot.Mode.ranked_battle:
                    Depot.CurrentMode = Depot.Mode.league_battle;
                    // Update schedule
                    Depot.ForceGetSchedule();
                    ((Storyboard)FindResource("fade_out")).Begin(spJob);
                    // Automatic schedule and shift update
                    tmScheduleAndShift.Start();
                    break;
                case Depot.Mode.league_battle:
                    Depot.CurrentMode = Depot.Mode.salmon_run;
                    // Update shift
                    Depot.ForceGetShift();
                    ((Storyboard)FindResource("fade_out")).Begin(spBattle);
                    // Automatic schedule and shift update
                    tmScheduleAndShift.Start();
                    break;
                case Depot.Mode.salmon_run:
                    Depot.CurrentMode = Depot.Mode.regular_battle;
                    // Update schedule
                    Depot.ForceGetSchedule();
                    ((Storyboard)FindResource("fade_out")).Begin(spJob);
                    // Automatic schedule and shift update
                    tmScheduleAndShift.Start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BdStage_MouseEnter(object sender, MouseEventArgs e)
        {
            scheduleWindow.Top = Top + Height + 10;
            scheduleWindow.Left = Left;
            ((Storyboard)FindResource("window_fade_out")).Begin(battleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(jobWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(shiftWindow);
            ((Storyboard)FindResource("window_fade_in")).Begin(scheduleWindow);
        }

        private void BdStage_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(scheduleWindow);
        }

        private void BdShiftStage_MouseEnter(object sender, MouseEventArgs e)
        {
            shiftWindow.Top = Top + Height + 10;
            shiftWindow.Left = Left;
            ((Storyboard)FindResource("window_fade_out")).Begin(battleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(jobWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(scheduleWindow);
            ((Storyboard)FindResource("window_fade_in")).Begin(shiftWindow);
        }

        private void BdShiftStage_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(shiftWindow);
        }

        private void LbLevel_MouseEnter(object sender, MouseEventArgs e)
        {
            battleWindow.Top = Top + Height + 10;
            battleWindow.Left = Left;
            ((Storyboard)FindResource("window_fade_out")).Begin(scheduleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(shiftWindow);
            Depot.GetLastBattle();
            ((Storyboard)FindResource("window_fade_out")).Begin(jobWindow);
            ((Storyboard)FindResource("window_fade_in")).Begin(battleWindow);
            // Automatic battle update
            tmBattle.Start();
        }

        private void LbLevel_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(battleWindow);
        }

        private void LbGrade_MouseEnter(object sender, MouseEventArgs e)
        {
            jobWindow.Top = Top + Height + 10;
            jobWindow.Left = Left;
            ((Storyboard)FindResource("window_fade_out")).Begin(scheduleWindow);
            ((Storyboard)FindResource("window_fade_out")).Begin(shiftWindow);
            Depot.GetLastJob();
            ((Storyboard)FindResource("window_fade_out")).Begin(battleWindow);
            ((Storyboard)FindResource("window_fade_in")).Begin(jobWindow);
            // Automatic job update
            tmJob.Start();
        }

        private void LbGrade_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_delay_fade_out")).Begin(jobWindow);
        }

        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_in")).Begin(settingsWindow);
            settingsWindow.ShowDialog();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            // Save mode on close
            Depot.StartMode = Depot.CurrentMode;
            // Save position on close
            Depot.StartX = Left;
            Depot.StartY = Top;
            // In use
            Depot.InUse = false;
            // Exit
            Environment.Exit(0);
        }

        #endregion

        private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("{0}\n\n{1}",
                string.Format(Translate("{0}._{1}", true),
                Translate("ikas_meets_an_unhandled_exception"),
                Translate("this_is_a_bug,_please_consider_submitting_the_issue.")
                ),
                e.ExceptionObject.ToString()
                ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

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
            if (Depot.CurrentMode != Depot.Mode.salmon_run)
            {
                lbGrade.IsHitTestVisible = false;
                bdShiftStageBackground.IsHitTestVisible = false;
                ((Storyboard)FindResource("fade_out")).Begin(spJob);
                ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            }
            ((Storyboard)FindResource("fade_out")).Begin(lbLevel);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage1);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage2);
        }

        private void ShiftChanged()
        {
            // Fade out labels and images
            if (Depot.CurrentMode == Depot.Mode.salmon_run)
            {
                lbLevel.IsHitTestVisible = false;
                bdStage1.IsHitTestVisible = false;
                bdStage2.IsHitTestVisible = false;
                ((Storyboard)FindResource("fade_out")).Begin(spBattle);
                ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            }
            ((Storyboard)FindResource("fade_out")).Begin(lbGrade);
            ((Storyboard)FindResource("fade_out")).Begin(bdShiftStage);
        }

        private void ScheduleUpdated()
        {
            if (Depot.CurrentMode != Depot.Mode.salmon_run)
            {
                ((Storyboard)FindResource("fade_in")).Begin(spBattle);
                lbLevel.IsHitTestVisible = true;
                bdStage1.IsHitTestVisible = true;
                bdStage2.IsHitTestVisible = true;
            }
            Schedule schedule = Depot.Schedule;
            Mode.Key mode = Mode.Key.regular_battle;
            switch (Depot.CurrentMode)
            {
                case Depot.Mode.regular_battle:
                case Depot.Mode.salmon_run:
                    mode = Mode.Key.regular_battle;
                    break;
                case Depot.Mode.ranked_battle:
                    mode = Mode.Key.ranked_battle;
                    break;
                case Depot.Mode.league_battle:
                    mode = Mode.Key.league_battle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            List<ScheduledStage> scheduledStages = schedule.GetStages(mode);
            if (scheduledStages.Count > 0)
            {
                // Change UI
                if (Depot.CurrentMode != Depot.Mode.salmon_run)
                {
                    switch (mode)
                    {
                        case Mode.Key.regular_battle:
                            lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            // tbStar.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                            break;
                        case Mode.Key.ranked_battle:
                            lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                            // tbStar.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                            break;
                        case Mode.Key.league_battle:
                            lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            // tbStar.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    lbMode.Content = Translate(((Rule.ShortName)scheduledStages[0].Rule).ToString());
                    // Fade in label
                    ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                }
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
                        if (Depot.SplatZonesRank > Rank.Key.s && Depot.SplatZonesRank < Rank.Key.x)
                        {
                            tbLevel.Text = Translate(Depot.SplatZonesRank.ToString());
                            tbStar.Text = (Depot.SplatZonesRank - Rank.Key.s_plus_0).ToString();
                        }
                        else
                        {
                            tbLevel.Text = Translate(Depot.SplatZonesRank.ToString());
                            tbStar.Text = "";
                        }
                        break;
                    case Rule.Key.tower_control:
                        if (Depot.TowerControlRank > Rank.Key.s && Depot.TowerControlRank < Rank.Key.x)
                        {
                            tbLevel.Text = Translate(Depot.TowerControlRank.ToString());
                            tbStar.Text = (Depot.TowerControlRank - Rank.Key.s_plus_0).ToString();
                        }
                        else
                        {
                            tbLevel.Text = Translate(Depot.TowerControlRank.ToString());
                            tbStar.Text = "";
                        }
                        break;
                    case Rule.Key.rainmaker:
                        if (Depot.RainmakerRank > Rank.Key.s && Depot.RainmakerRank < Rank.Key.x)
                        {
                            tbLevel.Text = Translate(Depot.RainmakerRank.ToString());
                            tbStar.Text = (Depot.RainmakerRank - Rank.Key.s_plus_0).ToString();
                        }
                        else
                        {
                            tbLevel.Text = Translate(Depot.RainmakerRank.ToString());
                            tbStar.Text = "";
                        }
                        break;
                    case Rule.Key.clam_blitz:
                        if (Depot.ClamBlitzRank > Rank.Key.s && Depot.ClamBlitzRank < Rank.Key.x)
                        {
                            tbLevel.Text = Translate(Depot.ClamBlitzRank.ToString());
                            tbStar.Text = (Depot.ClamBlitzRank - Rank.Key.s_plus_0).ToString();
                        }
                        else
                        {
                            tbLevel.Text = Translate(Depot.ClamBlitzRank.ToString());
                            tbStar.Text = "";
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                // Fade in label
                ((Storyboard)FindResource("fade_in")).Begin(lbLevel);
                // Update stages
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
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
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
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
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

        private void ScheduleFailed(Base.ErrorType error)
        {
            if (Depot.CurrentMode != Depot.Mode.salmon_run)
            {
                lbMode.Content = "Ikas";
                // Fade in label
                ((Storyboard)FindResource("fade_in")).Begin(lbMode);
            }
            tmScheduleAndShift.Stop();
            MessageBox.Show(string.Format(Translate("{0},_because_{1}._{2}", true),
                Translate("ikas_cannot_get_schdule"),
                Translate(error.ToString()),
                Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                ),"Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShiftUpdated()
        {
            if (Depot.CurrentMode == Depot.Mode.salmon_run)
            {
                ((Storyboard)FindResource("fade_in")).Begin(spJob);
                lbGrade.IsHitTestVisible = true;
                bdShiftStageBackground.IsHitTestVisible = true;
            }
            Shift shift = Depot.Shift;
            if (shift.Stages.Count > 0)
            {
                // Change UI
                if (Depot.CurrentMode == Depot.Mode.salmon_run)
                {
                    lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                    lbMode.Content = Translate("job", true);
                    // Fade in label
                    ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                }
                if (Depot.SalmonRunGrade >= 0)
                {
                    lbGrade.Content = Translate(Depot.SalmonRunGrade.ToString());
                }
                else
                {
                    lbGrade.Content = Translate("--", true);
                }
                // Fade in label
                ((Storyboard)FindResource("fade_in")).Begin(lbGrade);
                // Update stages
                Stage stage = shift.Stages[0];
                string image = FileFolderUrl.ApplicationData + stage.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdShiftStage.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdShiftStage);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Shift, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdShiftStage.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdShiftStage);
                    }));
                }
            }
        }

        private void ShiftFailed(Base.ErrorType error)
        {
            if (Depot.CurrentMode == Depot.Mode.salmon_run)
            {
                lbMode.Content = "Ikas";
                // Fade in label
                ((Storyboard)FindResource("fade_in")).Begin(lbMode);
            }
            tmScheduleAndShift.Stop();
            MessageBox.Show(string.Format(Translate("{0},_because_{1}._{2}", true),
                Translate("ikas_cannot_get_shift"),
                Translate(error.ToString()),
                Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void BattleFailed(Base.ErrorType error)
        {
            tmBattle.Stop();
            MessageBox.Show(string.Format(Translate("{0},_because_{1}._{2}", true),
                Translate("ikas_cannot_get_the_latest_battle"),
                Translate(error.ToString()),
                Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void JobFailed(Base.ErrorType error)
        {
            tmJob.Stop();
            MessageBox.Show(string.Format(Translate("{0},_because_{1}._{2}", true),
                Translate("ikas_cannot_get_the_latest_job"),
                Translate(error.ToString()),
                Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void CookieUpdated()
        {
            // Automatic schedule and bsattle update
            tmScheduleAndShift.Start();
            tmBattle.Start();
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
