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

using System.IO;
using Newtonsoft.Json.Linq;

using ClassLib;

namespace Ikas
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Control Event

        private void WdMain_Loaded(object sender, RoutedEventArgs e)
        {
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage2, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.CurrentModeChanged += new CurrentModeChangedEventHandler(CurrentModeChanged);
            Depot.ScheduleUpdated += new ScheduleUpdatedEventHandler(ScheduleUpdated);
            // Update Schedule
            Depot.GetSchedule();
        }

        private void WdMain_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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

        #endregion

        private void CurrentModeChanged()
        {
            switch (Depot.CurrentMode)
            {
                case Mode.Key.regular_battle:
                    lbMode.Content = "ランク";
                    lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                    lbLevel.Content = "";
                    bdStage1.Background = new SolidColorBrush(Colors.Black);
                    bdStage2.Background = new SolidColorBrush(Colors.Black);
                    break;
                case Mode.Key.ranked_battle:
                    lbMode.Content = "ウデマエ";
                    lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                    lbLevel.Content = "";
                    bdStage1.Background = new SolidColorBrush(Colors.Black);
                    bdStage2.Background = new SolidColorBrush(Colors.Black);
                    break;
                case Mode.Key.league_battle:
                    lbMode.Content = "ウデマエ";
                    lbMode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonRed));
                    lbLevel.Content = "";
                    bdStage1.Background = new SolidColorBrush(Colors.Black);
                    bdStage2.Background = new SolidColorBrush(Colors.Black);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Depot.GetSchedule();
        }

        private void ScheduleUpdated()
        {
            // TODO: brush animation
            Schedule schedule = Depot.Schedule;
            List<ScheduledStage> scheduledStages = schedule.GetStages(Depot.CurrentMode);
            if (scheduledStages.Count > 0 || Depot.CurrentMode == Mode.Key.regular_battle)
            {
                switch (Depot.CurrentMode)
                {
                    case Mode.Key.regular_battle:
                        lbMode.Content = ((Rule.ShortName)Depot.Schedule.GetStages(Mode.Key.regular_battle)[0].Rule).ToString();
                        lbLevel.Content = "--";
                        break;
                    case Mode.Key.ranked_battle:
                        lbMode.Content = ((Rule.ShortName)Depot.Schedule.GetStages(Mode.Key.ranked_battle)[0].Rule).ToString();
                        lbLevel.Content = "--";
                        break;
                    case Mode.Key.league_battle:
                        lbMode.Content = ((Rule.ShortName)Depot.Schedule.GetStages(Mode.Key.league_battle)[0].Rule).ToString();
                        lbLevel.Content = "--";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (scheduledStages.Count > 0)
                {
                    Stage stage = scheduledStages[0];
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage1.Background = brush;
                    }
                    else
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image);
                        Depot.downloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdStage1.Background = brush;
                        }));
                    }
                }
                if (scheduledStages.Count > 1)
                {
                    Stage stage = scheduledStages[1];
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage2.Background = brush;
                    }
                    else
                    {
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image);
                        Depot.downloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdStage2.Background = brush;
                        }));
                    }
                }
            }
            else
            {
                // Current mode do not has a schedule, switch to regular battle
                Depot.CurrentMode = Mode.Key.regular_battle;
            }
        }
    }
}
