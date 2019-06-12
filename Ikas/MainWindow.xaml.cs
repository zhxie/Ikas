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

using System.IO;
using System.Windows.Media.Animation;

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
            // Load user and system configuration
            if (!Depot.LoadUserConfiguration())
            {
                MessageBox.Show("Failed in loading user configuration!", "Ikas", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            Depot.LoadSystemConfiguration();
            // Initialize component
            InitializeComponent();
        }

        #region Control Event

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.Show();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage2, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.CurrentModeChanged += new CurrentModeChangedEventHandler(CurrentModeChanged);
            Depot.ScheduleUpdated += new ScheduleUpdatedEventHandler(ScheduleUpdated);
            // Update Schedule
            Depot.GetSchedule();
        }

        private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(lbMode);
            ((Storyboard)FindResource("fade_out")).Begin(lbLevel);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage1);
            ((Storyboard)FindResource("fade_out")).Begin(bdStage2);
            // Update Schedule
            Depot.GetSchedule();
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
                    lbMode.Content = ((Rule.ShortName)scheduledStages[0].Rule).ToString();
                    lbLevel.Content = "--";
                    // Fade in labels
                    ((Storyboard)FindResource("fade_in")).Begin(lbMode);
                    ((Storyboard)FindResource("fade_in")).Begin(lbLevel);
                    // Update Stages
                    Stage stage = scheduledStages[0];
                    string image = FileFolderUrl.ApplicationData + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage1.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdStage1);
                    }
                    else
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
                        stage = scheduledStages[1];
                        image = FileFolderUrl.ApplicationData + stage.Image;
                        if (File.Exists(image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.UniformToFill;
                            bdStage2.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdStage2);
                        }
                        else
                        {
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + stage.Image, image, Downloader.SourceType.Schedule, Depot.Proxy);
                            Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
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
    }
}
