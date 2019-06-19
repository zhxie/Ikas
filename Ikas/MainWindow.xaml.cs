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

        private DispatcherTimer timer;

        public MainWindow()
        {
            // Load user and system configuration
            if (!Depot.LoadUserConfiguration())
            {
                MessageBox.Show(Translate("Failed in loading user configuration!", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            Depot.LoadSystemConfiguration();
            // Load language
            if (Depot.Language != null)
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
            Depot.ScheduleChanged += new ScheduleChangedEventHandler(ScheduleChanged);
            Depot.ScheduleUpdated += new ScheduleUpdatedEventHandler(ScheduleUpdated);
            // Prepare Schedule and Battle window
            scheduleWindow = new ScheduleWindow();
            scheduleWindow.Opacity = 0;
            scheduleWindow.Visibility = Visibility.Hidden;
            battleWindow = new BattleWindow();
            battleWindow.Opacity = 0;
            battleWindow.Visibility = Visibility.Hidden;
            // Create timer
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler((object source, EventArgs e) => { Depot.GetSchedule(); });
            timer.Interval = new TimeSpan(0, 0, 15);
            timer.Start();
        }

        #region Control Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Update Schedule
            Depot.GetSchedule();
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
            System.Diagnostics.Process.Start(System.IO.Path.GetFileName(FileFolderUrl.UserConfiguration));
            System.Diagnostics.Process.Start(System.IO.Path.GetFileName(FileFolderUrl.SystemConfiguration));
        }

        private void MenuItemTopMost_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            miTopMost.IsChecked = Topmost;
        }

        private void MenuItemClearCache_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", FileFolderUrl.ApplicationData);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

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
                    lbLevel.Content = Translate("--", true);
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
