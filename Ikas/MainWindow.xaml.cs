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
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdStage1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdStage2, BitmapScalingMode.HighQuality);
            // Update Schedule
            Depot.GetSchedule();
            Depot.ScheduleUpdatedEvent += new ScheduleUpdatedEventHandler(ScheduleUpdated);
        }

        private void ScheduleUpdated()
        {
            // TODO: brush animation
            // TODO: download image if not exist
            Schedule schedule = Depot.Schedule;
            List<ScheduledStage> scheduledStages = schedule.GetStages(Depot.CurrentMode);
            if (scheduledStages.Count > 0 || Depot.CurrentMode == Mode.Key.regular_battle)
            {
                if (scheduledStages.Count > 0)
                {
                    Stage stage = scheduledStages[0];
                    string image = System.Environment.CurrentDirectory + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage1.Background = brush;
                    }
                    else
                    {

                    }
                }
                if (scheduledStages.Count > 1)
                {
                    Stage stage = scheduledStages[1];
                    string image = System.Environment.CurrentDirectory + stage.Image;
                    if (File.Exists(image))
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdStage2.Background = brush;
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
